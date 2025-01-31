using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace POM
{
    public class Searchresults : Basepage
    {
        public Searchresults(IWebDriver driver) : base(driver) { }
        private IWebElement searchBar => WaitForElementToBeVisible(By.Id("autocomplete"));
        private IWebElement searchBtn => WaitForElementToBeVisible(By.CssSelector("button[type='submit']"));
        private IWebElement homeBtn => WaitForElementToBeVisible(By.XPath("//nav//ul//li[a[@title='Go to Home']]"));

        public void Typeinsearch(string query)
        {
            searchBar.Clear();
            searchBar.SendKeys(query);
        }
        public void ClicksearchButton()
        {
            searchBtn.Click();
        }

        public Homepage ClickhomeButton()
        {
            homeBtn.Click();
            return new Homepage(Driver);
        }
    }
}
