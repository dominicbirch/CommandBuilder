using System;
using System.Threading;

namespace CommandBuilder.Adapters
{
    /// <summary>
    /// Allows an asynchronous command to be used as if it were a synchronous command.
    /// </summary>
    /// <typeparam name="TContext">The type of context used by the command.</typeparam>
    public class SynchronousCommandAdapter<TContext> : ICommand<TContext>
    {
        private readonly IAsyncCommand<TContext> _command;

        public SynchronousCommandAdapter(IAsyncCommand<TContext> command)
        {
            _command = command;
        }


        /// <inheritdoc />
        public void Execute(TContext context, CancellationToken cancellationToken = default)
        {
            _command.ExecuteAsync(context, cancellationToken).Wait(TimeSpan.FromSeconds(30));
        }
    }
}