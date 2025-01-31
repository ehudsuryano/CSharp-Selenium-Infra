using System.Collections;
using System.Collections.Generic;
using System.Text.Json;

namespace Infrastructure.Extensions
{
    public static class DotnetExtensions
    {
        /// <summary>
        /// Gets the value associated with the specified key from the dictionary.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary to retrieve the value from.</param>
        /// <param name="key">The key of the value to retrieve.</param>
        /// <returns>The value associated with the specified key, or the default value for the type if the key is not found.</returns>        
        public static T Get<T>(this IDictionary dictionary,string key)
        {
            // Check if the dictionary contains the specified key
            if (!dictionary.Contains(key))
            {
                // If the key is not found, return the default value for type T
                return default;
            }
            // Return the value associated with the key, casting it to type T
            return (T)dictionary[key];
        }

        /// <summary>
        /// Gets a value from the dictionary, or a default value if not found.
        /// </summary>
        /// <typeparam name="T">The expected type of the value.</typeparam>
        /// <param name="dictionary">The dictionary to retrieve the value from.</param>
        /// <param name="key">The key of the value to retrieve.</param>
        /// <param name="defaultValue">The default value to return if key is not found.</param>
        /// <returns>The retrieved value or the default value if not found.</returns>
        
        public static T Get<T>(this IDictionary dictionary ,string key, T defaultValue)
        {
            // Check if the key is present in the dictionary.
            var isKey = dictionary.Contains(key);

            // If the key is not present, return the default value.
            if (!isKey)
            {
                return defaultValue;
            }

            // Check if the value associated with the key is of the expected type.
            if (dictionary[key] is T value)
            {
                return value;
            }

            // If the value is not of the expected type, return the default value.
            return defaultValue;
        }
        /// <summary>
        /// Gets the value associated with the specified key from the dictionary.
        /// If the key is not found, returns the specified default value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <typeparam name="TValue">The type of the dictionary's values.</typeparam>
        /// <param name="dictionary">The dictionary to retrieve the value from.</param>
        /// <param name="key">The key of the value to retrieve.</param>
        /// <param name="defaultValue">The default value to return if the key is not found.</param>
        /// <returns>The value associated with the specified key, or the default value if the key is not found.</returns>

        public static T Get<T, TValue>(this IDictionary<string, TValue> dictionary, string key, T defaultValue)
        {
            // Check if the key is present in the dictionary.
            var isKey = dictionary.ContainsKey(key);

            // If the key is not present, return the default value.
            if (!isKey)
            {
                return defaultValue;
            }

            // Handle JsonElement values by resolving the type. Otherwise, use the dictionary value.
            var obj = dictionary[key] is JsonElement jsonElement
                ? ResolveType(jsonElement)
                : dictionary[key];

            // Check if the value associated with the key is of the expected type.
            if (obj is T value)
            {
                return value;
            }

            // If the value is not of the expected type, return the default value.
            return defaultValue;
        }

        // TODO: Add support for date and int numbers.
        // Resolves the value of a JsonElement based on its value kind.
        private static object ResolveType(JsonElement jsonElement) => jsonElement.ValueKind switch
        {
            JsonValueKind.String => $"{jsonElement}",
            JsonValueKind.Number => jsonElement.GetDouble(),
            JsonValueKind.True => jsonElement.GetBoolean(),
            JsonValueKind.False => jsonElement.GetBoolean(),
            JsonValueKind.Array => jsonElement,
            JsonValueKind.Object => jsonElement,
            JsonValueKind.Undefined => null,
            JsonValueKind.Null => null,
            _ => jsonElement
        };
    }
}
