using Newtonsoft.Json;

namespace GestaoDefeitos.Application.Utils
{
    public static class Serializer
    {
        public static string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
    }
}
