using System;
using System.Windows.Input;

namespace LogBuckets.Shared
{
    public class CommandHandler : CommandHandler<object>
    {
        public CommandHandler(Action<object> execute) : base(execute)
        { }

        public CommandHandler(Action<object> execute, Predicate<object> canExecute) : base(execute, canExecute)
        { }

        public CommandHandler(Action execute) : base(execute)
        { }

        public CommandHandler(Action execute, Func<bool> canExecute) : base(execute, canExecute)
        { }

        public CommandHandler(Action execute, Predicate<object> canExecute) : base(execute, canExecute)
        { }
    }

    public class CommandHandler<T> : ICommand
    {
        private Action<T> _execute;
        private Predicate<T> _canExecute;

        private Action _executeWithoutObj;
        private Func<bool> _canExecuteWithoutObj;

        public CommandHandler(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("Action delegate cannot be null.");

            _execute = execute;
            _canExecute = canExecute;
        }

        public CommandHandler(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("Action delegate cannot be null.");

            _executeWithoutObj = execute;
            _canExecuteWithoutObj = canExecute;
        }

        public CommandHandler(Action<T> execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("Action delegate cannot be null.");

            _execute = execute;
            _canExecuteWithoutObj = canExecute;
        }

        public CommandHandler(Action execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("Action delegate cannot be null.");

            _executeWithoutObj = execute;
            _canExecute = canExecute;
        }


        public CommandHandler(Action<T> execute)
            : this(execute, (Predicate<T>)null)
        { }

        public CommandHandler(Action execute)
            : this(execute, (Func<bool>)null)
        { }

        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
                return _canExecute((T)parameter);
            if (_canExecuteWithoutObj != null)
                return _canExecuteWithoutObj();

            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if (_execute != null)
            {
                _execute((T)parameter);
                return;
            }
            _executeWithoutObj();
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
