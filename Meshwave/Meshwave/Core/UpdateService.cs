namespace Core;

public class UpdateService : BackgroundService
{
    private readonly ILogger<UpdateService> _logger;
    private readonly Server _server;

    public UpdateService(ILogger<UpdateService> logger, Server server)
    {
        _logger = logger;
        _server = server;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Serviço de atualização iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            _server.Update(); // Chama a função de atualização do servidor
            //_logger.LogInformation($"Atualização realizada em: {DateTime.Now}");

            await Task.Delay(1000, stoppingToken); // Executa a cada 1 segundo
        }

        _logger.LogInformation("Serviço de atualização finalizado.");
    }
}