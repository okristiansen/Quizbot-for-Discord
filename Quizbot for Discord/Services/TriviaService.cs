using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Quizbot_for_Discord.Models;

namespace Quizbot_for_Discord.Services
{
    public class TriviaService
    {
        private readonly HttpClient _httpClient;

        public TriviaService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<TriviaQuestion?> GetQuestionAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<TriviaResponse>("https://opentdb.com/api.php?amount=1&type=multiple");
            return response?.Results?[0];
        }
    }
}
