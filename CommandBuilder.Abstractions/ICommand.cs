using System.Threading;

namespace CommandBuilder
{
    /// <summary>
    /// Abstraction of a synchronous command which may be executed against an context of <typeparamref name="TContext"/>.
    /// </summary>
    /// <typeparam name="TContext">The type which this command is intended to execute against.</typeparam>
    public interface ICommand<in TContext>
    {
        void Execute(TContext context, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Abstraction of a synchronous command which may be executed against an context of <typeparamref name="TContext"/> generating a <typeparamref name="TResult"/> result.
    /// </summary>
    public interface ICommand<in TContext, out TResult>
    {
        TResult Execute(TContext context, CancellationToken cancellationToken = default);
    }
}
