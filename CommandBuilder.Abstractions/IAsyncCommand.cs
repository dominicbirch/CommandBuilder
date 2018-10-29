using System.Threading.Tasks;

namespace CommandBuilder
{
    /// <summary>
    /// Abstraction of an asynchronous command which may be executed against an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type which this command is intended to execute against.</typeparam>
    public interface IAsyncCommand<T>
    {
        Task ExecuteAsync(T instance);
    }
}
