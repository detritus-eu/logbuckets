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

        private string _keywords;
        private string _keywords_lower;
        public string Keywords
        {
            get { return _keywords; }
            set
            {
                if (_keywords != value)
                {
                    _keywords = value;
                    _keywords_lower = value?.ToLower();
                    RaisePropertyChanged(nameof(Keywords));
                }
            }
        }


        protected override string Process(string data)
        {
            if (!IsValid()) return null;

            var entry = new EuLogEntry(data.ToLower());
            if ((!string.IsNullOrEmpty(_channel_lower) && entry.Channel.Contains(_channel_lower))
                || (!string.IsNullOrEmpty(_author_lower) && entry.Author.Contains(_author_lower))
                || (!string.IsNullOrEmpty(_keywords_lower) && SplitMatch(entry.Message, _keywords_lower)))
                return data;
            return null;
        }

        private bool SplitMatch(string line, string keywords)
        {
            foreach (var kw in keywords.Split(' '))
                if (line.Contains(kw)) return true;
            return false;
        }


        private bool IsValid() => !string.IsNullOrEmpty(Keywords) || !string.IsNullOrEmpty(Channel) || !string.IsNullOrEmpty(Author);


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

    }
}
