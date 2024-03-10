namespace DiscordBackup.Bot;

public class BackupBot(IServiceProvider Services, IConfiguration Config, ILogger<BackupBot> Logger) : IHostedService
{
	private readonly DiscordSocketClient _client = Services.GetRequiredService<DiscordSocketClient>();

	private readonly BackupChannelCommand _backupSlash = Services.GetRequiredService<BackupChannelCommand>();
	private readonly AddBackupGuildCommand _addBackup = Services.GetRequiredService<AddBackupGuildCommand>();

	private readonly CommandHandler _ch = Services.GetRequiredService<CommandHandler>();  
	private readonly JoinedHandler _jh = Services.GetRequiredService<JoinedHandler>();
	private readonly MessageHandler _mh = Services.GetRequiredService<MessageHandler>();

	private void AddBotEvents()
	{
		_client.Ready += Event_Ready;
		_client.Log += Event_Log;
		_client.MessageReceived += _mh.MessageCreated;
		_client.MessageDeleted += _mh.MessageDeleted;
		_client.UserCommandExecuted += _ch.UserCommandExecuted;		
		_client.SlashCommandExecuted += _ch.SlashCommandExecuted;
		_client.JoinedGuild += _jh.JoinedGuild;
		_client.UserJoined += _jh.UserJoined;
	}

	private async Task Event_Log(LogMessage arg)
	{
		 Logger.LogInformation(arg.Message);
	}

	private async Task Event_Ready()
	{
		await _client.SetActivityAsync(new Game("der Community", ActivityType.Listening));
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

		await _client.LoginAsync(TokenType.Bot, key);
		await _client.StartAsync();
	}

	public async Task StopAsync(CancellationToken cancellationToken)
	{
		await _client.LogoutAsync();
		await _client.StopAsync();
	}
}
