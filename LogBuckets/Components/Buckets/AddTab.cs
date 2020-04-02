using LogBuckets.Shared.Pipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogBuckets.Components.Buckets
{
    internal sealed class AddTab: BucketTab
    {
        public const string Label = "Add";

        public AddTab(IBucket bucket)
            :base(bucket)
        {
            Header = Label;
        }
    }
}
