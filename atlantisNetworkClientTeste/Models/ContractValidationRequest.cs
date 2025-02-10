using Core;
using Enums;

namespace Models;

public class ContractValidationRequest
{
    public ContractValidationRequest(){}
    public string userId { get; set; }
    public SmartContract contract { get; set; }
    public RequestCode requestCode { get; set; }
    public ActionCode actionCode { get; set; }
    public byte[] previousHash { get; set; }
    public Block block { get; set; }

    public ContractValidationRequest(string userId, SmartContract contract, RequestCode requestCode, ActionCode actionCode, byte[] previousHash, Block block)
    {
        this.userId = userId;
        this.contract = contract;
        this.requestCode = requestCode;
        this.actionCode = actionCode;
        this.previousHash = previousHash;
        this.block = block;
    }
}