using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace POM_DEMOQA
{
    public class ElementsPage :BasePage
    {
        public ElementsPage(IWebDriver driver) : base(driver) { }

        public void TypeText(string textbox , string text)
        {
            WaitForElementToBeVisible(By.Id(textbox)).SendKeys(text);
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

        public void ClickExpandCollaps(string Action)
        {
            WaitForElementToBeVisible(By.XPath($"//button[@aria-label='{Action}']")).Click();
        }
    }
}
