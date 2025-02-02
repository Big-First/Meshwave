using Core;
using Models;
using Xunit;

namespace Meshwave.Testes;

public class NodeOperationTeste
{
    public NodeOperationTeste(){}
    [Fact]
    public void ValidarNode()
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
    public void ExecutarNode()
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
    public void CriarNode()
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
    public void RegistrarNode()
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