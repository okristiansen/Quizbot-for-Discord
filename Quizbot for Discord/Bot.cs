using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Quizbot_for_Discord.Commands;

public class Bot
{
    private DiscordSocketClient _client;

    public async Task RunAsync()
    {
        // setup
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        string? token = config["Discord:Token"];

        var socketConfig = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages
        };

        _client = new DiscordSocketClient(socketConfig);

        // loghandler
        _client.Log += LogHandler;

        // ready handler
        _client.Ready += ReadyHandler;

        _client.SlashCommandExecuted += SlashCommandHandler;

        //launch
        await _client.LoginAsync(TokenType.Bot, token);

        await _client.StartAsync();

        await Task.Delay(-1);
    }

    Task LogHandler(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }

    async Task ReadyHandler()
    {
        await PingCommand.RegisterAsync(_client, 1485970234971258900); //Custom guild id for testing

        await QuizCommand.RegisterAsync(_client, 1485970234971258900);
    }

    async Task SlashCommandHandler(SocketSlashCommand command)
    {
        if (command.CommandName == "ping")
        {
            await PingCommand.HandleAsync(command);
        }

        if (command.CommandName == "quiz")
        {
            await QuizCommand.HandleAsync(command);
        }
    }
}
