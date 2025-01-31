using Infrastructure.Exceptions;
using Infrastructure.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Core
{
    /// <summary>
    /// Provides a factory for creating instances of models derived from <see cref="ModelBase"/>.
    /// </summary>
    public static class ModelFactory
    {
        // Cache of all models in the current AppDomain, indexed by their full name.
        private static readonly Dictionary<string, Type> s_cache = AppDomain
            .CurrentDomain
            .GetAssemblies()
            .SelectMany(i => i.GetTypes())
            .Where(i => typeof(ModelBase).IsAssignableFrom(i))
            .ToDictionary(k => k.FullName, v => v, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Creates a new instance of the specified model type with the given setup model.
        /// </summary>
        /// <typeparam name="T">The type of the model to create, which must derive from <see cref="ModelBase"/>.</typeparam>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        /// <returns>A new instance of the specified model type.</returns>
        public static T New<T>(ObjectSetupModel setupModel)
            where T : IFluentModel
        {
            // Call the overloaded method with no additional arguments.
            return New<T>(type: typeof(T), setupModel, arguments: null);
        }

        /// <summary>
        /// Creates a new instance of the specified model type with the given setup model and additional arguments.
        /// </summary>
        /// <typeparam name="T">The type of the model to create, which must derive from <see cref="ModelBase"/>.</typeparam>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        /// <param name="arguments">Additional arguments to pass to the model's constructor.</param>
        /// <returns>A new instance of the specified model type.</returns>
        public static T New<T>(ObjectSetupModel setupModel, params object[] arguments)
            where T : IFluentModel
        {
            // Call the overloaded method with additional arguments.
            return New<T>(type: typeof(T), setupModel, arguments);
        }

        /// <summary>
        /// Creates a new instance of the specified model type with the given type name and setup model.
        /// </summary>
        /// <typeparam name="T">The type of the model to create, which must derive from <see cref="ModelBase"/>.</typeparam>
        /// <param name="typeName">The name of the model type to create.</param>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        /// <returns>A new instance of the specified model type.</returns>
        public static T New<T>(string typeName, ObjectSetupModel setupModel)
            where T : IFluentModel
        {
            // Call the overloaded method with no additional arguments.
            return New<T>(typeName, setupModel, arguments: null);
        }

        /// <summary>
        /// Creates a new instance of the specified model type with the given type name, setup model, and additional arguments.
        /// </summary>
        /// <typeparam name="T">The type of the model to create, which must derive from <see cref="ModelBase"/>.</typeparam>
        /// <param name="typeName">The name of the model type to create.</param>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        /// <param name="arguments">Additional arguments to pass to the model's constructor.</param>
        /// <returns>A new instance of the specified model type.</returns>
        public static T New<T>(string typeName, ObjectSetupModel setupModel, params object[] arguments)
            where T : IFluentModel
        {
            try
            {
                // Retrieve the type from the cache using the type name.
                var type = s_cache[typeName];

                // Call the overloaded method with the retrieved type.
                return New<T>(type, setupModel, arguments);
            }
            catch (Exception e)
            {
                // Create a message indicating that the type could not be found in the cache.
                var message = $"The specified model type '{typeName}' could not be found in the current application domain. " +
                    "Please ensure the type name is correct and the type is available.";

                // Throw a custom exception with the message and inner exception.
                throw new ModelNotFoundException(message, e);
            }
        }

        /// <summary>
        /// Creates a new instance of the specified model type with the given type and setup model.
        /// </summary>
        /// <typeparam name="T">The type of the model to create, which must derive from <see cref="ModelBase"/>.</typeparam>
        /// <param name="type">The type of the model to create.</param>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        /// <returns>A new instance of the specified model type.</returns>
        public static T New<T>(Type type, ObjectSetupModel setupModel)
            where T : IFluentModel
        {
            // Call the overloaded method with no additional arguments.
            return New<T>(type, setupModel, arguments: null);
        }

        /// <summary>
        /// Creates a new instance of the specified model type with the given type, setup model, and additional arguments.
        /// </summary>
        /// <typeparam name="T">The type of the model to create, which must derive from <see cref="ModelBase"/>.</typeparam>
        /// <param name="type">The type of the model to create.</param>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        /// <param name="arguments">Additional arguments to pass to the model's constructor.</param>
        /// <returns>A new instance of the specified model type.</returns>
        public static T New<T>(Type type, ObjectSetupModel setupModel, params object[] arguments)
            where T : IFluentModel
        {
            // Combine the setup model and additional arguments into a single array.
            var args = new object[] { setupModel };

            // Create a new instance of the specified model type using the combined arguments.
            return (T)Activator.CreateInstance(type, args: args.Concat(arguments ?? Array.Empty<object>()).ToArray());
        }
    }
}
