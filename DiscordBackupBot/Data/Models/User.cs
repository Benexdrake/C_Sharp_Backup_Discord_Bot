namespace DiscordBackup.Bot.Data.Models;

public class User
{
    public int Id { get; set; }
    public bool IsBackup { get; set; }
    public bool IsAdmin { get; set; }
    public ICollection<Message> Messages { get; set; } = [];
}
