namespace DiscordBackup.Bot.Data.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasMany(u => u.Messages).WithOne(m => m.User);
	}
}
