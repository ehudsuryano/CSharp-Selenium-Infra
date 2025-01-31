using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using POM;

namespace seleniumpractice
{
    [TestClass]
    public class UnitTest1
    {
        private IWebDriver _driver;
        private Landingpage _landingpage;
        private Searchresults _searchresults;
        private Homepage _homepage;

        //private string query;
        [TestInitialize]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();

            // Initialize the login page object
            _landingpage = new Landingpage(_driver);
            _landingpage.NavigateTo("https://1337x.to/"); // Use NavigateTo from BasePage
        }
        [TestMethod]
        public void TestMethod1()
        {
            var query = "deadpool" ;
            // Act
            _landingpage.Typeinsearch($"{query}");
            _landingpage.ClicksearchButton();

            // Assert
            Assert.AreEqual($"https://1337x.to/search/{query}/1/", _driver.Url, "search was succsesful.");

            _searchresults = new Searchresults(_driver);
            query = "porn";
            _searchresults.Typeinsearch($"{query}");
            _searchresults.ClicksearchButton();
            Assert.AreEqual($"https://1337x.to/search/{query}/1/", _driver.Url, "search was succsesful.");
            _searchresults.ClickhomeButton();
            Assert.AreEqual("https://1337x.to/home/", _driver.Url, "search was succsesful.");
        }

        [TestCleanup]
        public void Teardown()
        {
            _driver.Quit();
        }
    }
}