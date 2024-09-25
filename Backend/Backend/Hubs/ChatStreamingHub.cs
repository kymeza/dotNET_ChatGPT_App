using System.ClientModel;
using System.Collections.Concurrent;
using System.Threading.Channels;
using Backend.Models.Config;
using Microsoft.AspNetCore.SignalR;
using OpenAI.Chat;

namespace Backend.Hubs;

public class ChatStreamingHub : Hub
{
    private static ConcurrentDictionary<string, ChatClient> _clients = new();

    private OpenAiSettings _openAiSettings;

    public ChatStreamingHub(OpenAiSettings openAiSettings)
    {
        _openAiSettings = openAiSettings;
    }

    // Called when a client connects
    public override async Task OnConnectedAsync()
    {
        string clientId = Context.ConnectionId;
        Console.WriteLine($"Client {clientId} connected.");
    }
    
    public async Task CreateClient(string model = "gpt-3.5-turbo")
    {
        string clientId = Context.ConnectionId;
        var gptInstance = new ChatClient(apiKey: _openAiSettings.ApiKey, model: model);
        
        // Store the instance for this connection
        _clients.TryAdd(clientId, gptInstance);

        Console.WriteLine($"Client {clientId} created a ChatGPT instance.");
    }
    
    public async IAsyncEnumerable<string> SendCompletion(string message)
    {
        string clientId = Context.ConnectionId;

        if (_clients.TryGetValue(clientId, out var gptInstance))
        {
            AsyncCollectionResult<StreamingChatCompletionUpdate> updates = gptInstance.CompleteChatStreamingAsync(message);
            await foreach (StreamingChatCompletionUpdate update in updates)
            {
                foreach (ChatMessageContentPart updatePart in update.ContentUpdate)
                {
                    yield return updatePart.Text;
                }
            }
        }
    }
    
    // Called when a client disconnects
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        string clientId = Context.ConnectionId;

        // Remove the instance for this connection
        if (_clients.TryRemove(clientId, out var gptInstance))
        {
            // Dispose of the client if necessary
            if (gptInstance is IDisposable disposableClient)
            {
                disposableClient.Dispose();
            }
        }

        Console.WriteLine($"Client {clientId} disconnected.");

        await base.OnDisconnectedAsync(exception);
    }
}