using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VocalChat.WPF.Commands
{
    public class StartStopCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public event Action RequestedAction;

        public StartStopCommand(Action action)
        {
            RequestedAction = action;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            RequestedAction?.Invoke();
        }
    }
}
