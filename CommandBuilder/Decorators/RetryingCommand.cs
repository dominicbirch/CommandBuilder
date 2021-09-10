using System;
using System.Threading;

namespace CommandBuilder.Decorators
{
    public class RetryingCommand<T> : ICommand<T>
    {
        private readonly ICommand<T> _inner;
        private readonly byte _maxAttempts;
        private readonly TimeSpan _delay;
        private readonly Action<T, Exception>? _handleFailedAttempt;

        public RetryingCommand(ICommand<T> inner, byte maxAttempts = 3, TimeSpan delay = default, Action<T, Exception>? handleFailedAttempt = default)
        {
            _inner = inner;
            _handleFailedAttempt = handleFailedAttempt;
            _maxAttempts = maxAttempts;
            _delay = delay;
        }


        public void Execute(T context, CancellationToken cancellationToken = default)
        {
            var attempts = 0;
            while (!cancellationToken.IsCancellationRequested && (_maxAttempts < 1 || ++attempts <= _maxAttempts))
                try
                {
                    _inner.Execute(context, cancellationToken);

                    return;
                }
                catch (Exception e)
                {
                    _handleFailedAttempt?.Invoke(context, e);

                    if (++attempts > _maxAttempts)
                        throw;

                    Thread.Sleep(_delay);
                }
        }
    }
}