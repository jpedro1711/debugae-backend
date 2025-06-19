using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GestaoDefeitos.Application.Utils
{

    public static class JsonComparer
    {
        public static List<(string Field, string? OldValue, string? NewValue)> CompareJson(string oldJson, string newJson)
        {
            var oldObj = JsonConvert.DeserializeObject<JObject>(oldJson);
            var newObj = JsonConvert.DeserializeObject<JObject>(newJson);

            var diffs = new List<(string Field, string? OldValue, string? NewValue)>();

            foreach (var property in oldObj!.Properties())
            {
                var field = property.Name;
                var oldValue = property.Value?.ToString();
                var newValue = newObj![field]?.ToString();

                if (oldValue != newValue)
                {
                    diffs.Add((field, oldValue, newValue));
                }
            }

            foreach (var property in newObj!.Properties())
            {
                if (!oldObj.ContainsKey(property.Name))
                {
                    diffs.Add((property.Name, null, property.Value?.ToString()));
                }
            }

            return diffs;
        }
    }
}
