﻿using Core;
using Models;

namespace Data;

public class MeshwavePersistence
{
    Queue<Block> _blocks = new Queue<Block>();
    Queue<SmartContract> _smartContracts = new Queue<SmartContract>();
    SemaphoreSlim _signal = new SemaphoreSlim(0);
    Server _server { get; set; }

    public MeshwavePersistence(Server server)
        => _server = server;

    public async Task SaveBlockchain(Block block)
    {
        _blocks.Enqueue(block);
        _signal.Release();
    }

    private async Task<Block> GetGenesys(string Id)
    {
        var _redisService = new RedisService();
        var block =await _redisService.GetObjectAsync<Block>(Id);
        return block;
    }

    public async Task SaveContract(SmartContract _contract)
    {
        var contract = await GetContract(_contract.contractId.ToString());
        if (contract == null)
        {
            _smartContracts.Enqueue(contract);
            _signal.Release();
        }
    }

    public async Task<Block?> LoadBlockchain(string Id)
    {
        var _redisService = new RedisService();
        var user = await _redisService.GetObjectAsync<Block>(Id);
        return user;
    }
    
    public async Task<SmartContract?> GetContract(string Id)
    {
        var _redisService = new RedisService();
        var contract =await _redisService.GetObjectAsync<SmartContract>(Id);
        return contract;
    }
}