using Infrastructure.Core;
using Infrastructure.Models;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.UiTests.TestCases.DemoUi
{
    /// <summary>
    /// Represents a test case for performing a simple Google search.
    /// </summary>
    /// <param name="context">The test context providing information about the test run.</param>
    internal class C0001(TestContext context) : TestCaseBase(context)
    {
        // Sets up the test environment before the test is run.
        protected override void OnSetup(ObjectSetupModel setupModel)
        {
            // Perform any necessary setup operations here.
        }

        // Cleans up the test environment after the test is run.
        protected override void OnTearDown(ObjectSetupModel setupModel)
        {
            // Perform any necessary teardown operations here.
        }

        // Executes the main automation test.
        protected override void AutomationTest(ObjectSetupModel setupModel)
        {
            // Log information about the test.
            setupModel.ExtentReports.Info("Starting automation test for Google search.");

            // Get the WebDriver instance from the setup model.
            var driver = setupModel.WebDriver;

            // Navigate to the Google homepage.
            driver.Navigate().GoToUrl("https://www.google.com");

            // Find the search box element by its name attribute.
            var searchBox = driver.FindElement(By.Name("q"));

            // Enter the search query into the search box.
            searchBox.SendKeys("Automation testing");

            // Assert that the search box is displayed.
            Assert.IsTrue(searchBox.Displayed);

            // Log that the search box is displayed.
            setupModel.ExtentReports.Info("Search box is displayed.");
        }
    }
}
