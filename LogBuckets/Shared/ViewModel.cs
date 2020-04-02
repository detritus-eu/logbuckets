using LogBuckets.Models;
using System;
using System.ComponentModel;

namespace LogBuckets.Shared
{
    internal abstract class ViewModel : IViewModel, INavigation
    {

        #region IViewModel

        public abstract string Name { get; }
        public abstract string Icon { get; }
        public abstract Type ViewType { get; }

        #endregion


        #region INavigation

        public virtual bool NavigateFrom() => true;

        public virtual bool NavigateTo() => true;



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
                    if (_config !=null) _config.PropertyChanged += Config_PropertyChanged;
                    OnConfigChanged();
                }
            }
        }

        private void Config_PropertyChanged(object sender, PropertyChangedEventArgs e) => OnConfigChanged();

        protected virtual void OnConfigChanged() { }


        public virtual void OnExit() { }

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
