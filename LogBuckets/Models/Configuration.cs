using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace LogBuckets.Models
{
    internal sealed class Configuration: INotifyPropertyChanged
    {
        private const int DefaultBucketSize = 200;
        private const string DefaultBucketExtension = "json";
        private const string DefaultBucketDirectoryName = "buckets";
        private const string EntropiaChatDirectory = "Entropia Universe";
        private const string EntropiaLogFile = "chat.log";


        public Configuration()
        {
            LogFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), EntropiaChatDirectory, EntropiaLogFile);
            BucketSize = DefaultBucketSize;
            BucketDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), DefaultBucketDirectoryName);
            BucketExtension = DefaultBucketExtension;
        }

        public string BucketDirectory { get; }

        public string BucketExtension { get; }

        private string _logFile;
        public string LogFile
        {
            get { return _logFile; }
            set
            {
                if (_logFile != value)
                {
                    _logFile = value;
                    RaisePropertyChanged(nameof(LogFile));
                }
            }
        }

        private int _bucketSize;
        public int BucketSize
        {
            get { return _bucketSize; }
            set
            {
                if (_bucketSize != value)
                {
                    _bucketSize = value;
                    RaisePropertyChanged(nameof(BucketSize));
                }
            }
        }




        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion


    }
}
