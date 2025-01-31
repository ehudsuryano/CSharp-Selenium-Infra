using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Uia.Client
{
    /// <summary>
    /// Represents the service for the UI Automation (UIA) driver, managing the lifecycle of the driver service executable.
    /// </summary>
    public class UiaDriverService : DriverService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UiaDriverService"/> class with the specified service path, port, and executable name.
        /// </summary>
        /// <param name="servicePath">The path to the service executable.</param>
        /// <param name="port">The port on which the service should listen.</param>
        /// <param name="driverServiceExecutableName">The name of the driver service executable.</param>
        public UiaDriverService(string servicePath, int port, string driverServiceExecutableName)
            : this(servicePath, port, driverServiceExecutableName, string.Empty)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiaDriverService"/> class with the specified service path, port, executable name, and .NET path.
        /// </summary>
        /// <param name="servicePath">The path to the service executable.</param>
        /// <param name="port">The port on which the service should listen.</param>
        /// <param name="driverServiceExecutableName">The name of the driver service executable.</param>
        /// <param name="dotnetPath">The path to the .NET executable, if applicable.</param>

        public UiaDriverService(string servicePath, int port, string driverServiceExecutableName, string dotnetPath)
            : base(servicePath, port, driverServiceExecutableName)
        {
            // If the driver service executable is not a .dll file, set the command line arguments for the executable.
            if (!driverServiceExecutableName.EndsWith(".dll"))
            {
                CommandLineArguments = $"--port {port}";
                return;
            }

            // If the executable is a .dll file, set the .NET path and adjust the command line arguments accordingly.
            DriverServicePath = dotnetPath;
            DriverServiceExecutableName = Path.Combine(dotnetPath, "dotnet");
            CommandLineArguments = $"{Path.Combine(servicePath, $"{driverServiceExecutableName}")} --port {port}";
        }

        /// <summary>
        /// Gets the default driver options for the UIA driver.
        /// </summary>
        /// <returns>A <see cref="UiaOptions"/> object with default settings.</returns>
        protected override DriverOptions GetDefaultDriverOptions()
        {
            return new UiaOptions
            {
                App = "Desktop"
            };
        }

        /// <summary>
        /// Gets the command line arguments for the driver service.
        /// </summary>
        protected override string CommandLineArguments { get; }

        /// <summary>
        /// Creates a new default instance of the <see cref="UiaDriverService"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="UiaDriverService"/> class.</returns>
        public static UiaDriverService NewDefaultService()
        {
            return NewDefaultService(servicePath: null);
        }

        /// <summary>
        /// Creates a new default instance of the <see cref="UiaDriverService"/> class with the specified service path.
        /// </summary>
        /// <param name="servicePath">The path to the service executable.</param>
        /// <returns>A new instance of the <see cref="UiaDriverService"/> class.</returns>
        public static UiaDriverService NewDefaultService(string servicePath)
        {
            return NewDefaultService(servicePath, dotnetPath: null);
        }

        /// <summary>
        /// Creates a new default instance of the <see cref="UiaDriverService"/> class with the specified service path and .NET path.
        /// </summary>
        /// <param name="servicePath">The path to the service executable.</param>
        /// <param name="dotnetPath">The path to the .NET executable, if applicable.</param>
        /// <returns>A new instance of the <see cref="UiaDriverService"/> class.</returns>
        public static UiaDriverService NewDefaultService(string servicePath, string dotnetPath)
        {
            return new UiaDriverService(
                servicePath,
                port: PortUtilities.FindFreePort(),
                driverServiceExecutableName: !string.IsNullOrEmpty(dotnetPath) ? "Uia.DriverServer.dll" : "Uia.DriverServer.exe",
                dotnetPath);
        }
    }
}
