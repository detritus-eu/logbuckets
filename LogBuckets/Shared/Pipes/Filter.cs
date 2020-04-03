using LogBuckets.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace LogBuckets.Shared.Pipes
{
    internal sealed class Filter: StringEventPipe, IFilter
    {
        public const string OperatorRequired = "+";
        public const string OperatorNegated = "-";
        public static readonly ISet<string> Operators = new HashSet<string>() { OperatorRequired, OperatorNegated };

        private string _channel;
        private string _channelLower;
        public string Channel
        {
            get { return _channel; }
            set
            {
                if (_channel != value)
                {
                    _channel = value;
                    _channelLower = value?.ToLower();
                    RaisePropertyChanged(nameof(Channel));
                }
            }
        }

        private string _author;
        private string _authorLower;
        public string Author
        {
            get { return _author; }
            set
            {
                if (_author != value)
                {
                    _author = value;
                    _authorLower = value?.ToLower();
                    RaisePropertyChanged(nameof(Author));
                }
            }
        }

        private string _message;
        private string _messageLower;
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    _messageLower = value?.ToLower();
                    RaisePropertyChanged(nameof(Message));
                }
            }
        }


        protected override string Process(string data)
        {
            if (!IsValid()) return null;

            var entry = new EuLogEntry(data.ToLower());
            if ((string.IsNullOrEmpty(_channelLower) || IsMatch(entry.Channel, _channelLower))
                && (string.IsNullOrEmpty(_authorLower) || IsMatch(entry.Author, _authorLower))
                && (string.IsNullOrEmpty(_messageLower) || IsMatch(entry.Message, _messageLower)))
                return data;
            return null;
        }


        private static bool IsMatch(string line, string matchString)
        {
            var required = new List<int>();
            var optional = new List<int>();
            var negated = new List<int>();

            foreach (var token in Tokenize(matchString))
            {
                var term = token;
                var op = string.Empty;
                foreach (var oper in Operators)
                {
                    if (token.StartsWith(oper))
                    {
                        op = oper;
                        term = token.Substring(oper.Length);
                    }
                }

                var match = term.StartsWith("(") ? IsMatch(line, term.TrimStart('(').TrimEnd(')')) : line.Contains(term);

                if (op == OperatorRequired) required.Add(match ? 1 : 0);
                else if (op == OperatorNegated) negated.Add(!match ? 1 : 0);
                else if (match) optional.Add(1);
            }

            var total = required.Concat(negated).Concat(optional);

            return total.Count() > 0 && total.Sum() == total.Count();
        }

        private static IEnumerable<string> Tokenize(string expression)
        {
            var tokens = new List<string>();
            if (string.IsNullOrEmpty(expression)) return tokens;

            var brace = 0;
            var token = string.Empty;
            for (int i = 0; i < expression.Length; i++)
            {
                var ch = expression[i];
                if (ch == ' ')
                {
                    if (token.Length > 0 && brace == 0)
                    {
                        tokens.Add(token);
                        token = string.Empty;
                    }
                    else if (brace > 0) token += ch;
                    else continue;
                }
                else
                {
                    token += ch;
                    if (ch == '(') brace++;
                    else if (ch == ')')
                        if (brace > 0) brace--;
                }
            }
            if (token.Length > 0) tokens.Add(token);
            return tokens;
        }



        private bool IsValid() => !string.IsNullOrEmpty(Message) || !string.IsNullOrEmpty(Channel) || !string.IsNullOrEmpty(Author);


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

    }
}
