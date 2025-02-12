using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using QuizApp.Domain.Models;
using System.Collections.Concurrent;

namespace QuizApp.API.Hubs;

public class ChatMessage
{
    public string? NickName { get; set; } = null;
    public string? AvatarUrl { get; set; } = null;
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}

public class ChatHub(UserManager<User> userManager) : Hub
{
    private static readonly ConcurrentQueue<ChatMessage> ChatHistory = new();

    public async Task SendMessage(string userId, string message)
    {
        var user = await userManager.FindByIdAsync(userId);
        var chatMessage = new ChatMessage
        {
            NickName = $"{user.FirstName} {user.LastName}",
            AvatarUrl = user.AvatarUrl,
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
