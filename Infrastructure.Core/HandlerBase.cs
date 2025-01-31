using Infrastructure.Models;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

using System;

namespace Infrastructure.Core
{
    /// <summary>
    /// Represents the base handler class that provides common functionality for handling web elements.
    /// </summary>
    public abstract class HandlerBase : ModelBase 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerBase"/> class with the specified setup model.
        /// Uses a default query and timeout.
        /// </summary>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        protected HandlerBase(ObjectSetupModel setupModel)
            : this(setupModel, query: string.Empty, timeout: TimeSpan.FromSeconds(10)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerBase"/> class with the specified setup model and query.
        /// Uses a default timeout.
        /// </summary>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        /// <param name="query">The query to find the web element.</param>
        protected HandlerBase(ObjectSetupModel setupModel, string query)
            : this(setupModel, query, timeout: TimeSpan.FromSeconds(10)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerBase"/> class with the specified setup model and timeout.
        /// Uses a default query.
        /// </summary>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        /// <param name="timeout">The timeout for waiting for the web element.</param>
        protected HandlerBase(ObjectSetupModel setupModel, TimeSpan timeout)
            : this(setupModel, query: string.Empty, timeout) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerBase"/> class with the specified setup model, query, and timeout.
        /// </summary>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        /// <param name="query">The query to find the web element.</param>
        /// <param name="timeout">The timeout for waiting for the web element.</param>
        protected HandlerBase(ObjectSetupModel setupModel, string query, TimeSpan timeout)
            : base(setupModel)
        {
            // Initialize the WebDriverWait with the specified timeout.
            Wait = new WebDriverWait(setupModel.WebDriver, timeout);

            // Get the web element based on the query.
            WebElement = GetElement(query);
        }

        /// <summary>
        /// Gets or sets the WebDriverWait instance used for waiting for web elements.
        /// </summary>
        public WebDriverWait Wait { get; set; }

        /// <summary>
        /// Gets or sets the web element found using the query.
        /// </summary>
        public IWebElement WebElement { get; set; }

        /// <summary>
        /// Abstract method to get the web element based on the query.
        /// </summary>
        /// <param name="query">The query to find the web element.</param>
        /// <returns>The web element found using the query.</returns>
        protected abstract IWebElement GetElement(string query);
    }
}
