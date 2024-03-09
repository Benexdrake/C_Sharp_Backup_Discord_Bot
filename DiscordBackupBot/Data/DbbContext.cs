namespace DiscordBackup.Bot.Data;

public class DbbContext : DbContext
{
    public DbbContext()
    {
        
    }

    public DbbContext(DbContextOptions<DbbContext> options) 
        : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }

	public Task Migrate() => Database.MigrateAsync();
}
