using System.Security.Cryptography;
using System.Text;

namespace Models;

public class Coin
{
    
    /*
    public string CalculateData()
    {
        string blockData = $"{uuid}{timestamp}";
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(blockData);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
    */
    /*
    public void Mint(Guid to, decimal amount)
    {
        if (!Balances.ContainsKey(to))
        {
            Console.WriteLine($"🏆 {to} Carteira nao foi localizada.");
        }

        TotalSupply += amount;
        Balances[to] += amount;

        Console.WriteLine($"🏆 {amount} {Symbol} criados e enviados para {to}.");
    }
    */
    /*
    public bool Transfer(Wallet sourceWallet, Wallet targetWallet, int amount)
    {
        if (sourceWallet.balances.Count < amount)
        {
            return false;
        }

        var cstsToTransfer = sourceWallet.balances.Take(amount).ToDictionary(c => c.Key, c => c.Value);

        foreach (var cst in cstsToTransfer)
        {
            targetWallet.balances.TryAdd(cst.Key, cst.Value);
            sourceWallet.balances.Remove(cst.Key);
        }
        Console.WriteLine($" Transferido {amount} {symbol} de {sourceWallet.uuid} para {targetWallet.uuid}.");
        return true;
    }
    */
    /*
    // Criar novas moedas (somente administradores)
    public void Mint(Guid to, decimal amount)
    {
        if (!Balances.ContainsKey(to))
        {
            Balances[to] = 0;
        }

        Console.WriteLine($"🏆 {amount} {Symbol} criados e enviados para {to}.");
    }
    */
}