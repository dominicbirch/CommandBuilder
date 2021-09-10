using System;
using System.Threading.Tasks;

namespace CommandBuilder
{
    using Adapters;
    using Decorators;

    public static class CommandExtensions
    {
        public static IAsyncCommand<T> ToAsync<T>(this ICommand<T> command)
            => new AsynchronousCommandAdapter<T>(command);

        public static ICommand<T> ToSync<T>(this IAsyncCommand<T> command)
            => new SynchronousCommandAdapter<T>(command);


        public static IAsyncCommand<T> WithRetry<T>(this IAsyncCommand<T> command, byte maxAttempts = 3,
            TimeSpan delay = default, Func<T, Exception, Task>? handleFailedAttemptAsync = default)
            => new RetryingAsyncCommand<T>(command, maxAttempts, delay, handleFailedAttemptAsync);

        public static ICommand<T> WithRetry<T>(this ICommand<T> command, byte maxAttempts = 3, TimeSpan delay = default,
            Action<T, Exception>? handleFailedAttempt = default)
            => new RetryingCommand<T>(command, maxAttempts, delay, handleFailedAttempt);
    }
}