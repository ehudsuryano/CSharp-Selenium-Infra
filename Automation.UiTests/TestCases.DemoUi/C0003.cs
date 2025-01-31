using Automation.Testing.Framework.Ui.PageModels;
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
    internal class C0003(TestContext context) : TestCaseBase(context)
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
            var demohq = ModelFactory.New<DemoqaHomePage>(setupModel, arguments: "https://demoqa.com/")
                .ClickElements()
                .ClickSideMenuItem("Text Box")
                .TypeText("userName", "Deadpool")
                .TypeText("userEmail", "Deadpool@gmail.com")
                .TypeText("currentAddress", "Current Address Israel")
                .TypeText("permanentAddress", "Rishon Le'zion");

            Assert.AreEqual("https://demoqa.com/text-box", setupModel.WebDriver.Url);

            
            Thread.Sleep(5000);
        }
    }
}
