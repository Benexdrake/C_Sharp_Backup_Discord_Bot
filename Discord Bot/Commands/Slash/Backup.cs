namespace Discord_Bot.Commands.Slash;
public class Backup
{
	private readonly DiscordSocketClient _client;
	private ulong _guildId;

	public Backup(IServiceProvider service, IConfiguration conf)
    {
        _client = service.GetRequiredService<DiscordSocketClient>();
        _guildId = ulong.Parse(conf["GuildID"]);
	}

    public async Task Build()
    {
		var guild = _client.GetGuild(_guildId);
		await guild.DeleteApplicationCommandsAsync();

		var backup = new SlashCommandBuilder()
					.WithName("backup")
					.WithDescription("backup service")
					.AddOption(new SlashCommandOptionBuilder().WithName("choose").WithDescription("choose between true or false").WithType(ApplicationCommandOptionType.Boolean));
		
		await guild.CreateApplicationCommandAsync(backup.Build());
		Log.Logger.Information("Slash Command Backup");
	}
}
