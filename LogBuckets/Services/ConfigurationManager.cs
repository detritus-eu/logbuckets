using LogBuckets.Models;
using System;
using System.IO;

namespace LogBuckets.Services
{
    internal sealed class ConfigurationManager: IConfigurationManager
    {

        #region Private Fields

        private readonly IObjectIo _io;

        private Configuration _config;
        private string _path;

        #endregion

        #region Ctor

        public ConfigurationManager(
            IObjectIo io)
        {
            _io = io ?? throw new ArgumentNullException(nameof(io));
            _config = new Configuration();
        }

        #endregion

        #region Public Interface

        public Configuration Config => _config;

        public void Initialize(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));

            if (_config != null) _config.PropertyChanged -= Config_PropertyChanged;

            _path = path;
            if (File.Exists(path)) _config = _io.Load<Configuration>(_path);
            if (_config == null) _config = new Configuration();
            _config.PropertyChanged += Config_PropertyChanged;
        }


        #endregion

        #region Private Methods

        private void Config_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _io.Save(ref _config, _path);
        }

        #endregion





    }
}
