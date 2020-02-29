using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ObjectPool.Example
{
    internal class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
                execute(parameter);

            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
