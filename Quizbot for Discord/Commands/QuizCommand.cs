using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Quizbot_for_Discord.Services;

namespace Quizbot_for_Discord.Commands
{
    public static class QuizCommand
    {
        public static async Task RegisterAsync(DiscordSocketClient client, ulong guildId)
        {
            var command = new SlashCommandBuilder()
                .WithName("quiz")
                .WithDescription("get question");
            await client.Rest.CreateGuildCommand(command.Build(), guildId);
        }

        public static async Task HandleAsync(SocketSlashCommand command)
        {
            var triviaService = new TriviaService();
            var question = await triviaService.GetQuestionAsync();

            if (question == null)
            {
                await command.RespondAsync("Failed to get a question. Try again!");
                return;
            }

            // decode HTML entities and shuffle answers
            var questionText = System.Net.WebUtility.HtmlDecode(question.Question);
            var answers = new List<string>(question.IncorrectAnswers.Select(a => System.Net.WebUtility.HtmlDecode(a)));
            answers.Add(System.Net.WebUtility.HtmlDecode(question.CorrectAnswer));
            answers = answers.OrderBy(_ => Guid.NewGuid()).ToList();

            // response
            var labels = new[] { "A", "B", "C", "D" };
            var answerText = string.Join("\n", answers.Select((a, i) => $"{labels[i]}) {a}"));

            var message = $"**Category:** {question.Category}\n" +
                          $"**Difficulty:** {question.Difficulty}\n\n" +
                          $"{questionText}\n\n" +
                          answerText;

            await command.RespondAsync(message);
        }   
    }
}
