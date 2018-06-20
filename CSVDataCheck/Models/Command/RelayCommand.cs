using System;

namespace CSVDataCheck.Models.Command
{
    public class RelayCommand<T> : BaseCommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            if (canExecute == null)
                canExecute = o => true;

            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute((T)parameter);
        }

        protected override void OnExecute(object parameter)
        {
            _execute((T)parameter);
        }
    }

    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null) : base(execute, canExecute)
        {
        }
    }
}
