namespace DiscordBackup.Bot.Data;

public class DbbContext : DbContext
{
    public DbbContext(DbContextOptions<DbbContext> options) : base(options)
    {}
    public DbSet<Channel> Channels { get; set; }
    public DbSet<Message> Messages { get; set; }

    public Task Migrate() => Database.MigrateAsync();
}
