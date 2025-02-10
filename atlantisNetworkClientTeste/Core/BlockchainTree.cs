using Models;

namespace Core;

public class BlockchainTree
    {
        public Block Root { get; set; }
        private int _difficulty;
        public Server server { get; set; }

        public BlockchainTree(int difficulty)
        {
            _difficulty = difficulty;
        }

        private Block CreateGenesisBlock()
        {
            var genesisBlock = new Block(Guid.NewGuid(), DateTime.Now,  "", "", "");
            //genesisBlock.MineBlock(_difficulty);
            return genesisBlock;
        }

        private string FindLastHash(Block current)
        {
            if (current == null) return "";

            if (current.left == null || current.right == null)
                return current.hash;

            // Traverse to the deepest right-most node
            return FindLastHash(current.right);
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

        public void PrintTree()
        {
            PrintNode(Root, 0);
        }

        private void PrintNode(Block node, int level)
        {
            if (node == null) return;

            Console.WriteLine(new string(' ', level * 4) + node);
            PrintNode(node.left, level + 1);
            PrintNode(node.right, level + 1);
        }
    }