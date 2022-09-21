using System;
using System.Windows.Input;

namespace Shared
{
    //Review :Move all event /Commands files in event folder
    public class DelegateCommand : ICommand
    {
        private SimpleEventHandler handler;
        public bool isEnabled = true;
        public event EventHandler CanExecuteChanged;
        public delegate void SimpleEventHandler();
        public DelegateCommand(SimpleEventHandler handler)
        {
            this.handler = handler;
        }
        public bool IsEnabled
        {
            get { return isEnabled; }
        }
        void ICommand.Execute(object org)
        {
            this.handler();
        }
        bool ICommand.CanExecute(object org)
        {
            return this.isEnabled;
        }
        private void OnCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
                this.CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
