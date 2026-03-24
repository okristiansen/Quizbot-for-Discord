
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;


IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

string? token = config["Discord:Token"];

var socketConfig = new DiscordSocketConfig
{
    GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages
};

var client = new DiscordSocketClient(socketConfig);

client.Log += LogHandler;

Task LogHandler(LogMessage message)
{
    Console.WriteLine(message.ToString());
    return Task.CompletedTask;
}

await client.LoginAsync(TokenType.Bot, token);

await client.StartAsync();

await Task.Delay(-1);
