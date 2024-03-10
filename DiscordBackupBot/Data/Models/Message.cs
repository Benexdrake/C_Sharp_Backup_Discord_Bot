namespace DiscordBackup.Bot.Data.Models;

public class Message
{
    public int Id { get; set; }
    public Channel Channel { get; set; } = null!;
    public string Content { get; set; } = string.Empty;
    public bool HasFiles { get; set; }
}
