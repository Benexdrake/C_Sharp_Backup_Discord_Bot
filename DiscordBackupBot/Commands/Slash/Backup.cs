namespace DiscordBackupBot.Commands.Slash;

public class Backup(IServiceProvider service, IConfiguration conf, ILogger<Backup> Logger)
{
	private readonly DiscordSocketClient _client = service.GetRequiredService<DiscordSocketClient>();
	private ulong _guildId = ulong.Parse(conf["GuildID"]);

	public async Task Build()
	{
		var guild = _client.GetGuild(_guildId);
		await guild.DeleteApplicationCommandsAsync();

		var backup = new SlashCommandBuilder()
					.WithName("backup")
					.WithDescription("backup service")
					.AddOption(new SlashCommandOptionBuilder().WithName("choose").WithDescription("choose between true or false").WithType(ApplicationCommandOptionType.Boolean));

		await guild.CreateApplicationCommandAsync(backup.Build());
		Logger.LogInformation("Slash Command Backup");
	}
}
