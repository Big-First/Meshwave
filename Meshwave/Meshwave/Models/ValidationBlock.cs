namespace Models;

public class ValidationBlock
{
    public ValidationBlock(){}
    public string userId { get; set; }
    public Block block { get; set; }

    public ValidationBlock(string userId, Block block)
    {
        this.userId = userId;
        this.block = block;
    }
}