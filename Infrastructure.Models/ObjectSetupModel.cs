using AventStack.ExtentReports;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Net.Http;

namespace Infrastructure.Models
{
    /// <summary>
    /// Represents the model for setting up automation objects.
    /// </summary>
    public class ObjectSetupModel
    {
        /// <summary>
        /// Gets an instance of <see cref="System.Net.Http.HttpClient"/> configured for the setup model.
        /// </summary>
        public static HttpClient HttpClient => new(new HttpClientHandler
        {
            AllowAutoRedirect = true,
            UseCookies = true
        });

        /// <summary>
        /// Gets or sets the automation context for the setup.
        /// </summary>
        public AutomationEnvironment Environment { get; set; }

        /// <summary>
        /// Gets or sets the ExtentTest instance used for reporting.
        /// </summary>
        public ExtentTest ExtentReports { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of web drivers used in the setup.
        /// The keys are driver names and the values are instances of <see cref="IWebDriver"/>.
        /// </summary>
        public IDictionary<string, IWebDriver> Drivers { get; set; }

        /// <summary>
        /// Gets or sets the primary web driver used in the setup.
        /// </summary>
        public IWebDriver WebDriver { get; set; }
    }
}
