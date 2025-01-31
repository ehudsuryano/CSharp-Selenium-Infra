using OpenQA.Selenium;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Uia.Client
{
    /// <summary>
    /// Provides extension methods for UI automation using WebDriver.
    /// </summary>
    public static class UiaExtensions
    {
        // Initialize the static HttpClient instance for sending requests to the WebDriver server
        private static HttpClient HttpClient => new();

        // Initialize the static JsonSerializerOptions instance for deserializing JSON responses
        private static JsonSerializerOptions JsonSerializerOptions => new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Gets the UI Automation attribute value of a web element.
        /// </summary>
        /// <param name="element">The web element to get the attribute from.</param>
        /// <param name="name">The name of the attribute.</param>
        /// <returns>The attribute value as a string.</returns>
        public static string GetUiaAttribute(this IWebElement element, string name)
        {
            // Get the session ID and element ID from the web element
            var (sessionId, elementId) = GetRouteData(element);

            // Get the WebDriver instance from the web element
            var driver = ((IWrapsDriver)element).WrappedDriver;

            // Get the remote server URI from the command executor
            var url = GetRemoteServerUri(driver);

            // Construct the route for the attribute command
            var requestUri = $"{url}/session/{sessionId}/element/{elementId}/attribute/{name}";

            // Create the HTTP request for the attribute
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            // Send the HTTP request
            var response = HttpClient.Send(request);

            // Read the response content and deserialize the JSON response
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent, JsonSerializerOptions);

            // Try to get the value from the response object
            var isValue = responseObject.TryGetValue("value", out var value);

            // Ensure the response is successful
            response.EnsureSuccessStatusCode();

            // Return the value as a string if it exists, otherwise return an empty string
            return isValue ? $"{value}" : string.Empty;
        }

        /// <summary>
        /// Sends a native click command to a web element.
        /// </summary>
        /// <param name="element">The web element to click.</param>
        public static void SendNativeClick(this IWebElement element)
        {
            // Get the session ID and element ID from the web element
            var (sessionId, elementId) = GetRouteData(element);

            // Get the WebDriver instance from the web element
            var driver = ((IWrapsDriver)element).WrappedDriver;

            // Get the remote server URI from the command executor
            var url = GetRemoteServerUri(driver);

            // Construct the route for the native click command
            var requestUri = $"{url}/session/{sessionId}/user32/element/{elementId}/click";

            // Create the HTTP request for the native click
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            // Send the HTTP request
            var response = HttpClient.Send(request);

            // Ensure the response is successful
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Sends a native double-click command to a web element.
        /// </summary>
        /// <param name="element">The web element to double-click.</param>
        public static void SendNativeDoubleClick(this IWebElement element)
        {
            // Get the session ID and element ID from the web element
            var (sessionId, elementId) = GetRouteData(element);

            // Get the WebDriver instance from the web element
            var driver = ((IWrapsDriver)element).WrappedDriver;

            // Get the remote server URI from the command executor
            var url = GetRemoteServerUri(driver);

            // Construct the route for the native double-click command
            var requestUri = $"{url}/session/{sessionId}/user32/element/{elementId}/dclick";

            // Create the HTTP request for the native double-click
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            // Send the HTTP request
            var response = HttpClient.Send(request);

            // Ensure the response is successful
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Sets focus on a web element.
        /// </summary>
        /// <param name="element">The web element to set focus on.</param>
        public static void SetFocus(this IWebElement element)
        {
            // Get the session ID and element ID from the web element
            var (sessionId, elementId) = GetRouteData(element);

            // Get the WebDriver instance from the web element
            var driver = ((IWrapsDriver)element).WrappedDriver;

            // Get the remote server URI from the command executor
            var url = GetRemoteServerUri(driver);

            // Construct the route for the native focus command
            var requestUri = $"{url}/session/{sessionId}/user32/element/{elementId}/focus";

            // Create the HTTP request for the native focus
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            // Send the HTTP request
            var response = HttpClient.Send(request);

            // Ensure the response is successful
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Sends input scan codes to the WebDriver server.
        /// </summary>
        /// <param name="driver">The WebDriver instance.</param>
        /// <param name="codes">The scan codes to send.</param>
        public static void SendInputs(this IWebDriver driver, params string[] codes)
        {
            SendInputs(driver, 1, codes);
        }

        /// <summary>
        /// Sends input scan codes to the WebDriver server multiple times.
        /// </summary>
        /// <param name="driver">The WebDriver instance.</param>
        /// <param name="repeat">The number of times to repeat the input.</param>
        /// <param name="codes">The scan codes to send.</param>
        public static void SendInputs(this IWebDriver driver, int repeat, params string[] codes)
        {
            // Get the session ID from the WebDriver
            var sessionId = $"{((IHasSessionId)driver).SessionId}";

            // Get the remote server URI from the command executor
            var url = GetRemoteServerUri(driver);

            // Construct the route for the input simulation command
            var requestUri = $"{url}/session/{sessionId}/user32/inputs";

            // Create the request body with the scan codes to send to the server for input simulation
            var requestBody = new ScanCodesInputModel
            {
                ScanCodes = codes
            };

            // Send the HTTP request and ensure the response is successful for the specified number of repeats
            for (var i = 0; i < repeat; i++)
            {
                // Create the HTTP content for the request body
                var content = new StringContent(
                    content: JsonSerializer.Serialize(requestBody, JsonSerializerOptions),
                    encoding: Encoding.UTF8,
                    mediaType: "application/json");

                // Create the HTTP request for the input simulation
                var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
                {
                    Content = content
                };

                // Send the HTTP request
                var response = HttpClient.Send(request);

                // Ensure the response is successful
                response.EnsureSuccessStatusCode();

                // Wait for a short interval before sending the next input
                Thread.Sleep(100);
            }
        }
        /// <summary>
        /// Sends text input to the WebDriver server.
        /// </summary>
        /// <param name="driver">The WebDriver instance.</param>
        /// <param name="text">The text to send.</param>
        public static void SendKeys(this IWebDriver driver, string text)
        {
            SendKeys(driver, repeat: 1, text);
        }

        /// <summary>
        /// Sends text input to the WebDriver server multiple times.
        /// </summary>
        /// <param name="driver">The WebDriver instance.</param>
        /// <param name="repeat">The number of times to repeat the input.</param>
        /// <param name="text">The text to send.</param>
        public static void SendKeys(this IWebDriver driver, int repeat, string text)
        {
            // Get the session ID from the WebDriver
            var sessionId = $"{((IHasSessionId)driver).SessionId}";

            // Get the remote server URI from the command executor
            var url = GetRemoteServerUri(driver);

            // Construct the route for the text input command
            var requestUri = $"{url}/session/{sessionId}/user32/value";

            // Create the request body with the text to send to the server for input simulation
            var requestBody = new TextInputModel
            {
                Text = text
            };

            // Send the HTTP request and ensure the response is successful for the specified number of repeats
            for (var i = 0; i < repeat; i++)
            {
                // Create the HTTP content for the request body
                var content = new StringContent(
                    content: JsonSerializer.Serialize(requestBody, JsonSerializerOptions),
                    encoding: Encoding.UTF8,
                    mediaType: "application/json");

                // Create the HTTP request for the text input
                var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
                {
                    Content = content
                };

                // Send the HTTP request
                var response = HttpClient.Send(request);

                // Ensure the response is successful
                response.EnsureSuccessStatusCode();

                // Wait for a short interval before sending the next input
                Thread.Sleep(100);
            }
        }

        // Gets the remote server URI from the WebDriver command executor.
        private static string GetRemoteServerUri(IWebDriver driver)
        {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;

            // Get the command executor from the WebDriver
            var invoker = ((IHasCommandExecutor)driver).CommandExecutor;

            // Get the remote server URI from the command executor using reflection
            return invoker
                .GetType()
                .GetField("remoteServerUri", bindingFlags)
                .GetValue(invoker)
                .ToString()
                .Trim('/');
        }

        // Gets the session ID and element ID from a web element.
        private static (string SessionId, string ElementId) GetRouteData(IWebElement element)
        {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;

            // Get the WebDriver instance from the web element
            var driver = ((IWrapsDriver)element).WrappedDriver;

            // Get the session ID from the WebDriver
            var sessionId = $"{((IHasSessionId)driver).SessionId}";

            // Use reflection to get the element ID from the web element
            var elementId = $"{element.GetType().GetField("elementId", bindingFlags).GetValue(element)}";

            // Return the session ID and element ID as a tuple
            return (sessionId, elementId);
        }

        /// <summary>
        /// Model for sending scan codes as input.
        /// </summary>
        private sealed class ScanCodesInputModel
        {
            /// <summary>
            /// Gets or sets the collection of scan codes.
            /// </summary>
            /// <value>A collection of scan codes required for the operation.</value>
            [Required]
            public IEnumerable<string> ScanCodes { get; set; }
        }

        /// <summary>
        /// Model for sending text input.
        /// </summary>
        private sealed class TextInputModel
        {
            /// <summary>
            /// Gets or sets the text to be input.
            /// </summary>
            /// <value>The text to be input.</value>
            [Required]
            public string Text { get; set; }
        }
    }
}
