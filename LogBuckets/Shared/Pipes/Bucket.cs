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
        private readonly IDeduper _deduper;

        private bool _initialized;

        #endregion

        #region Ctor

        public Bucket(
            IStringRing ringBuffer,
            IFilter filter,
            IToaster toaster,
            IDeduper deduper
            )
        {
            Buffer = ringBuffer ?? throw new ArgumentNullException(nameof(ringBuffer));
            Filter = filter ?? throw new ArgumentNullException(nameof(filter));
            _toaster = toaster ?? throw new ArgumentNullException(nameof(toaster));
            _deduper = deduper ?? throw new ArgumentNullException(nameof(deduper));

            _deduper.Items = Buffer.Items;
            _deduper.Selector = (_) => { return _?.Substring(21); }; //chop off the timestamp

            _toaster.Passthrough = true;
            Name = DefaultName;
            Dedupe = true;
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

        private bool _dedupe;
        public bool Dedupe
        {
            get { return _dedupe; }
            set
            {
                if (_dedupe != value)
                {
                    _dedupe = value;
                    RaisePropertyChanged(nameof(Dedupe));
                    _deduper.Passthrough = !Dedupe;
                }
            }
        }



        public void Initialize(BucketDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            Name = dto.Name;
            Notify = dto.Notify;
            Dedupe = dto.Dedupe;
            Filter.Channel = dto.Filter.Channel;
            Filter.Author = dto.Filter.Author;
            Filter.Keywords = dto.Filter.Keyword;
            Buffer.Clear();
            Buffer.AppendRange(dto.Buffer);

            Initialize(dto.Id);
        }

        public void Initialize(string id)
        {
            Id = id;

            if (_initialized) return;

            In = Filter.In;
            Filter.Out += _deduper.In;
            _deduper.Out += Buffer.In;
            Buffer.Out += _toaster.In;
            _toaster.Out += (o, e) => { RaiseOut(e); };

            _initialized = true;
        }

        public BucketDto ToDto() => new BucketDto
            {
                Id = Id,
                Name = Name,
                Notify = Notify,
                Dedupe = Dedupe,
                Filter = new BucketDto.FilterOptions {
                    Channel = Filter.Channel,
                    Author = Filter.Author,
                    Keyword = Filter.Keywords
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
