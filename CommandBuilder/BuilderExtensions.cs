using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommandBuilder
{
    public static class BuilderExtensions
    {
        public static ICommandBuilder<T> Add<T>(this ICommandBuilder<T> builder, Func<T, T> action) =>
           builder.Add(new Command<T>(action));

        public static ICommandBuilder<T> AddIf<T>(this ICommandBuilder<T> builder, Predicate<T> predicate, Func<T, T> action) =>
            builder.Add(new Command<T>(i => predicate(i) ? action(i) : i));

        public static ICommandBuilder<T> Add<T>(this ICommandBuilder<T> builder, Action<T> action) =>
            builder.Add(new Command<T>(i =>
            {
                action(i);

                return i;
            }));

        public static ICommandBuilder<T> Add<T>(this ICommandBuilder<T> builder, Action<T, CancellationToken> action) =>
            builder.Add(new Command<T>((c, ct) =>
            {
                action(c, ct);

                return c;
            }));

        public static ICommandBuilder<T> Add<T>(this ICommandBuilder<T> builder, AsyncContextTransformation<T> transformation) =>
            builder.Add(new AsyncCommand<T>(transformation).ToSync());

        public static ICommandBuilder<T> Add<T>(this ICommandBuilder<T> builder, AsyncContextHandler<T> action) =>
            builder.Add(new AsyncCommand<T>(async (c, ct) =>
            {
                await action(c, ct).ConfigureAwait(false);

                return c;
            }).ToSync());


        public static ICommandBuilder<T> AddIf<T>(this ICommandBuilder<T> builder, Predicate<T> predicate, Action<T> action) =>
            builder.Add(new Command<T>(i =>
            {
                if (predicate(i)) action(i);

                return i;
            }));

        public static ICommandBuilder<T> AddIf<T>(this ICommandBuilder<T> builder, Predicate<T> predicate, Action<T, CancellationToken> action) =>
            builder.Add(new Command<T>((i, ct) =>
            {
                if (predicate(i)) action(i, ct);

                return i;
            }));

        public static IAsyncCommandBuilder<T> Add<T>(this IAsyncCommandBuilder<T> builder, AsyncContextTransformation<T> action) =>
            builder.Add(new AsyncCommand<T>(action));

        public static IAsyncCommandBuilder<T> Add<T>(this IAsyncCommandBuilder<T> builder, AsyncContextHandler<T> action) =>
            builder.Add(new AsyncCommand<T>(async (c, ct) =>
            {
                await action(c, ct).ConfigureAwait(false);

                return c;
            }));

        public static IAsyncCommandBuilder<T> Add<T>(this IAsyncCommandBuilder<T> builder, Func<T, CancellationToken, T> transform) =>
            builder.Add(new Command<T>(transform).ToAsync());

        public static IAsyncCommandBuilder<T> Add<T>(this IAsyncCommandBuilder<T> builder, Action<T, CancellationToken> action) =>
            builder.Add(new Command<T>((c, ct) =>
            {
                action(c, ct);

                return c;
            }).ToAsync());


        public static IAsyncCommandBuilder<T> AddIf<T>(this IAsyncCommandBuilder<T> builder, Predicate<T> predicate, AsyncContextTransformation<T> action) =>
            builder.Add(new AsyncCommand<T>((i, ct) => predicate(i) ? action(i, ct) : Task.FromResult(i)));

        public static IAsyncCommandBuilder<T> AddIf<T>(this IAsyncCommandBuilder<T> builder, Predicate<T> predicate, AsyncContextHandler<T> action) =>
            builder.Add(new AsyncCommand<T>(async (i, ct) =>
            {
                if (predicate(i)) await action(i, ct).ConfigureAwait(false);

                return i;
            }));

        public static IAsyncCommandBuilder<T> If<T>(this IAsyncCommandBuilder<T> builder, Predicate<T> predicate, Func<IAsyncCommandBuilder<T>, IAsyncCommandBuilder<T>> command)
            => builder.Add(new AsyncCommand<T>((async (context, ct) =>
            {
                if (predicate(context))
                {
                    var innerCommand = command(new AsyncCommandBuilder<T>()).Build();

                    await innerCommand.ExecuteAsync(context, ct).ConfigureAwait(false);
                }

                return context;
            })));

        /// <summary>
        /// Adds a command step which throws the specified exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static IAsyncCommandBuilder<TContext> Throw<T, TContext>(this IAsyncCommandBuilder<TContext> builder, T exception)
            where T : Exception
            where TContext : class
            => builder.Add(new AsyncCommand<TContext>((_, _) => throw exception));

        /// <summary>
        /// Adds a command step which throws the specified exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static ICommandBuilder<TContext> Throw<T, TContext>(this ICommandBuilder<TContext> builder, T exception)
            where T : Exception
            where TContext : class
            => builder.Add(new Command<TContext>((_, _) => throw exception));



        public static IAsyncCommandBuilder<T> Retry<T>(this IAsyncCommandBuilder<T> builder,
            Func<IAsyncCommandBuilder<T>, IAsyncCommandBuilder<T>> command, byte maxAttempts = 3,
            TimeSpan delay = default,
            Func<T, Exception, Task>? handleFailedAttemptAsync = default)
            => builder.Add(command(new AsyncCommandBuilder<T>()).Build()
                .WithRetry(maxAttempts, delay, handleFailedAttemptAsync));

        public static ICommandBuilder<T> Retry<T>(this ICommandBuilder<T> builder,
            Func<ICommandBuilder<T>, ICommandBuilder<T>> command, byte maxAttempts = 3,
            TimeSpan delay = default,
            Action<T, Exception>? handleFailedAttempt = default)
            => builder.Add(command(new CommandBuilder<T>()).Build()
                .WithRetry(maxAttempts, delay, handleFailedAttempt));
    }
}
