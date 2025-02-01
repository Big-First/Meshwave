using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Models;

public class Transaction
{
    public Transaction(){}
    public Guid index { get; set; }
    public string sender { get; set; }
    public string receiver { get; set; }
    public byte[] data { get; set; }
    public decimal amount { get; set; }
    public byte[] hash { get; set; }
    public SmartContract? contract { get; set; }
    public DateTime timestamp { get; set; }

    public Transaction(Guid index,string sender, string receiver, decimal amount, DateTime timestamp, SmartContract? contract = null)
    {
        this.index = index;
        this.sender = sender;
        this.receiver = receiver;
        this.data = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes($"{BitConverter.ToString(contract.hash ?? new byte[0]).Replace("-", "")}{BitConverter.ToString(contract.hash ?? new byte[0]).Replace("-", "")}{amount}{timestamp}"));
        this.amount = amount;
        this.contract = contract;
        this.hash = CalculateHash();
        this.timestamp = timestamp;
    }
    
    public byte[] CalculateHash()
    {
        using (var sha256 = SHA256.Create())
        {
            var input = $"{"0x00"}{index}{timestamp}{Convert.ToBase64String(contract.hash ?? new byte[0])}{data}";
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        }
    }
}