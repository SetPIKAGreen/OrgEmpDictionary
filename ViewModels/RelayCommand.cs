using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrganizationsEmployeesDictionaryWPF.ViewModels
{
    internal class RelayCommand : ICommand
    {
        private readonly Action<object> _executeWithParam;
        private readonly Action _executeWithoutParam;
        private readonly Func<object , bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _executeWithParam = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _executeWithoutParam = execute;

            if(canExecute != null)
            {
                _canExecute = delegate (object _)
                {
                    return canExecute();
                };
            }
            else
            {
                _canExecute = null;
            }
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
            {
                return _canExecute.Invoke(parameter);
            }
            else
                return true;
        }
        public void Execute(object parameter)
        {
            if (_executeWithParam != null)
            {
                _executeWithParam(parameter);
            }
            else
            {
                _executeWithoutParam?.Invoke();
            }

        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
