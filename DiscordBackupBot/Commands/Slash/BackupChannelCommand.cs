namespace DiscordBackup.Bot.Commands.Slash;

public class BackupChannelCommand(IServiceProvider Service, IConfiguration Config, ILogger<BackupChannelCommand> Logger)
{
	private readonly DiscordSocketClient _client = Service.GetRequiredService<DiscordSocketClient>();

	public async Task Build()
	{
		foreach(var g in _client.Guilds)
		{
			//await g.DeleteApplicationCommandsAsync();

			var backup = new SlashCommandBuilder()
					.WithName("backup")
					.WithDescription("backup service for channel")
					.AddOption(new SlashCommandOptionBuilder().WithName("on").WithDescription("backup on for channel").WithType(ApplicationCommandOptionType.SubCommand))
					.AddOption(new SlashCommandOptionBuilder().WithName("off").WithDescription("backup off for channel").WithType(ApplicationCommandOptionType.SubCommand));
			await g.CreateApplicationCommandAsync(backup.Build());
			Logger.LogInformation("Slash Command Backup");
		}
	}
}
