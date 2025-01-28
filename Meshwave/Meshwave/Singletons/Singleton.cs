using Core;
using Data;
using Models;

namespace Meshwave.Singletons;

public class Singleton
{
    static Singleton? _instance { get; set; }
    public float stake = 0.1f;
    public Dictionary<string, SmartContract> _inventory = new ();
    public static Singleton? Instance()
    {
        if (_instance == null)_instance = new Singleton();
        return _instance;
    }

}