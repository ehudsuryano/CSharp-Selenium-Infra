using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Infrastructure.Models;
using Infrastructure.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Infrastructure.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="ObjectSetupModel"/> class.
    /// </summary>
    public static class FrameworkExtensions
    {
        /// <summary>
        /// Adds a metric to the test data within the specified setup model.
        /// </summary>
        /// <param name="setupModel">The setup model containing configuration and environment information.</param>
        /// <param name="metric">The name of the metric to add.</param>
        /// <param name="value">The value of the metric to add.</param>
        public static void AddMetrics(this ObjectSetupModel setupModel, string metric, object value)
        {
            try
            {
                // Check if the "Metrics" key exists in the test data and if it is a dictionary.
                var isMetrics = setupModel.Environment.TestData.ContainsKey("Metrics");
                var isDictionary = isMetrics && setupModel.Environment.TestData["Metrics"] is Dictionary<string, object>;

                // Get the existing metrics dictionary or create a new one if it does not exist.
                var metrics = isDictionary
                    ? (Dictionary<string, object>)setupModel.Environment.TestData["Metrics"]
                    : [];

                // Add or update the metric value in the dictionary.
                metrics[metric] = value;

                // Update the test data with the modified metrics dictionary.
                setupModel.Environment.TestData["Metrics"] = metrics;
            }
            catch
            {
                // Silently catch any exceptions that occur during the method execution
                // to prevent them from bubbling up and affecting the overall execution.
            }
        }

        /// <summary>
        /// Adds a new driver or replaces an existing driver in the <see cref="ObjectSetupModel"/>.
        /// </summary>
        /// <param name="setupModel">The setup model to update.</param>
        /// <param name="name">The name of the driver.</param>
        /// <param name="driver">The <see cref="IWebDriver"/> instance to add or replace.</param>
        public static void AddOrReplaceDriver(this ObjectSetupModel setupModel, string name, IWebDriver driver)
        {
            // Initialize the Drivers dictionary if it is null
            setupModel.Drivers ??= new Dictionary<string, IWebDriver>();

            // Add or replace the driver with the specified name
            setupModel.Drivers[name] = driver;
        }

        /// <summary>
        /// Adds a test parameter to the environment's TestParameters dictionary.
        /// </summary>
        /// <param name="environment">The automation environment to which the test parameter will be added.</param>
        /// <param name="key">The key of the test parameter.</param>
        /// <param name="value">The value of the test parameter.</param>
        /// <returns>The updated <see cref="AutomationEnvironment"/> instance.</returns>
        public static AutomationEnvironment AddTestParameter(this AutomationEnvironment environment, string key, object value)
        {
            // Initialize the TestParameters dictionary if it is null
            environment.TestParameters ??= new Dictionary<string, object>();

            // Add the test parameter with the specified key and value
            environment.TestParameters[key] = value;

            // Return the updated environment
            return environment;
        }

        /// <summary>
        /// Adds test parameters to the environment's TestParameters dictionary from a JSON string.
        /// </summary>
        /// <param name="environment">The automation environment to which the test parameters will be added.</param>
        /// <param name="json">The JSON string containing the test parameters.</param>
        /// <returns>The updated <see cref="AutomationEnvironment"/> instance.</returns>
        public static AutomationEnvironment AddTestParameter(this AutomationEnvironment environment, string json)
        {
            // Initialize the TestParameters dictionary if it is null
            environment.TestParameters ??= new Dictionary<string, object>();

            // Deserialize the JSON string to a dictionary and add each key-value pair to the TestParameters dictionary
            foreach (var item in JsonSerializer.Deserialize<Dictionary<string,object>>(json , options : AppSettings.JsonOptions))
            {
                environment.TestParameters[item.Key] = item.Value;
            }

            // Return the updated environment
            return environment;
        }

        /// <summary>
        /// Adds a test property to the environment's ContextProperties dictionary.
        /// </summary>
        /// <param name="environment">The automation environment to which the test property will be added.</param>
        /// <param name="key">The key of the test property.</param>
        /// <param name="value">The value of the test property.</param>
        /// <returns>The updated <see cref="AutomationEnvironment"/> instance.</returns>
        public static AutomationEnvironment AddTestProperty(this AutomationEnvironment environment, string key, object value)
        {
            // Add the test property with the specified key and value
            environment.ContextProperties[key] = value;

            // Return the updated environment
            return environment;
        }

        /// <summary>
        /// Adds test properties to the environment's ContextProperties dictionary from a JSON string.
        /// </summary>
        /// <param name="environment">The automation environment to which the test properties will be added.</param>
        /// <param name="json">The JSON string containing the test properties.</param>
        /// <returns>The updated <see cref="AutomationEnvironment"/> instance.</returns>
        public static AutomationEnvironment AddTestProperty(this AutomationEnvironment environment, string json)
        {
            // Deserialize the JSON string to a dictionary and add each key-value pair to the ContextProperties dictionary
            foreach (var item in JsonSerializer.Deserialize<Dictionary<string, object>>(json, options: AppSettings.JsonOptions))
            {
                environment.ContextProperties[item.Key] = item.Value;
            }

            // Return the updated environment
            return environment;
        }
        /// <summary>
        /// Disposes of the main web driver and any additional drivers in the setup model.
        /// </summary>
        /// <param name="setupModel">The setup model containing the web drivers to be disposed of.</param>
        /// <returns>The updated <see cref="ObjectSetupModel"/> instance.</returns>
        public static ObjectSetupModel ClearDrivers(this ObjectSetupModel setupModel)
        {
            try
            {
                // Dispose of the main web driver if it exists
                setupModel.WebDriver?.Dispose();

                // Dispose of any additional drivers in the setup model's Drivers collection
                foreach (var driver in setupModel.Drivers.Values)
                {
                    driver?.Dispose();
                }

                // Clear the Drivers dictionary to remove all drivers from the
                // setup model after disposing them all properly
                setupModel.Drivers.Clear();
            }
            catch
            {
                // If an exception occurs, we silently catch it and do nothing
                // This ensures that the method completes without throwing exceptions
            }

            // Return the updated setup model
            return setupModel;
        }

        /// <summary>
        /// Extension method to retrieve the <see cref="ExtentReports"/> instance from the <see cref="TestContext"/>.
        /// </summary>
        /// <param name="context">The <see cref="TestContext"/> instance.</param>
        /// <returns>The <see cref="ExtentReports"/> instance associated with the test context.</returns>        
        public static ExtentReports GetExtentReports(this TestContext context)
        {
            // Retrieve the ExtentReports instance from the Properties dictionary of the TestContext
            return context.Properties.Get<ExtentReports>("ExtentReport");
        }

        /// <summary>
        /// Retrieves the path where the report is stored for the current test run.
        /// </summary>
        /// <param name="context">The test context containing the properties and test results directory.</param>
        /// <returns>The full path to the report file, including the RunId as the file name.</returns>
        public static string GetReportPath(this TestContext context)
        {
            // Retrieve the report path from the context properties, defaulting to the TestResultsDirectory if not found.
            var reportPath = context.Properties.Get("ExtentReport.ReportPath", context.TestResultsDirectory);

            // If the report path is ".", use the TestResultsDirectory
            reportPath = reportPath.Equals(".")
                ? context.TestResultsDirectory
                : reportPath;

            // Combine the report path with the RunId property to generate the full report file path.
            return Path.Combine(reportPath, $"{context.Properties["RunId"]}");
        }

        /// <summary>
        /// Gets the <see cref="ExtentTest"/> instance for the current test suite.
        /// </summary>
        /// <param name="context">The <see cref="TestContext"/> instance providing information about the current test run.</param>
        /// <returns>The <see cref="ExtentTest"/> instance for the test suite, or <c>null</c> if no test suite is found.</returns>
        public static ExtentTest GetSuite(this TestContext context)
        {
            // Get the display name of the test suite
            var testSuiteName = GetSuiteDisplayName(context);

            // Retrieve the ExtentTest instance associated with the test suite from the context properties
            return context.Properties[testSuiteName] as ExtentTest;
        }

        /// <summary>
        /// Retrieves the display name and description of the test suite from the TestContext.
        /// </summary>
        /// <param name="context">The TestContext instance.</param>
        /// <returns>A tuple containing the test suite's name and description.</returns>
        public static (string Name, string Description) GetSuiteDisplayName(this TestContext context)
        {
            // Finds the test suite type based on the provided TestContext.
            static Type FindTestSuite(TestContext context)
            {
                // Search for the type in all loaded assemblies in the current app domain.
                return AppDomain
                    .CurrentDomain
                    .GetAssemblies()
                    .SelectMany(i => i.GetTypes())
                    .FirstOrDefault(i => i.FullName == context.ManagedType);
            }
            // Find the test suite type using the FindTestSuite method.
            var testSuite = FindTestSuite(context);

            // If the test suite type is not found, return the fully qualified test class name with an empty description.
            if (testSuite == null)
            {
                return (Name: context.FullyQualifiedTestClassName, Description: string.Empty);
            }
            // Get the DisplayName attribute from the test suite type.
            var attribute = testSuite.GetCustomAttribute<DisplayNameAttribute>();

            // If the DisplayName attribute is not found or is empty, return the fully qualified test class name with an empty description.
            if (attribute == null || string.IsNullOrEmpty(attribute.DisplayName))
            {
                return (Name: context.FullyQualifiedTestClassName, Description: string.Empty);
            }

            // Return the fully qualified test class name and the display name from the attribute.
            return (Name: context.FullyQualifiedTestClassName, Description: attribute.DisplayName);
        }

        /// <summary>
        /// Retrieves the test method's display name and description from the TestContext.
        /// </summary>
        /// <param name="context">The TestContext instance.</param>
        /// <returns>A tuple containing the test method's name and display name.</returns>
        public static (string Name, string Description) GetTestDisplayName(this TestContext context)
        {
            // Get the private field "_testMethod" from the TestContext instance.
            var testMethod = context
                .GetType()
                .GetField("_testMethod", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(context);

            // Get the private property "DisplayName" from the test method instance.
            var displayName = testMethod
                .GetType()
                .GetProperty("DisplayName", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(testMethod);

            // Return the test method's name and display name as a tuple.
            return (context.TestName, $"{displayName}");
        }

        /// <summary>
        /// Retrieves the method marked with the [TestMethod] attribute from the current stack trace or the TestContext.
        /// </summary>
        /// <param name="context">The TestContext instance.</param>
        /// <returns>The MethodBase of the test method, or null if not found.</returns>
        public static MethodBase GetTestMethod(this TestContext context)
        {
            // Get the current stack trace
            var stackTrace = new StackTrace();

            // Loop through the stack frames
            for (int i = 0; i < stackTrace.FrameCount; i++)
            {
                // Get the method from the current stack frame
                var frame = stackTrace.GetFrame(i);
                var method = frame.GetMethod();

                // Check if the method has the [TestMethod] attribute
                if (method.GetCustomAttribute<DisplayNameAttribute>() != null)
                {
                    // Return the method if the attribute is found
                    return method;
                }
            }

            // Get the managed type and method names from the TestContext
            var typeName = context.ManagedType;
            var methodName = context.TestName;

            // Get the type from the loaded assemblies using the type name
            var type = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(i => i.GetTypes())
                .FirstOrDefault(i => i.FullName == typeName);

            // Return null if no method with the [TestMethod] attribute is found
            return type.GetMethod(methodName);
        }

        /// <summary>
        /// Retrieves the search and load timeouts from the setup model's environment.
        /// </summary>
        /// <param name="setupModel">The setup model containing configuration and environment information.</param>
        /// <returns>A tuple containing the search timeout and load timeout as <see cref="TimeSpan"/>.</returns>
        public static (TimeSpan SearchTimeout, TimeSpan LoadTimeout) GetTimeouts(this ObjectSetupModel setupModel)
        {
            // Get the search timeout value from the environment's test context properties, with a default of "10000" milliseconds.
            var searchTimeoutValue = setupModel
                .Environment
                .TestContext
                .Properties
                .Get(key: "WebDriver.SearchTimeout", defaultValue: "10000");

            // Try to parse the search timeout value to an integer.
            var isSearchTimeout = int.TryParse(searchTimeoutValue, out int searchTimeout);

            // Get the load timeout value from the environment's test context properties, with a default of "30000" milliseconds.
            var loadTimeoutValue = setupModel
                .Environment
                .TestContext
                .Properties
                .Get(key: "WebDriver.LoadTimeout", defaultValue: "30000");

            // Try to parse the load timeout value to an integer.
            var isLoadTimeout = int.TryParse(loadTimeoutValue, out int loadTimeout);

            // Use the parsed values if valid, otherwise use the default values.
            searchTimeout = isSearchTimeout ? searchTimeout : 10000;
            loadTimeout = isLoadTimeout ? loadTimeout : 30000;

            // Return the search timeout and load timeout as TimeSpan.
            return (TimeSpan.FromMilliseconds(searchTimeout), TimeSpan.FromMilliseconds(loadTimeout));
        }

        /// <summary>
        /// Initializes a new test run with a unique identifier based on the current date and time.
        /// </summary>
        /// <param name="context">The <see cref="TestContext"/> instance.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if no calling method with the <see cref="AssemblyInitializeAttribute"/> attribute is found in the call stack.
        /// </exception>
        public static void NewTestRun(this TestContext context)
        {
            // Generate a unique RunId based on the current date and time
            NewTestRun(context, id: DateTime.Now.ToString("yyyyMMddHHmmssfff"));
        }

        /// <summary>
        /// Initializes a new test run with the specified identifier.
        /// </summary>
        /// <param name="context">The <see cref="TestContext"/> instance.</param>
        /// <param name="id">The unique identifier for the test run.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if no calling method with the <see cref="AssemblyInitializeAttribute"/> attribute is found in the call stack.
        /// </exception>
        public static void NewTestRun(this TestContext context, string id)
        {
            // Validates that the calling method has the AssemblyInitializeAttribute attribute.
            // Cycles up the stack until no more levels or until a method with the AssemblyInitializeAttribute is found.
            static void ConfirmCallingMethodAttribute()
            {
                // Get the stack trace to examine the call stack
                var stackTrace = new StackTrace();

                // Loop through the stack frames starting from the second frame
                // (skip the first frame which is the current method itself)
                for (int i = 1; i < stackTrace.FrameCount; i++)
                {
                    // Get the current method from the stack frame
                    var method = stackTrace.GetFrame(i).GetMethod();

                    // Check if the current method has the [AssemblyInitialize] attribute
                    if (method.GetCustomAttributes(typeof(AssemblyInitializeAttribute), false).Length != 0)
                    {
                        // If the attribute is found, return without throwing an exception
                        return;
                    }
                }

                // If no method with the [AssemblyInitialize] attribute is found in the call stack,
                // throw an InvalidOperationException
                throw new InvalidOperationException("No calling method with the [AssemblyInitialize] attribute found in the call stack.");
            }

            // Validate that the calling method has the [AssemblyInitialize] attribute
            ConfirmCallingMethodAttribute();

            // Store the unique RunId in the context properties
            context.Properties["RunId"] = id;

            // Retrieve the report path from the context properties, defaulting to the TestResultsDirectory
            var reportPath = context.Properties.Get(key: "ExtentReport.ReportPath", context.TestResultsDirectory);

            // If the report path is ".", use the TestResultsDirectory
            reportPath = reportPath.Equals(".")
                ? context.TestResultsDirectory
                : reportPath;

            // Generate a file name for the test run reports
            var fileName = $"TestRun.{id}";

            // Create HTML and JSON reporters with the specified file paths
            var htmlReporter = new ExtentSparkReporter(Path.Combine(reportPath, id, $"{fileName}.html"));
            var jsonReporter = new ExtentJsonFormatter(Path.Combine(reportPath, id, $"{fileName}.json"));

            // Initialize the ExtentReports instance and attach the reporters
            var extent = new ExtentReports();
            extent.AttachReporter(htmlReporter, jsonReporter);

            // Store the ExtentReports instance in the context properties
            context.Properties["ExtentReport"] = extent;
        }

        /// <summary>
        /// Switches the current WebDriver in the setup model to a different one based on the specified key.
        /// </summary>
        /// <param name="setupModel">The setup model containing the drivers.</param>
        /// <param name="driverKey">The key of the driver to switch to.</param>
        /// <returns>The updated setup model with the switched WebDriver.</returns>
        public static ObjectSetupModel SwitchDriver(this ObjectSetupModel setupModel, string driverKey)
        {
            // Try to get the driver from the dictionary using the specified key
            var isDriver = setupModel.Drivers.TryGetValue(key: driverKey, out IWebDriver driver);

            // If the driver is found, switch the WebDriver in the setup model to the new driver
            if (isDriver)
            {
                setupModel.WebDriver = driver;
            }

            // Return the updated setup model
            return setupModel;
        }

        /// <summary>
        /// Updates the setup phase results of the test result.
        /// </summary>
        /// <param name="testResult">The test result to update.</param>
        /// <param name="setupResult">The setup phase result to use for updating.</param>
        /// <returns>The updated test result.</returns>
        public static TestResultModel UpdateSetupResults(this TestResultModel testResult, TestPhaseResultModel setupResult)
        {
            // Check if the phase name is "Setup"
            if (!setupResult.PhaseName.Equals("Setup", StringComparison.OrdinalIgnoreCase))
            {
                return testResult;
            }

            // Update the setup start time, end time, and duration
            testResult.SetupStartTime = setupResult.StartTime;
            testResult.SetupEndTime = setupResult.EndTime;
            testResult.SetupDuration = setupResult.Duration;

            // Collect exceptions if there are any
            var exceptions = new List<TestPhaseExceptionModel>();
            if (setupResult.PhaseException != null)
            {
                exceptions.Add(setupResult.PhaseException);
            }

            // Ensure the exceptions list is initialized
            testResult.Exceptions ??= [];

            // Concatenate existing exceptions with new exceptions
            testResult.Exceptions = testResult.Exceptions.Concat(exceptions).ToList();

            // return the updated test result
            return testResult;
        }

        /// <summary>
        /// Updates the teardown phase results of the test result.
        /// </summary>
        /// <param name="testResult">The test result to update.</param>
        /// <param name="teardownResult">The teardown phase result to use for updating.</param>
        /// <returns>The updated test result.</returns>
        public static TestResultModel UpdateTeardownResults(this TestResultModel testResult, TestPhaseResultModel teardownResult)
        {
            // Check if the phase name is "Teardown"
            if (!teardownResult.PhaseName.Equals("Teardown", StringComparison.OrdinalIgnoreCase))
            {
                return testResult;
            }

            // Update the teardown start time, end time, and duration
            testResult.TeardownStartTime = teardownResult.StartTime;
            testResult.TeardownEndTime = teardownResult.EndTime;
            testResult.TeardownDuration = teardownResult.Duration;

            // Collect exceptions if there are any
            var exceptions = new List<TestPhaseExceptionModel>();
            if (teardownResult.PhaseException != null)
            {
                exceptions.Add(teardownResult.PhaseException);
            }

            // Ensure the exceptions list is initialized
            testResult.Exceptions ??= [];

            // Concatenate existing exceptions with new exceptions
            testResult.Exceptions = testResult.Exceptions.Concat(exceptions).ToList();

            // return the updated test result
            return testResult;
        }
    }
}
