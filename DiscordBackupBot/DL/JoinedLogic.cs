﻿namespace DiscordBackup.Bot.DL;

public class JoinedLogic(IServiceProvider Services, IConfiguration Config, ILogger<BackupBot> Logger)
{
	private readonly DiscordSocketClient _client = Services.GetRequiredService<DiscordSocketClient>();
	private readonly DbbContext _dbContext = Services.GetRequiredService<DbbContext>();

	public async Task UserJoined(SocketGuildUser arg)
	{
		if (_client.CurrentUser.Id == arg.Guild.Owner.Id)
			if (arg.Guild.Users.Count == 2)
			{
				var permissions = new GuildPermissions(administrator: true);

				var role = await arg.Guild.CreateRoleAsync("admin", permissions, color: Color.Red);
				await arg.AddRoleAsync(role);
			}
	}

	public async Task JoinedGuild(SocketGuild guild)
	{
		var backupDb = _dbContext.BackupGuild.FirstOrDefault();
		if (backupDb is null)
			return;

		if (guild.Id == backupDb.Id)
		{
			foreach (var channel in guild.Channels)
			{
				if (channel is SocketTextChannel && channel is not SocketVoiceChannel)
				{
					var c = channel as SocketTextChannel;
					var invite = await c.CreateInviteAsync();

					var webhook = new DiscordWebhookClient(Config["Webhook"]);
					await webhook.SendMessageAsync(invite.Url);
				}
			}
		}
	}
}
