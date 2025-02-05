using System.Security.Cryptography;
using System.Text;
using MessagePack;

namespace Models;

[MessagePackObject]
public class Block
{
    public Block(){}
    [Key(0)]
    public int index { get; set; }
    [Key(1)]
    public DateTime timestamp { get; set; }
    [Key(2)]
    public byte[] data { get; set; }
    [Key(3)]
    public List<Transaction> transactions { get; set; }
    [Key(4)]
    public string code { get; set; }
    [Key(5)]
    public string previousHash { get; set; }
    [Key(6)]
    public string hash { get; set; }
    [Key(7)]
    public  Block left { get; set; }
    [Key(8)]
    public Block right { get; set; }
    

    public Block(int index, DateTime timestamp, string previousHash, string code, List<Transaction> transactions, byte[] contract)
    {
        this.index = index;
        this.timestamp = timestamp;
        this.data = contract;
        this.code = code;
        this.transactions = transactions;
        this.previousHash = previousHash;
    }
    public string CalculateData()
    {
        string blockData = $"{index}{timestamp}";
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(blockData);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }

    public bool IsValid(Block currentBlock, Block previousBlock)
        => currentBlock.previousHash == previousBlock.hash;
}