using System;
using System.IO;
using System.Threading;

namespace LogBuckets.Services
{
    internal sealed class LogWatcher: ILogWatcher
    {
        private const int CheckInterval = 500;
        private static readonly string Delimiter = Environment.NewLine;

        #region Private Fields

        private Timer _timer;
        private string _path;
        private string _buffer;
        private long _readBytes;

        #endregion

        #region Ctor



        #endregion

        #region Public Interface

        public SynchronizationContext SynchronizationContext { get; set; }

        public void Start(string path)
        {
            Stop();

            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
            if (!File.Exists(path)) throw new ArgumentException($"The file '{path}' does not exist.", nameof(path));

            _path = path;
            _buffer = string.Empty;
            _readBytes =  new FileInfo(_path).Length;
            _timer = new Timer(CheckLog, null, CheckInterval, Timeout.Infinite);
        }

        public void Stop()
        {
            _timer?.Dispose();
            _timer = null;
        }

        public event EventHandler<string> Line;
        private void RaiseLine(string line)
        {
            if (SynchronizationContext != null) SynchronizationContext.Post((_) =>
             {
                 Line?.Invoke(this, line);
             }, null);
            else Line?.Invoke(this, line);
        }

        #endregion

        #region Private Methods

        private void CheckLog(object state)
        {
            var size = new FileInfo(_path).Length;

            if (_readBytes > size) _readBytes = 0;

            if (_readBytes < size)
            {
                using (var file = File.Open(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var sr = new StreamReader(file))
                {
                    sr.BaseStream.Seek(_readBytes, SeekOrigin.Begin);
                    var data = _buffer + sr.ReadToEnd();
                    _readBytes = size;

                    if (!data.EndsWith(Delimiter))
                    {
                        var idx = data.LastIndexOf(Delimiter);
                        if (idx > -1)
                        {
                            _buffer = data.Substring(idx);
                            data = data.Substring(0, idx);
                        }
                        else
                        {
                            _buffer = data;
                            data = string.Empty;
                        }
                    }
                    else _buffer = string.Empty;

                    var lines = data.Split(new string[] { Delimiter }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines) RaiseLine(line);
                }
            }

            _timer.Change(CheckInterval, Timeout.Infinite);
        }

        #endregion

    }
}
