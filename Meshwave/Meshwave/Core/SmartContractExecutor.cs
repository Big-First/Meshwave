﻿using System.Diagnostics;
using Models;
using Xunit.Abstractions;

namespace Core;

public class SmartContractExecutor
{
    Server _server { get; set; }
    long ticks;
    double nanosegundos;

    public SmartContractExecutor(Server server)
        => _server = server;
    
    readonly ITestOutputHelper _output;

    public async Task<string> ExecuteAsync(SmartContract contract, SmartContractContext context)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        if(contract == null) return $"failed: {405}";
        stopwatch.Stop();
        ticks = stopwatch.ElapsedTicks;
        nanosegundos = (ticks / (double)Stopwatch.Frequency) * 1_000_000;
        //Console.WriteLine($"valida a configured in {this.nanosegundos} nns");
        stopwatch.Restart();
        try
        {
            if (!_server._smartContractValidator.IsValid(contract.code, out var validationError))
            {
                Console.WriteLine($"Contract validation failed: {validationError}");
                return $"Contract validation failed: {validationError}";
            }
            stopwatch.Stop();
            ticks = stopwatch.ElapsedTicks;
            nanosegundos = (ticks / (double)Stopwatch.Frequency) * 1_000_000;
            //Console.WriteLine($"valida a contrato in {nanosegundos} nns");
            stopwatch.Restart();
            var result = await context.OnContractContext(contract, context);
            contract.code = result;
            stopwatch.Stop();
            ticks = stopwatch.ElapsedTicks;
            nanosegundos = (ticks / (double)Stopwatch.Frequency) * 1_000_000;
            //Console.WriteLine($"executa o contrato in {nanosegundos} nns");
            stopwatch.Restart();
            _server.CallValidator(contract);
            stopwatch.Stop();
            ticks = stopwatch.ElapsedTicks;
            nanosegundos = (ticks / (double)Stopwatch.Frequency) * 1_000_000;
            //Console.WriteLine($"salve contract in {nanosegundos} nns");
            return result;
        }
        catch (Exception ex)
        {
            return $"An error occurred during contract execution: {ex.Message}";
        }
    }
}