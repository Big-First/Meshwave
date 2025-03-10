﻿using System.Data;
using System.Text.Json;
using Data;
using Enums;
using Models;

namespace Core;

public class Server
{
    public List<Node> _nodes = new List<Node>();

    public Queue<ValidationBlock> _blocks = new ();
    public Dictionary<string, Block> blockchain = new ();
    public event EventHandler<ValidationBlock>? BlockAdded;
    
    public Queue<SmartContract> _smartContracts = new ();
    public Node root { get; set; }
    
    private Queue<Node> _levelOrderQueue = new Queue<Node>();
    
    Server _server { get; set; }
    
    public SemaphoreSlim _signal = new SemaphoreSlim(0);
    public SmartContractValidator _smartContractValidator { get; set; }
    public SmartContractExecutor _smartContractExecutor { get; set; }
    public SmartContractController _smartContractController { get; set; }
    public MeshwavePersistence _meshwavePersistence { get; set; }
    public Server()
    {
        _smartContractController = new SmartContractController(this);
        _smartContractExecutor = new SmartContractExecutor(this);
        _smartContractValidator = new SmartContractValidator(this);
        root = null;
    }
    public void AddBlock(ValidationBlock newBlock)
    {
        _blocks.Enqueue(newBlock);
        
        BlockAdded?.Invoke(this, newBlock);
    }
    public void Update()
    {
        
    }
    
    public async void CallValidator(SmartContract contract)
    {
        Node validator = SelectRandomValidator();
        if (validator != null)
        {
            var lastBlock = validator.GetBlock();
            string previusHash = lastBlock == null ? "" : lastBlock.hash; 
            var serial = new ObjectSerialization().Serialize(new ContractValidationRequest(validator.userId.ToString(),new byte[0],new byte[0], contract, RequestCode.Validation, ActionCode.Operation, previusHash, lastBlock));
            await validator.SendData(serial);
        }
        
    }

    public async Task SaveContract(SmartContract contract)
    => await _meshwavePersistence.SaveContract(contract);

    public async Task<object> ExecuteAsync(SmartContract contract, SmartContractContext smartContractContext)
    => await _smartContractExecutor.ExecuteAsync(contract, smartContractContext);

     // Inserção na árvore binária
    public void Insert(Node _node)
    {
        var newNode = _node;
        if (root == null)
        {
            root = newNode;
        }
        else
        {
            // Verificar se há um espaço disponível na árvore
            bool inserted = false;

            // Percorrer todos os níveis e verificar onde existe espaço
            var currentLevelNodes = new Queue<Node>();
            currentLevelNodes.Enqueue(root);

            while (currentLevelNodes.Count > 0 && !inserted)
            {
                int levelNodeCount = currentLevelNodes.Count;
                var nextLevelNodes = new Queue<Node>();

                for (int i = 0; i < levelNodeCount; i++)
                {
                    var node = currentLevelNodes.Dequeue();

                    // Verifica se há espaço no nó atual
                    if (node.left == null)
                    {
                        node.left = newNode; // Inserir no filho esquerdo
                        inserted = true;
                        break;
                        return;
                    }
                    else if (node.right == null)
                    {
                        node.right = newNode; // Inserir no filho direito
                        inserted = true;
                        break;
                    }

                    // Se o nó tem filhos, adicione-os ao próximo nível
                    if (node.left != null)
                    {
                        nextLevelNodes.Enqueue(node.left);
                    }
                    if (node.right != null)
                    {
                        nextLevelNodes.Enqueue(node.right);
                    }
                }

                // Se o nó foi inserido, parar o loop
                if (inserted)
                {
                    break;
                }

                // Caso contrário, continue para o próximo nível
                currentLevelNodes = nextLevelNodes;
            }

            // Enfileira o novo nó para garantir que a próxima inserção ocupe a próxima posição livre
            _levelOrderQueue.Enqueue(newNode);
        }
    }
    
    // Percorre a árvore e retorna um nó aleatório
    public Node SelectRandomValidator()
    {
        var allNodes = new List<Node>();
        Random random = new Random();
        CollectNodes(root, allNodes);

        if (allNodes.Count == 0) return null;

        int index = random.Next(allNodes.Count);
        return allNodes[index];
    }
    public List<Node> SelectAllNodes()
    {
        var allNodes = new List<Node>();
        Random random = new Random();
        CollectNodes(root, allNodes);

        if (allNodes.Count == 0) return allNodes;

        return allNodes;
    }

    // Percorre toda a árvore e coleta os nós
    private void CollectNodes(Node node, List<Node> nodes)
    {
        if (node == null) return;

        nodes.Add(node);

        CollectNodes(node.left, nodes);
        CollectNodes(node.right, nodes);
    }
    
    public int GetHeight()
    {
        return GetHeightOfNode(root);
    }

    private int GetHeightOfNode(Node node)
    {
        if (node == null)
        {
            return 0;
        }

        int leftHeight = GetHeightOfNode(node.left);
        int rightHeight = GetHeightOfNode(node.right);

        return Math.Max(leftHeight, rightHeight) + 1;
    }

    // Buscar um nó por UserId
    public Node Search(Guid userId)
    {
        return SearchNode(root, userId);
    }

    private Node SearchNode(Node node, Guid userId)
    {
        if (node == null || node.userId == userId)
        {
            return node;
        }

        if (userId != node.userId){
            return SearchNode(node.left, userId);
        }
        else
        {
            return SearchNode(node.right, userId);
        }
    }

    public void BroadcastMessage(string message, object o)
    {
        
    }

    public List<Wavechain> GetAllTrees()
    {
        var nodes = SelectAllNodes();
        List<Wavechain> _blocks = new List<Wavechain>();
        nodes.ForEach(b =>
        {
            _blocks.Add(b._wavechain);
        });
        return _blocks;
    }

    public async Task<object?> LoadBlockchain(Guid index)
    {
        var node = _nodes.Find(x => x.userId == index);
        if (node == null) return StatusCodes.Status404NotFound;
        return node.root;
    }
}