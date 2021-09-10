namespace CommandBuilder
{
    /// <summary>
    /// Builder which may be used to create async commands with a context of <typeparamref name="TContext"/>
    /// </summary>
    /// <typeparam name="TContext">The type of context used by the commands generated.</typeparam>
    public interface IAsyncCommandBuilder<TContext>
    {
        /// <summary>
        /// Adds an async command at the end of the chain of responsibility.
        /// </summary>
        /// <param name="command">The command to be added.</param>
        /// <returns>The current builder.</returns>
        IAsyncCommandBuilder<TContext> Add<T>(T command) where T : IAsyncCommand<TContext>;

        /// <summary>
        /// Returns the result of building the current chain of commands.
        /// </summary>
        /// <returns>An async command representing the chain of built commands.</returns>
        IAsyncCommand<TContext> Build();
    }

    /// <summary>
    /// Builder which may be used to create async commands with a context of <typeparamref name="TContext"/> and a result type of <typeparamref name="TResult"/>
    /// </summary>
    /// <typeparam name="TContext">The type of context used by the commands generated.</typeparam>
    /// <typeparam name="TResult">The result type for the commands.</typeparam>
    public interface IAsyncCommandBuilder<TContext, TResult>
    {
        /// <summary>
        /// Adds an async command at the end of the chain of responsibility.
        /// </summary>
        /// <param name="command">The command to be added.</param>
        /// <returns>The current builder.</returns>
        IAsyncCommandBuilder<TContext, TResult> Add<T>(T command) where T : IAsyncCommand<TContext, TResult>;

        /// <summary>
        /// Returns the result of building the current chain of commands.
        /// </summary>
        /// <returns>An async command representing the chain of built commands.</returns>
        IAsyncCommand<TContext, TResult> Build();
    }
}
