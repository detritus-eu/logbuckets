using System.ComponentModel;

namespace LogBuckets.Shared.Pipes
{
    internal interface IFilter: IStringEventPipe, INotifyPropertyChanged
    {
        string Channel { get; set; }
        string Author { get; set; }
        string Keyword { get; set; }
    }
}
