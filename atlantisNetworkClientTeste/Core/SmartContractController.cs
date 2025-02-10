namespace Core;

public class SmartContractController
{
    Wavechain _wavechain { get; set; }
    Server _server { get; set; }

    public SmartContractController(Server server)
        => _server = server;
}