using Discord.Interactions;

namespace DiscordBackupBot.Commands.Slash;

public class BackupCommand(IServiceProvider service, IConfiguration conf, ILogger<BackupCommand> Logger)
	: InteractionModuleBase<SocketInteractionContext>
{
	private readonly DiscordSocketClient _client = service.GetRequiredService<DiscordSocketClient>();

	public async Task Build()
	{
		foreach(var g in _client.Guilds)
		{
			await g.DeleteApplicationCommandsAsync();

			var backup = new SlashCommandBuilder()
					.WithName("backup")
					.WithDescription("backup service")
					.AddOption(new SlashCommandOptionBuilder().WithName("on").WithDescription("backup on for channel").WithType(ApplicationCommandOptionType.SubCommand))
					.AddOption(new SlashCommandOptionBuilder().WithName("off").WithDescription("backup off for channel").WithType(ApplicationCommandOptionType.SubCommand));
			await g.CreateApplicationCommandAsync(backup.Build());
			Logger.LogInformation("Slash Command Backup");
		}
	}

	[SlashCommand("backup", "backup", true, RunMode.Async)]
	public async Task Backup()
	{
		await ReplyAsync("BACK UP!");
	}
}
