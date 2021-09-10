using System.Threading;
using System.Threading.Tasks;

namespace CommandBuilder
{
    public class AsyncCommand<T> : IAsyncCommand<T>
    {
        private readonly AsyncContextTransformation<T> _body;
        public AsyncCommand(AsyncContextTransformation<T> body) => _body = body;


        /// <inheritdoc />
        public Task ExecuteAsync(T instance, CancellationToken cancellationToken = default)
            => _body?.Invoke(instance, cancellationToken) ?? Task.CompletedTask;


        public static implicit operator AsyncCommand<T>(AsyncContextTransformation<T> action)
            => new(action);
        public static implicit operator AsyncContextTransformation<T>(AsyncCommand<T> command)
            => command._body;

        public static implicit operator AsyncCommand<T>(AsyncContextHandler<T> action)
            => new(async (c, cts) =>
            {
                await action(c, cts).ConfigureAwait(false);

                return c;
            });
    }
}
