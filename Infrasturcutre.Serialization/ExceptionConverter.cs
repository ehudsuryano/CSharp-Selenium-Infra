using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Serialization
{
    /// <summary>
    /// JsonConverter for serializing and deserializing Exception objects.
    /// </summary>
    public class ExceptionConverter : JsonConverter<Exception>
    {
        /// <inheritdoc />
        public override Exception Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Deserialization is not supported, throw NotImplementedException
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, Exception value, JsonSerializerOptions options)
        {
            // Start writing the Exception object as a JSON object
            writer.WriteStartObject();

            // Write the properties of the Exception object
            writer.WriteString(nameof(value.Message), value.Message);
            writer.WriteString(nameof(value.StackTrace), value.StackTrace);
            writer.WriteString(nameof(value.HelpLink), value.HelpLink);
            writer.WriteNumber(nameof(value.HResult), value.HResult);
            writer.WriteString(nameof(value.Source), value.Source);
            writer.WriteString(nameof(value.TargetSite), $"{value.TargetSite?.DeclaringType.FullName}.{value.TargetSite?.Name}");

            // End writing the JSON object
            writer.WriteEndObject();
        }
    }
}
