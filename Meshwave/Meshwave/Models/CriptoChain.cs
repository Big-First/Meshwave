namespace Models;

public class CriptoChain
{
    public CriptoChain(){}
    public Guid uuid { get; set; }
    public string name { get; set; }
    public string symbol { get; set; }
    public DateTime timestamp { get; set; }
    public int TotalSupply { get; set; }
    public Dictionary<string, int> balances { get; set; }

    public CriptoChain(Guid uuid, string name, string symbol, DateTime timestamp, int totalSupply, Dictionary<string, int> balances)
    {
        this.uuid = uuid;
        this.name = name;
        this.symbol = symbol;
        this.timestamp = timestamp;
        TotalSupply = totalSupply;
        this.balances = balances;
    }
}