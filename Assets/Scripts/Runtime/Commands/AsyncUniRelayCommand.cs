using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Unity.AppUI.MVVM;

namespace Runtime.Commands
{
    /// <summary>
    /// A command that mirrors the functionality of <see cref="RelayCommand"/>, with the addition of
    /// accepting a <see cref="Func{TResult}"/> returning a <see cref="UniTask"/> as the execute
    /// action, and providing an <see cref="executionTask"/> property that notifies changes when
    /// <see cref="ExecuteAsync"/> is invoked and when the returned <see cref="UniTask"/> completes.
    /// </summary>
    public class AsyncUniRelayCommand : IAsyncRelayCommand
    {
        [CanBeNull] private readonly Func<bool> _canExecute;

        [CanBeNull] private readonly Func<UniTask> _execute;

        [CanBeNull] private readonly Func<CancellationToken, UniTask> _cancellableExecute;

        private readonly AsyncRelayCommandOptions _options;

        [CanBeNull] private Task _executionTask;

        [CanBeNull] private CancellationTokenSource _cancellationTokenSource;

        private UniTask? _executionUniTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncUniRelayCommand"/> class.
        /// </summary>
        /// <param name="execute"> The <see cref="Func{TResult}"/> to invoke when <see cref="ExecuteAsync"/> is called.</param>
        /// <exception cref="ArgumentNullException"> If the execute argument is null.</exception>
        public AsyncUniRelayCommand(Func<UniTask> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncUniRelayCommand"/> class.
        /// </summary>
        /// <param name="execute"> The <see cref="Func{TResult}"/> to invoke when <see cref="ExecuteAsync"/> is called.</param>
        /// <param name="options"> The <see cref="AsyncRelayCommandOptions"/> to use.</param>
        /// <exception cref="ArgumentNullException"> If the execute argument is null.</exception>
        public AsyncUniRelayCommand(Func<UniTask> execute, AsyncRelayCommandOptions options)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _options = options;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncUniRelayCommand"/> class.
        /// </summary>
        /// <param name="cancellableExecute"> The <see cref="Func{TResult}"/> to invoke when <see cref="ExecuteAsync"/> is called.</param>
        /// <exception cref="ArgumentNullException"> If the execute argument is null.</exception>
        public AsyncUniRelayCommand(Func<CancellationToken, UniTask> cancellableExecute)
        {
            _cancellableExecute = cancellableExecute ?? throw new ArgumentNullException(nameof(cancellableExecute));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncUniRelayCommand"/> class.
        /// </summary>
        /// <param name="cancellableExecute"> The <see cref="Func{TResult}"/> to invoke when <see cref="ExecuteAsync"/> is called.</param>
        /// <param name="options"> The <see cref="AsyncRelayCommandOptions"/> to use.</param>
        /// <exception cref="ArgumentNullException"> If the execute argument is null.</exception>
        public AsyncUniRelayCommand(Func<CancellationToken, UniTask> cancellableExecute,
            AsyncRelayCommandOptions options)
        {
            _cancellableExecute = cancellableExecute ?? throw new ArgumentNullException(nameof(cancellableExecute));
            _options = options;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncUniRelayCommand"/> class.
        /// </summary>
        /// <param name="execute"> The <see cref="Func{TResult}"/> to invoke when <see cref="ExecuteAsync"/> is called.</param>
        /// <param name="canExecute"> The <see cref="Func{TResult}"/> to invoke when <see cref="CanExecute"/> is called.</param>
        /// <exception cref="ArgumentNullException"> If the execute argument is null.</exception>
        public AsyncUniRelayCommand(Func<UniTask> execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncUniRelayCommand"/> class.
        /// </summary>
        /// <param name="execute"> The <see cref="Func{TResult}"/> to invoke when <see cref="ExecuteAsync"/> is called.</param>
        /// <param name="canExecute"> The <see cref="Func{TResult}"/> to invoke when <see cref="CanExecute"/> is called.</param>
        /// <param name="options"> The <see cref="AsyncRelayCommandOptions"/> to use.</param>
        /// <exception cref="ArgumentNullException"> If the execute argument is null.</exception>
        public AsyncUniRelayCommand(Func<UniTask> execute, Func<bool> canExecute, AsyncRelayCommandOptions options)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
            _options = options;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncUniRelayCommand"/> class.
        /// </summary>
        /// <param name="cancellableExecute"> The <see cref="Func{TResult}"/> to invoke when <see cref="ExecuteAsync"/> is called.</param>
        /// <param name="canExecute"> The <see cref="Func{TResult}"/> to invoke when <see cref="CanExecute"/> is called.</param>
        /// <exception cref="ArgumentNullException"> If the execute argument is null.</exception>
        public AsyncUniRelayCommand(Func<CancellationToken, UniTask> cancellableExecute, Func<bool> canExecute)
        {
            _cancellableExecute = cancellableExecute ?? throw new ArgumentNullException(nameof(cancellableExecute));
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncUniRelayCommand"/> class.
        /// </summary>
        /// <param name="cancellableExecute"> The <see cref="Func{TResult}"/> to invoke when <see cref="ExecuteAsync"/> is called.</param>
        /// <param name="canExecute"> The <see cref="Func{TResult}"/> to invoke when <see cref="CanExecute"/> is called.</param>
        /// <param name="options"> The <see cref="AsyncRelayCommandOptions"/> to use.</param>
        /// <exception cref="ArgumentNullException"> If the execute argument is null.</exception>
        public AsyncUniRelayCommand(Func<CancellationToken, UniTask> cancellableExecute, Func<bool> canExecute,
            AsyncRelayCommandOptions options)
        {
            _cancellableExecute = cancellableExecute ?? throw new ArgumentNullException(nameof(cancellableExecute));
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
            _options = options;
        }


        public event EventHandler CanExecuteChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanExecute(object parameter)
        {
            var canExecute = _canExecute?.Invoke() ?? true;
            return canExecute &&
                   ((_options & AsyncRelayCommandOptions.AllowConcurrentExecutions) != 0 ||
                    executionTask is not {IsCompleted: false});
        }
        
        

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            ExecuteAsync(parameter);
        }

        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }


        public Task ExecuteAsync(object parameter)
        {
            if (!CanExecute(parameter))
                throw new InvalidOperationException("ExecuteAsync should not be called when CanExecute returns false.");

            if (_execute is not null)
            {
                // Non cancelable command delegate
                _executionUniTask = _execute();
            }
            else
            {
                // Cancel the previous operation, if one is pending
                _cancellationTokenSource?.Cancel();
                var cts = _cancellationTokenSource = new CancellationTokenSource();
                _executionUniTask = _cancellableExecute!(cts.Token);
            }

            if ((_options & AsyncRelayCommandOptions.AllowConcurrentExecutions) == 0)
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }


            var task = executionTask = _executionUniTask?.AsTask();
            return task ?? Task.Delay(0);
        }

        public void Cancel()
        {
            if (_cancellationTokenSource is {IsCancellationRequested: false} cancellationTokenSource)
            {
                cancellationTokenSource.Cancel();
                PropertyChanged?.Invoke(this, AsyncRelayCommand.CanBeCanceledChangedEventArgs);
                PropertyChanged?.Invoke(this, AsyncRelayCommand.IsCancellationRequestedChangedEventArgs);
            }
        }


        public Task executionTask
        {
            get => _executionTask;

            private set
            {
                if (ReferenceEquals(_executionTask, value))
                    return;

                _executionTask = value;

                PropertyChanged?.Invoke(this, AsyncRelayCommand.ExecutionTaskChangedEventArgs);
                PropertyChanged?.Invoke(this, AsyncRelayCommand.IsRunningChangedEventArgs);

                var isAlreadyCompletedOrNull = value?.IsCompleted ?? true;

                if (_cancellationTokenSource is not null)
                {
                    PropertyChanged?.Invoke(this, AsyncRelayCommand.CanBeCanceledChangedEventArgs);
                    PropertyChanged?.Invoke(this, AsyncRelayCommand.IsCancellationRequestedChangedEventArgs);
                }

                if (isAlreadyCompletedOrNull)
                    return;

                MonitorTask(this, value!);
                return;

                static async void MonitorTask(AsyncUniRelayCommand command, Task task)
                {
                    await task;
                    if (!ReferenceEquals(command.executionTask, task))
                        return;

                    command.PropertyChanged?.Invoke(command, AsyncRelayCommand.ExecutionTaskChangedEventArgs);
                    command.PropertyChanged?.Invoke(command, AsyncRelayCommand.IsRunningChangedEventArgs);

                    if (command._cancellationTokenSource is not null)
                    {
                        command.PropertyChanged?.Invoke(command, AsyncRelayCommand.CanBeCanceledChangedEventArgs);
                    }

                    if ((command._options & AsyncRelayCommandOptions.AllowConcurrentExecutions) == 0)
                    {
                        command.CanExecuteChanged?.Invoke(command, EventArgs.Empty);
                    }
                }
            }
        }


        public bool canBeCancelled => isRunning && _cancellationTokenSource is {IsCancellationRequested: false};
        public bool isCancellationRequested => _cancellationTokenSource is {IsCancellationRequested: false};
        public bool isRunning => executionTask is {IsCompleted: false};
    }
}