using System.Security.Cryptography;
using System.Text;
using MessagePack;

namespace Models;

[MessagePackObject]
public class SmartContract
{
    public SmartContract(){}
    [Key(0)]
    public Guid contractId { get; set; }
    [Key(1)]
    public string code { get; set; }
    [Key(2)]
    public string data { get; set; }
    [Key(3)]
    public byte[] hash { get; set; }
    [Key(4)]
    public string creator { get; set; }
    [Key(5)]
    public DateTime createdAt { get; set; }

    public SmartContract(Guid contractId, string code, string creator, DateTime createdAt)
    {
        this.contractId = contractId;
        this.code = code;
        this.data = $"{(CalculateData()).Replace("-", "")}{("").Replace("-", "")}{createdAt}";
        this.hash = CalculateHash();
        this.creator = creator;
        this.createdAt = createdAt;
    }
    
    public string CalculateData()
    {
        string blockData = $"{contractId}{createdAt}";
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(blockData);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
    public byte[] CalculateHash()
    {
        using (var sha256 = SHA256.Create())
        {
            var input = $"{contractId}{createdAt}{Convert.ToBase64String(null ?? new byte[0])}{data}";
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        }
    }
}