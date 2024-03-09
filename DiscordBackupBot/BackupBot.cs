using DiscordBackup.Bot.Commands;

namespace DiscordBackupBot;

public class BackupBot(IServiceProvider Services, IConfiguration Config, ILogger<BackupBot> Logger) : IHostedService
{
	private readonly IConfiguration _config = Config;
	private readonly DiscordSocketClient _dsc = Services.GetRequiredService<DiscordSocketClient>();
	private readonly BackupCommand _backupSlash = Services.GetRequiredService<BackupCommand>();
	private readonly CommandHandler _ch = Services.GetRequiredService<CommandHandler>();  

	private void AddBotEvents()
	{
		_dsc.Ready += Event_Ready;
		_dsc.Log += Event_Log;
		_dsc.MessageReceived += Event_MessageCreated;
		_dsc.MessageDeleted += Event_MessageDeleted;
		_dsc.UserCommandExecuted += _ch.UserCommandExecuted;		
		_dsc.SlashCommandExecuted += _ch.SlashCommandExecuted;
	}

	

	private async Task Event_MessageDeleted(Cacheable<IMessage, ulong> arg1, Cacheable<IMessageChannel, ulong> arg2)
	{
		Logger.LogInformation("Message: {arg1} - Deleted", arg1.Id);
	}

	private async Task Event_MessageCreated(SocketMessage arg)
	{
		Logger.LogInformation(arg.Content);

	}

	private async Task Event_Log(LogMessage arg)
	{
		 Logger.LogInformation(arg.Message);
	}

	private async Task Event_Ready()
	{
		await _dsc.SetActivityAsync(new Game("der Community", ActivityType.Listening));
		await _backupSlash.Build();
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		AddBotEvents();

		using var scope = Services.CreateScope();
		var db = scope.ServiceProvider.GetRequiredService<DbbContext>();
		await db.Migrate();

		var key = _config["Token"];

		await _dsc.LoginAsync(TokenType.Bot, key);
		await _dsc.StartAsync();
		await Task.Delay(Timeout.Infinite, cancellationToken);
	}

	public async Task StopAsync(CancellationToken cancellationToken)
	{
		await _dsc.LogoutAsync();
		await _dsc.StopAsync();
	}
}
