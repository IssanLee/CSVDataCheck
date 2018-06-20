using CSVDataCheck.Models.Command;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSVDataCheck.Models
{
    public class ProgressContent : INotifyPropertyChanged
    {
        private ICommand closeCommand;
        public ICommand CloseCommand
        {
            get { return closeCommand; }
        }

        public ProgressContent(Action<ProgressContent> closeAction)
        {
            closeCommand = new RelayCommand(p => closeAction(this));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
