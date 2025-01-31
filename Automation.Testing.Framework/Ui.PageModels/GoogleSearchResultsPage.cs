using Infrastructure.Models;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using Infrastructure.Core;

namespace Automation.Testing.Framework.Ui.PageModels
{
    /// <summary>
    /// Represents the google results page model for interacting with search results.
    /// </summary>
    public class GoogleSearchResultsPage : ModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultsPageModel"/> class with the specified setup model.
        /// </summary>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        public GoogleSearchResultsPage(ObjectSetupModel setupModel) : base(setupModel)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultsPageModel"/> class with the specified setup model and URL.
        /// </summary>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        /// <param name="url">The URL to navigate to.</param>
        public GoogleSearchResultsPage(ObjectSetupModel setupModel, string url) : base(setupModel, url)
        { }

        /// <summary>
        /// Retrieves the search results from the results page.
        /// </summary>
        /// <returns>An enumerable collection of web elements representing the search results.</returns>
        public IEnumerable<IWebElement> GetResults()
        {
            // Create a new WebDriverWait instance with a timeout of 10 seconds.
            var wait = new WebDriverWait(SetupModel.WebDriver, TimeSpan.FromSeconds(10));

            // Wait until the search result elements are found and return them.
            return wait.Until(d =>
            {
                // Find the search result elements using CSS selector.
                var elements = d.FindElements(By.CssSelector("div.g"));

                // Return null if no elements are found.
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
