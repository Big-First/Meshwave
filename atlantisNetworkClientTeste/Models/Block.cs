
using System.Security.Cryptography;
using System.Text;

namespace Models;

public class Block
{
    public Block(){}
    public Guid index { get; set; }
    public DateTime timestamp { get; set; }
    public string data { get; set; }
    public string code { get; set; }
    public string previousHash { get; set; }
    public string hash { get; set; }
    public Block left { get; set; }
    public Block right { get; set; }
    

    public Block(Guid index, DateTime timestamp, string previousHash, string code, string data)
    {
        this.index = index;
        this.timestamp = timestamp;
        this.code = code;
        this.data = data;
        this.previousHash = previousHash;
        hash = CalculateHash();
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
    public string CalculateHash()
    {
        string blockData = $"{"0x00"}{index}{timestamp}{data}{previousHash}";
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(blockData);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}