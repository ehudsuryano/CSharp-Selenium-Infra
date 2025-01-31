using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace POM_DEMOQA
{
    public abstract class BasePage
    {
        protected readonly IWebDriver Driver;
        private readonly WebDriverWait _wait;
        private readonly Actions actions;

       protected BasePage(IWebDriver driver, int timeoutInSeconds = 3)
        {
            Driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            actions = new Actions(driver);
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
