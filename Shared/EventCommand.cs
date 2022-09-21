using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Shared
{
    public class CommadEventHandler<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public Action<T> action;
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.action((T)parameter);
        }
        public CommadEventHandler(Action<T> action)
        {
            this.action = action;

        }
    }

}
