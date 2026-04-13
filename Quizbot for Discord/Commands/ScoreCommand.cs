using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Quizbot_for_Discord.Services;

namespace Quizbot_for_Discord.Commands
{
    public static class ScoreCommand
    {
        public static async Task RegisterAsync(DiscordSocketClient client, ulong guildId)
        {
            var command = new SlashCommandBuilder()
                .WithName("score")
                .WithDescription("Get user quiz Score");
            await client.Rest.CreateGuildCommand(command.Build(), guildId);
        }

        public static async Task HandleAsync(SocketSlashCommand command)
        {
            var userId = command.User.Id;
            var score = ScoreService.GetScore(userId);
            await command.RespondAsync($"Your score is: {score}");
        }

    }
}
