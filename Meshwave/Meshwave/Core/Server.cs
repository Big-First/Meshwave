using Data;
using Models;

namespace Core;

public class Server
{
    
    public Queue<Block> _blocks = new ();
    public Queue<SmartContract> _smartContracts = new ();
    public SemaphoreSlim _signal = new SemaphoreSlim(0);
    public SmartContractValidator _smartContractValidator { get; set; }
    public SmartContractExecutor _smartContractExecutor { get; set; }
    public SmartContractController _smartContractController { get; set; }
    public MeshwavePersistence _meshwavePersistence { get; set; }
    public NodeBinaryTree _nodeTree { get; set; }
    public Wavechain _wavechain { get; set; }

    public Server()
    {
        _smartContractController = new SmartContractController(this);
        _smartContractExecutor = new SmartContractExecutor(this);
        _smartContractValidator = new SmartContractValidator(this);
        _meshwavePersistence = new MeshwavePersistence(this);
        _nodeTree = new NodeBinaryTree(this);
        _wavechain = new Wavechain(this);
    }
    public async Task SaveContract(SmartContract contract)
    => await _meshwavePersistence.SaveContract(contract);

    public void ValidationBlockchain<T>(T newBlock)
    {
        
    }

    public void ValidationOperation(SmartContract contract)
    {
        var newBlock = _wavechain.Insert(new Block(Guid.NewGuid(), DateTime.Now, contract.hash));
        ValidationBlockchain<Block>(newBlock);
        SaveContract(contract);
    }

    public async Task<object> ExecuteAsync(SmartContract contract, SmartContractContext smartContractContext)
    => await _smartContractExecutor.ExecuteAsync(contract, smartContractContext);
}