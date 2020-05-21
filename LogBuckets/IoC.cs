using DryIoc;
using LogBuckets.Services;
using LogBuckets.Shared.Pipes;
using System;

namespace LogBuckets
{
    internal sealed class IoC
    {
        private static readonly Lazy<Container> _container = new Lazy<Container>(() => 
            new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient()));

        public static IContainer Container => _container.Value;

        public static T Get<T>() => (T)Container.Resolve(typeof(T));

        static IoC()
        {
            Initialize();
        }

        private static void Initialize()
        {

            // Single-instance

            Container.Register<IObjectIo, ObjectIo>(reuse:Reuse.Singleton);
            Container.Register<IBucketService, BucketService>(reuse: Reuse.Singleton);

            // Transient

            Container.Register<IConfigurationManager, ConfigurationManager>();
            Container.Register<ILogWatcher, LogWatcher>();
            Container.Register<IStringRing, StringRing>();
            Container.Register<IBucket, Bucket>();
            Container.Register<IFilter, Filter>();
            Container.Register<IToaster, Toaster>();
            Container.Register<IDeduper, Deduper>();
            Container.Register<IAudioAlert, AudioAlert>();

            // UI

            Container.Register<Components.Main.MainViewModel>();
            Container.Register<Components.Navbar.NavbarViewModel>();
            Container.Register<Components.Settings.SettingsViewModel>();
            Container.Register<Components.Buckets.BucketsViewModel>();
            Container.Register<Components.Buckets.LogViewModel>();
            Container.Register<Components.Buckets.BucketTab>();
            Container.Register<Components.Buckets.AddTab>();
            Container.Register<Components.Help.HelpViewModel>();
        }
    }
}
