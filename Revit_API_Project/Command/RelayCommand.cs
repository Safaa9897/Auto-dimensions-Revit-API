using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Revit_API_Project.Command
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action _excute;


        private readonly Predicate<object> _canExcute;



        public RelayCommand(Action excute, Predicate<object> canExcute = null)
        {

            _excute = excute;

            _canExcute = canExcute;

        }


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _excute();
        }
    }
}
