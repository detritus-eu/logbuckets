using LogBuckets.Models;
using LogBuckets.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LogBuckets.Shared.Pipes
{
    internal sealed class Bucket: StringEventPipe, IBucket
    {
        private const string DefaultName = "New";

        #region Private Fields

        private readonly IToaster _toaster;

        private bool _initialized;

        #endregion

        #region Ctor

        public Bucket(
            IStringRing ringBuffer,
            IFilter filter,
            IToaster toaster)
        {
            Buffer = ringBuffer ?? throw new ArgumentNullException(nameof(ringBuffer));
            Filter = filter ?? throw new ArgumentNullException(nameof(filter));
            _toaster = toaster ?? throw new ArgumentNullException(nameof(toaster));

            _toaster.Passthrough = true;
            Name = DefaultName;
        }

        #endregion

        #region Public Interface

        public IFilter Filter { get; }
        public IStringRing Buffer { get; }


        public string Id { get; private set; }


        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged(nameof(Name));
                    _toaster.Name = Name;
                }
            }
        }

        private bool _notify;
        public bool Notify
        {
            get { return _notify; }
            set
            {
                if (_notify != value)
                {
                    _notify = value;
                    RaisePropertyChanged(nameof(Notify));
                    _toaster.Passthrough = !Notify;
                }
            }
        }

        private int _size;
        public int Size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    _size = value;
                    RaisePropertyChanged(nameof(Size));
                    Buffer.Size = Size;
                }
            }
        }

        public void Initialize(BucketDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            Name = dto.Name;
            Notify = dto.Notify;
            Filter.Channel = dto.Filter.Channel;
            Filter.Author = dto.Filter.Author;
            Filter.Keyword = dto.Filter.Keyword;
            Buffer.Clear();
            Buffer.AppendRange(dto.Buffer);

            Initialize(dto.Id);
        }

        public void Initialize(string id)
        {
            Id = id;

            if (_initialized) return;

            In = Filter.In;
            Filter.Out += _toaster.In;
            _toaster.Out += Buffer.In;
            Buffer.Out += (o, e) => { RaiseOut(e); };

            _initialized = true;
        }

        public BucketDto ToDto() => new BucketDto
            {
                Id = Id,
                Name = Name,
                Notify = Notify,
                Filter = new BucketDto.FilterOptions {
                    Channel = Filter.Channel,
                    Author = Filter.Author,
                    Keyword = Filter.Keyword
                },
                Buffer = Buffer.Items.ToArray()
            };
        


        #endregion

        #region Private Methods

        private void Filter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(string.Empty);
        }


        #endregion



        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

    }
}
