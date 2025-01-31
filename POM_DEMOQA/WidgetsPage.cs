using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;


namespace POM_DEMOQA
{
    public class WidgetsPage : BasePage
    {
        private readonly Actions actions;
        public WidgetsPage(IWebDriver driver) : base(driver)
        {
            actions = new Actions(driver);
        }

        public void ClickSideMenuItem(string itemName)
        {
            WaitForElementToBeVisible(By.XPath($"//*[text()='{itemName}']")).Click();
        }

        public void ClickBtn(string itemName)
        {
            WaitForElementToBeVisible(By.XPath($"//*[text()='{itemName}']")).Click();
        }

        public bool ElementExist(string itemName)
        {
            try
            {
                WaitForElementToBeVisible(By.Id(itemName));
                return true;
            }
            catch
            {
                return false;
            }

        }

        public IWebDriver ScrollToElement(string itemName, IWebDriver driver)
        {
            IWebElement element = driver.FindElement(By.XPath($"//*[text()='{itemName}']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            return driver;
        }

        public void DragElementToElement(string dragElement , string toElement)
        {
            var drag = WaitForElementToBeVisible(By.XPath($"//*[text()='{dragElement}']"));
            var destiantion = WaitForElementToBeVisible(By.XPath($"//*[text()='{toElement}']"));
            actions.ClickAndHold(drag).MoveToElement(destiantion).Release().Perform();
        }
    }
}
