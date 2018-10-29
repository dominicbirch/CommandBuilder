using System;
using System.Threading.Tasks;

namespace CommandBuilder
{
    /// <summary>
    /// Used to create commands which execute each of the supplied commands once in sequence.
    /// </summary>
    /// <typeparam name="T">The type which the built commands may execute against.</typeparam>
    public class AsyncCommandBuilder<T> : IAsyncCommandBuilder<T>
    {
        Func<T, Task<T>> _sequence = i => Task.FromResult(i);
        Func<T, Task<T>> Compose(Func<T, Task<T>> f1, Func<T, Task> f2) =>
            async instance =>
            {
                await f2(await f1(instance));

                return instance;
            };

        /// <inheritdoc />
        public IAsyncCommandBuilder<T> Add(IAsyncCommand<T> command)
        {
            _sequence = Compose(_sequence, command.ExecuteAsync);

            return this;
        }

        /// <inheritdoc />
        public IAsyncCommand<T> Build() =>
            new AsyncCommand<T>(_sequence);
    }
}
