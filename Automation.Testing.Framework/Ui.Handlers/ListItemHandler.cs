using Infrastructure.Core;
using Infrastructure.Extensions;
using Infrastructure.Models;

using OpenQA.Selenium;

using System;
using System.Collections.Generic;

namespace Automation.Testing.Framework.Ui.Handlers
{
    /// <summary>
    /// Handles interactions with list item elements on a web page.
    /// </summary>
    /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
    /// <param name="query">The query to identify the list item element.</param>
    /// <param name="timeout">The timeout for waiting for the list item element.</param>
    public class ListItemHandler(ObjectSetupModel setupModel, string query, TimeSpan timeout)
        : UiControllerHandlerBase(setupModel, query, timeout)
    {
        /// <inheritdoc />
        public ListItemHandler(ObjectSetupModel setupModel)
            : this(setupModel, query: string.Empty, timeout: setupModel.GetTimeouts().SearchTimeout)
        { }

        /// <inheritdoc />
        public ListItemHandler(ObjectSetupModel setupModel, string query)
            : this(setupModel, query, timeout: setupModel.GetTimeouts().SearchTimeout)
        { }

        /// <inheritdoc />
        public ListItemHandler(ObjectSetupModel setupModel, TimeSpan timeout)
            : this(setupModel, query: string.Empty, timeout)
        { }

        /// <inheritdoc />
        protected override IWebElement OnGetElement(string query)
        {
            // Return null if the query is null or empty.
            if (string.IsNullOrEmpty(query))
            {
                return default;
            }

            // Construct the XPath to find the list item element with the specified query.
            var xpath = $"//ul[@role='listbox']/li[.//span[.='{query}']]";

            // Create a By object using the constructed XPath.
            var by = By.XPath(xpath);

            // Wait until the list item element is found and return it.
            return Wait.Until(driver => driver.FindElement(by));
        }

        /// <inheritdoc />
        protected override IEnumerable<IWebElement> OnGetElements()
        {
            // Define the XPath to find all list item elements within the list box.
            const string xpath = "//ul[@role='listbox']/li";

            // Create a By object using the defined XPath.
            var by = By.XPath(xpath);

            // Wait until the list item elements are found and return them.
            return Wait.Until(driver =>
            {
                // Find all elements matching the specified XPath.
                var elements = driver.FindElements(by);

                // If no elements are found, return null.
                if (elements.Count == 0)
                {
                    return null;
                }

                // Return the found elements.
                return elements;
            });
        }
    }
}
