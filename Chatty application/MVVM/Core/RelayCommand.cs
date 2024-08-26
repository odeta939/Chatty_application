using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Chatty_application.MVVM.Core
{
    internal class RelayCommand : ICommand
    {
        private Action<object> execute;
     
        private Func<object, bool> canExecute;
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }



        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }



        public bool CanExecute(object? parameter)
        {
          
                return this.canExecute == null || this.canExecute(parameter);
        
        }

        public void Execute(object? parameter)
        {
                this.execute(parameter);
          
        }
    }
}
