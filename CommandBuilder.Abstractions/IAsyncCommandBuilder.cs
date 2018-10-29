namespace CommandBuilder
{
    public interface IAsyncCommandBuilder<T>
    {
        IAsyncCommandBuilder<T> Add(IAsyncCommand<T> command);
        IAsyncCommand<T> Build();
    }
}
