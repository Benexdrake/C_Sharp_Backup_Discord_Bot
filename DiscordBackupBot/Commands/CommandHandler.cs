namespace DiscordBackup.Bot.Commands;

public class CommandHandler(IServiceProvider Services, IConfiguration Config, ILogger<CommandHandler> Logger) 
{
	
	public async Task SlashCommandExecuted(SocketSlashCommand arg)
	{
		Logger.LogInformation("Executed: {arg}", arg.CommandName);
	}

	public async Task UserCommandExecuted(SocketUserCommand arg)
	{
		Logger.LogInformation("Executed: {arg}", arg.CommandName);
	}
}
