namespace CommandBuilder
{
    /// <summary>
    /// Abstraction of a synchronous command which may be executed against an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type which this command is intended to execute against.</typeparam>
    public interface ICommand<T>
    {
        void Execute(T instance);
    }
}
