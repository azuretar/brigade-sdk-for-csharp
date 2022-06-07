using System.Text.Json;
using System.Text.Json.Serialization;

namespace Brigade.Json
{
    internal class ConcreteTypeConverter<TConcrete, TInterface>: JsonConverter<TInterface> where TConcrete : class, TInterface
    {
        public override TInterface? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<TConcrete>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, TInterface value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
