using Models;

namespace Core;

public class SmartContractContext
{
    public SmartContractContext(){}
    public string sender { get; set; }
    public string receiver { get; set; }
    public decimal amount { get; set; }
    
    public SmartContractContext(string sender, string receiver, decimal amount)
    {
        this.sender = sender;
        this.receiver = receiver;
        this.amount = amount;
    }
    
    public async Task<string> OnContractContext(SmartContract smartContract, SmartContractContext contract)
    {
        sender = contract.sender;
        receiver = contract.receiver;
        amount = contract.amount;
        return await Task.FromResult(ProcessSmartContractCode(smartContract.code));
    }
    
    private string ProcessSmartContractCode(string code)
    {
        return code.Replace("{sender}", sender)
            .Replace("{receiver}", receiver)
            .Replace("{amount}", amount.ToString());
    }
}