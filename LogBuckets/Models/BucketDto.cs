using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogBuckets.Models
{
    internal sealed class BucketDto
    {
        public class FilterOptions
        {
            public string Channel { get; set; }
            public string Author { get; set; }
            public string Message { get; set; }
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public bool Notify { get; set; }
        public bool Dedupe { get; set; }
        public FilterOptions Filter { get; set; }
        public string[] Buffer { get; set; }
    }
}
