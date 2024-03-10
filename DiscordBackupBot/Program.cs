using DiscordBackup.Bot.DL;

Log.Logger = new LoggerConfiguration()
		    .Enrich.FromLogContext()
			.WriteTo.Console()
		    .CreateLogger();

try
{
	var host = Host.CreateDefaultBuilder()
		.ConfigureAppConfiguration(builder =>
		{
			builder.AddUserSecrets<Program>()
				   .AddEnvironmentVariables();
		})
		.ConfigureServices((context, service) =>
		{
			service.AddHttpClient("default")
				   .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
				   {
					   ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
				   });

			service.AddSingleton(sp =>
			{
				var socketConfig = new DiscordSocketConfig
				{
					GatewayIntents = GatewayIntents.All
				};

				return new DiscordSocketClient(socketConfig);
			});

			service.AddScoped<BackupChannelCommand>();
			service.AddScoped<AddBackupGuildCommand>();
			
			service.AddScoped<BackupLogic>();
			service.AddScoped<JoinedLogic>();

			service.AddSingleton<CommandHandler>();
			service.AddSingleton<JoinedHandler>();
			service.AddSingleton<MessageHandler>();
			
			service.AddHostedService<BackupBot>();

			service.AddDbContext<DbbContext>(d => d.UseSqlite("Data Source=database.db"));
		})
		.UseSerilog((h, l) => l.ReadFrom.Configuration(h.Configuration))
		.Build();

	await host.RunAsync();
}
catch(HostAbortedException)
{
	// ignore
}
catch(Exception e)
{
	Log.Fatal(e, "Fatal Exception!");
}
