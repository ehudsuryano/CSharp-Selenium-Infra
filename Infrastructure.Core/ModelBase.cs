using Infrastructure.Models;

using OpenQA.Selenium;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Infrastructure.Core
{
    /// <summary>
    /// Represents the base model class that provides common setup and WebDriver functionality.
    /// </summary>
    public abstract class ModelBase : IFluentModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase"/> class with the specified setup model.
        /// </summary>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        protected ModelBase(ObjectSetupModel setupModel) : this(setupModel, url: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase"/> class with the specified setup model and URL.
        /// </summary>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        /// <param name="url">The URL to navigate to.</param>
        protected ModelBase(ObjectSetupModel setupModel, string url)
        {
            // Set the setup model and WebDriver instance.
            SetupModel = setupModel;

            // Get the WebDriver instance from the setup model.
            WebDriver = SetupModel.WebDriver;

            // Navigate to the specified URL if it is not null or empty.
            if (!string.IsNullOrEmpty(url))
            {
                WebDriver.Navigate().GoToUrl(url);
            }
        }

        /// <summary>
        /// Gets the setup model containing configuration and WebDriver information.
        /// </summary>
        public ObjectSetupModel SetupModel { get; }

        /// <summary>
        /// Gets the WebDriver instance used for browser automation.
        /// </summary>
        public IWebDriver WebDriver { get; }

        /// <summary>
        /// Executes an auditable action, logs the action performed, and records the duration.
        /// </summary>
        /// <typeparam name="T">The type of the result returned by the action.</typeparam>
        /// <param name="action">The name of the action to be performed.</param>
        /// <param name="actionToPerform">The action to perform, which takes an <see cref="ObjectSetupModel"/> as a parameter and returns a result of type <typeparamref name="T"/>.</param>
        /// <returns>The result of the action performed.</returns>

        public T AuditableAction<T>(string action, Func<ObjectSetupModel, T> actionToPerform)
        {
            try
            {
                // Check if the "AuditableActions" key exists in the test data and if it is a dictionary.
                var isAuditableActions = SetupModel.Environment.TestData.ContainsKey("AuditableActions");
                var isDictionary = isAuditableActions && SetupModel.Environment.TestData["AuditableActions"] is Dictionary<string, object>;

                // Get the existing auditable actions dictionary or create a new one if it does not exist.
                var auditableActions = isDictionary
                    ? (Dictionary<string, object>)SetupModel.Environment.TestData["AuditableActions"]
                    : new Dictionary<string, object>();

                // Start a stopwatch to measure the duration of the action.
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                try
                {
                    // Perform the specified action.
                    var result = actionToPerform(SetupModel);

                    // Update the test data with the modified auditable actions dictionary.
                    SetupModel.Environment.TestData["AuditableActions"] = auditableActions;

                    // Log a user-friendly message indicating the action performed.
                    SetupModel.ExtentReports.Info($"Successfully invoked action: '{action}'.");

                    // Return the result of the action.
                    return result;
                }
                finally
                {
                    // Stop the stopwatch and log the elapsed time in ticks for the action.
                    stopwatch.Stop();
                    auditableActions[$"{action}Duration"] = stopwatch.Elapsed.Ticks;
                }
            }
            catch (Exception ex)
            {
                // Log an error message if the action fails.
                SetupModel.ExtentReports.Fail($"Failed to invoke action: '{action}' due to error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Switches to a new instance of the specified model type.
        /// </summary>
        /// <typeparam name="T">The type of the model to switch to.</typeparam>
        /// <returns>A new instance of the specified model type.</returns>
        
        public T SwitchModel<T>() where T : ModelBase
        {
            // Switch to the specified model type with no additional arguments.
            return SwitchModel<T>(type: typeof(T), SetupModel, arguments: null);
        }

        /// <summary>
        /// Switches to a new instance of the specified model type with the given type name.
        /// </summary>
        /// <typeparam name="T">The type of the model to switch to.</typeparam>
        /// <param name="typeName">The name of the model type to switch to.</param>
        /// <returns>A new instance of the specified model type.</returns>
        public T SwitchModel<T>(string typeName) where T : ModelBase
        {
            // Switch to the specified model type with the given type name and no additional arguments.
            return SwitchModel<T>(typeName, SetupModel, arguments: null);
        }
        /// <summary>
        /// Switches to a new instance of the specified model type with the given type name and setup model.
        /// </summary>
        /// <typeparam name="T">The type of the model to switch to.</typeparam>
        /// <param name="typeName">The name of the model type to switch to.</param>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        /// <returns>A new instance of the specified model type.</returns>
        public T SwitchModel<T>(string typeName, ObjectSetupModel setupModel) where T : ModelBase
        {
            // Switch to the specified model type with the given type name and setup model, and no additional arguments.
            return SwitchModel<T>(typeName, setupModel, arguments: null);
        }

        /// <summary>
        /// Switches to a new instance of the specified model type with the given type name, setup model, and additional arguments.
        /// </summary>
        /// <typeparam name="T">The type of the model to switch to.</typeparam>
        /// <param name="typeName">The name of the model type to switch to.</param>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        /// <param name="arguments">Additional arguments to pass to the model's constructor.</param>
        /// <returns>A new instance of the specified model type.</returns>
        public T SwitchModel<T>(string typeName, ObjectSetupModel setupModel, params object[] arguments) where T : ModelBase
        {
            // Log the switch operation.
            setupModel.ExtentReports.Info($"Switching to model: {typeName} with setup model.");

            // Create a new instance of the specified model type.
            var model = ModelFactory.New<T>(typeName, setupModel, arguments);

            // Log the successful switch operation.
            setupModel.ExtentReports.Info($"Successfully switched to model: {typeName}.");

            // Return the new model instance.
            return model;
        }

        /// <summary>
        /// Switches to a new instance of the specified model type.
        /// </summary>
        /// <typeparam name="T">The type of the model to switch to.</typeparam>
        /// <param name="type">The type of the model to switch to.</param>
        /// <returns>A new instance of the specified model type.</returns>
        public T SwitchModel<T>(Type type) where T : ModelBase
        {
            // Switch to the specified model type with no additional arguments.
            return SwitchModel<T>(type, SetupModel, arguments: null);
        }

        /// <summary>
        /// Switches to a new instance of the specified model type with the given type and setup model.
        /// </summary>
        /// <typeparam name="T">The type of the model to switch to.</typeparam>
        /// <param name="type">The type of the model to switch to.</param>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        /// <returns>A new instance of the specified model type.</returns>
        public T SwitchModel<T>(Type type, ObjectSetupModel setupModel) where T : ModelBase
        {
            // Switch to the specified model type with the given type and setup model, and no additional arguments.
            return SwitchModel<T>(type, setupModel, arguments: null);
        }

        /// <summary>
        /// Switches to a new instance of the specified model type with additional arguments.
        /// </summary>
        /// <typeparam name="T">The type of the model to switch to.</typeparam>
        /// <param name="type">The type of the model to switch to.</param>
        /// <param name="arguments">Additional arguments to pass to the model's constructor.</param>
        /// <returns>A new instance of the specified model type.</returns>
        public T SwitchModel<T>(Type type, params object[] arguments) where T : ModelBase
        {
            // Switch to the specified model type with the given type and additional arguments.
            return SwitchModel<T>(type, SetupModel, arguments);
        }

        /// <summary>
        /// Switches to a new instance of the specified model type with the given type, setup model, and additional arguments.
        /// </summary>
        /// <typeparam name="T">The type of the model to switch to.</typeparam>
        /// <param name="type">The type of the model to switch to.</param>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        /// <param name="arguments">Additional arguments to pass to the model's constructor.</param>
        /// <returns>A new instance of the specified model type.</returns>
        public T SwitchModel<T>(Type type, ObjectSetupModel setupModel, params object[] arguments) where T : ModelBase
        {
            // Log the switch operation.
            setupModel.ExtentReports.Info($"Switching to model: {type.Name} with setup model.");

            // Create a new instance of the specified model type.
            var model = ModelFactory.New<T>(type, setupModel, arguments);

            // Log the successful switch operation.
            setupModel.ExtentReports.Info($"Successfully switched to model: {type.Name}.");

            // Return the new model instance.
            return model;
        }
    }
}
