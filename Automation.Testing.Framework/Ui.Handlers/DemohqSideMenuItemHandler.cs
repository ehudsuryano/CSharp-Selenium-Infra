using Infrastructure.Core;
using Infrastructure.Extensions;
using Infrastructure.Models;
using OpenQA.Selenium;

namespace Automation.Testing.Framework.Ui.Handlers
{
    public class DemohqSideMenuItemHandler(ObjectSetupModel setupModel, string query, TimeSpan timeout)
        : UiControllerHandlerBase(setupModel, query, timeout)
    {
        public DemohqSideMenuItemHandler(ObjectSetupModel setupModel)
            : this(setupModel, query: string.Empty, timeout: setupModel.GetTimeouts().SearchTimeout)
        { }

        /// <inheritdoc />
        public DemohqSideMenuItemHandler(ObjectSetupModel setupModel, string query)
            : this(setupModel, query, timeout: setupModel.GetTimeouts().SearchTimeout)
        { }

        /// <inheritdoc />
        public DemohqSideMenuItemHandler(ObjectSetupModel setupModel, TimeSpan timeout)
            : this(setupModel, query: string.Empty, timeout)
        { }

        protected override IWebElement OnGetElement(string itemName)
        {
            // Return null if the query is null or empty.
            if (string.IsNullOrEmpty(query))
            {
                return default;
            }

            // Construct the XPath to find the list item element with the specified query.
            var xpath = $"//*[text()='{itemName}']";

            // Create a By object using the constructed XPath.
            var by = By.XPath(xpath);

            // Wait until the list item element is found and return it.
            return Wait.Until(driver => driver.FindElement(by));
        }
    }
}
