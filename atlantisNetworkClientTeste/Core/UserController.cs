using atlantisNetworkClientTeste;
using Models;

namespace Core;

public class UserController
{
    BlockchainClient client { get; set; }
    long ticks;
    double nanosegundos;

    public UserController(BlockchainClient client)
        => this.client = client;

    public void ExtractAsync(string output)
    {
        Console.WriteLine($"{nameof(ExtractAsync)} >> {output}");
        client.userId = output;
    }
}