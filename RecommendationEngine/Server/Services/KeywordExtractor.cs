using Server.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Server.Services
{
    public class KeywordExtractor : IKeywordExtractor
    {
        private static readonly HashSet<string> StopWords = new()
        {
        "the", "is", "at", "which", "on", "and", "a", "an", "to", "for", "in", "of", "with", "that", "by", "this", "it", "from", "as", "are", "was", "were"
        };

        public List<string> ExtractKeywords(string comment)
        {
            var words = Regex.Split(comment.ToLower(), @"\W+")
                .Where(word => word.Length > 2 && !StopWords.Contains(word))
                .ToList();

            return words;
        }
    }
}
