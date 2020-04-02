using System;
using System.Threading;

namespace LogBuckets.Shared.Pipes
{

    internal interface IStringEventPipe
    {
        EventHandler<string> In { get; }

        event EventHandler<string> Out;

        SynchronizationContext SynchronizationContext { get; set; }

        bool Passthrough { get; set; }
    }
}
