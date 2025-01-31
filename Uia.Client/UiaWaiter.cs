using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;


namespace Uia.Client
{
    /// <summary>
    /// Provides a mechanism to wait for conditions in the UI Automation (UIA) context.
    /// </summary>
    /// <param name="driver">The WebDriver instance to be used.</param>
    /// <param name="ignoredException">The list of exceptions to ignore while waiting.</param>
    /// <param name="timeout">The timeout duration for the wait.</param>
    public class UiaWaiter(IWebDriver driver, List<Type> ignoredException, TimeSpan timeout)
    {
        // Initialize the private fields with the provided values from the constructor parameters
        private readonly IWebDriver _driver = driver;
        private readonly List<Type> _ignoredExceptions = ignoredException;
        private readonly TimeSpan _timeout = timeout;

        /// <summary>
        /// Initializes a new instance of the <see cref="UiaWaiter"/> class with a default timeout of 10 seconds.
        /// </summary>
        /// <param name="driver">The WebDriver instance to be used.</param>
        public UiaWaiter(IWebDriver driver) : this(driver, ignoredException: [], timeout: TimeSpan.FromSeconds(10)){ }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiaWaiter"/> class.
        /// </summary>
        /// <param name="driver">The WebDriver instance to be used.</param>
        /// <param name="timeout">The timeout duration for the wait.</param>
        
        public UiaWaiter(IWebDriver driver , TimeSpan timeout) : this(driver, ignoredException: [], timeout) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiaWaiter"/> class with a default timeout of 10 seconds.
        /// </summary>
        /// <param name="driver">The WebDriver instance to be used.</param>
        /// <param name="ignoredException">The list of exceptions to ignore while waiting.</param>
        
        public UiaWaiter(IWebDriver driver, List<Type> ignoredException) : this (driver , ignoredException , timeout: TimeSpan.FromSeconds(10)) { }

        /// <summary>
        /// Waits until a condition is met or the timeout is reached.
        /// </summary>
        /// <param name="condition">The condition to be met.</param>
        /// <returns>The web element that satisfies the condition.</returns>
        /// <exception cref="NoSuchElementException">Thrown if the condition is not met within the timeout period.</exception>

        public IWebElement Until(Func<IWebDriver, IWebElement> condition)
        {
            // Calculate the time when the wait should timeout
            var conditionTimeout = DateTime.Now.Add(_timeout);

            do
            {
                try
                {
                    // Try to evaluate the condition
                    return condition(_driver);
                }
                catch (Exception e)
                {
                    // Check if the exception should be ignored
                    if (_ignoredExceptions.Exists(i => i == e.GetType()))
                    {
                        // Wait for a short interval before retrying
                        Thread.Sleep(100);
                        continue;
                    }
                    // Rethrow the exception if it's not to be ignored
                    throw;
                }
            } while (DateTime.Now < conditionTimeout);

            // Throw an exception if the condition is not met within the timeout
            throw new NoSuchElementException();
        }
    }
}
