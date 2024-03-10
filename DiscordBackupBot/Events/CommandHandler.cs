using DiscordBackup.Bot.DL;

namespace DiscordBackup.Bot.Events;

public class CommandHandler(IServiceProvider Services, IConfiguration Config, ILogger<CommandHandler> Logger)
{
    private readonly BackupLogic backupLogic = Services.GetRequiredService<BackupLogic>();

    public async Task SlashCommandExecuted(SocketSlashCommand arg)
    {
        if (arg.Channel is SocketTextChannel)
        {
            switch (arg.CommandName.ToLower())
            {
                case "backup":
                    await backupLogic.InsertUpdateChannel(arg);
                    break;
                case "add_backup_server":
                    await backupLogic.BackupGuildHandler(arg);
                    break;
            }
        }
    }

    public async Task UserCommandExecuted(SocketUserCommand arg)
    {
        Logger.LogInformation("Executed: {arg}", arg.CommandName);
    }
}
