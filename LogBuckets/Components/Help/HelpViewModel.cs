using LogBuckets.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogBuckets.Components.Help
{
    internal sealed class HelpViewModel: ViewModel
    {
        public override string Name => "Help";
        public override string Icon => "pathHelp";
        public override Type ViewType => typeof(HelpView);
    }
}
