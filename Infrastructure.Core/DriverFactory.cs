using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Uia.Client;

namespace Infrastructure.Core
{
    /// <summary>
    /// A factory class for creating instances of <see cref="IWebDriver"/> with various configurations.
    /// </summary>
    public static class DriverFactory
    {
        /// <summary>
        /// Creates a new instance of the specified <see cref="IWebDriver"/> type.
        /// </summary>
        /// <typeparam name="T">The specific <see cref="IWebDriver"/> implementation to instantiate.</typeparam>
        /// <returns>A newly created instance of the specified <see cref="IWebDriver"/> type.</returns>
        public static IWebDriver New<T>() where T : IWebDriver, new()
        {
            return NewDriver<T>(service: null, options: default);
        }

        /// <summary>
        /// Creates a new instance of the specified <see cref="IWebDriver"/> type with custom driver options.
        /// </summary>
        /// <typeparam name="T">The specific <see cref="IWebDriver"/> implementation to instantiate.</typeparam>
        /// <param name="options">Configuration options for the web driver.</param>
        /// <returns>A newly created instance of the specified <see cref="IWebDriver"/> type configured with the provided options.</returns>
        public static IWebDriver New<T>(DriverOptions options) where T : IWebDriver, new()
        {
            return NewDriver<T>(service: null, options);
        }

        /// <summary>
        /// Creates a new instance of the specified <see cref="IWebDriver"/> type with a custom driver service.
        /// </summary>
        /// <typeparam name="T">The specific <see cref="IWebDriver"/> implementation to instantiate.</typeparam>
        /// <param name="service">The driver service to use.</param>
        /// <returns>A newly created instance of the specified <see cref="IWebDriver"/> type configured with the provided service.</returns>
        public static IWebDriver New<T>(DriverService service)
            where T : IWebDriver, new()
        {
            return NewDriver<T>(service, options: default);
        }

        /// <summary>
        /// Creates a new instance of the specified <see cref="IWebDriver"/> type with both a custom driver service and options.
        /// </summary>
        /// <typeparam name="T">The specific <see cref="IWebDriver"/> implementation to instantiate.</typeparam>
        /// <param name="service">The driver service to use.</param>
        /// <param name="options">Configuration options for the web driver.</param>
        /// <returns>A newly created instance of the specified <see cref="IWebDriver"/> type configured with the provided service and options.</returns>
        public static IWebDriver New<T>(DriverService service, DriverOptions options)
            where T : IWebDriver, new()
        {
            return NewDriver<T>(service, options);
        }

        /// <summary>
        /// Creates a new instance of <see cref="RemoteWebDriver"/> configured with a remote address and custom options.
        /// </summary>
        /// <param name="remoteAddress">The URI of the remote WebDriver server.</param>
        /// <param name="options">Configuration options for the web driver.</param>
        /// <returns>A newly created instance of <see cref="RemoteWebDriver"/> configured with the specified remote address and options.</returns>
        public static IWebDriver New(Uri remoteAddress, DriverOptions options)
        {
            return NewDriver(remoteAddress, options);
        }

        /// <summary>
        /// Creates a new instance of <see cref="IWebDriver"/> using a specified driver path and options.
        /// </summary>
        /// <param name="driverPath">The file path to the web driver executable or the URI for a remote driver.</param>
        /// <param name="options">Configuration options for the web driver.</param>
        /// <returns>A newly created instance of <see cref="IWebDriver"/> based on the specified path and options.</returns>
        public static IWebDriver New(string driverPath, DriverOptions options)
        {
            return New(driverType: options.PlatformName, driverPath, options);
        }

        /// <summary>
        /// Creates a new instance of the specified web driver type (either local or remote) using the provided driver path.
        /// </summary>
        /// <param name="driverType">The type of web driver (e.g., "FIREFOX", "EDGE", "SAFARI", "CHROME").</param>
        /// <param name="driverPath">The file path to the web driver executable or the URI for a remote driver.</param>
        /// <returns>A newly created instance of <see cref="IWebDriver"/> based on the specified driver type and path.</returns>
        public static IWebDriver New(string driverType, string driverPath)
        {
            return New(driverType, driverPath, options: default);
        }

        /// <summary>
        /// Creates a new instance of the specified web driver type (either local or remote) using the provided driver path and options.
        /// </summary>
        /// <param name="driverType">The type of web driver (e.g., "FIREFOX", "EDGE", "SAFARI", "CHROME").</param>
        /// <param name="driverPath">The file path to the web driver executable or the URI for a remote driver.</param>
        /// <param name="options">Configuration options for the web driver.</param>
        /// <returns>A newly created instance of <see cref="IWebDriver"/> based on the specified driver type, path, and options.</returns>
        public static IWebDriver New(string driverType, string driverPath, DriverOptions options) 
        {
            // Helper method to create a new instance of the specified driver options type with common defaults.
            static T NewOptions<T>() where T : DriverOptions, new() => new();

            // Helper method to create a new instance of a remote web driver based on the specified URI.
            static IWebDriver NewRemote(string driverType, string uri, DriverOptions options)
            {
                // Default to the platform name if no driver type is provided
                driverType = string.IsNullOrEmpty(driverType) ? options?.PlatformName : driverType;

                // Default to a Chrome user data directory if no options are provided
                var defaultChromeOptions = new ChromeOptions();

                // Add the user data directory to the default Chrome options
                defaultChromeOptions.AddArgument("--user-data-dir=C:\\temp");
                defaultChromeOptions.AddArgument("--disable-extensions");
                defaultChromeOptions.AddArgument("--disable-gpu");
                defaultChromeOptions.AddArgument("--disable-dev-shm-usage");
                defaultChromeOptions.AddArgument("--no-sandbox");

                // Instantiate the appropriate remote web driver based on the driver type
                return driverType.ToUpper() switch
                {
                    "FIREFOX" => new RemoteWebDriver(new Uri(uri), options ?? NewOptions<FirefoxOptions>()),
                    "EDGE" => new RemoteWebDriver(new Uri(uri), options ?? NewOptions<EdgeOptions>()),
                    "SAFARI" => new RemoteWebDriver(new Uri(uri), options ?? NewOptions<SafariOptions>()),
                    "UIA" => new RemoteWebDriver(new Uri(uri), options ?? NewOptions<UiaOptions>()),
                    _ => new RemoteWebDriver(new Uri(uri), options ?? defaultChromeOptions)
                };
            }

            // Helper method to create a new instance of a local web driver based on the specified path.
            static IWebDriver NewLocal(string driverType, string uri, DriverOptions options)
            {
                // Default to the browser name if no driver type is provided
                driverType = string.IsNullOrEmpty(driverType) ? options?.BrowserName : driverType;

                // Default to a Chrome user data directory if no options are provided
                var defaultChromeOptions = new ChromeOptions();

                // Add the user data directory to the default Chrome options
                defaultChromeOptions.AddArgument("--user-data-dir=C:\\temp");
                defaultChromeOptions.AddArgument("--disable-extensions");
                defaultChromeOptions.AddArgument("--disable-gpu");
                defaultChromeOptions.AddArgument("--disable-dev-shm-usage");
                defaultChromeOptions.AddArgument("--no-sandbox");

                // Instantiate the appropriate local web driver based on the driver type
                return driverType.ToUpper() switch
                {
                    "FIREFOX" => new FirefoxDriver(uri, options != null ? (FirefoxOptions)options : NewOptions<FirefoxOptions>()),
                    "EDGE" => new EdgeDriver(uri, options != null ? (EdgeOptions)options : NewOptions<EdgeOptions>()),
                    "SAFARI" => new SafariDriver(uri, options != null ? (SafariOptions)options : NewOptions<SafariOptions>()),
                    "UIA" => new UiaDriver(uri, options != null ? (UiaOptions)options : NewOptions<UiaOptions>()),
                    _ => new ChromeDriver(uri, (ChromeOptions)options ?? defaultChromeOptions)
                };
            }
            // Determine if the provided driver path is a URI (indicating a remote driver)
            var isRemote = Regex.IsMatch(input: driverPath, pattern: "^http(s)?://");

            // Create and return the appropriate web driver instance based on the type (remote or local)
            return isRemote
                ? NewRemote(driverType, driverPath, options)
                : NewLocal(driverType, driverPath, options);
        }

        /// <summary>
        /// Internal method to create a new instance of the specified <see cref="IWebDriver"/> type with the given service and options.
        /// </summary>
        /// <typeparam name="T">The specific <see cref="IWebDriver"/> implementation to instantiate.</typeparam>
        /// <param name="service">The driver service to use (optional).</param>
        /// <param name="options">The driver options to use (optional).</param>
        /// <returns>A newly created instance of the specified <see cref="IWebDriver"/> type configured with the provided service and options.</returns>
        private static IWebDriver NewDriver<T>(DriverService service, DriverOptions options)
            where T : IWebDriver, new()
        {
            // Prepare a list to hold the constructor arguments
            var args = new List<object>();

            // Add the service to the argument list if provided
            if (service != default)
            {
                args.Add(service);
            }

            // Add the options to the argument list if provided
            if (options != default)
            {
                args.Add(options);
            }

            // Get the type of the web driver
            var type = typeof(T);

            // Instantiate and return the web driver with the specified arguments
            return (T)Activator.CreateInstance(type, args.ToArray());
        }

        /// <summary>
        /// Internal method to create a new instance of <see cref="RemoteWebDriver"/> with the specified remote address and options.
        /// </summary>
        /// <param name="remoteAddress">The URI of the remote WebDriver server.</param>
        /// <param name="options">Configuration options for the web driver.</param>
        /// <returns>A newly created instance of <see cref="RemoteWebDriver"/> configured with the specified remote address and options.</returns>
        private static RemoteWebDriver NewDriver(Uri remoteAddress, DriverOptions options)
        {
            return new RemoteWebDriver(remoteAddress, options);
        }
    }
}
