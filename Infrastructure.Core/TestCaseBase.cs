using AventStack.ExtentReports;
using Infrastructure.Core.Extensions;
using Infrastructure.Exceptions;
using Infrastructure.Extensions;
using Infrastructure.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium;

using System;
using System.Collections.Generic;
using System.Threading;

namespace Infrastructure.Core
{
    /// <summary>
    /// Represents the base class for test cases with setup and teardown functionality.
    /// </summary>
    public abstract class TestCaseBase
    {
        // The time interval between test case attempts.
        private readonly int _attemptsInterval;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestCaseBase"/> class.
        /// </summary>
        /// <param name="context">The test context providing properties and methods for the test case.</param>
        protected TestCaseBase(TestContext context)
        {
            // Initialize the automation environment or use an existing one
            AutomationEnvironment ??= new AutomationEnvironment(context);

            // Parse and set the time interval between test case attempts
            _attemptsInterval = int.Parse(context.Properties.Get(key: "TestSettings.AttemptsDelay", defaultValue: "15000"));

            // Assign the provided test context to the TestContext property
            TestContext = context;

            // Parse and set the number of test case attempts
            NumberOfAttempts = int.Parse(context.Properties.Get(key: "TestSettings.NumberOfAttempts", defaultValue: "1"));

            // Set the application under test from the test context properties
            ApplicationUnderTest = context.Properties.Get(key: "TestSettings.ApplicationUnderTest", defaultValue: string.Empty);
        }
        /// <summary>
        /// Gets or sets the name of the application under test.
        /// </summary>
        public string ApplicationUnderTest { get; set; }

        /// <summary>
        /// Gets the automation environment for the test case.
        /// </summary>
        public AutomationEnvironment AutomationEnvironment { get; private set; }

        /// <summary>
        /// Gets or sets the number of attempts for the test case.
        /// </summary>
        public int NumberOfAttempts { get; set; }

        /// <summary>
        /// Gets the test context providing properties and methods for the test case.
        /// </summary>
        public TestContext TestContext { get; }

        /// <summary>
        /// Sets the automation environment for the test case.
        /// </summary>
        /// <param name="environment">The automation environment to set.</param>
        /// <returns>The current instance of the TestCase for method chaining.</returns>
        public TestCaseBase SetEnvironment(AutomationEnvironment environment)
        {
            // Ensure that TestData and TestParameters dictionaries are initialized if not already
            environment.TestData ??= new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            environment.TestParameters ??= new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            // Set the provided environment
            AutomationEnvironment = environment;

            // Return the current instance of the TestCase for method chaining
            return this;
        }

        /// <summary>
        /// Invokes the test case and returns the result.
        /// </summary>
        /// <returns>The result of the test case.</returns>
        public TestResultModel Invoke()
        {
            // Retrieve the number of attempts from the automation environment or use the default value from the test case
            NumberOfAttempts = AutomationEnvironment.ContextProperties.TryGetValue("NumberOfAttempts", out object value)
                ? int.Parse($"{value}")
                : NumberOfAttempts;
            // Initialize the test result model
            var testResult = new TestResultModel();

            // Create a list to store any exceptions that occur during the test case attempts
            var exceptions = new List<TestPhaseExceptionModel>();

            // Initialize the 'attempts' variable to 1
            var attempts = 1;

            // Retrieve the ExtentTest instance from the test context properties for the current test case node
            var extentReports = TestContext.Properties["TestNode"] as ExtentTest;

            // Create a new setup model for the test case
            var setupModel = new ObjectSetupModel
            {
                Environment = AutomationEnvironment
            };

            // Iterate through the test case attempts
            for (int i = 1; i <= NumberOfAttempts; i++)
            {
                // Create a new node in the ExtentReports for the current attempt
                setupModel.ExtentReports = extentReports.CreateNode($"Attempt {i}");

                // Set the current attempt number in the test context properties
                TestContext.Properties["Attempt"] = i;

                // Set the current attempt number
                attempts = i;

                // Invoke the test case for the current iteration
                testResult = Invoke(attempt: i, testCase: this, setupModel);

                // Add any exceptions from the current iteration to the list of exceptions
                exceptions.AddRange(testResult.Exceptions);

                // Set the number of attempts in the actual test results
                testResult.AttemptNumber = attempts;

                // Set the exceptions in the actual test results
                testResult.Exceptions = exceptions;

                // Retrieve the 'Metrics' key from the test data dictionary in the environment and set it in
                // the actual test results if found or an empty dictionary if not found
                var isMetrics = setupModel.Environment.TestData.TryGetValue(key: "Metrics", out object metrics);
                testResult.Metrics = isMetrics
                    ? metrics as Dictionary<string, object>
                    : [];

                // Retrieve the 'AuditableActions' key from the test data dictionary in the environment and set it in
                // the actual test results if found or an empty dictionary if not found
                var isAuditableActions = setupModel.Environment.TestData.TryGetValue(key: "AuditableActions", out object auditableActions);
                testResult.AuditableActions = isAuditableActions
                    ? auditableActions as Dictionary<string, object>
                    : [];

                // Retrieve the 'FindElementsDuration' key from the test data dictionary in the environment
                var findElementsDuration = setupModel.Environment.TestData.Get(key: "FindElementsDuration", defaultValue: 0L);
                testResult.FindElementsDuration = TimeSpan.FromTicks(findElementsDuration);

                // Calculate the total time taken for the setup phase
                testResult.SetupDuration = testResult.SetupEndTime - testResult.SetupStartTime;

                // Calculate the total time taken for the teardown phase
                testResult.TeardownDuration = testResult.TeardownEndTime - testResult.TeardownStartTime;

                // Calculate the total time taken for the test case
                testResult.TestDuration = testResult.TestEndTime - testResult.TestStartTime;

                // Set the display name for the test case
                testResult.DisplayName = setupModel.Environment.TestContext.GetTestDisplayName().Description;

                // Set the test class name for the test case
                testResult.TestClassName = TestContext.FullyQualifiedTestClassName;

                // Set the test method name for the test case
                testResult.TestMethodName = TestContext.TestName;

                // Determine the overall result based on the 'actual' test parameter from the environment
                var result = testResult.Passed ? "Pass" : "Fail";

                // Write the completion message to the test context
                TestContext.WriteLine($"Test case '{GetType().Name}' has completed with a result of '{result}'.");

                // Check the result of the test execution
                if (testResult.Passed)
                {
                    // Log a pass message to the ExtentReports if the test passed
                    extentReports.Pass($"The test case '{TestContext.TestName}' has " +
                        $"successfully passed on attempt number {attempts}.");
                }
                else
                {
                    // Log a fail message to the ExtentReports if the test failed
                    extentReports.Fail($"The test case '{TestContext.TestName}' failed after {attempts} attempts. " +
                        "Please review the logs and investigate the cause of the failure.");
                }

                // Write the metrics to the ExtentReports node for the test case iteration
                // and set the result of the test case iteration in the ExtentReports node
                // as well as the actual test result object for the test case iteration to
                // the ExtentReports node for the test case iteration to display the metrics
                // and results in the ExtentReports report for the test case iteration in the
                // test suite report  for the test case in the test suite report
                setupModel.ExtentReports.WriteMetrics(testResult);

                // If the test case passes, break the loop
                if (testResult.Passed)
                {
                    break;
                }

                // Sleep for a specified time before retrying the iteration if the
                // test case fails and there are more attempts remaining to try again
                // after the interval has passed and the number of attempts is not exceeded yet
                if (i < NumberOfAttempts)
                {
                    Thread.Sleep(_attemptsInterval);
                }
            }

            // Return the result of the test case
            return testResult;
        }

        // Invokes a single iteration of a test case within a specific environment.
        private static TestResultModel Invoke(int attempt, TestCaseBase testCase, ObjectSetupModel setupModel)
        {
            // Creates a new instance of TestPhaseExceptionModel using the provided exception.
            TestPhaseExceptionModel NewException(Exception e)
            {
                // Create a new instance of TestPhaseExceptionModel with the provided exception
                return new TestPhaseExceptionModel(e)
                {
                    // Assign the attempt reference to the new exception model
                    AttemptReference = attempt,

                    // Assign the context properties from the setup model's environment to the new exception model
                    Context = setupModel.Environment.ContextProperties,

                    // Assign the display name to the new exception model
                    DisplayName = setupModel.Environment.TestContext.GetTestDisplayName().Description,

                    // Initialize the screenshot property as an empty string
                    Screenshot = ((ITakesScreenshot)setupModel.WebDriver)?.GetScreenshot().AsBase64EncodedString
                };
            }

            // Create a new TestResultModel to store the results of the test case
            var testResult = new TestResultModel
            {
                RunId = setupModel.Environment.ContextProperties.Get(key: "RunId", defaultValue: $"{Guid.NewGuid()}")
            };

            // Create a list to store any exceptions that occur during the test case iteration
            var exceptions = new List<TestPhaseExceptionModel>();

            // Get the ExtentTest instance for logging
            var testNode = setupModel.ExtentReports;
            try
            {
                // Execute the preconditions for the test case
                var setupResult = Setup(testCase, setupModel);

                // Set the start time for the setup phase of the test case
                testResult.UpdateSetupResults(setupResult);

                // Add the setup exception to the list of exceptions
                if (setupResult.PhaseException != null)
                {
                    exceptions.Add(setupResult.PhaseException);
                    Assert.Inconclusive($"{setupResult.PhaseException.Exception}");
                }

                // Set the start time for the test case execution
                testResult.TestStartTime = DateTime.Now;

                // Execute the automation test for the test case and capture actual results and responses
                testCase.AutomationTest(setupModel);

                // Set the test case as passed if no exceptions were thrown during the test case execution
                // and no Assert.Inconclusive was called during the test case execution
                testResult.Passed = true;

                // Log the successful test case execution
                testNode.Pass($"Test case passed on attempt {attempt}.");

                // Return the test result to indicate that the test case passed
                return testResult;
            }
            catch (Exception e) when (e is NotImplementedException || e is AssertInconclusiveException)
            {
                // Handle exceptions that indicate inconclusive results
                var driver = setupModel.Environment.ContextProperties.Get(key: "WebDriver.Type", defaultValue: "N/A");
                var message = $"Unable to determine test results for the '{driver}' driver.";

                // Log the inconclusive test case execution
                testNode.Warning(message);

                // Mark the test as inconclusive with the provided message
                throw e is NotImplementedException
                    ? new AssertInconclusiveException(message, ex: e)
                    : new AssertInconclusiveException(message);
            }
            catch (Exception e) when (e is TestSetupException || e is TestTeardownException)
            {
                // Add the caught exception to the list of exceptions
                exceptions.Add(NewException(e));

                // Log the setup or teardown exception
                testNode.Fail($"Exception during setup or teardown: {e.Message}");

                // Handle setup and teardown exceptions that may occur during the test case iteration
                throw;
            }
            catch (Exception e)
            {
                // Create a new TestPhaseExceptionModel with the caught exception
                var exception = NewException(e);

                // Add the screenshot to the ExtentReports node for the test case iteration
                testNode.AddScreenCaptureFromBase64String(
                    base64: exception.Screenshot,
                    title: $"{testResult.RunId}.{testCase.GetType().Name}.{attempt}");

                // Add the caught exception to the list of exceptions
                exceptions.Add(exception);

                // Log the failure of the test case execution
                var message = $"Failed to invoke '{testCase.TestContext.ManagedMethod}' iteration. Exception: {e.GetBaseException()}";
                testNode.Fail(message);
                testCase.TestContext.WriteLine(message);

                // Return the test result to indicate that the test case failed
                return testResult;
            }
            finally
            {
                // Set the end time for the test case execution
                testResult.TestEndTime = DateTime.Now;

                // Set the exceptions in the test result object for the test case iteration
                // to the list of exceptions caught during the iteration of the test case execution
                testResult.Exceptions = exceptions;

                // Execute cleanup operations for the test case
                var teardownResult = Teardown(testCase, setupModel);

                // Update the test results with the teardown results
                testResult.UpdateTeardownResults(teardownResult);
            }
        }

        /// <summary>
        /// Method to be executed during the automation test.
        /// Must be implemented in a derived class to provide test execution logic.
        /// </summary>
        /// <param name="setupModel">The setup model containing the automation environment.</param>
        protected abstract void AutomationTest(ObjectSetupModel setupModel);

        /// <summary>
        /// Method to be executed during the setup phase of the test case.
        /// Override this method in a derived class to provide custom setup logic.
        /// </summary>
        /// <param name="setupModel">The setup model containing the automation environment.</param>
        protected virtual void OnSetup(ObjectSetupModel setupModel)
        {
            // Custom setup logic can be added here in derived classes
        }

        /// <summary>
        /// Method to be executed during the teardown phase of the test case.
        /// Override this method in a derived class to provide custom teardown logic.
        /// </summary>
        /// <param name="setupModel">The setup model containing the automation environment.</param>
        protected virtual void OnTearDown(ObjectSetupModel setupModel)
        {
            // Custom teardown logic can be added here in derived classes
        }

        // Handles the setup process for a given test case.
        private static TestPhaseResultModel Setup(TestCaseBase testCase, ObjectSetupModel setupModel)
        {
            // Create a new TestPhaseResultModel to store the results of
            // the setup phase for the test case iteration
            var result = new TestPhaseResultModel
            {
                StartTime = DateTime.Now,
                PhaseName = "Setup"
            };

            // Create a new node for the setup phase
            var setupNode = setupModel.ExtentReports.CreateNode("Setup Phase");

            try
            {
                // Initialize the web driver for the test case
                InitializeDriver(setupModel, driverNode: setupNode);

                // Invoke the setup method for the test case
                testCase.OnSetup(setupModel);

                // Set the result of the setup phase for the test case iteration to true
                result.IsSuccess = true;
                setupNode.Pass("Setup phase completed successfully.");
            }
            catch (Exception e)
            {
                // Create a new TestSetupException with the caught exception as the inner exception
                var setupException = new TestSetupException(
                    message: "An error occurred during test setup.",
                    innerException: e);

                // Set the result of the setup phase for the test case iteration to false
                result.IsSuccess = false;

                // Add the setup exception to the result object and set the exception as the
                // result of the setup phase for the test case iteration
                result.PhaseException = new TestPhaseExceptionModel(exception: setupException);
                setupNode.Fail($"Setup phase failed. Exception: {setupException.Message}");
            }
            finally
            {
                // Set the end time for the setup phase of the test case in the result object
                result.EndTime = DateTime.Now;

                // Calculate the duration of the setup phase and set the value in the result object
                result.Duration = result.EndTime - result.StartTime;
            }

            // Return the result of the setup phase for the test case iteration
            return result;
        }
        // Handles the teardown process for a given test case.
        private static TestPhaseResultModel Teardown(TestCaseBase testCase, ObjectSetupModel setupModel)
        {
            // Create a new TestPhaseResultModel to store the results of
            // the teardown phase for the test case iteration
            var result = new TestPhaseResultModel
            {
                StartTime = DateTime.Now,
                PhaseName = "Teardown"
            };

            // Create a new node for the teardown phase
            var teardownNode = setupModel.ExtentReports.CreateNode("Teardown Phase");

            try
            {
                // Invoke the teardown method for the test case
                testCase.OnTearDown(setupModel);

                // Set the result of the teardown phase for the test case iteration to true
                result.IsSuccess = true;
                teardownNode.Pass("Teardown phase completed successfully.");
            }
            catch (Exception e)
            {
                // Create a new TestTeardownException with the caught exception as the inner exception
                var teardownException = new TestTeardownException(
                    message: "An error occurred during test teardown.",
                    innerException: e);

                // Set the result of the teardown phase for the test case iteration to false
                result.IsSuccess = false;

                // Add the teardown exception to the result object and set the exception as the
                // result of the teardown phase for the test case iteration
                result.PhaseException = new TestPhaseExceptionModel(exception: teardownException);
                teardownNode.Fail($"Teardown phase failed. Exception: {teardownException.Message}");
            }
            finally
            {
                // Set the end time for the teardown phase of the test case in the result object
                result.EndTime = DateTime.Now;

                // Calculate the duration of the teardown phase and set the value in the result object
                result.Duration = result.EndTime - result.StartTime;

                // Clear the drivers from the setup model after the teardown
                // phase is complete to release resources properly
                setupModel.ClearDrivers();
                teardownNode.Info("Drivers cleared.");
            }

            // Return the result of the teardown phase for the test case iteration
            return result;
        }

        // Initializes a new instance of a web driver based on the provided setup model and assigns it to the setup model.
        private static void InitializeDriver(ObjectSetupModel setupModel, ExtentTest driverNode)
        {
            // Retrieve the driver type from the test parameters, defaulting to an empty string if not found
            var driverType = setupModel.Environment.ContextProperties.Get(key: "WebDriver.Type", defaultValue: string.Empty);

            // Retrieve the driver path from the test parameters, defaulting to the current directory if not found
            var driverPath = setupModel.Environment.ContextProperties.Get(key: "WebDriver.Endpoint", defaultValue: ".");

            // Retrieve the driver options from the test parameters, defaulting to an empty string if not found
            var driverOptions = setupModel
                .Environment
                .ContextProperties
                .Get(key: "WebDriver.Options", defaultValue: default(DriverOptions));

            // If the driver type is not specified, exit the method
            if (string.IsNullOrEmpty(driverType))
            {
                driverNode.Warning("Driver type is not specified. Skipping driver initialization.");
                return;
            }

            // If the driver path is not specified or is set to ".", use the current directory
            driverPath = string.IsNullOrEmpty(driverPath) || driverPath == "."
                ? Environment.CurrentDirectory
                : driverPath;

            try
            {
                // Create a new instance of the web driver using the DriverFactory
                var driver = driverOptions == default
                    ? DriverFactory.New(driverType, driverPath)
                    : DriverFactory.New(driverPath, driverOptions);

                // Assign the created web driver to the setup model's WebDriver property
                setupModel.WebDriver = driver;

                // Add or replace the driver in the setup model with the name "Main"
                setupModel.AddOrReplaceDriver(name: "Main", driver);

                // Log the successful driver initialization
                driverNode.Pass($"Web driver '{driverType}' at '{driverPath}' initialized successfully.");
            }
            catch (Exception e)
            {
                // Log the failure of the driver initialization
                driverNode.Fail($"Failed to initialize web driver. Exception: {e.Message}");
                throw;
            }
        }
    }
}
