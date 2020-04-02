using LogBuckets.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace LogBuckets.Shared.Pipes
{
    internal interface IBucket: IStringEventPipe, INotifyPropertyChanged
    {

        string Id { get; }

        string Name { get; set; }
        bool Notify { get; set; }
        int Size { get; set; }
        bool Dedupe { get; set; }

        void Initialize(string id);
        void Initialize(BucketDto dto);
        BucketDto ToDto();

    }
}
