using System.Text.Json;
using Core;
using Data;
using Models;
using Meshwave.Singletons;
using Microsoft.AspNetCore.Mvc;

namespace Hosts;

public class Startup
{
    Server _server = new Server();
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSingleton(_server);
        services.AddHostedService<UpdateService>();
        services.AddHostedService<BlockListenerService>();
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
                    var newNode =  new Node(Guid.NewGuid(),webSocket,DateTime.Now, _server);
                    _server.Insert(newNode);
                    await newNode.SendWelcomeMessage(webSocket);
                    await newNode.Echo(context, webSocket);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
                _server.Update();
            });
            
            endpoints.MapGet("/status", async () =>
            {
                return $"Meshwave Service Is Running ... ! {DateTime.Now}";
            });
            
            endpoints.MapGet("/createWallet", async () =>
            {
                return new Wallet();
            });
            
            endpoints.MapGet("/showTree", async () =>
            {
                return _server.root;
            });
            endpoints.MapGet("/showBlocksTree", async () =>
            {
                return _server.GetAllTrees();
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