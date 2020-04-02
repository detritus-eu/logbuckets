using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace LogBuckets.Shared
{
    internal sealed class MenuAction : IMenuAction
    {
        public MenuAction(ICommand command, string glyphName, object commandParameter = null, string toolTip = null)
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));
            Glyph = GetGlyph(glyphName);
            CommandParameter = commandParameter ?? throw new ArgumentNullException(nameof(commandParameter));
            ToolTip = toolTip;
        }

        public ICommand Command { get; }

        public Path Glyph { get; }

        public object CommandParameter { get; }

        public string ToolTip { get; }


        private Path GetGlyph(string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName)) throw new ArgumentNullException(nameof(resourceName));

            return Application.Current.FindResource(resourceName) as Path;
        }
    }
}
