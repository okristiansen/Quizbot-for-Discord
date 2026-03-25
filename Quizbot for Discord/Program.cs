
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

// setup
IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

string? token = config["Discord:Token"];

var socketConfig = new DiscordSocketConfig
{
    GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages
};

var client = new DiscordSocketClient(socketConfig);

// loghandler
client.Log += LogHandler;

Task LogHandler(LogMessage message)
{
    Console.WriteLine(message.ToString());
    return Task.CompletedTask;
}

// ready handelr
client.Ready += ReadyHandler;

async Task ReadyHandler()
{
    var pingCommand = new SlashCommandBuilder()
        .WithName("ping")
        .WithDescription("Pong Ding Dong!");
    await client.Rest.CreateGuildCommand(pingCommand.Build(), 1485970234971258900); //Custom guild id for testing, CreateGlobalApplicationCommandAsync for global
}


client.SlashCommandExecuted += SlashCommandHandler;

async Task SlashCommandHandler(SocketSlashCommand command)
{
    if (command.CommandName == "ping")
    {
        await command.RespondAsync("Pong Ding Dong!");
    }
}



//launch
await client.LoginAsync(TokenType.Bot, token);

await client.StartAsync();

await Task.Delay(-1);


