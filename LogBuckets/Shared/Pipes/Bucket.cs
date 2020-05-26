using LogBuckets.Models;
using LogBuckets.Services;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace LogBuckets.Shared.Pipes
{
    internal sealed class Bucket: StringEventPipe, IBucket
    {
        private const string DefaultName = "New";
        private const int DefaultSize = 100;
        private const int MinSize = 1;
        private const int MaxSize = 2000;

        #region Private Fields

        private readonly IToaster _toaster;
        private readonly IDeduper _deduper;
        private readonly IAudioAlert _audioAlert;

        private bool _initialized;

        #endregion

        #region Ctor

        public Bucket(
            IStringRing ringBuffer,
            IFilter filter,
            IToaster toaster,
            IDeduper deduper,
            IAudioAlert audioAlert
            )
        {
            Buffer = ringBuffer ?? throw new ArgumentNullException(nameof(ringBuffer));
            Filter = filter ?? throw new ArgumentNullException(nameof(filter));
            _toaster = toaster ?? throw new ArgumentNullException(nameof(toaster));
            _deduper = deduper ?? throw new ArgumentNullException(nameof(deduper));
            _audioAlert = audioAlert ?? throw new ArgumentNullException(nameof(audioAlert));

            _deduper.Items = Buffer.Items;
            _deduper.Selector = (_) => { return _?.Substring(21); }; //chop off the timestamp

            Filter.PropertyChanged += (o,e) => { Buffer.Clear(); };

            _toaster.Passthrough = _audioAlert.Passthrough = true;
            Name = DefaultName;
            Size = DefaultSize;
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
                    var size = value;
                    if (size < MinSize) size = MinSize;
                    if (size > MaxSize) size = MaxSize;
                    _size = size;
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


        private bool _useAudio;
        public bool UseAudio
        {
            get { return _useAudio; }
            set
            {
                if (_useAudio != value)
                {
                    _useAudio = value;
                    RaisePropertyChanged(nameof(UseAudio));
                    _audioAlert.Passthrough = !UseAudio;
                }
            }
        }


        private string _audioFile;
        public string AudioFile
        {
            get { return _audioFile; }
            set
            {
                if (_audioFile != value)
                {
                    _audioFile = value;
                    RaisePropertyChanged(nameof(AudioFile));
                    _audioAlert.Filename = value;
                }
            }
        }


        private bool _isDisabled;
        public bool IsDisabled
        {
            get { return _isDisabled; }
            set
            {
                if (_isDisabled != value)
                {
                    _isDisabled = value;
                    RaisePropertyChanged(nameof(IsDisabled));
                    if (IsDisabled) Filter.Out -= _deduper.In;
                    else Filter.Out += _deduper.In;
                }
            }
        }



        public void Initialize(BucketDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            Name = dto.Name;
            Size = dto.Size;
            Notify = dto.Notify;
            Dedupe = dto.Dedupe;
            UseAudio = dto.UseAudio;
            AudioFile = dto.AudioFile;
            IsDisabled = dto.IsDisabled;
            Filter.Channel = dto.Filter.Channel;
            Filter.Author = dto.Filter.Author;
            Filter.Message = dto.Filter.Message;
            Buffer.Clear();
            Buffer.AppendRange(dto.Buffer);

            Initialize(dto.Id);
        }

        public void Initialize(string id)
        {
            Id = id;

            if (_initialized) return;

            In = Filter.In;
            if (!IsDisabled) Filter.Out += _deduper.In;
            _deduper.Out += Buffer.In;
            Buffer.Out += _audioAlert.In;
            _audioAlert.Out += _toaster.In;
            _toaster.Out += (o, e) => { RaiseOut(e); };

            _initialized = true;
        }

        public BucketDto ToDto() => new BucketDto
            {
                Id = Id,
                Name = Name,
                Size = Size,
                Notify = Notify,
                Dedupe = Dedupe,
                UseAudio = UseAudio,
                AudioFile = AudioFile,
                IsDisabled = IsDisabled,
                Filter = new BucketDto.FilterOptions {
                    Channel = Filter.Channel,
                    Author = Filter.Author,
                    Message = Filter.Message
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
