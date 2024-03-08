using Discord;

namespace Discord_Bot;

public class Bot : IHostedService
{
	private readonly IConfiguration _config;
	private readonly DiscordSocketClient _dsc;
	private readonly Backup _backupSlash;

	public Bot(IServiceProvider service, IConfiguration config)
    {
		_config = config;
		_dsc = service.GetRequiredService<DiscordSocketClient>();
		_backupSlash = service.GetRequiredService<Backup>();
	}

	private void BotEvents()
	{
		_dsc.Ready += Event_Ready;
		_dsc.Log += Event_Log;
		_dsc.MessageReceived += Event_MessageCreated;
		_dsc.MessageDeleted += Event_MessageDeleted;
		_dsc.SlashCommandExecuted += Event_SlashCommandExecuted;
	}

	private async Task Event_SlashCommandExecuted(SocketSlashCommand arg)
	{
		Log.Logger.Information($"Executed: {arg.CommandName}");
	}

	private async Task Event_MessageDeleted(Cacheable<IMessage, ulong> arg1, Cacheable<IMessageChannel, ulong> arg2)
	{
        Log.Logger.Information($"Message: {arg1.Id} - Deleted");
    }

	private async Task Event_MessageCreated(SocketMessage arg)
	{
		Log.Logger.Information(arg.Content);
	}

	private async Task Event_Log(LogMessage arg)
	{
		Log.Logger.Information(arg.Message);
	}

	private async Task Event_Ready()
	{
		await _dsc.SetActivityAsync(new Game("der Community", ActivityType.Listening));
		await _backupSlash.Build();
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		BotEvents();

		var key = _config["Token"];
		await _dsc.LoginAsync(TokenType.Bot, key);
		await _dsc.StartAsync();

		Console.ReadKey();

		await _dsc.LogoutAsync();
		await _dsc.StopAsync();
	}

	public async Task StopAsync(CancellationToken cancellationToken)
	{
		
	}
}
