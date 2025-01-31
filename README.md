# CSharp-Selenium-Infra

A robust and scalable Selenium testing infrastructure written in C# for automated web application testing. This framework follows the Page Object Model (POM) design pattern and includes features like cross-browser testing, detailed reporting, and parallel execution.

## Features

- **Selenium WebDriver Integration**: Automates web interactions efficiently.
- **Page Object Model (POM)**: Enhances maintainability and readability.
- **Extent Reports**: Generates detailed and structured test reports.
- **Cross-Browser Testing**: Supports multiple browsers, including Chrome, Firefox, and Edge.
- **Parallel Execution**: Runs multiple tests concurrently for improved efficiency.
- **MSTest Support**: Utilizes MSTest for test execution and management.
- **Configurable Test Execution**: Easily customize test execution settings.

## Installation

1. Clone the repository:
   ```sh
   git clone https://github.com/ehudsuryano/CSharp-Selenium-Infra.git
   ```
2. Open the project in Visual Studio.
3. Restore dependencies using NuGet Package Manager:
   ```sh
   dotnet restore
   ```

## Usage

### Running Tests
To execute tests, use the following command in the terminal:
```sh
   dotnet test
```
Alternatively, you can run tests from Visual Studio Test Explorer.

### Configuration
Modify the `appsettings.json` file to customize settings such as:
- Default browser
- Test environment URLs
- Reporting configurations

## Project Structure

```
CSharp-Selenium-Infra/
│── Tests/                  # Test cases organized by modules
│── Pages/                  # Page Object Model (POM) implementations
│── Utilities/              # Helper classes and utilities
│── Reports/                # Generated test reports
│── appsettings.json        # Configuration settings
│── TestBase.cs             # Base test class for common setup/teardown
│── README.md               # Project documentation
```

## Dependencies

- Selenium WebDriver
- MSTest
- ExtentReports
- Newtonsoft.Json

## Contributing
Contributions are welcome! Feel free to open an issue or submit a pull request.

## License
This project is licensed under the MIT License.

## Contact
For any questions or issues, reach out via [GitHub Issues](https://github.com/ehudsuryano/CSharp-Selenium-Infra/issues).

