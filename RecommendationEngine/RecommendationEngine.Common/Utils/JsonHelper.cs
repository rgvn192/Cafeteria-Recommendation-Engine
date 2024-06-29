using Newtonsoft.Json;


namespace RecommendationEngine.Common.Utils
{
    public static class JsonHelper
    {
        public static string SerializeObjectIgnoringCycles(object obj)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() }
            };
            return JsonConvert.SerializeObject(obj, Formatting.None, settings);
        }
    }
}
