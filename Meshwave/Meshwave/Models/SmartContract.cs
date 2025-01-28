using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Models;

public class SmartContract
{
    public SmartContract(){}
    public Guid contractId { get; set; }
    public string code { get; set; }
    public string data { get; set; }
    public byte[] hash { get; set; }
    public string creator { get; set; }
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
        string blockData = $"{contractId}";
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