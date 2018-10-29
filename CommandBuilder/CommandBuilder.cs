using System;

namespace CommandBuilder
{
    /// <summary>
    /// Used to create commands which execute each of the supplied commands once in sequence.
    /// </summary>
    /// <typeparam name="T">The type which the built commands may execute against.</typeparam>
    public class CommandBuilder<T> : ICommandBuilder<T>
    {
        Func<T, T> _sequence = i => i;
        Func<T, T> Compose(Func<T, T> f1, Action<T> f2) =>
            instance =>
            {
                f2(f1(instance));

                return instance;
            };

        /// <inheritdoc />
        public ICommandBuilder<T> Add(ICommand<T> command)
        {
            _sequence = Compose(_sequence, command.Execute);

            return this;
        }
        /// <inheritdoc />
        public ICommand<T> Build() =>
            new Command(_sequence);


        class Command : ICommand<T>
        {
            readonly Func<T, T> _body;
            public Command(Func<T, T> body) =>
                _body = body;

            /// <inheritdoc />
            public void Execute(T instance) =>
                _body?.Invoke(instance);
        }
    }
}
