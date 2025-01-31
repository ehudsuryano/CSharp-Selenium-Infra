using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Serialization
{
    /// <summary>
    /// JsonConverter for serializing and deserializing Exception objects.
    /// </summary>
    public class TypeConverter : JsonConverter<Type>
    {
        /// <inheritdoc />
        public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Deserialization is not supported, throw NotSupportedException
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
        {
            // Start writing the Type object as a JSON object
            writer.WriteStartObject();

            // Write the full name of the Type object
            writer.WriteString(nameof(Type), value.FullName);

            // End writing the JSON object
            writer.WriteEndObject();
        }
    }
}
