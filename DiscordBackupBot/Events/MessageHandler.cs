using Discord;
using Discord.Rest;
using DiscordBackup.Bot.Data.Models;
using DiscordBackup.Bot.DL;
using System.IO;

namespace DiscordBackup.Bot.Events;

public class MessageHandler(IServiceProvider Services, IConfiguration Config, ILogger<BackupBot> Logger)
{
	private readonly BackupLogic _backupLogic = Services.GetRequiredService<BackupLogic>();
	private readonly DiscordSocketClient _client = Services.GetRequiredService<DiscordSocketClient>();
	private readonly DbbContext _dbContext = Services.GetRequiredService<DbbContext>();

	public async Task MessageDeleted(Cacheable<IMessage, ulong> arg1, Cacheable<IMessageChannel, ulong> arg2)
	{
		Logger.LogInformation("Message: {arg1} - Deleted", arg1.Id);
	}

	public async Task MessageCreated(SocketMessage arg)
	{
		var cDb = _dbContext.Channels.Where(x => x.Id == arg.Channel.Id).FirstOrDefault();

		if (cDb is null || cDb.hasBackup == false)
			return;

		if (arg.Author.IsBot)
			return;
		await _backupLogic.MessageHandler(arg);

		var bgID = _dbContext.BackupGuild.FirstOrDefault();
		if (bgID is null)
			return;

		var guild = _client.Guilds.ToList().Where(x => x.Id == bgID.Id).FirstOrDefault();
		if (guild is null)
			return;

		var channel = guild.Channels.Where(x => x.Name.Equals(arg.Channel.Name)).FirstOrDefault() as SocketTextChannel;
		if (channel is null)
			return;

		var wc = channel.GetWebhooksAsync().Result.FirstOrDefault();
		
		if(wc is null)
		return;
		
		await WebHookMessage(arg,wc);
    }

	private async Task WebHookMessage(SocketMessage message, RestWebhook wc)
	{
		var webhook = new DiscordWebhookClient(wc);
		if (message.Attachments.Count == 0)
			await webhook.SendMessageAsync(message.Content, username: message.Author.GlobalName, avatarUrl: message.Author.GetDisplayAvatarUrl());
		else
		{
			var fileUrl = message.Attachments.ElementAt(0).Url;
			Stream stream = await new HttpClient().GetStreamAsync(fileUrl);
			if (stream is null)
				return;
			var attachment = new FileAttachment(stream,"image.png");
			if(message.Content.Length > 0)
				await webhook.SendFilesAsync([attachment],text:message.Content, username: message.Author.GlobalName, avatarUrl:message.Author.GetDisplayAvatarUrl());
			else
				await webhook.SendFilesAsync([attachment]," ", username: message.Author.GlobalName, avatarUrl: message.Author.GetDisplayAvatarUrl());
			Console.WriteLine();
		}
	}
}
