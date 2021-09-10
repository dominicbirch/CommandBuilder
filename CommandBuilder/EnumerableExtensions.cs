using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CommandBuilder
{
    public static class EnumerableExtensions
    {
        public static async Task ForEachAsync<T>(this IAsyncEnumerable<T> source, Func<T, Task> action, int maxDegreeOfParallelism = 0, CancellationToken cancellationToken = default)
        {
            if (maxDegreeOfParallelism == 1)
            {
                await foreach (var item in source.WithCancellation(cancellationToken))
                    await action(item).ConfigureAwait(false);

                return;
            }

            var tasks = new List<Task>();
            var isThrottled = maxDegreeOfParallelism > 0;

            await foreach (var item in source.WithCancellation(cancellationToken))
            {
                tasks.Add(action(item));

                if (isThrottled && tasks.Count == maxDegreeOfParallelism)
                    tasks.Remove(await Task.WhenAny(tasks).ConfigureAwait(false));
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        public static async Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> action, int maxDegreeOfParallelism = 0, CancellationToken cancellationToken = default)
        {
            if (maxDegreeOfParallelism <= 0)
            {
                await Task.WhenAll(source.Select(action)).ConfigureAwait(false);

                return;
            }
            if (maxDegreeOfParallelism == 1)
            {
                foreach (var item in source)
                    await action(item).ConfigureAwait(false);

                return;
            }


            var tasks = new List<Task>();

            foreach (var item in source)
            {
                tasks.Add(action(item));

                if (tasks.Count == maxDegreeOfParallelism)
                    tasks.Remove(await Task.WhenAny(tasks).ConfigureAwait(false));

                if (cancellationToken.IsCancellationRequested)
                    break;
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}