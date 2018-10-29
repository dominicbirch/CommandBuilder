using System;
using System.Threading.Tasks;

namespace CommandBuilder
{
    internal class AsyncCommand<T> : IAsyncCommand<T>
    {
        readonly Func<T, Task<T>> _body;
        public AsyncCommand(Func<T, Task<T>> body) =>
            _body = body;

        /// <inheritdoc />
        public Task ExecuteAsync(T instance) =>
            _body?.Invoke(instance);
    }
}
