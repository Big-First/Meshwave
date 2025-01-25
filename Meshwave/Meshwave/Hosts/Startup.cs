using System.Text.Json;
using System.Text.Json.Serialization;
using Core;

namespace Hosts;

public class Startup
{
    NodeBinaryTree nodeTree = new NodeBinaryTree();
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
                    var newNode =  nodeTree.Insert(Guid.NewGuid().ToString(),webSocket);
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
                return $"{JsonSerializer.Serialize(nodeTree)}";
            });
        });
    }
}