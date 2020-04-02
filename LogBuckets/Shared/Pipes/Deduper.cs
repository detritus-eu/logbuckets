using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogBuckets.Shared.Pipes
{
    internal class Deduper: StringEventPipe, IDeduper
    {
        public IEnumerable<string> Items { get; set; }

        public Func<string,string> Selector { get; set; }

        protected override string Process(string data)
        {
            var newData = Selector(data);
            foreach (var item in Items.Select(Selector))
                if (newData == item) return null;
            return data;
        }

    }
}
