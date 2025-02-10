using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MessagePack;

namespace Models;

[MessagePackObject]
public class Transaction
{
    public Transaction(){}
    [Key(0)]
    public Guid index { get; set; }
    [Key(1)]
    public string sender { get; set; }
    [Key(2)]
    public string receiver { get; set; }
    [Key(3)]
    public byte[] data { get; set; }
    [Key(4)]
    public decimal amount { get; set; }
    [Key(5)]
    public byte[] hash { get; set; }
    [Key(6)]
    public SmartContract? contract { get; set; }
    [Key(7)]
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