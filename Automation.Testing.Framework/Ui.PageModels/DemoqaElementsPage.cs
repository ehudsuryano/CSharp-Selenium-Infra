using Automation.Testing.Framework.Ui.ComponentModels;
using Automation.Testing.Framework.Ui.Handlers;
using Infrastructure.Core;
using Infrastructure.Models;
using Infrastructure.Extensions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Testing.Framework.Ui.PageModels
{
    public class DemoqaElementsPage : ModelBase
    {
        /// <inheritdoc />
        public DemoqaElementsPage(ObjectSetupModel setupModel)
            : base(setupModel)
        { }

        /// <inheritdoc />
        public DemoqaElementsPage(ObjectSetupModel setupModel, string url)
            : base(setupModel, url)
        { }


        public DemoqaElementsPage ClickSideMenuItem(string itemName)
        {
            return AuditableAction(action: "ClickSideMenuItem", (setupModel) =>
            {
                // Create a new instance of TextboxHandler for the search box with name 'q'.
                var menuItem = ModelFactory.New<DemohqSideMenuItemHandler>(setupModel, itemName);

                // Send the search query to the search box element.
                menuItem.WebElement.Click();

                return this;
            });
        }

        public IWebDriver ScrollToElement(string itemName, IWebDriver driver)
        {
            IWebElement element = driver.FindElement(By.XPath($"//*[text()='{itemName}']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            return driver;
        }

        public DemoqaElementsPage TypeText(string textbox, string text)
        {
            return AuditableAction(action: "ClickSideMenuItem", (setupModel) =>
            {
                var box = ModelFactory.New<TextboxHandler>(setupModel, textbox);

                box.WebElement.SendKeys(text);

                return this;
            });
        }

       

        public void FillTextBoxForm(string itemName)
        {

        }


        

        public void ClickExpandCollaps(string Action)
        {

        }
    }
}
