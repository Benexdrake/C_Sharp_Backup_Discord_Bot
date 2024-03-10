using DiscordBackup.Bot.Data.Logic;

namespace DiscordBackup.Bot.Commands;

public class CommandHandler(IServiceProvider Services, IConfiguration Config, ILogger<CommandHandler> Logger) 
{
	private readonly BackupLogic backupLogic = Services.GetRequiredService<BackupLogic>();
	
	public async Task SlashCommandExecuted(SocketSlashCommand arg)
	{
		switch(arg.CommandName.ToLower())
		{
			case "backup":
				if(arg.Channel is SocketTextChannel)
				{
					var backup = arg.Data.Options.FirstOrDefault().Name;
					await backupLogic.InsertUpdateChannel(arg.Channel as SocketTextChannel, backup);
					await arg.RespondAsync($"Backup {backup}",ephemeral:true);
				}
				break;
		}
	}

	public async Task UserCommandExecuted(SocketUserCommand arg)
	{
		Logger.LogInformation("Executed: {arg}", arg.CommandName);
	}
}
