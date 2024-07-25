using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Common.Utils
{
    public static class DictionaryUtility
    {

        public static string ConvertToString<T>(this Dictionary<T, T> dict)
        {
            StringBuilder str = new();

            if (dict.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var lastKey = dict.Keys.Last();

            foreach (var d in dict)
            {
                if (d.Key.Equals(lastKey))
                {
                    str.Append($"{d.Key}: {d.Value}");
                }
                else
                {
                    str.Append($"{d.Key}: {d.Value}, ");
                }
            }

            return str.ToString();
        }

        public static bool IsNullOrEmpty<T>(this Dictionary<T, T> dict)
        {
            return dict == null || dict.Count == 0;
        }
    }
}
