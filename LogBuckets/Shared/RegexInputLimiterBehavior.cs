using Microsoft.Xaml.Behaviors;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace LogBuckets.Shared
{
    public class RegexInputLimiterBehavior : Behavior<TextBox>
    {


        public string AllowedPattern
        {
            get { return (string)GetValue(AllowedPatternProperty); }
            set { SetValue(AllowedPatternProperty, value); }
        }

        public static readonly DependencyProperty AllowedPatternProperty =
            DependencyProperty.Register("AllowedPattern", typeof(string), typeof(RegexInputLimiterBehavior), new PropertyMetadata(null));


        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewTextInput += AssociatedObject_PreviewTextInput;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewTextInput -= AssociatedObject_PreviewTextInput;
        }


        private void AssociatedObject_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (string.IsNullOrEmpty(AllowedPattern)) return;

            if (!Regex.IsMatch(e.Text, AllowedPattern)) e.Handled = true;
        }

    }
}
