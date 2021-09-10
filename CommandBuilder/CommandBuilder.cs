using System;
using System.Threading;

namespace CommandBuilder
{
    /// <summary>
    /// Used to create commands which execute each of the supplied commands once in sequence.
    /// </summary>
    /// <typeparam name="T">The type which the built commands may execute against.</typeparam>
    public class CommandBuilder<T> : ICommandBuilder<T>
    {
        private Func<T, CancellationToken, T> _sequence = (i, _) => i;
        private static Func<T, CancellationToken, T> Compose(Func<T, CancellationToken, T> f1, Action<T, CancellationToken> f2) =>
            (instance, cancellationToken) =>
            {
                f2(f1(instance, cancellationToken), cancellationToken);

                return instance;
            };


        /// <inheritdoc />
        public ICommandBuilder<T> Add<T1>(T1 command) where T1 : ICommand<T>
        {
            _sequence = Compose(_sequence, command.Execute);

            return this;
        }

        /// <inheritdoc />
        public ICommand<T> Build() => new Command<T>(_sequence);
    }
}
