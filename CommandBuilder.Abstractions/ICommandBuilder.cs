namespace CommandBuilder
{
    /// <summary>
    /// A builder for creating synchronous commands.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface ICommandBuilder<TContext>
    {
        /// <summary>
        /// Adds a command to the end of the chain.
        /// </summary>
        /// <typeparam name="T">The type of command.</typeparam>
        /// <param name="command">The command to add.</param>
        /// <returns>The current builder.</returns>
        ICommandBuilder<TContext> Add<T>(T command) where T : ICommand<TContext>;

        /// <summary>
        /// Builds a command from the current chain of commands.
        /// </summary>
        /// <returns>A command which executes each command added to the chain in sequence.</returns>
        ICommand<TContext> Build();
    }

    /// <summary>
    /// A builder for creating synchronous commands with a result.
    /// </summary>
    /// <typeparam name="TContext">The type of context.</typeparam>
    /// <typeparam name="TResult">The type of results.</typeparam>
    public interface ICommandBuilder<TContext, TResult>
    {
        /// <summary>
        /// Adds a command to the end of the chain.
        /// </summary>
        /// <typeparam name="T">The type of command.</typeparam>
        /// <param name="command">The command to add.</param>
        /// <returns>The current builder.</returns>
        ICommandBuilder<TContext, TResult> Add<T>(T command) where T : ICommand<TContext, TResult>;

        /// <summary>
        /// Builds a command from the current chain of commands.
        /// </summary>
        /// <returns>A command which executes each command added to the chain in sequence.</returns>
        ICommand<TContext, TResult> Build();
    }
}
