using System;
using System.Threading.Tasks;

namespace CommandBuilder
{
    public static class CommandBuilderExtensions
    {
        public static ICommandBuilder<T> Add<T>(this ICommandBuilder<T> builder, Func<T, T> action) =>
            builder.Add(new Command<T>(action));

        public static ICommandBuilder<T> Add<T>(this ICommandBuilder<T> builder, Action<T> action) =>
            builder.Add(new Command<T>(i =>
            {
                action(i);

                return i;
            }));

        public static IAsyncCommandBuilder<T> Add<T>(this IAsyncCommandBuilder<T> builder, Func<T, Task<T>> action) =>
            builder.Add(new AsyncCommand<T>(action));

        public static IAsyncCommandBuilder<T> Add<T>(this IAsyncCommandBuilder<T> builder, Func<T, Task> action) =>
            builder.Add(new AsyncCommand<T>(async i =>
            {
                await action(i);

                return i;
            }));
    }
}
