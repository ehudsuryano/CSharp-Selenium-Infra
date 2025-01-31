using Automation.Testing.Framework.Ui.PageModels;
using Infrastructure.Core;
using Infrastructure.Models;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Testing.Framework.Ui.ComponentModels
{
    /// <summary>
    /// Represents an auto-complete item component within the UI.
    /// </summary>
    /// <param name="setupModel">The setup model containing configuration and WebDriver information.</param>
    /// <param name="element">The web element representing the auto-complete item.</param>
    public class AutoCompleteItemComponent(ObjectSetupModel setupModel, IWebElement element)
        : ModelBase(setupModel)
    {
        /// <summary>
        /// Gets the text content of the auto-complete item.
        /// </summary>
        public string Text { get; } = element.Text;

        /// <summary>
        /// Gets the web element representing the auto-complete item.
        /// </summary>
        public IWebElement Element { get; } = element;

        /// <summary>
        /// Selects the auto-complete item and returns a new instance of the <see cref="GoogleSearchResultsPage"/>.
        /// </summary>
        /// <returns>A new instance of the <see cref="ResultsPageModel"/>.</returns>
        public GoogleSearchResultsPage Select()
        {
            // Click the auto-complete item element.
            Element.Click();

            // Create and return a new instance of ResultsPageModel.
            return ModelFactory.New<GoogleSearchResultsPage>(SetupModel);
        }
    }
}
