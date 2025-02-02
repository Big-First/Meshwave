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
    public byte[] data { get; set; }
    [Key(3)]
    public string code { get; set; }
    [Key(4)]
    public byte[] previousHash { get; set; }
    [Key(5)]
    public byte[] hash { get; set; }
    [Key(6)]
    public  Block left { get; set; }
    [Key(7)]
    public Block right { get; set; }
    

    public Block(Guid index, DateTime timestamp, byte[] previousHash, string code, byte[] contract)
    {
        this.index = index;
        this.timestamp = timestamp;
        this.data = contract;
        this.code = code;
        this.previousHash = previousHash;
        hash = CalculateHash(previousHash);
    }
    
    public byte[] CalculateHash(byte[] contract)
    {
        using var sha256 = SHA256.Create();
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);

        bw.Write((byte)0x00); // Prefixo para evitar ambiguidade
        bw.Write(index.ToString());
        bw.Write(timestamp.ToString());
        bw.Write(contract ?? Array.Empty<byte>());

        return sha256.ComputeHash(ms.ToArray());
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