using Infrastructure.Core;
using Infrastructure.Extensions;
using Infrastructure.Models;

using OpenQA.Selenium;

using System;

namespace Automation.Testing.Framework.Ui.Handlers
{
    /// <summary>
    /// Handles interactions with button elements on a web page.
    /// </summary>
    /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
    /// <param name="query">The query to identify the button element.</param>
    /// <param name="timeout">The timeout for waiting for the button element.</param>
    public class ButtonHandler(ObjectSetupModel setupModel, string query, TimeSpan timeout)
        : UiControllerHandlerBase(setupModel, query, timeout)
    {
        /// <inheritdoc />
        public ButtonHandler(ObjectSetupModel setupModel)
            : this(setupModel, query: string.Empty, timeout: setupModel.GetTimeouts().SearchTimeout)
        { }

        /// <inheritdoc />
        public ButtonHandler(ObjectSetupModel setupModel, string query)
            : this(setupModel, query, timeout: setupModel.GetTimeouts().SearchTimeout)
        { }

        /// <inheritdoc />
        public ButtonHandler(ObjectSetupModel setupModel, TimeSpan timeout)
            : this(setupModel, query: string.Empty, timeout)
        { }

        /// <inheritdoc />
        protected override IWebElement OnGetElement(string query)
        {
            // Construct the XPath to find the button element with the specified query.
            var xpath = $"//input[@type='button' and @id='submit']|"+
                        $"//input[@type='submit' and @role='button' and @value='{query}']|"+
                        $"//input[@type='submit' and @role='button' and @value='{query}']|";

            // Create a By object using the constructed XPath.
            var by = By.XPath(xpath);

            // Wait until the button element is found and return it.
            return Wait.Until(driver => driver.FindElement(by));
        }
    }
}
