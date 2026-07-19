using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace Tests.Uinsure.Integration.Helpers;

internal static class HttpHelpers
{
    private static readonly JsonSerializerSettings _settings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver
        {
            IgnoreSerializableAttribute = true,
        },
        
    };

    internal static StringContent ToJsonContent<TType>(this TType obj)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(obj);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    internal static async Task<TType?> GetAs<TType>(this HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<TType>(json, _settings);
    }

}
