﻿using System.Text.Json;
using Data;
using Models;

namespace Core;

public class Wavechain
{
    public Block root { get; set; }
    public int difficulty; // Dificuldade da mineração
    Node node { get; set; }

    public Wavechain(Node node)
    {
        this.node = node;
        root = null;
    }

    public Wavechain(int difficulty)
    {
        this.difficulty = difficulty;
    }

    private Queue<Block> _levelOrderQueue = new Queue<Block>();

    
    public async void WavechainPersistence(string id)
    {
        /*
        if(!string.IsNullOrWhiteSpace(id))root = await _server._meshwavePersistence.LoadBlockchain(id);
        _server._meshwavePersistence.SaveBlockchain(null);
        if (root == null)
        {
            Insert(root);
            _server._meshwavePersistence.SaveBlockchain(null);
        }
        */
    }
    
    // Inserção na árvore binária
    public void Insert( Block block)
    {
        Block newNode = block;
        if (root == null)
        {
            root = newNode;
        }
        else
        {
            // Verificar se há um espaço disponível na árvore
            bool inserted = false;

            // Percorrer todos os níveis e verificar onde existe espaço
            var currentLevelNodes = new Queue<Block>();
            currentLevelNodes.Enqueue(root);

            while (currentLevelNodes.Count > 0 && !inserted)
            {
                int levelNodeCount = currentLevelNodes.Count;
                var nextLevelNodes = new Queue<Block>();

                for (int i = 0; i < levelNodeCount; i++)
                {
                    var node = currentLevelNodes.Dequeue();

                    // Verifica se há espaço no nó atual
                    if (node.left == null)
                    {
                        node.left = newNode; // Inserir no filho esquerdo
                        inserted = true;
                        break;
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
    
    public Block GetBlock()
    {
        if (root == null)
        {
            return null;
        }
        else
        {
            // Verificar se há um espaço disponível na árvore
            bool inserted = false;

            // Percorrer todos os níveis e verificar onde existe espaço
            var currentLevelNodes = new Queue<Block>();
            currentLevelNodes.Enqueue(root);

            while (currentLevelNodes.Count > 0 && !inserted)
            {
                int levelNodeCount = currentLevelNodes.Count;
                var nextLevelNodes = new Queue<Block>();

                for (int i = 0; i < levelNodeCount; i++)
                {
                    var node = currentLevelNodes.Dequeue();

                    // Verifica se há espaço no nó atual
                    if (node.left == null)
                    {
                        return node;
                    }
                    else if (node.right == null)
                    {
                        return node.left;
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
        }

        return null;
    }

    private bool AddBlock(SmartContract smartContract)
    {
        var newBlock = new Block(0, DateTime.UtcNow,FindLastHash(root), "", new List<Transaction>(), new byte[0]);
        return AddToTree(root, newBlock);
    }
    private bool AddToTree(Block parent, Block newBlock)
    {
        if (parent.left == null)
        {
            parent.left = newBlock;
            return true;
        }

        if (parent.right == null)
        {
            parent.right = newBlock;
            return true;
        }

        // Recursively add to the left and right subtrees
        var addedToLeft = AddToTree(parent.left, newBlock);
        if (!addedToLeft)
        {
            return AddToTree(parent.right, newBlock);
        }

        return addedToLeft;
    }
    private string FindLastHash(Block current)
    {
        if (current == null) return "";

        if (current.left == null || current.right == null)
            return current.hash;

        // Traverse to the deepest right-most node
        return FindLastHash(current.right);
    }

    public void MineAndAddBlock(SmartContract smartContract, Node node)
    {
        if (AddBlock(smartContract))
        {
            Console.WriteLine($"{node.userId}: Block mined and added!");
            var newBlock = GetLastBlock();
            var message = JsonSerializer.Serialize(new { type = "NEW_BLOCK", block = newBlock, smartContract });
            //_server.BroadcastMessage(message, null);
        }
    }
    public  List<Block> GetLastBlock()
    {
        var lastBlocks = new List<Block>();
        return lastBlocks;
    }
    public void PrintTree()
    {
        PrintNode(root, 0);
    }
        
    private void PrintNode(Block node, int level)
    {
        if (node == null) return;

        Console.WriteLine(new string(' ', level * 4) + node);
        PrintNode(node.left, level + 1);
        PrintNode(node.right, level + 1);
    }
    
    // Método para percorrer a árvore e adicionar nós não preenchidos na fila
    public void AddUnfilledNodesToQueue()
    {
        // Faremos uma travessia por nível para encontrar os nós da última altura
        if (root == null) return;

        var currentLevelNodes = new Queue<Block>();
        currentLevelNodes.Enqueue(root);

        // Listar todos os nós da última altura
        while (currentLevelNodes.Count > 0)
        {
            int levelNodeCount = currentLevelNodes.Count;
            var nextLevelNodes = new Queue<Block>();

            for (int i = 0; i < levelNodeCount; i++)
            {
                var node = currentLevelNodes.Dequeue();

                // Adiciona filhos ao próximo nível se existirem
                if (node.left != null)
                {
                    nextLevelNodes.Enqueue(node.left);
                }
                if (node.right != null)
                {
                    nextLevelNodes.Enqueue(node.right);
                }
            }

            // Se o próximo nível tiver nós, significa que ainda não atingimos a última altura
            if (nextLevelNodes.Count > 0)
            {
                currentLevelNodes = nextLevelNodes;
            }
            else
            {
                // Chegamos à última altura da árvore
                // Agora verificamos os nós da última altura e adicionamos os não preenchidos à fila
                foreach (var node in currentLevelNodes)
                {
                    if (node.left == null || node.right == null)
                    {
                        // Se o nó da última altura tem filhos não preenchidos, adiciona à fila
                        if (node.left == null)
                        {
                            _levelOrderQueue.Enqueue(node.left);
                        }
                        if (node.right == null)
                        {
                            _levelOrderQueue.Enqueue(node.right);
                        }
                    }
                }
                break;
            }
        }
    }
    
    public int GetHeight()
    {
        return GetHeightOfNode(root);
    }

    private int GetHeightOfNode(Block block)
    {
        if (block == null)
        {
            return 0;
        }

        int leftHeight = GetHeightOfNode(block.left);
        int rightHeight = GetHeightOfNode(block.right);

        return Math.Max(leftHeight, rightHeight) + 1;
    }

    // Buscar um nó por UserId
    public Block Search(int index)
    {
        return SearchNode(root, index);
    }

    private Block SearchNode(Block block, int index)
    {
        if (block == null || block.index == index)
        {
            return block;
        }

        if (index != block.index){
            return SearchNode(block.left, index);
        }
        else
        {
            return SearchNode(block.right, index);
        }
    }
}