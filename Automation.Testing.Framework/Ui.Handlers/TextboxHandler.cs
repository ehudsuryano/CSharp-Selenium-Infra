using Infrastructure.Core;
using Infrastructure.Extensions;
using Infrastructure.Models;
using OpenQA.Selenium;
using System;

namespace Automation.Testing.Framework.Ui.Handlers
{
    /// <summary>
    /// Handles interactions with textbox elements on a web page.
    /// </summary>
    /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
    /// <param name="query">The query to identify the textbox element.</param>
    /// <param name="timeout">The timeout for waiting for the textbox element.</param>
    public class TextboxHandler(ObjectSetupModel setupModel, string query, TimeSpan timeout)
        : UiControllerHandlerBase(setupModel, query, timeout)
    {
        /// <inheritdoc />
        public TextboxHandler(ObjectSetupModel setupModel)
            : this(setupModel, query: string.Empty, timeout: setupModel.GetTimeouts().SearchTimeout)
        { }

        /// <inheritdoc />
        public TextboxHandler(ObjectSetupModel setupModel, string query)
            : this(setupModel, query, timeout: setupModel.GetTimeouts().SearchTimeout)
        { }

        /// <inheritdoc />
        public TextboxHandler(ObjectSetupModel setupModel, TimeSpan timeout)
            : this(setupModel, query: string.Empty, timeout)
        { }

        /// <inheritdoc />
        protected override IWebElement OnGetElement(string query)
        {
            // Construct the XPath to find the textbox element with the specified query.
            var xpath =
                $"//textarea[@name='{query}' or @title='{query}'or @id='{query}']|" +
                $"//input[@name='{query}' and (@type='text' or @type='password')]|" +
                $"//input[@title='{query}' and (@type='text' or @type='password')]|"+
                $"//input[@id='{query}' and (@type='text' or @type='password'or @type='email')]";

            // Create a By object using the constructed XPath.
            var by = By.XPath(xpath);

            // Wait until the textbox element is found and return it.
            return Wait.Until(driver => driver.FindElement(by));
        }
    }
}
