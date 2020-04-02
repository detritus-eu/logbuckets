using LogBuckets.Models;
using LogBuckets.Services;
using LogBuckets.Shared.Pipes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogBuckets.Components.Buckets
{
    internal sealed class LogViewModel: BucketTab, INotifyPropertyChanged
    {
        public const string Label = "Main Log";
        private const int BufferSize = 100;

        #region Private Fields

        private readonly IStringRing _ringBuffer;
        private readonly ILogWatcher _logWatcher;

        #endregion

        #region Ctor

        public LogViewModel(
            IBucket bucket,
            IStringRing ringBuffer,
            ILogWatcher logWatcher)
            :base(bucket)
        {
            _ringBuffer = ringBuffer ?? throw new ArgumentNullException(nameof(ringBuffer));
            _logWatcher = logWatcher ?? throw new ArgumentNullException(nameof(logWatcher));

            bucket.Name = Label;
            In = _ringBuffer.In;
            _ringBuffer.Size = BufferSize;

            _logWatcher.SynchronizationContext = SynchronizationContext.Current;
            _logWatcher.Line += In;
            _logWatcher.Line += (o, e) => { Out?.Invoke(o, e); };
        }

        #endregion

        #region Public Interface

        public EventHandler<string> In { get; }

        public IReadOnlyCollection<string> Lines => _ringBuffer.Items;

        public event EventHandler<string> Out;


        #endregion

        #region Private Methods


        protected override void OnConfigChanged()
        {
            try
            {
                _logWatcher.Start(Config?.LogFile);
            }
            catch (Exception ex)
            {
                if (!(ex is ArgumentNullException || ex is ArgumentException))
                    throw;
            }
        }

        #endregion



    }
}
