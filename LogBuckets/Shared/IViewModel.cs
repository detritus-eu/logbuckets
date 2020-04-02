using LogBuckets.Models;
using System;
using System.ComponentModel;

namespace LogBuckets.Shared
{
    internal interface IViewModel : INotifyPropertyChanged
    {
        string Name { get; }

        string Icon { get; }

        Type ViewType { get; }

        Configuration Config { get; set; }

        void OnExit();
    }
}
