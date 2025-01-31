using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace POM
{
    public abstract class Basepage
    {
        protected readonly IWebDriver Driver;
        private readonly WebDriverWait _wait;

        protected Basepage(IWebDriver driver, int timeoutInSeconds = 3) 
        {
            Driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

        }

        // Waits for an element to be visible
        protected IWebElement WaitForElementToBeVisible(By locator)
        {
            return _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
        }

        // Waits for an element to be clickable
        protected IWebElement WaitForElementToBeClickable(By locator)
        {
            return _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
        }

        public string GetPageTitle()
        {
            return Driver.Title;
        }

        public void NavigateTo(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

    }
}
