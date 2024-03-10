namespace DiscordBackup.Bot.Commands.Slash;

public class AddBackupGuildCommand(IServiceProvider Service, IConfiguration Config, ILogger<BackupChannelCommand> Logger)
{
	private readonly DiscordSocketClient _client = Service.GetRequiredService<DiscordSocketClient>();

	public async Task Build()
	{
		foreach(var g in _client.Guilds)
		{
			//await g.DeleteApplicationCommandsAsync();

			var command = new SlashCommandBuilder()
						.WithName("add_backup_server")
						.WithDescription("create or add with id a discord server")
						.AddOption(new SlashCommandOptionBuilder().WithName("id").WithDescription("insert guild id").WithType(ApplicationCommandOptionType.Number));
			await g.CreateApplicationCommandAsync(command.Build());
			Logger.LogInformation("Slash Command Add_Backup_Server");
		}
	}
}
