namespace DiscordBackup.Bot.Data.Models;

public class Channel
{
    public ulong Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool hasBackup { get; set; }
}
