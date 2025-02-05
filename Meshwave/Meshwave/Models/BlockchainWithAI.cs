using Core;

namespace Models;

public class BlockchainWithAI
{
    private ProofOfStake pos;
    private AIagent ai;
    public List<Block> Chain { get; private set; }

    public BlockchainWithAI()
    {
        Chain = new List<Block>();
        //pos = new ProofOfStake();
        ai = new AIagent();
    }

    public void AddStake(string nodeId, decimal amount)
    {
        pos.SetStake(nodeId, amount);
    }

    public void ProcessTransaction(string sender, string receiver, decimal amount)
    {
        ai.RegisterTransaction(sender, amount);
    }

    public void AddBlock(Block newBlock)
    {
        /*
        string suggestedValidator = ai.SuggestValidator(pos.GetStake());

        if (string.IsNullOrEmpty(suggestedValidator))
        {
            Console.WriteLine("❌ Nenhum nó confiável disponível para validação!");
            return;
        }
        Console.WriteLine($"🎯 Nó selecionado pela IA: {suggestedValidator}");
        if (newBlock.IsValid())
        {
            Chain.Add(newBlock);
            Console.WriteLine($"✅ Bloco {newBlock.index} validado por {suggestedValidator}.");
        }
        else
        {
            Console.WriteLine($"❌ Validação falhou. O nó {suggestedValidator} foi rejeitado!");
        }
        */
    }
}