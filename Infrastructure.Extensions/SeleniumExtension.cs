using HtmlAgilityPack;
using OpenQA.Selenium;

namespace Infrastructure.Extensions
{
    /// <summary>
    /// Provides extension methods for Selenium web elements.
    /// </summary>
    public static class SeleniumExtension
    {
        /// <summary>
        /// Retrieves the outer HTML of a specified <see cref="IWebElement"/> as an <see cref="HtmlDocument"/> object.
        /// This method uses JavaScript execution to obtain the outer HTML of the element from the web page.
        /// </summary>
        /// <param name="element">The <see cref="IWebElement"/> for which to retrieve the outer HTML.</param>
        /// <returns>An <see cref="HtmlDocument"/> object representing the outer HTML of the specified element.</returns>
        /// <exception cref="System.InvalidCastException">
        /// Thrown if the <paramref name="element"/> does not implement <see cref="IWrapsDriver"/> or if the driver does not support <see cref="IJavaScriptExecutor"/>.
        /// </exception>
        
        public static HtmlDocument GetOuterHtml(this IWebElement element)
        {
            // Retrieve the WebDriver instance associated with the provided web element.
            var driver = ((IWrapsDriver)element).WrappedDriver;

            // Execute JavaScript to get the outer HTML of the element. This script runs in the context of the web page.
            var html = ((IJavaScriptExecutor)driver)
                .ExecuteScript("return arguments[0].outerHTML;", element)
                .ToString();

            // Create a new HtmlDocument instance to parse the HTML string.
            var htmlDocument = new HtmlDocument();

            // Load the retrieved HTML into the HtmlDocument instance.
            htmlDocument.LoadHtml(html);

            // Return the parsed HtmlDocument containing the outer HTML of the specified element.
            return htmlDocument;
        }

        /// <summary>
        /// Clicks on a web element using JavaScript.
        /// </summary>
        /// <param name="element">The web element to click.</param>
        
        public static void JavaScriptClick(this IWebElement element)
        {
            // Get the WebDriver instance from the wrapped element.
            var driver = ((IWrapsDriver)element).WrappedDriver;

            // Cast the WebDriver to IJavaScriptExecutor.
            var executor = (IJavaScriptExecutor)driver;

            // Execute JavaScript to click the element.
            var html = ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].click();", element);      
        }

        /// <summary>
        /// Sets an attribute of a web element using JavaScript.
        /// </summary>
        /// <param name="element">The web element.</param>
        /// <param name="attributeName">The name of the attribute to set.</param>
        /// <param name="value">The value to set for the attribute.</param>
        public static void JavaScriptSetAttribute(this IWebElement element, string attributeName, string value)
        {
            // Get the WebDriver instance from the wrapped element.
            var driver = ((IWrapsDriver)element).WrappedDriver;

            // Cast the WebDriver to IJavaScriptExecutor.
            var executor = (IJavaScriptExecutor)driver;

            // Execute JavaScript to set the attribute value of the element.
            executor.ExecuteScript("arguments[0].setAttribute(arguments[1], arguments[2]);", element, attributeName, value);
        }

        /// <summary>
        /// Sets the value of a web element using JavaScript.
        /// </summary>
        /// <param name="element">The web element.</param>
        /// <param name="value">The value to set.</param>
        public static void JavaScriptSetValue(this IWebElement element, string value)
        {
            // Get the WebDriver instance from the wrapped element.
            var driver = ((IWrapsDriver)element).WrappedDriver;

            // Cast the WebDriver to IJavaScriptExecutor.
            var executor = (IJavaScriptExecutor)driver;

            // Execute JavaScript to set the value of the element.
            executor.ExecuteScript("arguments[0].value = arguments[1];", element, value);
        }

        /// <summary>
        /// Scrolls the web element into view using JavaScript.
        /// </summary>
        /// <param name="element">The web element to scroll into view.</param>
        public static void JavaScriptScrollIntoView(this IWebElement element)
        {
            JavaScriptScrollIntoView(element, alignToTop: true);
        }

        /// <summary>
        /// Scrolls the web element into view using JavaScript, with an option to align to the top of the viewport.
        /// </summary>
        /// <param name="element">The web element to scroll into view.</param>
        /// <param name="alignToTop">Whether to align the element to the top of the viewport.</param>
        public static void JavaScriptScrollIntoView(this IWebElement element, bool alignToTop)
        {
            // Get the WebDriver instance from the wrapped element.
            var driver = ((IWrapsDriver)element).WrappedDriver;

            // Cast the WebDriver to IJavaScriptExecutor.
            var executor = (IJavaScriptExecutor)driver;

            // Execute JavaScript to scroll the element into view.
            executor.ExecuteScript("arguments[0].scrollIntoView(arguments[1]);", element, alignToTop);
        }

        /// <summary>
        /// Scrolls to the bottom of the page using JavaScript.
        /// </summary>
        /// <param name="driver">The WebDriver instance.</param>
        public static void JavaScriptScrollToBottom(this IWebDriver driver)
        {
            // Cast the WebDriver to IJavaScriptExecutor.
            var executor = (IJavaScriptExecutor)driver;

            // Execute JavaScript to scroll to the bottom of the page.
            executor.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
        }

        /// <summary>
        /// Scrolls to the top of the page using JavaScript.
        /// </summary>
        /// <param name="driver">The WebDriver instance.</param>
        public static void JavaScriptScrollToTop(this IWebDriver driver)
        {
            // Cast the WebDriver to IJavaScriptExecutor.
            var executor = (IJavaScriptExecutor)driver;

            // Execute JavaScript to scroll to the top of the page.
            executor.ExecuteScript("window.scrollTo(0, 0)");
        }
    }
}
