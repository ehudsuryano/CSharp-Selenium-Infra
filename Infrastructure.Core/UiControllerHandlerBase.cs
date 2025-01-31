using Infrastructure.Extensions;
using Infrastructure.Models;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Infrastructure.Core
{
    /// <summary>
    /// Represents the base handler class that provides common functionality for handling web elements.
    /// </summary>
    public abstract class UiControllerHandlerBase : ModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UiControllerHandlerBase"/> class with the specified setup model.
        /// Uses a default query and timeout.
        /// </summary>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        protected UiControllerHandlerBase(ObjectSetupModel setupModel)
            : this(setupModel, query: string.Empty, timeout: TimeSpan.FromSeconds(10)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiControllerHandlerBase"/> class with the specified setup model and query.
        /// Uses a default timeout.
        /// </summary>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        /// <param name="query">The query to find the web element.</param>
        protected UiControllerHandlerBase(ObjectSetupModel setupModel, string query)
            : this(setupModel, query, timeout: TimeSpan.FromSeconds(10)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiControllerHandlerBase"/> class with the specified setup model and timeout.
        /// Uses a default query.
        /// </summary>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        /// <param name="timeout">The timeout for waiting for the web element.</param>
        protected UiControllerHandlerBase(ObjectSetupModel setupModel, TimeSpan timeout)
            : this(setupModel, query: string.Empty, timeout) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiControllerHandlerBase"/> class with the specified setup model, query, and timeout.
        /// </summary>
        /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
        /// <param name="query">The query to find the web element.</param>
        /// <param name="timeout">The timeout for waiting for the web element.</param>
        protected UiControllerHandlerBase(ObjectSetupModel setupModel, string query, TimeSpan timeout): base(setupModel)
        {
            // Initialize the WebDriverWait with the specified timeout.
            Wait = new WebDriverWait(setupModel.WebDriver, timeout);

            // Get the web element based on the query.
            if (!string.IsNullOrEmpty(query))
            {
                WebElement = GetElement(query);
            }
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
        /// Gets the web element based on the query and logs the query.
        /// </summary>
        /// <param name="query">The query to find the web element.</param>
        /// <returns>The web element found using the query.</returns>
        public IWebElement GetElement(string query)
        {
            // Log the query used to find the web element, including the current type name.
            SetupModel.ExtentReports.Info($"[{GetType().FullName}] Attempting to find element using query: '{query}'");

            // Get the search counter from the test data.
            var findElementsDuration = SetupModel.Environment.TestData.Get(key: "FindElementsDuration", defaultValue: 0L);

            // Start the stopwatch to measure the time taken to find the web element.
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                // Get the web element based on the query.
                return OnGetElement(query);
            }
            finally
            {
                // Stop the stopwatch and add the elapsed time to the search counter.
                stopwatch.Stop();
                findElementsDuration += stopwatch.Elapsed.Ticks;
                SetupModel.Environment.TestData["FindElementsDuration"] = findElementsDuration;
            }
        }

        /// <summary>
        /// Gets all web elements and logs the action.
        /// </summary>
        /// <returns>The collection of web elements found.</returns>
        public IEnumerable<IWebElement> GetElements()
        {
            // Log the action of finding all elements, including the current type name.
            SetupModel.ExtentReports.Info($"[{GetType().FullName}] Attempting to find all elements");

            // Get the search counter from the test data.
            var findElementsDuration = SetupModel.Environment.TestData.Get(key: "FindElementsDuration", defaultValue: 0L);

            // Start the stopwatch to measure the time taken to find the web elements.
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                // Get the web elements.
                return OnGetElements();
            }
            finally
            {
                // Stop the stopwatch and add the elapsed time to the search counter.
                stopwatch.Stop();
                findElementsDuration += stopwatch.Elapsed.Ticks;
                SetupModel.Environment.TestData["FindElementsDuration"] = findElementsDuration;
            }
        }

        /// <summary>
        /// Abstract method to get the web element based on the query.
        /// </summary>
        /// <param name="query">The query to find the web element.</param>
        /// <returns>The web element found using the query.</returns>
        protected abstract IWebElement OnGetElement(string query);

        /// <summary>
        /// Abstract method to get all web elements.
        /// </summary>
        /// <returns>The collection of web elements found.</returns>
        protected virtual IEnumerable<IWebElement> OnGetElements()
        {
            return [];
        }
    }
}
