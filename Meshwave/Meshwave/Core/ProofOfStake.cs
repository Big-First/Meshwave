namespace Core;

public class ProofOfStake
{
    private Dictionary<string, decimal> stakes; // Armazena o stake de cada nó (endereço → moedas apostadas)
    private Random random;

    public ProofOfStake()
    {
        stakes = new Dictionary<string, decimal>();
        random = new Random();
    }

    // Adiciona ou atualiza o stake de um nó
    public void SetStake(string nodeId, decimal amount)
    {
        if (stakes.ContainsKey(nodeId))
            stakes[nodeId] += amount;
        else
            stakes[nodeId] = amount;
    }

    // Escolhe um nó validador com base no stake
    public string SelectValidator()
    {
        if (stakes.Count == 0)
            throw new InvalidOperationException("Nenhum nó possui stake para validação.");

        // Criar um sorteio ponderado pelo stake
        decimal totalStake = stakes.Values.Sum();
        decimal randomValue = (decimal)random.NextDouble() * totalStake;

        decimal cumulativeStake = 0;
        foreach (var entry in stakes)
        {
            cumulativeStake += entry.Value;
            if (randomValue <= cumulativeStake)
            {
                Console.WriteLine($"🎯 Nó selecionado para validação: {entry.Key}");
                return entry.Key;
            }
        }

        return stakes.Keys.First(); // Caso algo falhe, retorna o primeiro nó
    }

    // Obter o stake de um nó
    public decimal GetStake(string nodeId)
    {
        return stakes.ContainsKey(nodeId) ? stakes[nodeId] : 0;
    }
}