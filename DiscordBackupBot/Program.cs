try
{
	var host = Host.CreateDefaultBuilder()
		.ConfigureAppConfiguration(builder =>
		{
			builder.SetBasePath(Directory.GetCurrentDirectory())
				   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				   .AddUserSecrets<Program>()
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

			service.AddScoped<Backup>();
			service.AddHostedService<BackupBot>();
		})
		.UseSerilog((h, l) => l.ReadFrom.Configuration(h.Configuration))
		.Build();

	host.Start();

	await host.RunAsync();
}
catch(Exception e)
{
	Log.Fatal(e, "Fatal Exception!");
}
