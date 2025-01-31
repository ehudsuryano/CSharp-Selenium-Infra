using AventStack.ExtentReports;
using Infrastructure.Extensions;
using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileProviders;


namespace Automation.UiTests
{
    /// <summary>
    /// Represents a base class for test suites, providing common setup and cleanup functionality.
    /// </summary>
    [TestClass]
    [DeploymentItem(path: "ExtentReports", "ExtentReports")]
    [DeploymentItem(path: "Fonts", "Fonts")]
    public abstract class TestSuiteBase
    {
        // Static property to hold the TestContext
        private static TestContext s_testContext;

        /// <summary>
        /// Cleans up the assembly after tests are completed. This method is used to copy report-related assets,
        /// flush ExtentReports, and modify the report's HTML to use local assets.
        /// </summary>
        [AssemblyCleanup]
        public static void ClearAssembly()
        {
            try
            {
                // Get the path where the report is stored.
                var reportPath = s_testContext.GetReportPath();

                // Extract the RunId from the report path to use it in the HTML file name.
                var id = Path.GetFileName(reportPath);

                // Copy the ExtentReports assets and Fonts to the assets directory.
                Utilities.CopyDirectory("ExtentReports", Path.Combine(reportPath, "assets"));
                Utilities.CopyDirectory("Fonts", Path.Combine(reportPath, "fonts"));

                // Flush the ExtentReports to ensure all the test data is written out.
                s_testContext.GetExtentReports().Flush();

                // Read the HTML report file and modify its contents to reference local assets.
                var htmlReportPath = Path.Combine(reportPath, $"TestRun.{id}.html");
                var htmlReport = File.ReadAllText(htmlReportPath);
                htmlReport = Regex.Replace(
                    input: htmlReport,
                    pattern: @"https?:\/\/[^\/\s""']+\/(?:[^\/\s""']*\/)*",
                    replacement: "./assets/");
                string resultPath = Directory.GetParent(Environment.CurrentDirectory)?.Parent.ToString();
                Utilities.CopyFile(htmlReportPath, resultPath);
                // Write the modified HTML content to a new file named 'TestRun.datetime.html'.
                //File.WriteAllText(path: @"E:\reports", contents: htmlReport);      
                //Utilities.CopyDirectory(reportPath, "E:\\reports");  //there is a bug that deletes the files once program done running this line is temp solution
            }
            catch (Exception e)
            {
                // Log any errors that occur during the cleanup process.
                s_testContext.WriteLine($"Error flushing the ExtentReports: {e.GetBaseException().Message}");
            }
        }


        /// <summary>
        /// One-time setup for the entire test assembly. This method is called once before any tests are run.
        /// </summary>
        /// <param name="context">The test context providing information about the current test run.</param>
        [AssemblyInitialize]
        public static void InitializeAssembly(TestContext context)
        {
            // Start a new test run and set the RunId
            context.NewTestRun();
            RunId = context.Properties.Get(key: "RunId", defaultValue: string.Empty);

            // Store the test context
            s_testContext = context;
        }

        /// <summary>
        /// Initialization for the test class. This method is called once before any tests in the test class are run.
        /// </summary>
        /// <param name="context">The test context providing information about the current test run.</param>
        [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void InitializeClass(TestContext context)
        {
            // Get the ExtentReports instance and the suite display name
            var extent = s_testContext.GetExtentReports();
            var (name, description) = context.GetSuiteDisplayName();

            // Create a new test in the ExtentReports for the test suite
            s_testContext.Properties[name] = extent.CreateTest(name, description);
        }

        /// <summary>
        /// Initializes the test. This method is called before each test method.
        /// </summary>
        [TestInitialize]
        public void InitializeTest()
        {
            // Get the ExtentReports instance and the suite display name
            var suiteName = TestContext.GetSuiteDisplayName().Name;

            // Get the test display name and description
            var (name, description) = TestContext.GetTestDisplayName();

            // Retrieve the ExtentTest instance for the suite from the shared test context
            var testSuite = s_testContext.Properties.Get<ExtentTest>(key: suiteName);

            // Store the test node and RunId in the context properties for use in the test
            TestContext.Properties["TestNode"] = testSuite?.CreateNode(name, description);
            TestContext.Properties["RunId"] = RunId;

            // Call any additional initialization steps defined in derived classes
            OnInitializeTest();
        }

        /// <summary>
        /// Provides additional initialization steps for derived classes. This method is called
        /// by <see cref="InitializeTest" /> to allow derived classes to perform additional setup.
        /// </summary>
        protected virtual void OnInitializeTest()
        {
            // Derived classes can override this method to perform additional initialization steps
        }

        /// <summary>
        /// Gets or sets the unique RunId for the test run.
        /// </summary>
        public static string RunId { get; set; }

        /// <summary>
        /// Gets or sets the test context providing information about the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }
    }
}