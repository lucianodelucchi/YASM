using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace YASM.Utils
{
    /// <summary>
    /// 
    /// </summary>
    /// <see cref="https://msdn.microsoft.com/en-us/magazine/dn630647.aspx"></see>
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }

    public abstract class AsyncCommandBase : IAsyncCommand
    {
        public abstract bool CanExecute(object parameter);

        public abstract Task ExecuteAsync(object parameter);

        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public class AsyncCommand : AsyncCommandBase, INotifyPropertyChanged
    {
        private readonly Func<object, Task> _command;
        private NotifyTask _execution;
        private readonly Func<object, bool> _canExecute;
        
        /// <summary>
        /// Whether the asynchronous command is currently executing.
        /// </summary>
        public bool IsExecuting
        {
            get
            {
                if (Execution == null)
                    return false;
                return Execution.IsNotCompleted;
            }
        }

        public AsyncCommand(Func<object, Task> command, Func<object, bool> canExecute = null)
        {
            _command = command;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return !IsExecuting;
            return _canExecute(parameter);
        }

        public override async Task ExecuteAsync(object parameter)
        {
            Execution = NotifyTask.Create(_command(parameter));
            if (_canExecute == null)
                base.RaiseCanExecuteChanged();
            OnPropertyChanged("Execution");
            OnPropertyChanged("IsExecuting");
            await Execution.TaskCompleted;
            if (_canExecute == null)
                base.RaiseCanExecuteChanged();
            OnPropertyChanged("IsExecuting");
        }

        public NotifyTask Execution
        {
            get { return _execution; }
            private set
            {
                _execution = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
