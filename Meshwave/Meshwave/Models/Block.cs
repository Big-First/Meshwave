using System.Security.Cryptography;
using System.Text;
using MessagePack;

namespace Models;

[MessagePackObject]
public class Block
{
    public Block(){}
    [Key(0)]
    public Guid index { get; set; }
    [Key(1)]
    public DateTime timestamp { get; set; }
    [Key(2)]
    public string data { get; set; }
    [Key(3)]
    public string code { get; set; }
    [Key(4)]
    public string previousHash { get; set; }
    [Key(5)]
    public string hash { get; set; }
    [Key(6)]
    public Block left { get; set; }
    [Key(7)]
    public Block right { get; set; }
    

    public Block(Guid index, DateTime timestamp, string previousHash, string code)
    {
        this.index = index;
        this.timestamp = timestamp;
        data =  $"{(CalculateData()).Replace("-", "")}{("").Replace("-", "")}{timestamp}";
        this.code = code;
        this.previousHash = previousHash;
        hash = CalculateHash(1);
    }
    
    public string CalculateHash(int difficulty)
    {
        var target = new string('0', difficulty);
        string blockData = $"{target}{index}{timestamp}{data}{previousHash}";
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(blockData);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            return Convert.ToBase64String(hashBytes);
        }
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
    public void MineBlock(int difficulty)
    {
        var target = new string('0', difficulty);
        while (!hash.ToString().StartsWith(target))
        {
            CalculateHash(difficulty);
        }
    }
}