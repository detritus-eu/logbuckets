using System;
using System.Threading;

namespace LogBuckets.Services
{
    internal interface ILogWatcher
    {
        void Start(string path);
        void Stop();

        event EventHandler<string> Line;

        bool HtmlDecoderEnabled { get; set; }

        SynchronizationContext SynchronizationContext { get; set; }
    }
}
