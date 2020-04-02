using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogBuckets.Models
{
    internal sealed class EuLogEntry
    {
        public EuLogEntry(string line)
        {
            if (string.IsNullOrEmpty(line)) throw new ArgumentNullException(nameof(line));

            var match = Regex.Match(line, @"^([\d-]+\s[\d:]+)\s+\[([^]]*)?\]\s+\[([^]]*)?\]\s+(.*)$");
            if (match.Success)
            {
                Time = Convert.ToDateTime(match.Groups[1].Value);
                Channel = match.Groups[2].Value;
                Author = match.Groups[3].Value;
                Message = match.Groups[4].Value;
            }
        }

        public DateTime Time { get; }
        public string Channel { get; }
        public string Author { get; }
        public string Message { get; }



    }
}
