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
            await command.DeferAsync();

            var triviaService = new TriviaService();
            var question = await triviaService.GetQuestionAsync();

            if (question == null)
            {
                await command.FollowupAsync("Failed to get a question. Try again!");
                return;
            }

            // decode HTML entities and build answer list with correctness flag
            var questionText = System.Net.WebUtility.HtmlDecode(question.Question);
            var answerList = question.IncorrectAnswers
                .Select(a => (Text: System.Net.WebUtility.HtmlDecode(a), IsCorrect: false))
                .ToList();
            answerList.Add((Text: System.Net.WebUtility.HtmlDecode(question.CorrectAnswer), IsCorrect: true));
            answerList = answerList.OrderBy(_ => Guid.NewGuid()).ToList();

            var labels = new[] { "A", "B", "C", "D" };
            var componentBuilder = new ComponentBuilder();

            for (int i = 0; i < answerList.Count; i++)
            {
                componentBuilder.WithButton(
                    label: $"{labels[i]}) {answerList[i].Text}",
                    customId: answerList[i].IsCorrect ? "quiz_correct" : $"quiz_wrong_{i}",
                    style: ButtonStyle.Primary);
            }

            var message = $"**Category:** {question.Category}\n" +
                          $"**Difficulty:** {question.Difficulty}\n\n" +
                          $"{questionText}";

            await command.FollowupAsync(message, components: componentBuilder.Build());




        }   
    }
}
