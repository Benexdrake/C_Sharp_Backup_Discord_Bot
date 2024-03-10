using DiscordBackup.Bot.DL;

namespace DiscordBackup.Bot.Events;

public class JoinedHandler(IServiceProvider Services, IConfiguration Config, ILogger<BackupBot> Logger)
{
	private readonly JoinedLogic _joinedLogic = Services.GetRequiredService<JoinedLogic>();
	public async Task UserJoined(SocketGuildUser arg)
	{
		await _joinedLogic.UserJoined(arg);
	}

	public async Task JoinedGuild(SocketGuild guild)
	{
		await _joinedLogic.JoinedGuild(guild);
	}
}
