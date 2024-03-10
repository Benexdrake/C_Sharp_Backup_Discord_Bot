﻿using System.Net.WebSockets;

namespace DiscordBackup.Bot.DL;

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

		if (message.Attachments.Count > 0)
		{
			hasFiles = true;
			await DownloadFiles(message);
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

	public async Task InsertUpdateChannel(SocketSlashCommand arg)
    {
        var backup = arg.Data.Options.FirstOrDefault().Name;
        var channel = arg.Channel;

        var dbChannel = _dbContext.Channels.Where(x => x.Id == channel.Id).FirstOrDefault();

        if (dbChannel is not null)
        {
            if (backup.Equals("on"))
            {
                dbChannel.hasBackup = true;
            }
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
        await arg.RespondAsync($"Backup {backup} for {arg.Channel.Name}", ephemeral: true);
    }

    public async Task DownloadFiles(SocketMessage message)
    {
        var messageId = message.Id;
        var channelId = message.Channel.Id;
        var path = $"{Config["DownloadPath"]}\\{channelId}\\{messageId}";

        if (Directory.Exists(path))
            return;

        Directory.CreateDirectory(path);

        foreach (var file in message.Attachments)
        {
            using (var client = new HttpClient())
            using (var result = await client.GetAsync(file.Url))
                if (result.IsSuccessStatusCode)
                {
                    var f = await result.Content.ReadAsByteArrayAsync();
                    await File.WriteAllBytesAsync(path + "\\" + file.Filename, f);
                }
        }
    }

    private async Task CreateBackupGuild()
    {
        var region = await _client.GetVoiceRegionAsync("japan");
        if (region is not null)
        {
            var backupGuild = _client.Guilds.Where(x => x.Name.Contains("backup")).FirstOrDefault();

            await backupGuild.DeleteAsync();

            var newGuild = await _client.CreateGuildAsync($"backup", region);

            //insert GuildID into DB
        }
    }

    public async Task BackupGuildHandler(SocketSlashCommand arg)
    {
        if(arg.Data.Options.Count > 0)
        {

            return;
        }

        await CreateBackupGuild();
		// insert ID of new Guild into DB
		//var backupGuild = _client.Guilds.Where(x => x.Name.Contains("backup")).FirstOrDefault();

        

        await Console.Out.WriteLineAsync(_client.Guilds.Count.ToString());
        await Console.Out.WriteLineAsync();
    }
}