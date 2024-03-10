using DiscordBackup.Bot.Data.Models;

namespace DiscordBackup.Bot.Data.Logic;

public class BackupLogic(IServiceProvider Services, IConfiguration Config, ILogger<BackupBot> Logger)
{
	private readonly DbbContext _dbContext = Services.GetRequiredService<DbbContext>();
	private readonly DiscordSocketClient _client = Services.GetRequiredService<DiscordSocketClient>();

	public async Task MessageHandler(SocketMessage msg)
	{
		if (msg.Channel is not SocketTextChannel || msg.Author.IsBot || msg.Content.Length <= 0 || msg.Content.StartsWith('/'))
			return;

		var dbChannel = _dbContext.Channels.Where(x => x.Id == msg.Channel.Id).FirstOrDefault();

		if (dbChannel is null)
			return;

		if (!dbChannel.hasBackup)
			return;		

		var dbMessage = _dbContext.Messages.Where(x => x.Id == msg.Id).FirstOrDefault();
		if (dbMessage is null)
			await InsertMessage(msg);
	}

	private async Task InsertMessage(SocketMessage message)
	{
		var hasFiles = false;

		if(message.Attachments.Count > 0)
		{
			hasFiles = true;
		}

		var addMessage = new Message()
		{
			Id = message.Id,
			Content = message.Content,
			HasFiles = hasFiles,
			UserId = message.Author.Id,
			CreatedAt = message.CreatedAt.Date,
			ChannelId = message.Channel.Id,
		};

		_dbContext.Messages.Add(addMessage);
		_dbContext.SaveChanges();
	}

	public async Task InsertUpdateChannel(SocketTextChannel channel, string backup)
	{
		var dbChannel = _dbContext.Channels.Where(x => x.Id == channel.Id).FirstOrDefault();

		if(dbChannel is not null)
		{
			if(backup.Equals("on"))
				dbChannel.hasBackup = true;
			else
				dbChannel.hasBackup = false;
		}
		else
		{
			var addChannel = new Channel()
			{
				Id = channel.Id,
				Name = channel.Name,
				hasBackup = true
			};

			_dbContext.Channels.Add(addChannel);
		}

		_dbContext.SaveChanges();

	}
}
