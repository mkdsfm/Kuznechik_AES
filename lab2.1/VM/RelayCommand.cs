using System;
using System.Windows.Input;

namespace lab2._1
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        // событие Executed прикрепляет обработчик, который будет выполняться при вызове команды.
        // А свойство Command уставливает саму команду, к которой относится обработчик.
        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
