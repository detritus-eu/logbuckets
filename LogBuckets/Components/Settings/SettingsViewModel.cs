using LogBuckets.Models;
using LogBuckets.Shared;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Input;

namespace LogBuckets.Components.Settings
{
    internal sealed class SettingsViewModel: ViewModel
    {
        
        #region Private Fields



        #endregion

        #region Ctor

        public SettingsViewModel()
        {
            BrowseLogFileCommand = new CommandHandler(BrowseLogFile);
        }

        #endregion

        #region Public Interface

        public override string Name => "Settings";
        public override string Icon => "pathCog";
        public override Type ViewType => typeof(SettingsView);


        public ICommand BrowseLogFileCommand { get; }


        #endregion

        #region Private Methods

        private void BrowseLogFile()
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "Log Files (*.log)|*.log|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            dlg.DefaultExt = "log";
            dlg.InitialDirectory = !string.IsNullOrEmpty(Config?.LogFile) ? Path.GetDirectoryName(Config.LogFile) : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (dlg.ShowDialog() == true) Config.LogFile = dlg.FileName;
        }

        #endregion

    }
}
