using System.Net.WebSockets;

namespace Core;

public class NodeBinaryTree
{
    public Node root { get; set; }
    private Queue<Node> _levelOrderQueue = new Queue<Node>(); // Para garantir a inserção na última posição livre
    
    public NodeBinaryTree()
    {
        root = null;
    }

    // Inserção na árvore binária
    public Node Insert(string userName,WebSocket webSocket)
    {
        var newNode = new Node(userName,webSocket);
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
                    if (node.leftChild == null)
                    {
                        node.leftChild = newNode; // Inserir no filho esquerdo
                        inserted = true;
                        break;
                    }
                    else if (node.rightChild == null)
                    {
                        node.rightChild = newNode; // Inserir no filho direito
                        inserted = true;
                        break;
                    }

                    // Se o nó tem filhos, adicione-os ao próximo nível
                    if (node.leftChild != null)
                    {
                        nextLevelNodes.Enqueue(node.leftChild);
                    }
                    if (node.rightChild != null)
                    {
                        nextLevelNodes.Enqueue(node.rightChild);
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

        return newNode;
    }
    
    // Método para percorrer a árvore e adicionar nós não preenchidos na fila
    public void AddUnfilledNodesToQueue()
    {
        // Faremos uma travessia por nível para encontrar os nós da última altura
        if (root == null) return;

        var currentLevelNodes = new Queue<Node>();
        currentLevelNodes.Enqueue(root);

        // Listar todos os nós da última altura
        while (currentLevelNodes.Count > 0)
        {
            int levelNodeCount = currentLevelNodes.Count;
            var nextLevelNodes = new Queue<Node>();

            for (int i = 0; i < levelNodeCount; i++)
            {
                var node = currentLevelNodes.Dequeue();

                // Adiciona filhos ao próximo nível se existirem
                if (node.leftChild != null)
                {
                    nextLevelNodes.Enqueue(node.leftChild);
                }
                if (node.rightChild != null)
                {
                    nextLevelNodes.Enqueue(node.rightChild);
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
                    if (node.leftChild == null || node.rightChild == null)
                    {
                        // Se o nó da última altura tem filhos não preenchidos, adiciona à fila
                        if (node.leftChild == null)
                        {
                            _levelOrderQueue.Enqueue(node.leftChild);
                        }
                        if (node.rightChild == null)
                        {
                            _levelOrderQueue.Enqueue(node.rightChild);
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

    private int GetHeightOfNode(Node node)
    {
        if (node == null)
        {
            return 0;
        }

        int leftHeight = GetHeightOfNode(node.leftChild);
        int rightHeight = GetHeightOfNode(node.rightChild);

        return Math.Max(leftHeight, rightHeight) + 1;
    }

    // Buscar um nó por UserId
    public Node Search(string userId)
    {
        return SearchNode(root, userId);
    }

    private Node SearchNode(Node node, string userId)
    {
        if (node == null || node.userId == userId)
        {
            return node;
        }

        if (userId != node.userId){
            return SearchNode(node.leftChild, userId);
        }
        else
        {
            return SearchNode(node.rightChild, userId);
        }
    }
}