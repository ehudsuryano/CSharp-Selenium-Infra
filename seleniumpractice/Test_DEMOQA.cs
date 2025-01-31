using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using POM;
using POM_DEMOQA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace seleniumpractice
{
    [TestClass]
    public class Test_DEMOQA
    {
        private ChromeDriver _driver;
        private HomePage _homepage;
        private ElementsPage _elements;
        private WidgetsPage _widgets;
        private InteractionsPage _interactions;

        [TestInitialize]
        public void Setup()
        {
            _driver = new ChromeDriver();
           // _driver.Manage().Window.Maximize();
            _homepage = new HomePage(_driver);
            _homepage.NavigateTo("https://demoqa.com/"); // Use NavigateTo from BasePage
        }
        [TestMethod]
        public void TestMethod1()
        {
            _homepage.ClickOnTile("Elements");
            Assert.AreEqual("https://demoqa.com/elements", _driver.Url);
            _elements = new ElementsPage(_driver);
            _elements.ClickSideMenuItem("Text Box");
            _elements.TypeText("userName", "NAME");
            _elements.TypeText("userEmail", "NAME@gmail.com");
            _elements.TypeText("currentAddress", "some Address");
            _elements.TypeText("permanentAddress", "some other Address");
            _elements.ClickBtn("Submit");
            Assert.IsTrue(_elements.ElementExist("output"));
            _elements.ClickSideMenuItem("Check Box");
            _elements.ClickExpandCollaps("Expand all");
            _elements.ClickExpandCollaps("Collapse all");
            _elements.ClickExpandCollaps("Expand all");
            _elements.ClickSideMenuItem("Forms");
            _elements.ClickSideMenuItem("Practice Form");
            _elements.ScrollToElement("Widgets", _driver);
            _elements.ClickSideMenuItem("Widgets");
            _elements.ClickSideMenuItem("Select Menu");
            _widgets = new WidgetsPage(_driver);
            _widgets.ClickBtn("Select Option");
            _widgets.ClickBtn("Another root option");
            _widgets.ScrollToElement("Interactions", _driver);
            _widgets.ClickSideMenuItem("Interactions");
            _widgets.ClickSideMenuItem("Droppable");
            _interactions = new InteractionsPage(_driver);
            _interactions.DragElementToElement("Drag me", "Drop here",_driver);
            _interactions.ClickSideMenuItem("Sortable");
            _interactions.SortDragElementToElement("Six", "One", _driver);

        }

        [TestCleanup]
        public void Teardown()
        {
            Thread.Sleep(5000);
            _driver.Quit();
        }
    }
}
