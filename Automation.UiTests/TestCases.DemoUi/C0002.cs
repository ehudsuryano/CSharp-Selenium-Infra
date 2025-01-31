using Automation.Testing.Framework.Ui.PageModels;
using Infrastructure.Core;
using Infrastructure.Models;
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
    internal class C0002(TestContext context) : TestCaseBase(context)
    {
        // Executes the main automation test.
        protected override void AutomationTest(ObjectSetupModel setupModel)
        {
            // Perform a search on Google and get the search results.
            var results = ModelFactory
                .New<GoogleSearchPage>(setupModel, arguments: "https://www.google.com")
                .Search("Automation Testing")
                .First()
                .Select()
                .GetResults();
            // Assert that there are search results.
            Assert.IsTrue(results.Any(), "Search results must not be empty.");
        }
    }
}
