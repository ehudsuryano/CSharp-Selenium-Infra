using Infrastructure.Core;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Testing.Framework.Ui.PageModels
{
    public class DemoqaBookPage : ModelBase
    {
        /// <inheritdoc />
        public DemoqaBookPage(ObjectSetupModel setupModel)
            : base(setupModel)
        { }

        /// <inheritdoc />
        public DemoqaBookPage(ObjectSetupModel setupModel, string url)
            : base(setupModel, url)
        { }
    }
}
