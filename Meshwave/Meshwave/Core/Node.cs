using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using Enums;

namespace Core;

public class Node
{
    public Node(){}
    public string userId { get; set; }
    public WebSocket socket;
    public Node leftChild { get; set; }
    public Node rightChild { get; set; }
    
    public Node(string userName,WebSocket socket)
    {
        this.userId = userName;
        this.socket = socket;
        this.leftChild = null;
        this.rightChild = null;
        Console.WriteLine($"User connected: {userId}");
    }
    
    public async Task SendWelcomeMessage(WebSocket webSocket)
    {
        var buffer = new Message().ConcatenateByteArrays(RequestCode.User, ActionCode.UserId, userId);
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
            //if (webSocket.State != WebSocketState.Closed)_server.RemoveUser(this);
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
        throw new NotImplementedException();
    }
}