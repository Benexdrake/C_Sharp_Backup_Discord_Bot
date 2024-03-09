namespace DiscordBackupBot.Models;

public class Message
{
	public int Id { get; set; }
	public User User { get; set; } = null!;
	public string Content { get; set; } = string.Empty;
	public bool HasFiles { get; set; }
}
