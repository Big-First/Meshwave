using Core;
using Enums;
using MessagePack;

namespace Models;

[MessagePackObject]
public class ContractValidationRequest
{
    public ContractValidationRequest(){}
    [Key(0)]
    public string userId { get; set; }
    [Key(1)]
    public SmartContract contract { get; set; }
    [Key(2)]
    public RequestCode requestCode { get; set; }
    [Key(3)]
    public ActionCode actionCode { get; set; }
    [Key(4)]
    public byte[] previousHash { get; set; }
    [Key(5)]
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