namespace DiscordBackup.Bot;

public class BackupBot(IServiceProvider Services, IConfiguration Config, ILogger<BackupBot> Logger) : IHostedService
{
	private readonly DiscordSocketClient _dsc = Services.GetRequiredService<DiscordSocketClient>();

	private readonly BackupChannelCommand _backupSlash = Services.GetRequiredService<BackupChannelCommand>();
	private readonly AddBackupGuildCommand _addBackup = Services.GetRequiredService<AddBackupGuildCommand>();

	private readonly CommandHandler _ch = Services.GetRequiredService<CommandHandler>();  
	private readonly JoinedHandler _jh = Services.GetRequiredService<JoinedHandler>();
	private readonly MessageHandler _mh = Services.GetRequiredService<MessageHandler>();

	private void AddBotEvents()
	{
		_dsc.Ready += Event_Ready;
		_dsc.Log += Event_Log;
		_dsc.MessageReceived += _mh.MessageCreated;
		_dsc.MessageDeleted += _mh.MessageDeleted;
		_dsc.UserCommandExecuted += _ch.UserCommandExecuted;		
		_dsc.SlashCommandExecuted += _ch.SlashCommandExecuted;
		_dsc.JoinedGuild += _jh.JoinedGuild;
		_dsc.UserJoined += _jh.UserJoined;
	}

	private async Task Event_Log(LogMessage arg)
	{
		 Logger.LogInformation(arg.Message);
	}

	private async Task Event_Ready()
	{
		await _dsc.SetActivityAsync(new Game("der Community", ActivityType.Listening));
		await _backupSlash.Build();
		await _addBackup.Build();
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		AddBotEvents();

		using var scope = Services.CreateScope();
		var db = scope.ServiceProvider.GetRequiredService<DbbContext>();
		await db.Migrate();

		var key = Config["Token"];

		await _dsc.LoginAsync(TokenType.Bot, key);
		await _dsc.StartAsync();
	}

	public async Task StopAsync(CancellationToken cancellationToken)
	{
		await _dsc.LogoutAsync();
		await _dsc.StopAsync();
	}
}
