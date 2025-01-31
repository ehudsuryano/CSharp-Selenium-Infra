using Automation.Testing.Framework.Ui.ComponentModels;
using Automation.Testing.Framework.Ui.Handlers;

using Infrastructure.Core;
using Infrastructure.Extensions;
using Infrastructure.Models;

using System.Collections.Generic;
using System.Linq;

namespace Automation.Testing.Framework.Ui.PageModels
{
    /// <summary>
    /// Represents the page model for the google search page.
    /// </summary>
    public class GoogleSearchPage : ModelBase
    {
        /// <inheritdoc />
        public GoogleSearchPage(ObjectSetupModel setupModel)
            : base(setupModel)
        { }

        /// <inheritdoc />
        public GoogleSearchPage(ObjectSetupModel setupModel, string url)
            : base(setupModel, url)
        { }

        /// <summary>
        /// Performs a search operation using the specified query and returns the search results as a collection of <see cref="AutoCompleteItemComponent"/>.
        /// </summary>
        /// <param name="query">The search query to be entered in the search box.</param>
        /// <returns>An enumerable collection of <see cref="AutoCompleteItemComponent"/> representing the search results.</returns>
        public IEnumerable<AutoCompleteItemComponent> Search(string query)
        {
            return AuditableAction(action: "Search", (setupModel) =>
            {
                // Create a new instance of TextboxHandler for the search box with name 'q'.
                var searchBox = ModelFactory.New<TextboxHandler>(setupModel, "q");

                // Send the search query to the search box element.
                searchBox.WebElement.SendKeys(query);

                // Create a new instance of ListItemHandler and get the search result elements.
                var elements = ModelFactory.New<ListItemHandler>(setupModel).GetElements();

                // Add the number of search results to the metrics.
                setupModel.AddMetrics("SearchResults", elements.Count());

                // Convert the elements to AutoCompleteItemComponent instances and return them.
                return elements
                    .Select(i => new AutoCompleteItemComponent(setupModel, element: i))
                    .ToList();
            });
        }
    }
}
