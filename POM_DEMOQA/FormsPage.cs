using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POM_DEMOQA
{
    public class FormsPage : BasePage
    {
        public FormsPage(IWebDriver driver) : base(driver) { }

        public void TypeText(string textbox, string text)
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
    }
}
