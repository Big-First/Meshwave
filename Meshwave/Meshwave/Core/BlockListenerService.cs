using Models;

namespace Core;

public class BlockListenerService : BackgroundService
{
    private readonly Server _server;
    private readonly ILogger<BlockListenerService> _logger;

    public BlockListenerService(Server server, ILogger<BlockListenerService> logger)
    {
        _server = server;
        _logger = logger;

        // Inscreve-se no evento de adição de blocos
        _server.BlockAdded += OnBlockAdded;
    }

    private void OnBlockAdded(object? sender, ValidationBlock block)
    {
        Console.WriteLine($"{block.userId} \t Novo bloco adicionado: hash {block.block.hash} \t Timestamp {block.block.timestamp}");
        //_server.ValidationBlockchain(block);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Serviço de monitoramento de blocos iniciado.");
        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _server.BlockAdded -= OnBlockAdded;
        return base.StopAsync(cancellationToken);
    }
}