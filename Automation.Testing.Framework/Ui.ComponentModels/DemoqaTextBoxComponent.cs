using Automation.Testing.Framework.Ui.PageModels;
using Infrastructure.Core;
using Infrastructure.Models;
using OpenQA.Selenium;

namespace Automation.Testing.Framework.Ui.ComponentModels
{
    public class DemoqaTextBoxComponent(ObjectSetupModel setupModel, IWebElement element)
        : ModelBase(setupModel)
    {
        public IWebElement Element { get; } = element;


    }
}
