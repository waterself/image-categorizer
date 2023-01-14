using System;
using System.Windows.Input;

namespace image_categorizer.Core
{
    public class RelayCommand : ICommand
    {
        private Action<object> _execute;
        private Func<object, bool>? _canExecute;
        private Action<object> value;
        private EventArgs? _args;

        public event System.EventHandler? CanExecuteChanged;

        public RelayCommand(Action<object> execute, Func<object, bool>? canExecute = null, EventArgs? args = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            _args = args;
        }


        public event System.EventHandler CanExcuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}
