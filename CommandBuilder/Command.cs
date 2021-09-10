using System;
using System.Threading;

namespace CommandBuilder
{
    internal class Command<T> : ICommand<T>
    {
        readonly Func<T, CancellationToken, T> _body;
        public Command(Func<T, T> body) => _body = (c, _) => body(c);
        public Command(Func<T, CancellationToken, T> body) => _body = body;

        /// <inheritdoc />
        public void Execute(T instance, CancellationToken cancellationToken = default) => _body?.Invoke(instance, cancellationToken);
    }
}
