using Infrastructure.Models;

using System;

namespace Infrastructure.Core
{
    /// <summary>
    /// Provides methods for switching between different models in a fluent API manner.
    /// </summary>
    public interface IFluentModel
    {
        /// <summary>
        /// Switches to a new instance of the specified model type.
        /// </summary>
        /// <typeparam name="T">The type of the model to switch to.</typeparam>
        /// <returns>A new instance of the specified model type.</returns>
        T SwitchModel<T>() where T : ModelBase;

        /// <summary>
        /// Switches to a new instance of the specified model type with the given type name.
        /// </summary>
        /// <typeparam name="T">The type of the model to switch to.</typeparam>
        /// <param name="typeName">The name of the model type to switch to.</param>
        /// <returns>A new instance of the specified model type.</returns>
        T SwitchModel<T>(string typeName) where T : ModelBase;

        /// <summary>
        /// Switches to a new instance of the specified model type with the given type name and setup model.
        /// </summary>
        /// <typeparam name="T">The type of the model to switch to.</typeparam>
        /// <param name="typeName">The name of the model type to switch to.</param>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        /// <returns>A new instance of the specified model type.</returns>
        T SwitchModel<T>(string typeName, ObjectSetupModel setupModel) where T : ModelBase;

        /// <summary>
        /// Switches to a new instance of the specified model type with the given type name, setup model, and additional arguments.
        /// </summary>
        /// <typeparam name="T">The type of the model to switch to.</typeparam>
        /// <param name="typeName">The name of the model type to switch to.</param>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        /// <param name="arguments">Additional arguments to pass to the model's constructor.</param>
        /// <returns>A new instance of the specified model type.</returns>
        T SwitchModel<T>(string typeName, ObjectSetupModel setupModel, params object[] arguments) where T : ModelBase;

        /// <summary>
        /// Switches to a new instance of the specified model type.
        /// </summary>
        /// <typeparam name="T">The type of the model to switch to.</typeparam>
        /// <param name="type">The <see cref="Type"/> of the model to switch to.</param>
        /// <returns>A new instance of the specified model type.</returns>
        T SwitchModel<T>(Type type) where T : ModelBase;

        /// <summary>
        /// Switches to a new instance of the specified model type with the given setup model.
        /// </summary>
        /// <typeparam name="T">The type of the model to switch to.</typeparam>
        /// <param name="type">The <see cref="Type"/> of the model to switch to.</param>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        /// <returns>A new instance of the specified model type.</returns>
        T SwitchModel<T>(Type type, ObjectSetupModel setupModel) where T : ModelBase;

        /// <summary>
        /// Switches to a new instance of the specified model type with additional arguments.
        /// </summary>
        /// <typeparam name="T">The type of the model to switch to.</typeparam>
        /// <param name="type">The <see cref="Type"/> of the model to switch to.</param>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        /// <param name="arguments">Additional arguments to pass to the model's constructor.</param>
        /// <returns>A new instance of the specified model type.</returns>
        T SwitchModel<T>(Type type, ObjectSetupModel setupModel, params object[] arguments) where T : ModelBase;
    }
}
