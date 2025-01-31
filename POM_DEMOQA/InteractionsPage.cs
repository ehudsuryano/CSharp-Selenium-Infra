using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POM_DEMOQA
{
    public class InteractionsPage : BasePage
    {
        private readonly Actions actions;
        public InteractionsPage(IWebDriver driver) : base(driver)
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

        public void DragElementToElement(string dragElement, string toElement, IWebDriver driver)
        {
            var drag = WaitForElementToBeVisible(By.XPath($"//*[text()='{dragElement}']"));
            var destiantion = WaitForElementToBeVisible(By.XPath($"//*[text()='{toElement}']"));

            TimeSpan time = new TimeSpan(0, 0, 2);
            actions.MoveToElement(drag).Pause(time).ClickAndHold(drag).Pause(time).MoveToElement(destiantion).Pause(time).Release().Perform();
        }

        public void SortDragElementToElement(string dragElement, string toElement, IWebDriver driver)
        {
            var drag = WaitForElementToBeVisible(By.XPath($"//div[@id='demo-tabpane-list']//*[text()='{dragElement}']"));
            var destiantion = WaitForElementToBeVisible(By.XPath($"//div[@id='demo-tabpane-list']//*[text()='{toElement}']"));

            TimeSpan time = new TimeSpan(0, 0, 2);
            actions.MoveToElement(drag).Pause(time).ClickAndHold(drag).Pause(time).MoveToElement(destiantion).Pause(time).Release().Perform();
        }

        public void DragElementToElementOffset(string dragElement, string toElement, IWebDriver driver)
        {
            var drag = WaitForElementToBeVisible(By.XPath($"//*[text()='{dragElement}']"));
            var destiantion = WaitForElementToBeVisible(By.XPath($"//*[text()='{toElement}']"));
            var sortableItems = driver.FindElements(By.CssSelector("#demo-tabpane-list .list-group-item.list-group-item-action"));
            int offsetY = sortableItems[5].Location.Y - sortableItems[0].Location.Y;
            //actions.DragAndDrop(drag, destiantion);
            TimeSpan time = new TimeSpan(0, 0, 3);
            actions.ClickAndHold(sortableItems[0]).Pause(time).MoveByOffset(0, offsetY).Pause(time).Release().Perform();
        }
    }
}
