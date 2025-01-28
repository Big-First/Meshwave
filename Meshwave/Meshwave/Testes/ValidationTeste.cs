using Core;
using Models;
using Xunit;

namespace Meshwave.Testes;

public class ValidationTeste
{
    public ValidationTeste(){}
    
    [Fact]
    public void ValidarContrato()
    {
        var server = new Server();
        var contrato = new SmartContract()
        {
            contractId = Guid.NewGuid(),
            code = "Subscribe {amount} from {sender} to {receiver} completed.",
            data = "",
            creator = "",
            createdAt = DateTime.Now
        };
        server.ExecuteAsync(contrato, new SmartContractContext(Guid.NewGuid().ToString(),Guid.NewGuid().ToString(), 1));
    }
    
    [Fact]
    public void ExecutarContrato()
    {
        var server = new Server();
        var contrato = new SmartContract()
        {
            contractId = Guid.NewGuid(),
            code = "Subscribe {amount} from {sender} to {receiver} completed.",
            data = "",
            creator = "",
            createdAt = DateTime.Now
        };
        server.ExecuteAsync(contrato, new SmartContractContext(Guid.NewGuid().ToString(),Guid.NewGuid().ToString(), 1));
    }
    
    [Fact]
    public void CriarContrato()
    {
        var server = new Server();
        var contrato = new SmartContract()
        {
            contractId = Guid.NewGuid(),
            code = "Subscribe {amount} from {sender} to {receiver} completed.",
            data = "",
            creator = "",
            createdAt = DateTime.Now
        };
        server.ExecuteAsync(contrato, new SmartContractContext(Guid.NewGuid().ToString(),Guid.NewGuid().ToString(), 1));
    }
    [Fact]
    public void RecuperarContrato()
    {
        var server = new Server();
        var contrato = new SmartContract()
        {
            contractId = Guid.NewGuid(),
            code = "Subscribe {amount} from {sender} to {receiver} completed.",
            data = "",
            creator = "",
            createdAt = DateTime.Now
        };
        server.ExecuteAsync(contrato, new SmartContractContext(Guid.NewGuid().ToString(),Guid.NewGuid().ToString(), 1));
    }
}