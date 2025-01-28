using StackExchange.Redis;

namespace Data;

public class RedisConnector
{
    private static Lazy<ConnectionMultiplexer> _lazyConnection;

    static RedisConnector()
    {
        // Configura o Redis para conectar ao endereço fornecido
        _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect($"localhost:6379");
        });
    }

    public static ConnectionMultiplexer Connection => _lazyConnection.Value;

    public static IDatabase GetDatabase() => Connection.GetDatabase();
}