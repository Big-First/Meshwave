﻿namespace Models;

public class AI_Agent
{
    private Dictionary<string, List<decimal>> transactionHistory;

    public AI_Agent()
    =>  transactionHistory = new Dictionary<string, List<decimal>>();
    
    public void RegisterTransaction(string nodeId, decimal amount)
    {
        if (!transactionHistory.ContainsKey(nodeId))
            transactionHistory[nodeId] = new List<decimal>();

        transactionHistory[nodeId].Add(amount);
    }
    public bool DetectFraud(string nodeId)
    {
        if (!transactionHistory.ContainsKey(nodeId) || transactionHistory[nodeId].Count < 5)
            return false; 

        var transactions = transactionHistory[nodeId];
        double mean = (double)transactions.Average();
        double variance = transactions.Select(x => Math.Pow((double)(x - (decimal)mean), 2)).Average();
        double stdDev = Math.Sqrt(variance);

        decimal lastTransaction = transactions.Last();
        return Math.Abs((double)(lastTransaction - (decimal)mean)) > 2 * stdDev;
    }
    
    public string SuggestValidator(Dictionary<string, decimal> stakes)
    {
        if (stakes.Count == 0)
            return null;

        // Escolhe o nó com maior stake e sem histórico de fraude
        var sortedNodes = stakes.OrderByDescending(x => x.Value)
            .Where(x => !DetectFraud(x.Key))
            .ToList();

        return sortedNodes.Any() ? sortedNodes.First().Key : stakes.Keys.First();
    }
}