using System.Windows.Input;
using System.Windows.Shapes;

namespace LogBuckets.Shared
{
    public interface IMenuAction
    {
        ICommand Command { get; }
        object CommandParameter { get; }
        Path Glyph { get; }
        string ToolTip { get; }
    }
}
