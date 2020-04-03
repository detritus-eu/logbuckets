using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace LogBuckets.Models
{
    internal sealed class Configuration: INotifyPropertyChanged
    {

        private const int DefaultFontSize = 14;
        private const int MinFontSize = 6;
        private const int MaxFontSize = 48;
        private const string DefaultBucketExtension = "json";
        private const string DefaultBucketDirectoryName = "buckets";
        private const string EntropiaChatDirectory = "Entropia Universe";
        private const string EntropiaLogFile = "chat.log";


        public Configuration()
        {
            LogFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), EntropiaChatDirectory, EntropiaLogFile);
            BucketDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), DefaultBucketDirectoryName);
            BucketExtension = DefaultBucketExtension;
            FontSize = DefaultFontSize;
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


        private int _fontSize;
        public int FontSize
        {
            get { return _fontSize; }
            set
            {
                if (_fontSize != value)
                {
                    var size = value;
                    if (size < MinFontSize) size = MinFontSize;
                    if (size > MaxFontSize) size = MaxFontSize;
                    _fontSize = size;
                    RaisePropertyChanged(nameof(FontSize));
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
