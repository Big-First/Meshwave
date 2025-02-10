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
    public byte[] publicKey { get; set; }
    [Key(2)]
    public byte[] privateKey { get; set; }
    [Key(3)]
    public SmartContract contract { get; set; }
    [Key(4)]
    public RequestCode requestCode { get; set; }
    [Key(5)]
    public ActionCode actionCode { get; set; }
    [Key(6)]
    public string previousHash { get; set; }
    [Key(7)]
    public Block block { get; set; }

    public ContractValidationRequest(string userId,byte[] publicKey, byte[] privateKey,SmartContract contract, RequestCode requestCode, ActionCode actionCode, string previousHash, Block block)
    {
        this.userId = userId;
        this.publicKey = publicKey;
        this.privateKey = privateKey;
        this.contract = contract;
        this.requestCode = requestCode;
        this.actionCode = actionCode;
        this.previousHash = previousHash;
        this.block = block;
    }
}