﻿using Infrastructure.Core;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Testing.Framework.Ui.PageModels
{
    public class DemoqaWidgetsPage : ModelBase
    {
        /// <inheritdoc />
        public DemoqaWidgetsPage(ObjectSetupModel setupModel)
            : base(setupModel)
        { }

        /// <inheritdoc />
        public DemoqaWidgetsPage(ObjectSetupModel setupModel, string url)
            : base(setupModel, url)
        { }
    }
}
