using System;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Infrastructure.Serialization
{
    /// <summary>
    /// JsonConverter for serializing and deserializing MethodBase objects.
    /// </summary>
    public class MethodBaseConverter : JsonConverter<MethodBase>
    {
        /// <inheritdoc />
        public override MethodBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Deserialization is not supported, throw NotImplementedException
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, MethodBase value, JsonSerializerOptions options)
        {
            // Start writing the MethodBase object as a JSON object
            writer.WriteStartObject();

            // Write the properties of the MethodBase object
            writer.WriteString(nameof(value.Name), value.Name);
            writer.WriteBoolean(nameof(value.IsAbstract), value.IsAbstract);
            writer.WriteBoolean(nameof(value.IsAssembly), value.IsAssembly);
            writer.WriteBoolean(nameof(value.IsStatic), value.IsStatic);

            // End writing the JSON object
            writer.WriteEndObject();
        }
    }
}
