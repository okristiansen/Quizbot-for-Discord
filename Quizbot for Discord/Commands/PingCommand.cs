using Discord;
using Discord.WebSocket;

namespace Quizbot_for_Discord.Commands
{
    public static class PingCommand
    {
        // for initial testing of api
        public static async Task RegisterAsync(DiscordSocketClient client, ulong guildId)
        {
            var command = new SlashCommandBuilder()
                .WithName("ping")
                .WithDescription("Pong Ding Dong!");
            await client.Rest.CreateGuildCommand(command.Build(), guildId);
        }

        public static async Task HandleAsync(SocketSlashCommand command)
        {
            await command.RespondAsync("Pong Ding Dong!");
        }
    }
}
