using System;
using System.Windows;

namespace LogBuckets.Shared
{
    public static class ErrorBox
    {
        public static void Show(string msg)
        {
            MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Show(Exception ex)
        {
            MessageBox.Show($"Exception of type {ex.GetType()}: {ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
