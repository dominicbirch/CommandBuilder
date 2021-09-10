using System.Threading;
using System.Threading.Tasks;

namespace CommandBuilder.Adapters
{
    /// <summary>
    /// Allows a synchronous command to be used as if it were an asynchronous command.
    /// </summary>
    /// <typeparam name="TContext">The type of context used by the command.</typeparam>
    public class AsynchronousCommandAdapter<TContext> : IAsyncCommand<TContext>
    {
        private readonly ICommand<TContext> _command;

        public AsynchronousCommandAdapter(ICommand<TContext> command)
        {
            _command = command;
        }


        /// <inheritdoc />
        public Task ExecuteAsync(TContext context, CancellationToken cancellationToken = default)
        {
            _command.Execute(context, cancellationToken);

            return Task.CompletedTask;
        }
    }
}