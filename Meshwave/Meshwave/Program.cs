using System.Text.Json;
using Core;
using Data;
using Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner
builder.Services.AddSingleton<Server>();
builder.Services.AddHostedService<UpdateService>();
builder.Services.AddHostedService<BlockListenerService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuração do ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware para WebSockets
app.UseWebSockets();

// Instância do servidor
var server = app.Services.GetRequiredService<Server>();

// Endpoint WebSocket (Separado do HTTP)
app.MapGet("/input", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var newNode = new Node(Guid.NewGuid(), webSocket, DateTime.Now, server);
        server.Insert(newNode);
        await newNode.SendWelcomeMessage(webSocket);
        await newNode.Echo(context, webSocket);
    }
    else
    {
        context.Response.StatusCode = 400;
    }
    server.Update();
});

app.MapGet("/output", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var newNode = new Node(Guid.NewGuid(), webSocket, DateTime.Now, server);
        server.Insert(newNode);
        await newNode.SendWelcomeMessage(webSocket);
        await newNode.Echo(context, webSocket);
    }
    else
    {
        context.Response.StatusCode = 400;
    }
    server.Update();
});

// Endpoints HTTP
app.MapGet("/status", () =>
{
    return Results.Ok($"Meshwave Service Is Running ... ! {DateTime.Now}");
});

app.MapGet("/createWallet", () =>
{
    return Results.Ok(new Wallet());
});

app.MapPost("/MakeCriptoCoin", ([FromBody] CriptoChain coin) =>
{
    return Results.Ok(coin);
});

app.MapGet("/showTree", () =>
{
    return Results.Ok(server.root);
});

app.MapGet("/showBlockchain", async (Guid index) =>
{
    var blockchain = await server.LoadBlockchain(index);
    return Results.Ok(blockchain);
});

app.MapPost("/SmartContractTeste", async ([FromBody] SmartContract _user) =>
{
    var contract = new SmartContract(_user.contractId, _user.code, _user.creator, _user.createdAt);
    return Results.Ok(await ContractService(server, contract));
});

app.MapGet("/showSmartContract", async (string id) =>
{
    var contract = server._meshwavePersistence.GetContract(id);
    return Results.Ok(contract);
});

app.MapGet("/showBlock", async (string index) =>
{
    var block = await server._meshwavePersistence.LoadBlockchain(index);
    return Results.Ok(block);
});

// Método separado para Smart Contract
async Task<object> ContractService(Server server, SmartContract contract)
{
    return await server.ExecuteAsync(contract, new SmartContractContext(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 1));
}

app.Run();
