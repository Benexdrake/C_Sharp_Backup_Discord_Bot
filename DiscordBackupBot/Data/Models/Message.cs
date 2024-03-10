namespace DiscordBackup.Bot.Data.Models;

public class Message
{
    public ulong Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public ulong ChannelId { get; set; }
    public ulong UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool HasFiles { get; set; }
}
