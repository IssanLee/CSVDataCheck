using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace CSVDataCheck.Models.Command
{
    /// <summary>
    /// 用于通知AsyncCommand的完成
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class NotifyTaskCompletion<TResult> : INotifyPropertyChanged
    {
        #region property

        public Task<TResult> Task { get; private set; }

        public Task TaskCompletion { get; private set; }

        public TResult Result => (Task.Status == TaskStatus.RanToCompletion) ? Task.Result : default(TResult);

        public TaskStatus Status => Task.Status;

        public bool IsCompleted => Task.IsCompleted;

        public bool IsNotCompleted => !Task.IsCompleted;

        public bool IsSuccessfullyCompleted => Task.Status == TaskStatus.RanToCompletion;

        public bool IsCanceled => Task.IsCanceled;

        public bool IsFaulted => Task.IsFaulted;

        public AggregateException Exception => Task.Exception;

        public Exception InnerException => Exception?.InnerException;

        public string ErrorMessage => InnerException?.InnerException.Message;

        #endregion

        public NotifyTaskCompletion(Task<TResult> task)
        {
            Task = task;
            TaskCompletion = WatchTaskAsync(task);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task WatchTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch
            {
                // ignored
            }
            finally
            {
                NotifyPropertiesChanged(task);
            }
        }

        private void NotifyPropertiesChanged(Task task)
        {
            var handle = PropertyChanged;
            if (handle == null)
            {
                return;
            }

            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(IsCompleted));
            OnPropertyChanged(nameof(IsNotCompleted));
            if (task.IsCanceled)
            {
                OnPropertyChanged(nameof(IsCanceled));
            }
            else if (task.IsFaulted)
            {
                OnPropertyChanged(nameof(IsFaulted));
                OnPropertyChanged(nameof(Exception));
                OnPropertyChanged(nameof(InnerException));
                OnPropertyChanged(nameof(ErrorMessage));
            }
            else
            {
                OnPropertyChanged(nameof(IsSuccessfullyCompleted));
                OnPropertyChanged(nameof(Result));
            }
        }
    }
}
