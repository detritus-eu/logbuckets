using System;
using System.Threading;

namespace LogBuckets.Shared.Pipes
{
    internal abstract class StringEventPipe: IStringEventPipe
    {
        public StringEventPipe()
        {
            In = Pipe;
            SynchronizationContext = SynchronizationContext.Current;
        }


        public virtual EventHandler<string> In { get; protected set; }

        protected virtual string Process(string data) => data;


        public event EventHandler<string> Out;
        protected void RaiseOut(string data)
        {
            if (string.IsNullOrEmpty(data)) return;

            if (SynchronizationContext != null)
                SynchronizationContext.Post(_ => { Out?.Invoke(this, data); }, null);
            else Out?.Invoke(this, data);
        }

        public SynchronizationContext SynchronizationContext { get; set; }

        public bool Passthrough { get; set; }

        private void Pipe(object sender, string evt)
        {
            var data = Passthrough ? evt : Process(evt);
            RaiseOut(data);
        }
    }
}
