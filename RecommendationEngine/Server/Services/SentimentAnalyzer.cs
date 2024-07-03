using Server.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{

    public class SentimentAnalyzer : ISentimentAnalyzer
    {
        private static readonly Dictionary<string, int> PositiveWords = new Dictionary<string, int>
        {
            { "delicious", 5 }, { "tasty", 4 }, { "yummy", 3 }, { "flavorful", 4 }, { "fresh", 3 },
            { "excellent", 5 }, { "amazing", 5 }, { "great", 4 }, { "fantastic", 5 }, { "satisfying", 4 },
            { "wonderful", 5 }, { "perfect", 5 }, { "savory", 3 }, { "delectable", 4 }, { "appetizing", 3 }
        };

        private static readonly Dictionary<string, int> NegativeWords = new Dictionary<string, int>
        {
            { "bland", -3 }, { "stale", -3 }, { "bad", -4 }, { "awful", -5 }, { "terrible", -5 }, { "horrible", -5 },
            { "disgusting", -5 }, { "unsatisfying", -4 }, { "poor", -3 }, { "unappetizing", -4 }, { "overcooked", -3 },
            { "undercooked", -3 }, { "salty", -2 }, { "greasy", -2 }, { "mediocre", -2 }
        };

        private static readonly List<string> NegationWords = new List<string> { "not", "never", "no" };

        public decimal AnalyzeSentiment(string comment)
        {
            string[] words = comment.ToLower().Split(new[] { ' ', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            int positiveScore = 0;
            int negativeScore = 0;

            for (int i = 0; i < words.Length; i++)
            {
                if (PositiveWords.ContainsKey(words[i]))
                {
                    if (i > 0 && NegationWords.Contains(words[i - 1]))
                    {
                        negativeScore += PositiveWords[words[i]] / 2;
                    }
                    else
                    {
                        positiveScore += PositiveWords[words[i]];
                    }
                }
                else if (NegativeWords.ContainsKey(words[i]))
                {
                    if (i > 0 && NegationWords.Contains(words[i - 1]))
                    {
                        positiveScore += -NegativeWords[words[i]] / 2;
                    }
                    else
                    {
                        negativeScore += -NegativeWords[words[i]];
                    }
                }
            }

            int rawSentimentScore = positiveScore - negativeScore;
            return NormalizeScore(rawSentimentScore);
        }

        private decimal NormalizeScore(int rawScore)
        {
            const int minScore = -20;
            const int maxScore = 20;

            const decimal normalizedMin = 0m;
            const decimal normalizedMax = 5m;

            decimal normalizedScore = ((decimal)rawScore - minScore) / (maxScore - minScore) * (normalizedMax - normalizedMin) + normalizedMin;
            return Math.Clamp(normalizedScore, normalizedMin, normalizedMax);
        }
    }


}
