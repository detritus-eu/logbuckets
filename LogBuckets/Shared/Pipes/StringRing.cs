using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LogBuckets.Shared.Pipes
{
    internal sealed class StringRing: StringEventPipe, IStringRing
    {
        public const int DefaultSize = 100;

        private const double SoftLimitMult = 0.9;

        private readonly ObservableCollection<string> _items;
        private int _softLimit;

        public StringRing(int size = DefaultSize)
        {
            _items = new ObservableCollection<string>();
            Size = size;
        }

        public IReadOnlyCollection<string> Items => _items;


        private int _size;
        public int Size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    _size = value;
                    _softLimit = (int)Math.Floor(_size * SoftLimitMult);
                    Truncate();
                }
            }
        }

        protected override string Process(string data)
        {
            _items.Insert(0, data);
            Truncate();
            return data;
        }

        private void Truncate()
        {
            if (_items.Count > _size)
                while (_items.Count > _softLimit) 
                    _items.RemoveAt(_items.Count - 1);
        }

        public void Clear() => _items.Clear();

        public void AppendRange(string[] items)
        {
            foreach (var item in items) _items.Add(item);
            Truncate();
        }

    }
}
