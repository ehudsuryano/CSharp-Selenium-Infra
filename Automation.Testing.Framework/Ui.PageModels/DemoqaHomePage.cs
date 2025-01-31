using Automation.Testing.Framework.Ui.Handlers;
using Infrastructure.Core;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Automation.Testing.Framework.Ui.PageModels
{
    /// <summary>
    /// Represents the page model for the demoqa homepage.
    /// </summary>
    public class DemoqaHomePage : ModelBase
    {
        /// <inheritdoc />
        public DemoqaHomePage(ObjectSetupModel setupModel)
            : base(setupModel)
        { }

        /// <inheritdoc />
        public DemoqaHomePage(ObjectSetupModel setupModel, string url)
            : base(setupModel, url)
        { }

        /// <summary>
        /// perform a click on a card on the home page of DemoQA.com/>.
        /// </summary>
        /// <param name="cardName">name of the wanted card</param>
        /// <returns>An enumerable collection of <see cref="AutoCompleteItemComponent"/> representing the search results.</returns>

        public ModelBase ClickCardtest(string cardName)
        {
            return AuditableAction<ModelBase>(action: "ClickCard", (setupModel) =>
            {
                // Create a new instance of TextboxHandler for the search box with name 'q'.
                var card = ModelFactory.New<CardHandler>(setupModel, cardName);

                // Click the card WebElement
                card.WebElement.Click();

                // Return the appropriate page model based on the card name
                if (cardName == "Elements")
                {
                    return ModelFactory.New<DemoqaElementsPage>(setupModel);
                }
                if (cardName == "Forms")
                {
                    return ModelFactory.New<DemoqaFormsPage>(setupModel);
                }
                if (cardName == "Alerts, Frame & Windows")
                {
                    return ModelFactory.New<DemoqaAlertsPage>(setupModel);
                }
                if (cardName == "Widgets")
                {
                    return ModelFactory.New<DemoqaWidgetsPage>(setupModel);
                }
                if (cardName == "Interactions")
                {
                    return ModelFactory.New<DemoqaInteractionsPage>(setupModel);
                }
                if (cardName == "Book Store Application")
                {
                    return ModelFactory.New<DemoqaBookPage>(setupModel);
                }
                
                // Default case
                return ModelFactory.New<DemoqaFormsPage>(setupModel);
            });
        }

        public ModelBase ClickCard(string cardName)
        {
            var cardMapping = new Dictionary<string, Func<ObjectSetupModel, ModelBase>>
    {
        { "Elements", setupModel => ModelFactory.New<DemoqaElementsPage>(setupModel) },
        { "Forms", setupModel => ModelFactory.New<DemoqaFormsPage>(setupModel) },
        { "Alerts, Frame & Windows", setupModel => ModelFactory.New<DemoqaAlertsPage>(setupModel) },
        { "Widgets", setupModel => ModelFactory.New<DemoqaWidgetsPage>(setupModel) },
        { "Interactions", setupModel => ModelFactory.New<DemoqaInteractionsPage>(setupModel) },
        { "Book Store Application", setupModel => ModelFactory.New<DemoqaBookPage>(setupModel) }
    };

            return AuditableAction<ModelBase>(action: "ClickCard", setupModel =>
            {
                // Create a new instance of TextboxHandler for the card
                var card = ModelFactory.New<CardHandler>(setupModel, cardName);
                card.WebElement.Click();

                // Find the model for the card or default to DemoqaFormsPage
                return cardMapping.TryGetValue(cardName, out var createModel)
                    ? createModel(setupModel)
                    : ModelFactory.New<DemoqaFormsPage>(setupModel);
            });
        }

        public DemoqaElementsPage ClickElements()
        {
            return AuditableAction<DemoqaElementsPage>(action: "ClickCard", setupModel =>
            {
                // Create a new instance of CardHandler for the card with name 'Elements'.
                var card = ModelFactory.New<CardHandler>(setupModel, "Elements");

                // Click the card WebElement
                card.WebElement.Click();

                // Return the appropriate page model based on the card name

                return ModelFactory.New<DemoqaElementsPage>(setupModel);
            });
        }
    }
}
