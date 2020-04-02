using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogBuckets.Shared.Pipes
{
    internal interface IDeduper: IStringEventPipe
    {
        IEnumerable<string> Items { get; set; }

        Func<string,string> Selector { get; set; }
    }
}
