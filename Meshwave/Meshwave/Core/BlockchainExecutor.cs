using Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Core;

public class BlockchainExecutor
{
    public BlockchainExecutor(){}
    public Node node { get; set; }

    public BlockchainExecutor(Node node)
    {
        this.node = node;
    }

    public void ExtractAsync(ContractValidationRequest output)
    {
        var block = output.block;
        if (block != null)
        {
            node.server.AddBlock(new ValidationBlock(node.userId.ToString(), block));
            node._wavechain.Insert(block);
        }
    }
}