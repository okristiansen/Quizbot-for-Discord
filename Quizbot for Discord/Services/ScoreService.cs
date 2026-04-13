using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizbot_for_Discord.Services
{
    public static class ScoreService
    {

        static Dictionary<ulong, int> score = new Dictionary<ulong, int>();


        public static void AddPoint(ulong userId)
        {
            if (score.ContainsKey(userId))
            {
                score[userId] = score[userId] + 1;
            } else
            {
                score[userId] = 1;
            }
        }

        public static int GetScore (ulong userId)
        {
            return score.GetValueOrDefault(userId, 0);
        }
    }
}
