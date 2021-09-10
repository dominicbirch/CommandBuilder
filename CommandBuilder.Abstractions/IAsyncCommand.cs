using System.Threading;
using System.Threading.Tasks;

namespace CommandBuilder
{
    /// <summary>
    /// Abstraction of an asynchronous command which may be executed against an context of <typeparamref name="TContext"/>.
    /// </summary>
    /// <typeparam name="TContext">The type which this command is intended to execute against.</typeparam>
    public interface IAsyncCommand<in TContext>
    {
        /// <summary>
        /// Executes this command against the context provided.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task ExecuteAsync(TContext context, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Abstraction of an asynchronous command which may be executed against a context of <typeparamref name="TContext"/> generating a <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TContext">The type which this command is intended to execute against.</typeparam>
    /// <typeparam name="TResult">The type of result created by executing the command.</typeparam>
    public interface IAsyncCommand<in TContext, TResult>
    {
        /// <summary>
        /// Executes this command against the context provided.
        /// </summary>
        Task<TResult> ExecuteAsync(TContext context, CancellationToken cancellationToken = default);
    }
}
