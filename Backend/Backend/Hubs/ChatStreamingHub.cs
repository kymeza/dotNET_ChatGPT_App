using System.ClientModel;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using Backend.Models;
using Backend.Models.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using OpenAI.Chat;

namespace Backend.Hubs;

[Authorize("RequireChatPermission")]
public class ChatStreamingHub : Hub
{
    private static ConcurrentDictionary<string, ChatClient> _clients = new();
    private static ConcurrentDictionary<string, List<ChatEntry>> _chatHistory = new();

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
    
    public async Task CreateClient(string? model)
    {
        if (string.IsNullOrWhiteSpace(model))
        {
            model = "gpt-3.5-turbo";
        }
        string clientId = Context.ConnectionId;
        var gptInstance = new ChatClient(apiKey: _openAiSettings.ApiKey, model: model);
        
        // Store the instance for this connection
        _clients.TryAdd(clientId, gptInstance);
        _chatHistory.TryAdd(clientId, new List<ChatEntry>());

        Console.WriteLine($"Client {clientId} created a ChatGPT instance.");
    }
    
    public async IAsyncEnumerable<string> SendCompletion(string message)
    {
        string clientId = Context.ConnectionId;

        if (_clients.TryGetValue(clientId, out var gptInstance))
        {
            // Get or create the chat history for the client using ConcurrentBag
            var chatHistory = _chatHistory.GetOrAdd(clientId, new List<ChatEntry>());
            
            // Store the user message in the chat history
            var userMessage = new ChatEntry
            {
                Sender = ChatGptRoles.User,
                Content = message
            };
            chatHistory.Add(userMessage);

            var serializedChat = JsonSerializer.Serialize(chatHistory);
            Console.WriteLine(serializedChat);

            // Start streaming chat completion
            AsyncCollectionResult<StreamingChatCompletionUpdate> updates = gptInstance.CompleteChatStreamingAsync(serializedChat);

            // StringBuilder to accumulate the chunks
            StringBuilder accumulatedMessage = new StringBuilder();

            await foreach (StreamingChatCompletionUpdate update in updates)
            {
                foreach (ChatMessageContentPart updatePart in update.ContentUpdate)
                {
                    // Append the chunk to the accumulated message
                    accumulatedMessage.Append(updatePart.Text);

                    // Yield the chunk back to the client
                    yield return updatePart.Text;
                }
            }

            // After streaming is complete, add the full message to the chat history
            var fullChatMessage = accumulatedMessage.ToString();
            
            var assistantMessage = new ChatEntry
            {
                Sender = ChatGptRoles.Assistant,
                Content = fullChatMessage
            };
            
            chatHistory.Add(assistantMessage);
            serializedChat = JsonSerializer.Serialize(chatHistory);
            Console.WriteLine(serializedChat);
        }
    }

    
    // Called when a client disconnects
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        string clientId = Context.ConnectionId;

        _clients.Remove(clientId, out _);
        _chatHistory.Remove(clientId, out _);

        Console.WriteLine($"Client {clientId} disconnected.");

        await base.OnDisconnectedAsync(exception);
    }
}