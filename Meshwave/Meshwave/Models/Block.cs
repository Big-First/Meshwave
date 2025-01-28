using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Models;

public class Block
{
    public Block(){}
    public Guid index { get; set; }
    public DateTime timestamp { get; set; }
    public byte[] data { get; set; }
    public byte[] previousHash { get; set; }
    public byte[] hash { get; set; }
    public int nonce { get; private set; }
    public Block left { get; set; }
    public Block right { get; set; }
    

    public Block(Guid index, DateTime timestamp, byte[] previousHash)
    {
        this.index = index;
        this.timestamp = timestamp;
        data =  SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes($"{BitConverter.ToString(previousHash).Replace("-", "")}{BitConverter.ToString(previousHash).Replace("-", "")}{nonce}{timestamp}"));
        this.previousHash = previousHash;
        hash = CalculateHash();
        nonce = 0;
    }

    public bool IsValid(Block currentBlock, Block previousBlock)
        => currentBlock.previousHash == previousBlock.hash;

    public byte[] CalculateHash()
    {
        using (var sha256 = SHA256.Create())
        {
            var input = $"{"0x00"}{index}{timestamp}{Convert.ToBase64String(previousHash ?? new byte[0])}{data}";
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        }
    }
}