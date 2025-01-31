using OpenQA.Selenium;
using System.Collections.Generic;


namespace Uia.Client
{
    /// <summary>
    /// Represents the options for UI Automation (UIA) driver.
    /// </summary>
    public class UiaOptions : DriverOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UiaOptions"/> class with default values.
        /// </summary>
        public UiaOptions()
        {
            UiaOptionsDictionary = [];
        }

        /// <summary>
        /// Gets or sets the additional UIA options.
        /// </summary>
        public Dictionary<string, object> UiaOptionsDictionary { get; }

        /// <summary>
        /// Gets or sets the application to be automated.
        /// </summary>
        public string App
        {
            get => GetOption<string>("app");
            set => SetOption("app", value);

        }

        /// <summary>
        /// Gets or sets the arguments for the application.
        /// </summary>
        public string[] Arguments
        {
            get => GetOption<string[]>("arguments");
            set => SetOption("arguments", value);
        }

        /// <summary>
        /// Gets or sets the binary location for the UIA driver.
        /// </summary>
        public override string BinaryLocation { get; set; }

        /// <summary>
        /// Gets or sets the impersonation options for the UIA driver.
        /// Setting this will start the application process as the impersonated user.
        /// </summary>
        public ImpersonationOptions Impersonation
        {
            get => GetOption<ImpersonationOptions>("impersonation");
            set => SetOption("impersonation", value);
        }

        /// <summary>
        /// Gets or sets the label for the UIA driver.
        /// This helps to target a driver on a specific machine when using a grid.
        /// </summary>
        public string Label
        {
            get => GetOption<string>("label");
            set => SetOption("label", value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to scope into an existing open application.
        /// </summary>
        public bool Mount
        {
            get => GetOption<bool>("mount");
            set => SetOption("mount", value);
        }

        /// <summary>
        /// Gets or sets the working directory for the application.
        /// </summary>
        public string WorkingDirectory
        {
            get => GetOption<string>("workingDirectory");
            set => SetOption("workingDirectory", value);
        }

        /// <summary>
        /// Returns the capabilities required for UIA in a dictionary format.
        /// </summary>
        /// <returns>A dictionary of capabilities.</returns>

        public override ICapabilities ToCapabilities()
        {
            // Generate the desired capabilities with default values
            var capabilities = GenerateDesiredCapabilities(true);

            // Set the specific capabilities for the UIA driver
            capabilities.SetCapability("browserName", "uia");
            capabilities.SetCapability("platformName", "windows");
            capabilities.SetCapability("uia:options", UiaOptionsDictionary);

            // Return the capabilities as a read-only dictionary
            return capabilities.AsReadOnly();
        }

        /// <summary>
        /// Provides the default capabilities for the UIA WebDriver.
        /// </summary>
        /// <returns>A dictionary of default capabilities.</returns>
        public ICapabilities DefaultCapabilities()
        {
            // Generate the desired capabilities with default values
            var capabilities = GenerateDesiredCapabilities(true);

            // Set the default capabilities for the UIA driver
            capabilities.SetCapability("browserName", "uia");
            capabilities.SetCapability("platform", "windows");
            capabilities.SetCapability("uia:options", new { app = "Desktop" });

            // Return the capabilities as a read-only dictionary
            return capabilities.AsReadOnly();
        }

        // Gets the value of the specified option from the UIA options dictionary.
        private T GetOption<T>(string key)
        {
            // Check if the UIA options dictionary contains the specified key.
            return UiaOptionsDictionary.TryGetValue(key, out object value)
                ? (T)value
                : default;
        }

        // Sets the value of the specified option in the UIA options dictionary.
        private void SetOption<T>(string key, T value)
        {
            // Set the value of the specified key in the UIA options dictionary.
            UiaOptionsDictionary[key] = value;
        }

        /// <summary>
        /// Represents the impersonation options for UI Automation (UIA) driver.
        /// </summary>
        public class ImpersonationOptions
        {
            /// <summary>
            /// Gets or sets the domain for impersonation.
            /// </summary>
            public string Domain { get; set; }

            /// <summary>
            /// Gets or sets the username for impersonation.
            /// </summary>
            public string Username { get; set; }

            /// <summary>
            /// Gets or sets the password for impersonation.
            /// </summary>
            public string Password { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether impersonation is enabled.
            /// </summary>
            public bool Enabled { get; set; }
        }
    }
}
