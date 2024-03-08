IConfiguration conf;

var builder = new ConfigurationBuilder();
BuildConfig(builder);

Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Build())
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.CreateLogger();

conf = builder.Build();

var host = Host.CreateDefaultBuilder()
	.ConfigureServices((context, service) =>
	{
		service.AddSingleton(GetHttp());
		service.AddSingleton(GetDiscordClient());
		service.AddSingleton<Backup>();
		service.AddHostedService<Bot>();
	})
	.UseSerilog()
	.Build();


host.Start();
static void BuildConfig(IConfigurationBuilder builder)
{
	builder.SetBasePath(Directory.GetCurrentDirectory())
		.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
		.AddEnvironmentVariables();
}

static DiscordSocketClient GetDiscordClient()
{
	var socketConfig = new DiscordSocketConfig
	{
		GatewayIntents = GatewayIntents.All
	};

	return new DiscordSocketClient(socketConfig);
}

static HttpClient GetHttp()
{
	var policy = new HttpClientHandler();
	policy.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
	return new HttpClient(policy);
}


