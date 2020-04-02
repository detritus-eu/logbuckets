using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LogBuckets.Shared.Pipes
{
    internal sealed class Toaster: StringEventPipe, IToaster
    {
        private readonly static TaskbarIcon _tb = new TaskbarIcon
        {
            IconSource = new BitmapImage(new Uri("pack://application:,,,/LogBuckets;component/Resources/bucket.ico"))
        };

        public string Name { get; set; }

        protected override string Process(string data)
        {
            _tb.HideBalloonTip();
            _tb.ShowBalloonTip(Name, data, BalloonIcon.None);
            return data;
        }
    }
}
