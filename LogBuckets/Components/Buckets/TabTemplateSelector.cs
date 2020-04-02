using System;
using System.Windows;
using System.Windows.Controls;

namespace LogBuckets.Components.Buckets
{
    public class TabTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BucketTemplate { get; set; }
        public DataTemplate AddTemplate { get; set; }
        public DataTemplate LogTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var tab = item as IBucketTab;
            if (tab == null)
                throw new InvalidOperationException("Template selector can only be used with items implementing IBucketTab");

            if (tab.Header == AddTab.Label) return AddTemplate;
            if (tab.Header == LogViewModel.Label) return LogTemplate;
            return BucketTemplate;
        }
    }
}
