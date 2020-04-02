using LogBuckets.Components.Buckets;
using LogBuckets.Components.Help;
using LogBuckets.Components.Navbar;
using LogBuckets.Components.Settings;
using LogBuckets.Models;
using LogBuckets.Services;
using LogBuckets.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace LogBuckets.Components.Main
{
    internal sealed class MainViewModel: INotifyPropertyChanged
    {

        #region Private Fields

        private readonly IList<IViewModel> _vms;
        private bool _initialized;

        #endregion

        #region Ctor

        public MainViewModel(
            NavbarViewModel navbarVm,
            BucketsViewModel bucketsVm,
            SettingsViewModel settingsVm,
            HelpViewModel helpVm)
        {
            NavbarVm = navbarVm ?? throw new ArgumentNullException(nameof(navbarVm));

            _vms = new List<IViewModel>
            {
                bucketsVm,
                settingsVm,
                helpVm
            };
        }



        #endregion

        #region Public Interface

        public NavbarViewModel NavbarVm { get; }


        private object _content;
        public object Content
        {
            get { return _content; }
            set
            {
                if (_content != value)
                {
                    _content = value;
                    RaisePropertyChanged(nameof(Content));
                }
            }
        }


        private Configuration _config;
        public Configuration Config
        {
            get { return _config; }
            set
            {
                if (_config != value)
                {
                    _config = value;
                    RaisePropertyChanged(nameof(Config));
                    foreach (var vm in _vms) vm.Config = Config;
                }
            }
        }

        public void OnLoaded(object sender, RoutedEventArgs e)
        {
            Initialize(sender as Window);
        }

        public void OnExit()
        {
            foreach (var vm in _vms)
                vm.OnExit();
        }

        #endregion

        #region Private Methods


        private void Initialize(Window win)
        {
            if (win == null) throw new ArgumentNullException(nameof(win));
            if (_initialized) return;

            foreach(var vm in _vms)
            {
                var template = Wpf.CreateDataTemplate(vm.GetType(), vm.ViewType);
                win.Resources.Add(template.DataTemplateKey, template);
                NavbarVm.Buttons.Add(new MenuAction(new CommandHandler(Navigate), vm.Icon, vm, vm.Name));
            }

            Content = NavbarVm.Buttons.First().CommandParameter;

            _initialized = true;
        }


        private void Navigate(object target)
        {
            if (Content is INavigation oldContent && !oldContent.NavigateFrom()) return;
            if (target is INavigation newContent && newContent.NavigateTo()) Content = target;
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
