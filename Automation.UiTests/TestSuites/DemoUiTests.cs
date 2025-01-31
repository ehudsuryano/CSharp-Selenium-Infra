using Automation.UiTests.TestCases.DemoUi;
using Infrastructure.Core;

using OpenQA.Selenium.Chrome;

using System.ComponentModel;

namespace Automation.UiTests.TestSuites
{
    [TestClass, DisplayName("Demo UI Tests")]
    public class DemoUiTests : TestSuiteBase
    {
        // This method is automatically called before each test method in the test suite, ensuring that
        // the WebDriver is properly configured with the necessary options for each test.
        // The TestContext object is used to share configuration and state across test methods.
        protected override void OnInitializeTest()
        {
            // Set the WebDriver options for the test context.
            TestContext.Properties["WebDriver.Options"] = new ChromeOptions();
        }

        [TestMethod(displayName: "Verify Google Search Functionality with WebDriver")]

        public void T0001()
        {
            // Set the WebDriver options for the test context.
            // This will override the default options set in the OnInitializeTest method.
            TestContext.Properties["WebDriver.Options"] = new ChromeOptions();

            // Create a new instance of the C0001 test case with the current test context.
            var actualResult = new C0001(TestContext).Invoke();

            // Assert that the test case passed.
            Assert.IsTrue(actualResult.Passed);
        }

        [TestMethod(displayName: "Verify Google Search Functionality with Models and Handlers")]

        public void T0002()
        {
            // Set the WebDriver options for the test context.
            // This will override the default options set in the OnInitializeTest method.
            TestContext.Properties["WebDriver.Options"] = new ChromeOptions();

            // Create a new instance of the C0001 test case with the current test context.
            var actualResult = new C0002(TestContext).Invoke();

            // Assert that the test case passed.
            Assert.IsTrue(actualResult.Passed);
        }

        [TestMethod(displayName: "Load DemoQA.com")]

        public void T0003()
        {
            // Set the WebDriver options for the test context.
            // This will override the default options set in the OnInitializeTest method.
            TestContext.Properties["WebDriver.Options"] = new ChromeOptions();

            // Create a new instance of the C0001 test case with the current test context.
            var actualResult = new C0003(TestContext).Invoke();

            // Assert that the test case passed.
            Assert.IsTrue(actualResult.Passed);
        }
    }
}
