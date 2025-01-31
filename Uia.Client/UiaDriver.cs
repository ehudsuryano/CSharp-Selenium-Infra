using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Uia.Client
{
    /// <summary>
    /// Represents the UI Automation (UIA) driver, providing capabilities to interact with UI elements using the UIA protocol.
    /// </summary>
    /// <param name="service">The UIA driver service to use.</param>
    /// <param name="options">The UIA options to use.</param>
    /// <param name="commandTimeout">The command timeout duration.</param>
    public class UiaDriver(UiaDriverService service, UiaOptions options, TimeSpan commandTimeout)
        : RemoteWebDriver(commandExecutor: NewCommandExecutor(service, commandTimeout), capabilities: options.ToCapabilities())
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UiaDriver"/> class with default service, options, and command timeout.
        /// </summary>
        public UiaDriver()
            : this(
                  service: UiaDriverService.NewDefaultService(),
                  options: new UiaOptions(),
                  commandTimeout: DefaultCommandTimeout) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiaDriver"/> class with the specified options and default service and command timeout.
        /// </summary>
        /// <param name="options">The UIA options to use.</param>
        public UiaDriver(UiaOptions options)
            : this(service: UiaDriverService.NewDefaultService(), options, commandTimeout: DefaultCommandTimeout) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiaDriver"/> class with the specified UIA driver directory, default options, and command timeout.
        /// </summary>
        /// <param name="uiaDriverDirectory">The directory where the UIA driver is located.</param>
        public UiaDriver(string uiaDriverDirectory)
            : this(
                  service: UiaDriverService.NewDefaultService(uiaDriverDirectory),
                  options: new UiaOptions(),
                  commandTimeout: DefaultCommandTimeout) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiaDriver"/> class with the specified service, default options, and command timeout.
        /// </summary>
        /// <param name="service">The UIA driver service to use.</param>
        public UiaDriver(UiaDriverService service)
            : this(service, options: new UiaOptions(), commandTimeout: DefaultCommandTimeout) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiaDriver"/> class with the specified UIA driver directory, options, and command timeout.
        /// </summary>
        /// <param name="uiaDriverDirectory">The directory where the UIA driver is located.</param>
        /// <param name="options">The UIA options to use.</param>
        public UiaDriver(string uiaDriverDirectory, UiaOptions options)
            : this(service: UiaDriverService.NewDefaultService(uiaDriverDirectory), options, commandTimeout: DefaultCommandTimeout) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiaDriver"/> class with the specified UIA driver directory, options, and command timeout.
        /// </summary>
        /// <param name="uiaDriverDirectory">The directory where the UIA driver is located.</param>
        /// <param name="options">The UIA options to use.</param>
        /// <param name="commandTimeout">The command timeout duration.</param>
        public UiaDriver(string uiaDriverDirectory, UiaOptions options, TimeSpan commandTimeout)
            : this(service: UiaDriverService.NewDefaultService(uiaDriverDirectory), options, commandTimeout) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiaDriver"/> class with the specified service, options, and command timeout.
        /// </summary>
        /// <param name="service">The UIA driver service to use.</param>
        /// <param name="options">The UIA options to use.</param>
        public UiaDriver(UiaDriverService service, UiaOptions options)
            : this(service, options, commandTimeout: DefaultCommandTimeout) { }

        /// <summary>
        /// Creates a new instance of <see cref="DriverServiceCommandExecutor"/> for the specified service, options, and command timeout.
        /// </summary>
        /// <param name="service">The driver service to use.</param>
        /// <param name="commandTimeout">The command timeout duration.</param>
        /// <returns>A new instance of <see cref="DriverServiceCommandExecutor"/>.</returns>
        private static DriverServiceCommandExecutor NewCommandExecutor(DriverService service, TimeSpan commandTimeout)
        {
            // Set the driver service path to an empty string if it is null.
            service.DriverServicePath ??= string.Empty;

            // Create and return a new instance of DriverServiceCommandExecutor with the configured service and command timeout.
            return new DriverServiceCommandExecutor(service, commandTimeout);
        }

        /// <summary>
        /// Disposes the driver instance, releasing all resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether the method is being called from the Dispose method.</param>
        protected override void Dispose(bool disposing)
        {
            // Call the base class Dispose method with false to release resources.
            base.Dispose(disposing: false);
        }
    }
}
