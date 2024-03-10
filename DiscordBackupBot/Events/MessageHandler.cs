using DiscordBackup.Bot.DL;

namespace DiscordBackup.Bot.Events;

public class MessageHandler(IServiceProvider Services, IConfiguration Config, ILogger<BackupBot> Logger)
{
	private readonly BackupLogic _backupLogic = Services.GetRequiredService<BackupLogic>();

	public async Task MessageDeleted(Cacheable<IMessage, ulong> arg1, Cacheable<IMessageChannel, ulong> arg2)
	{
		Logger.LogInformation("Message: {arg1} - Deleted", arg1.Id);
	}

	public async Task MessageCreated(SocketMessage arg)
	{
		if (arg.Author.IsBot)
			return;
		await _backupLogic.MessageHandler(arg);
		// backup Logic for sending  Messages to Backup Server
	}
}
