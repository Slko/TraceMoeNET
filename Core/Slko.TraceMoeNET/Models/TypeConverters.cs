using Newtonsoft.Json;
using System;

namespace Slko.TraceMoeNET.Models
{
    internal class MillisecondsTimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
        {
            writer.WriteValue(value.TotalMilliseconds);
        }

        public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = reader.Value ?? throw new ArgumentNullException(nameof(reader.Value));
            return reader.TokenType switch
            {
                JsonToken.Integer => TimeSpan.FromMilliseconds((long)value),
                JsonToken.Float => TimeSpan.FromMilliseconds((double)value),
                _ => throw new ArgumentException($"Invalid data type (expected: Number, received: {reader.TokenType})", nameof(value)),
            };
        }
    }

    internal class SecondsTimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
        {
            writer.WriteValue(value.TotalSeconds);
        }

        public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = reader.Value ?? throw new ArgumentNullException(nameof(reader.Value));
            return reader.TokenType switch
            {
                JsonToken.Integer => TimeSpan.FromSeconds((long)value),
                JsonToken.Float => TimeSpan.FromSeconds((double)value),
                _ => throw new ArgumentException($"Invalid data type (expected: Number, received: {reader.TokenType})", nameof(value)),
            };
        }
    }
}
