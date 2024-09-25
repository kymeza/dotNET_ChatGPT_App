using System.Text.Json.Serialization;

namespace Backend.Models;

public class ChatEntry
{
    [JsonPropertyName("role")]
    public string Sender { get; set; } = string.Empty;
    
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}