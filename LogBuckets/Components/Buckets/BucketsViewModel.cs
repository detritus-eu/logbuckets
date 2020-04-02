using LogBuckets.Models;
using LogBuckets.Services;
using LogBuckets.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Input;

namespace LogBuckets.Components.Buckets
{
    internal sealed class BucketsViewModel: ViewModel
    {

        #region Private Fields

        private readonly IBucketService _bucketSvc;

        private ObservableCollection<IBucketTab> _tabs;
        private LogViewModel _logVm;

        #endregion

        #region Ctor

        public BucketsViewModel(
            IBucketService bucketSvc,
            LogViewModel logVm,
            AddTab addTab)
        {
            _bucketSvc = bucketSvc ?? throw new ArgumentNullException(nameof(bucketSvc));
            _logVm = logVm ?? throw new ArgumentNullException(nameof(logVm));

            _tabs = new ObservableCollection<IBucketTab>();
            
            _tabs.Add(logVm);
            _tabs.Add(addTab);
            SelectedTab = _tabs.First();

            CloseTabCommand = new CommandHandler<IBucketTab>(CloseTab);
            AddTabCommand = new CommandHandler(AddTab);

            LoadBuckets();
        }

        #endregion

        #region Public Interface

        public override string Name => "Buckets";
        public override string Icon => "pathBucket";
        public override Type ViewType => typeof(BucketsView);

        public IReadOnlyCollection<IBucketTab> Tabs => _tabs;

        public ICommand CloseTabCommand { get; }
        public ICommand AddTabCommand { get; }


        private IBucketTab _selectedTab;
        public IBucketTab SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                if (_selectedTab != value)
                {
                    _selectedTab = value;
                    RaisePropertyChanged(nameof(SelectedTab));
                    OnSelectedTabChanged();
                }
            }
        }

        public override void OnExit()
        {
            SaveBuckets();
        }

        #endregion

        #region Private Methods

        private void CloseTab(IBucketTab tab)
        {
            _logVm.Out -= tab.Bucket.In;
            var idx = _tabs.IndexOf(tab);
            SelectedTab = _tabs[idx - 1];
            _bucketSvc.Delete(tab.Bucket.Id);
            _tabs.Remove(tab);
        }

        private void AddTab()
        {
            var tab = BucketTab.Create();
            _logVm.Out += tab.Bucket.In;
            _tabs.Insert(_tabs.Count - 1, tab);
            SelectedTab = _tabs[_tabs.Count - 2];
        }

        protected override void OnConfigChanged()
        {
            SetConfig();
        }

        private void OnSelectedTabChanged()
        {
            if (SelectedTab == null) SelectedTab = _tabs.First();
            else if (SelectedTab is AddTab) AddTab();
        }


        private void SetConfig()
        {
            foreach(var tab in _tabs) tab.Config = Config;
        }


        private void LoadBuckets()
        {
            foreach(var id in _bucketSvc.GetIds())
            {
                var dto = _bucketSvc.Load(id);
                var tab = BucketTab.Create(dto);
                _logVm.Out += tab.Bucket.In;
                _tabs.Insert(_tabs.Count - 1, tab);
            }
        }

        private void SaveBuckets()
        {
            foreach (var vm in _tabs)
                if (!(vm is LogViewModel || vm is AddTab)) _bucketSvc.Save(vm.Bucket.ToDto());
        }


        #endregion

    }
}
