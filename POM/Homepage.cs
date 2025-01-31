using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace POM
{
    public class Homepage :Basepage
    {
        public Homepage(IWebDriver driver) : base(driver) { }

        private IWebElement searchBar => WaitForElementToBeVisible(By.Id("autocomplete"));
        private IWebElement searchBtn => WaitForElementToBeVisible(By.CssSelector("button[type='submit']"));

        public void Typeinsearch(string query)
        {
            searchBar.Clear();
            searchBar.SendKeys(query);
        }
        public void ClicksearchButton()
        {
            searchBtn.Click();
        }
    }
}
