using Discord.Interactions;

namespace DiscordBackupBot.Commands.Slash;

public class BackupCommand(IServiceProvider service, IConfiguration conf, ILogger<BackupCommand> Logger)
	: InteractionModuleBase<SocketInteractionContext>
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

	[SlashCommand("backup", "backup", true, RunMode.Async)]
	public async Task Backup()
	{
		await ReplyAsync("BACK UP!");
	}
}
