using System.Text.Json.Serialization;
using System.Text.Json;

namespace QuizApp.Domain.Converters;

public class JsonStringListConverter : JsonConverter<List<string>>
{
    public override List<string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<List<string>>(ref reader, options) ?? new List<string>();
    }

    public override void Write(Utf8JsonWriter writer, List<string> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
