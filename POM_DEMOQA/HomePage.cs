using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POM_DEMOQA
{
    public class HomePage : BasePage
    {
        public HomePage(IWebDriver driver) : base(driver) { }

        private IWebElement tile => WaitForElementToBeClickable(By.ClassName("card mt-4 top-card"));

        /** public void ClickOnNthElementWithClass(IWebDriver Driver, string compoundClassName, int indexToClick)
         {
             try
             {
                 // Set up an explicit wait
                 WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

                 // Use CSS selector for compound class names
                 string cssSelector = compoundClassName.Replace(" ", ".");
                 wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector($"." + cssSelector)));

                 // Find all elements with the specified compound class name
                 IReadOnlyList<IWebElement> elements = Driver.FindElements(By.CssSelector("." + cssSelector));

                 // Log the count of elements found
                 Console.WriteLine($"Found {elements.Count} elements with class name '{compoundClassName}'.");

                 // Ensure the specified index exists
                 if (indexToClick < elements.Count)
                 {
                     // Wait until the specific element at index is clickable before clicking
                     wait.Until(ExpectedConditions.ElementToBeClickable(elements[indexToClick]));
                     elements[indexToClick].Click(); // Click on the specified element
                     Console.WriteLine($"Clicked on element at index {indexToClick}.");
                 }
                 else
                 {
                     Console.WriteLine($"Element at index {indexToClick} was not found. Only {elements.Count} elements are available.");
                 }
             }
             catch (WebDriverTimeoutException)
             {
                 Console.WriteLine("Timeout: Elements with the specified class name did not become visible within the specified time.");
             }
             catch (NoSuchElementException)
             {
                 Console.WriteLine($"No elements were found with the class name '{compoundClassName}'.");
             }
             catch (ElementClickInterceptedException)
             {
                 Console.WriteLine("The element was not clickable because it was overlapped by another element.");
             }
             catch (Exception ex)
             {
                 Console.WriteLine($"An unexpected error occurred: {ex.Message}");
             }
        }
        **/

        public void ClickOnTile(string tileName)
        {
            WaitForElementToBeClickable(By.XPath($"//div//h5[text()='{tileName}']")).Click();
        }


    }
}
