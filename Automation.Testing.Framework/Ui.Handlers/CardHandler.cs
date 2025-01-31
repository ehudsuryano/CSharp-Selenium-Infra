using Infrastructure.Core;
using Infrastructure.Extensions;
using Infrastructure.Models;
using OpenQA.Selenium;
using System;

namespace Automation.Testing.Framework.Ui.Handlers
{
    /// <summary>
    /// Handles interactions with "cards" type div elements on a web page.
    /// </summary>
    /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
    /// <param name="query">The query to identify the "cards" type div element.</param>
    /// <param name="timeout">The timeout for waiting for the "cards" type div element.</param>
    public class CardHandler(ObjectSetupModel setupModel, string query, TimeSpan timeout)
        : UiControllerHandlerBase(setupModel, query, timeout)
    {
        /// <inheritdoc />
        public CardHandler(ObjectSetupModel setupModel)
            : this(setupModel, query: string.Empty, timeout: setupModel.GetTimeouts().SearchTimeout)
        { }

        /// <inheritdoc />
        public CardHandler(ObjectSetupModel setupModel, string query)
            : this(setupModel, query, timeout: setupModel.GetTimeouts().SearchTimeout)
        { }

        /// <inheritdoc />
        public CardHandler(ObjectSetupModel setupModel, TimeSpan timeout)
            : this(setupModel, query: string.Empty, timeout)
        { }

        /// <inheritdoc />
        protected override IWebElement OnGetElement(string cardName)
        {
            // Construct the XPath to find the textbox element with the specified query.
            var xpath =
                $"//div//h5[text()='{cardName}']";

            // Create a By object using the constructed XPath.
            var by = By.XPath(xpath);

            // Wait until the textbox element is found and return it.
            return Wait.Until(driver => driver.FindElement(by));
        }
    }
}
