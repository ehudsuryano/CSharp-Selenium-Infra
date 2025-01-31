using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace POM
{
    public class Landingpage :Basepage
    {
        public Landingpage(IWebDriver driver) : base(driver) { }

        private IWebElement searchBar => WaitForElementToBeVisible(By.Id("autocomplete"));
        private IWebElement searchBtn => WaitForElementToBeVisible(By.CssSelector("button[type='submit']"));
        private IWebElement homeBtn => WaitForElementToBeClickable(By.XPath("//ul//li[a[text()='Home']]"));

        public void Typeinsearch(string query)
        {
            searchBar.Clear();
            searchBar.SendKeys(query);
        }
        public Searchresults ClicksearchButton()
        {
            searchBtn.Click();
            return new Searchresults(Driver);
        }
    }
}
