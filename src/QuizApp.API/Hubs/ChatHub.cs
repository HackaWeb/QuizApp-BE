using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace QuizApp.API.Hubs;

public class ChatMessage
{
    public string UserId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}

public class ChatHub : Hub
{
    private static readonly ConcurrentQueue<ChatMessage> ChatHistory = new();

    public async Task SendMessage(string userId, string message)
    {
        var chatMessage = new ChatMessage
        {
            UserId = userId,
            Message = message,
            Timestamp = DateTime.UtcNow
        };

        ChatHistory.Enqueue(chatMessage);
        while (ChatHistory.Count > 50)
        {
            ChatHistory.TryDequeue(out _);
        }

        await Clients.All.SendAsync("ReceiveMessage", chatMessage);
    }

    public async Task GetChatHistory()
    {
        await Clients.Caller.SendAsync("ChatHistory", ChatHistory.ToList());
    }

    public override async Task OnConnectedAsync()
    {
        await GetChatHistory();
        await base.OnConnectedAsync();
    }
}
