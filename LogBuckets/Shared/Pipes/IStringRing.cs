using System.Collections.Generic;

namespace LogBuckets.Shared.Pipes
{
    internal interface IStringRing: IStringEventPipe
    {
        int Size { get; set; }
        IReadOnlyCollection<string> Items { get; }
        void Clear();
        void AppendRange(string[] items);
    }
}
