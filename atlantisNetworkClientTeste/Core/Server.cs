using Models;

namespace Core;

public class Server
{
    public BlockchainTree blockchainTree { get; set; }
    public Server()
    {
        this.blockchainTree = new BlockchainTree(2);
    }

    public Block GerateBlock(string code, string previousHash, string data)
    {
        var block = new Block(Guid.NewGuid(), DateTime.Now, previousHash, code, data);
        return block == null ? new Block() : block;
    }
}