using Infrastructure.Core;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Testing.Framework.Ui.PageModels
{
    public class DemoqaInteractionsPage : ModelBase
    {
        /// <inheritdoc />
        public DemoqaInteractionsPage(ObjectSetupModel setupModel)
            : base(setupModel)
        { }

        /// <inheritdoc />
        public DemoqaInteractionsPage(ObjectSetupModel setupModel, string url)
            : base(setupModel, url)
        { }
    }
}
