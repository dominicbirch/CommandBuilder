using System;

namespace CommandBuilder
{
    internal class Command<T> : ICommand<T>
    {
        readonly Func<T, T> _body;
        public Command(Func<T, T> body) =>
            _body = body;

        /// <inheritdoc />
        public void Execute(T instance) =>
            _body?.Invoke(instance);
    }
}
