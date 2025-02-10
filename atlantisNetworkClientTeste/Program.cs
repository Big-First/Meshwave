using Core;

namespace atlantisNetworkClientTeste;

class Program
{
    Server server = new Server();
    static async Task Main(string[] args)
    {
        var client = new BlockchainClient(new Server());
            await client.ConnectToServer("ws://localhost:3333/", CancellationToken.None);
    }
}