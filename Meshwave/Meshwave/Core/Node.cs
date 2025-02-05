using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Data;
using Enums;
using Models;

namespace Core;

public class Node
{
    public Node(){}
    public Wavechain _wavechain;
    public Queue<Block> _blocks = new ();
    public event EventHandler<Block>? BlockAdded;
    public Guid userId { get; set; }
    public byte[] publicKey { get; set; }
    public byte[] privateKey { get; set; }
    public DateTime timestamp { get; set; }
    private readonly ConcurrentDictionary<WebSocket, Task> _peers;
    public WebSocket socket;
    public Server server;
    public Node left { get; set; }
    public Node right { get; set; }
    
    public Node(Guid userName, WebSocket socket,DateTime timestamp, Server server)
    {
        this.userId = userName;
        this.socket = socket;
        this.server = server;
        this.timestamp = timestamp;
        _peers = new ConcurrentDictionary<WebSocket, Task>();
        this.left = null;
        this.right = null;
        GenerateKeys();
        this._wavechain = new Wavechain(this);
        Console.WriteLine($"User connected: {userId}");
    }
    
    private void GenerateKeys()
    {
        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            rsa.PersistKeyInCsp = false;
            privateKey = rsa.ExportRSAPrivateKey();
            publicKey = rsa.ExportRSAPublicKey();
        }
    }
    public void AddBlock(Block newBlock)
    {
        _blocks.Enqueue(newBlock);
        
        BlockAdded?.Invoke(this, newBlock);
    }
    public async Task SendWelcomeMessage(WebSocket webSocket)
    {
        var buffer = new ObjectSerialization().Serialize(new ContractValidationRequest(userId.ToString(),publicKey,privateKey, null, RequestCode.User, ActionCode.UserId, "", null));
        await SendData(buffer);
    }
    
    public async Task Echo(HttpContext context, WebSocket webSocket)
    {
        var buffer = new byte[1024];
        try
        {
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (result.MessageType != WebSocketMessageType.Close)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                ProcessReceivedBytes(buffer, result.Count);
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await socket!.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
        catch (WebSocketException ex)
        {
            Console.WriteLine($" UserId : {userId} Error:{ex.Message}");
        }
    }
    public async Task SendData(byte[] msg)
    {
        
        try
        {
            if (msg != null)
            {
                await this.socket.SendAsync(new ArraySegment<byte>(msg), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine(string.Format("{0} >> {1} >> {2}", DateTime.Now, nameof(SendData), e.Message));
        }
    }

    private void ProcessReceivedBytes(byte[] buffer, int resultCount)
    {
        byte[] receivedData = new byte[resultCount];
        Array.Copy(buffer, receivedData, resultCount);
        var value = new ObjectSerialization().Deserialize<ContractValidationRequest>(receivedData) as ContractValidationRequest;
        if (value != null) OnProcessMessage(value.requestCode, value.actionCode, value);
    }

    public void OnProcessMessage(RequestCode requestCode, ActionCode actionCode, ContractValidationRequest output)
        => new ControllerManager().HandleRequest(requestCode, actionCode, output, this);

    public async void ExecutePersistence()
    {
        var _redisService = new RedisService();
        await _redisService.SaveObjectAsync($"{userId}", _wavechain.root);
    }
}