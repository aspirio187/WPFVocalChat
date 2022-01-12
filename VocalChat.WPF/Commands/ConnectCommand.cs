using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VocalChat.WPF.Commands
{
    public class ConnectCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public event Action CommandAction;

        public ConnectCommand(Action action)
        {
            CommandAction = action;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            CommandAction?.Invoke();
        }
    }
}
