using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CommandBuilder
{
    /// <summary>
    /// Represents a transformation which may be applied to <paramref name="context"/>, with cancellation support.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The new context value</returns>
    public delegate Task<T> AsyncContextTransformation<T>(T context, CancellationToken cancellationToken = default);

    /// <summary>
    /// Represents a handler which performs an action using an instance of <paramref name="context"/>, with cancellation support.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public delegate Task AsyncContextHandler<in T>(T context, CancellationToken cancellationToken = default);
    public delegate Task AsyncContextHandler<in T1, in T2>(T1 context, T2 detail, CancellationToken cancellationToken = default);

    public delegate void ContextHandler<in T>(T context, CancellationToken cancellationToken = default);
    public delegate void ContextHandler<in T1, in T2>(T1 context, T2 detail, CancellationToken cancellationToken = default);

    public delegate IEnumerable<T> Iterator<in TArgs, out T>(TArgs args, CancellationToken cancellationToken = default);
}