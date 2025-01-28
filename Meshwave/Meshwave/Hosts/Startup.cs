using System.Text.Json;
using Core;
using Data;
using Models;
using Meshwave.Singletons;
using Microsoft.AspNetCore.Mvc;

namespace Hosts;

public class Startup
{
    private Server _server = new Server();
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseRouting();

        app.UseWebSockets();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", async context =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    var newNode =  _server._nodeTree.Insert(Guid.NewGuid().ToString(),webSocket);
                    await newNode.SendWelcomeMessage(webSocket);
                    await newNode.Echo(context, webSocket);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            });
            
            endpoints.MapGet("/status", async () =>
            {
                return $"Meshwave Service Is Running ... ! {DateTime.Now}";
            });
            
            endpoints.MapGet("/showTree", async () =>
            {
                return $"{JsonSerializer.Serialize(_server._nodeTree)}";
            });
            endpoints.MapGet("/createBlock", async () =>
            {
                _server._wavechain.Insert(new Block());
                return"200";
            });
            endpoints.MapGet("/showBlocksTree", async () =>
            {
                return _server._wavechain;
            });
            endpoints.MapPost("/SmartContractTeste", async ([FromBody] SmartContract _user) =>
            {
                var contract = new SmartContract(_user.contractId, _user.code, _user.creator, _user.createdAt);
                return await ContractService(contract);

            });
            endpoints.MapGet("/showSmartContract", async (string id) =>
            {
                var contract = _server._meshwavePersistence.GetContract(id);
                return contract;
            });
            endpoints.MapGet("/showBlock", async (string index) =>
            {
                return await _server._meshwavePersistence.LoadBlockchain(index);
            });
        });
    }

    private async Task<object> ContractService(SmartContract contract)
    => await _server.ExecuteAsync(contract, new SmartContractContext(Guid.NewGuid().ToString(),Guid.NewGuid().ToString(), 1));
}