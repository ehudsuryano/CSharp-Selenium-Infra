using Automation.Testing.Framework.Ui.Handlers;
using Infrastructure.Core;
using Infrastructure.Models;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Testing.Framework.Ui.PageModels
{
    public class DemoqaFormsPage : ModelBase
    {
        /// <inheritdoc />
        public DemoqaFormsPage(ObjectSetupModel setupModel)
            : base(setupModel)
        { }

        /// <inheritdoc />
        public DemoqaFormsPage(ObjectSetupModel setupModel, string url)
            : base(setupModel, url)
        { }


        public DemohqSideMenuItemHandler ClickSideMenuItem(string itemName)
        {
            return AuditableAction(action: "ClickSideMenuItem", (setupModel) =>
            {
                // Create a new instance of TextboxHandler for the search box with name 'q'.
                var menuItem = ModelFactory.New<DemohqSideMenuItemHandler>(setupModel, itemName);

                // Send the search query to the search box element.
                menuItem.WebElement.Click();

                return menuItem;
            });
        }

        public IWebDriver ScrollToElement(string itemName, IWebDriver driver)
        {
            IWebElement element = driver.FindElement(By.XPath($"//*[text()='{itemName}']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            return driver;
        }
    }
}
