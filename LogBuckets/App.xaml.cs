using Hardcodet.Wpf.TaskbarNotification;
using LogBuckets.Components.Main;
using LogBuckets.Models;
using LogBuckets.Services;
using LogBuckets.Shared;
using System.IO;
using System.Reflection;
using System.Windows;

namespace LogBuckets
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly string ConfigPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "config.json");


        private readonly IConfigurationManager _configMgr;
        private MainViewModel _mainVm;

        public App()
        {
            _configMgr = IoC.Get<IConfigurationManager>();
            _configMgr.Initialize(ConfigPath);
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DispatcherUnhandledException += App_DispatcherUnhandledException;

            _mainVm = IoC.Get<MainViewModel>();
            _mainVm.Config = _configMgr.Config;

            MainWindow = new MainView
            {
                DataContext = _mainVm
            };
            MainWindow.Show();
        }

        

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            _mainVm.OnExit();
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            ErrorBox.Show(e.Exception);
        }
    }
}
