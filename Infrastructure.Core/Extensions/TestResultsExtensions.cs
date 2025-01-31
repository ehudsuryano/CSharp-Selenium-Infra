using AventStack.ExtentReports;

using Infrastructure.Core;
using Infrastructure.Models;
using Infrastructure.Serialization;
using Infrastructure.Settings;

using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Infrastructure.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for updating test results.
    /// </summary>
    public static class TestResultsExtensions
    {
        // Provides a pre-configured instance of JsonSerializerOptions for JSON serialization.
        // This configuration includes custom converters and naming policies for properties and dictionary keys.
        private static JsonSerializerOptions JsonOptions
        {
            get
            {
                // Initialize a new JsonSerializerOptions object with specific settings.
                var options = new JsonSerializerOptions()
                {
                    // Ignore properties with null values during serialization.
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,

                    // Format the JSON output to be indented and more human-readable.
                    WriteIndented = true,

                    // Use camelCase for dictionary keys during serialization.
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,

                    // Use camelCase for property names during serialization.
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                // Add custom converters to handle specific types during serialization and deserialization.
                options.Converters.Add(new MethodBaseConverter());
                options.Converters.Add(new ExceptionConverter());
                options.Converters.Add(new TypeConverter());

                // Return the configured JsonSerializerOptions object.
                return options;

            }
        }
        /// <summary>
        /// Adds a test parameter to the test case's AutomationEnvironment TestParameters dictionary.
        /// </summary>
        /// <param name="testCase">The test case to which the test parameter will be added.</param>
        /// <param name="key">The key of the test parameter.</param>
        /// <param name="value">The value of the test parameter.</param>
        /// <returns>The updated <see cref="TestCaseBase"/> instance.</returns>
        public static TestCaseBase AddTestParameter(this TestCaseBase testCase, string key, object value)
        {
            // Initialize the TestParameters dictionary if it is null
            testCase.AutomationEnvironment.TestParameters ??= new Dictionary<string, object>();

            // Add the test parameter with the specified key and value
            testCase.AutomationEnvironment.TestParameters[key] = value;

            // Return the updated test case
            return testCase;
        }

        /// <summary>
        /// Adds test parameters to the test case's AutomationEnvironment TestParameters dictionary from a JSON string.
        /// </summary>
        /// <param name="testCase">The test case to which the test parameters will be added.</param>
        /// <param name="json">The JSON string containing the test parameters.</param>
        /// <returns>The updated <see cref="TestCaseBase"/> instance.</returns>
        public static TestCaseBase AddTestParameter(this TestCaseBase testCase, string json)
        {
            // Initialize the TestParameters dictionary if it is null
            testCase.AutomationEnvironment.TestParameters ??= new Dictionary<string, object>();

            // Deserialize the JSON string to a dictionary and add each key-value pair to the TestParameters dictionary
            foreach (var item in JsonSerializer.Deserialize<Dictionary<string, object>>(json, options: AppSettings.JsonOptions))
            {
                testCase.AutomationEnvironment.TestParameters[item.Key] = item.Value;
            }

            // Return the updated test case
            return testCase;
        }

        /// <summary>
        /// Adds a test property to the test case's AutomationEnvironment ContextProperties dictionary.
        /// </summary>
        /// <param name="testCase">The test case to which the test property will be added.</param>
        /// <param name="key">The key of the test property.</param>
        /// <param name="value">The value of the test property.</param>
        /// <returns>The updated <see cref="TestCaseBase"/> instance.</returns>
        public static TestCaseBase AddTestProperty(this TestCaseBase testCase, string key, object value)
        {
            // Add the test property with the specified key and value
            testCase.AutomationEnvironment.ContextProperties[key] = value;

            // Return the updated test case
            return testCase;
        }

        /// <summary>
        /// Adds test properties to the test case's AutomationEnvironment ContextProperties dictionary from a JSON string.
        /// </summary>
        /// <param name="testCase">The test case to which the test properties will be added.</param>
        /// <param name="json">The JSON string containing the test properties.</param>
        /// <returns>The updated <see cref="TestCaseBase"/> instance.</returns>
        public static TestCaseBase AddTestProperty(this TestCaseBase testCase, string json)
        {
            // Deserialize the JSON string to a dictionary and add each key-value pair to the ContextProperties dictionary
            foreach (var item in JsonSerializer.Deserialize<Dictionary<string, object>>(json, options: AppSettings.JsonOptions))
            {
                testCase.AutomationEnvironment.ContextProperties[item.Key] = item.Value;
            }

            // Return the updated test case
            return testCase;
        }
        /// <summary>
        /// Writes the metrics from the test result to the extent report.
        /// </summary>
        /// <param name="extent">The extent test instance to write the metrics to.</param>
        /// <param name="testResult">The test result containing the metrics to write.</param>
        public static void WriteMetrics(this ExtentTest extent, TestResultModel testResult)
        {
            // Serialize the test result metrics to a JSON string using the specified options.
            var metrics = JsonSerializer.Serialize(testResult, JsonOptions);

            // Create a node for the metrics in the extent report.
            var metricsNode = extent.CreateNode(name: "Metrics");

            // Write the serialized metrics to the extent report, formatted as preformatted text.
            metricsNode.Info($"<pre style='font-family: Courier New, Courier, monospace;'>{metrics}</pre>");
        }
    }
}
