using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using atlantisNetworkClientTeste;
using Enums;
using Models;

namespace Core;

public class SmartContractExecutor
{
    BlockchainClient client { get; set; }
    long ticks;
    double nanosegundos;

    public SmartContractExecutor(BlockchainClient client)
        => this.client = client;

    public async void ExtractAsync(string data)
    {
        var matches = Regex.Matches(data, @"\{([^}]*)\}");
        if (matches.Count > 0)
        {
            var block = new Server().GerateBlock(matches[0].Value, matches[1].Value, matches[2].Value);
            client._trees.TryGetValue(client.userId, out Wavechain? tree);
            if (tree == null)
            {
                var _tree = new Wavechain(1);
                _tree.Insert(block);
                client._trees.TryAdd(client.userId, _tree);
            }
            if (tree != null)tree.Insert(block);
             await client.SendData(new Message().ConcatenateByteArrays(RequestCode.Validation, ActionCode.Block, JsonSerializer.Serialize(block)));


        }
        
    }
}