using System;
using System.Threading.Tasks;

namespace CommandBuilder
{
    /// <summary>
    /// Used to create commands which execute each of the supplied commands once in sequence.
    /// </summary>
    /// <typeparam name="TContext">The type which the built commands may execute against.</typeparam>
    public class AsyncCommandBuilder<TContext> : IAsyncCommandBuilder<TContext>
    {
        private AsyncContextTransformation<TContext> _sequence = (c, _) => Task.FromResult(c);
        private static AsyncContextTransformation<TContext> Compose(AsyncContextTransformation<TContext> f1, AsyncContextHandler<TContext> f2) =>
            async (c, ct) =>
            {
                var r = await f1(c, ct).ConfigureAwait(false);
                if (!ct.IsCancellationRequested)
                    await f2(r, ct).ConfigureAwait(false);

                return r;
            };


        /// <inheritdoc />
        public IAsyncCommandBuilder<TContext> Add<TCommand>(TCommand command) where TCommand : IAsyncCommand<TContext>
        {
            _sequence = Compose(_sequence, command.ExecuteAsync);

            return this;
        }

        /// <inheritdoc />
        public IAsyncCommand<TContext> Build() => new AsyncCommand<TContext>(_sequence);
    }
}
