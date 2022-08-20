using System.Text.Json;
using System.Text.Json.Serialization;

namespace CustomerApi.IntegrationTests.Helpers;

public static class SerializerOptions
{
    public static JsonSerializerOptions Default { get; } = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };
}
