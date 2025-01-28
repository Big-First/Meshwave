using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Models;

public class Transaction
{
    public Transaction(){}
    public Guid index { get; set; }
    public string sender { get; set; }
    public string receiver { get; set; }
    public string data { get; set; }
    public decimal amount { get; set; }
    public string hash { get; set; }
    public SmartContract? contract { get; set; } // Contrato inteligente (opcional)
    public DateTime timestamp { get; set; }

    public Transaction(Guid index,string sender, string receiver, decimal amount, DateTime timestamp, SmartContract? contract = null)
    {
        this.index = index;
        this.sender = sender;
        this.receiver = receiver;
        this.data = $"{("").Replace("-", "")}{("").Replace("-", "")}{index}{timestamp}";
        this.amount = amount;
        this.contract = contract;
        this.hash = $"{"0x00"}{CalculateHash()}";
        this.timestamp = timestamp;
    }
    public string CalculateHash()
    {
        var secretKey = index.ToString(); // Deve ter pelo menos 16 caracteres
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Informações do token (claims)
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, $"{timestamp}"), // Identificação do usuário
            new Claim(JwtRegisteredClaimNames.Email, data), // Email
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // ID único
        };

        // Configuração do token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1), // Expira em 1 hora
            SigningCredentials = credentials
        };

        // Gerar o token
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }
}