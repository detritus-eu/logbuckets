using LogBuckets.Models;
using System.ComponentModel;

namespace LogBuckets.Shared.Pipes
{
    internal sealed class Filter: StringEventPipe, IFilter
    {


        private string _channel;
        private string _channel_lower;
        public string Channel
        {
            get { return _channel; }
            set
            {
                if (_channel != value)
                {
                    _channel = value;
                    _channel_lower = value?.ToLower();
                    RaisePropertyChanged(nameof(Channel));
                }
            }
        }

        private string _author;
        private string _author_lower;
        public string Author
        {
            get { return _author; }
            set
            {
                if (_author != value)
                {
                    _author = value;
                    _author_lower = value?.ToLower();
                    RaisePropertyChanged(nameof(Author));
                }
            }
        }

        private string _keyword;
        private string _keyword_lower;
        public string Keyword
        {
            get { return _keyword; }
            set
            {
                if (_keyword != value)
                {
                    _keyword = value;
                    _keyword_lower = value?.ToLower();
                    RaisePropertyChanged(nameof(Keyword));
                }
            }
        }


        protected override string Process(string data)
        {
            if (!IsValid()) return null;

            var entry = new EuLogEntry(data.ToLower());
            if ((!string.IsNullOrEmpty(_channel_lower) && entry.Channel.Contains(_channel_lower))
                || (!string.IsNullOrEmpty(_author_lower) && entry.Author.Contains(_author_lower))
                || (!string.IsNullOrEmpty(_keyword_lower) && entry.Message.Contains(_keyword_lower)))
                return data;
            return null;
        }


        private bool IsValid() => !string.IsNullOrEmpty(Keyword) || !string.IsNullOrEmpty(Channel) || !string.IsNullOrEmpty(Author);


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

    }
}
