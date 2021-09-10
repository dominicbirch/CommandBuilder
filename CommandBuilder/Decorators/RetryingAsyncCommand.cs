using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommandBuilder.Decorators
{
    public class RetryingAsyncCommand<T> : IAsyncCommand<T>
    {
        private readonly IAsyncCommand<T> _inner;
        private readonly byte _maxAttempts;
        private readonly TimeSpan _delay;
        private readonly Func<T, Exception, Task>? _handleFailedAttemptAsync;

        public RetryingAsyncCommand(IAsyncCommand<T> inner, byte maxAttempts = 3, TimeSpan delay = default, Func<T, Exception, Task>? handleFailedAttemptAsync = default)
        {
            _inner = inner;
            _maxAttempts = maxAttempts;
            _delay = delay;
            _handleFailedAttemptAsync = handleFailedAttemptAsync;
        }


        public async Task ExecuteAsync(T context, CancellationToken cancellationToken = default)
        {
            var attempts = 0;
            while (!cancellationToken.IsCancellationRequested && (_maxAttempts < 1 || ++attempts <= _maxAttempts))
                try
                {
                    await _inner.ExecuteAsync(context, cancellationToken).ConfigureAwait(false);

                    return;
                }
                catch (Exception e)
                {
                    await (_handleFailedAttemptAsync?.Invoke(context, e) ?? Task.CompletedTask);

                    if (++attempts > _maxAttempts)
                        throw;

                    await Task.Delay(_delay, cancellationToken).ConfigureAwait(false);
                }
        }
    }
}