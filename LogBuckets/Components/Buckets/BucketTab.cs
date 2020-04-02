using LogBuckets.Models;
using LogBuckets.Shared.Pipes;
using System;
using System.ComponentModel;

namespace LogBuckets.Components.Buckets
{
    internal class BucketTab: IBucketTab, INotifyPropertyChanged
    {


        public static IBucketTab Create(BucketDto dto = null)
        {
            var tab = IoC.Get<BucketTab>();
            if (dto == null) tab.Bucket.Initialize(Guid.NewGuid().ToString());
            else tab.Bucket.Initialize(dto);
            return tab;
        }


        #region Ctor

        public BucketTab(
            IBucket bucket)
        {
            Bucket = bucket ?? throw new ArgumentNullException(nameof(bucket));

            Bucket.PropertyChanged += (o, e) => { RaisePropertyChanged(string.Empty); };
        }

        #endregion

        #region Public Interface

        public IBucket Bucket { get; }

        public string Header { get => Bucket.Name; set => Bucket.Name = value; }


        private Configuration _config;
        public Configuration Config
        {
            get { return _config; }
            set
            {
                if (_config != value)
                {
                    if (_config != null) _config.PropertyChanged -= Config_PropertyChanged;
                    _config = value;
                    RaisePropertyChanged(nameof(Config));
                    if (_config != null) _config.PropertyChanged += Config_PropertyChanged;
                    OnConfigChanged();
                }
            }
        }

        #endregion

        #region Private Methods

        private void Config_PropertyChanged(object sender, PropertyChangedEventArgs e) => OnConfigChanged();

        protected virtual void OnConfigChanged()
        {
            Bucket.Size = Config.BucketSize;
        }

        #endregion



        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion


    }
}
