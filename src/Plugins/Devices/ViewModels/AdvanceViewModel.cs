using BasicPlugin.CustomAttribute;
using MEF.Launcher.Platform.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicPlugin.ViewModels
{
    [PluggablePartExport(typeof(AdvanceViewModel))]
    public class AdvanceViewModel : ScreenBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; } = "Advance view";
    }
}
