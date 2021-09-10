using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CommandBuilder.Adapters
{
    public class BatchExecutionOptions<TContext>
    {
        public int MaxDegreeOfParallelism { get; set; }

        public IEnumerable<TContext>? Sequence { get; set; }
        public IAsyncEnumerable<TContext>? AsyncSequence { get; set; }

        public AsyncContextHandler<TContext>? SuccessHandler { get; set; }
        public AsyncContextHandler<TContext, Exception>? FailureHandler { get; set; }
    }

    public class BatchAsyncCommandAdapter<TBatchArgs, TContext> : IAsyncCommand<TBatchArgs>
    {
        private readonly IAsyncCommand<TContext> _command;
        private readonly Func<TBatchArgs, BatchExecutionOptions<TContext>> _optionsFactory;

        public BatchAsyncCommandAdapter(IAsyncCommand<TContext> command, Func<TBatchArgs, BatchExecutionOptions<TContext>> optionsFactory)
        {
            _command = command;
            _optionsFactory = optionsFactory;
        }


        /// <inheritdoc />
        public async Task ExecuteAsync(TBatchArgs args, CancellationToken cancellationToken = default)
        {
            var options = _optionsFactory(args);
            async Task InvokeCommandAsync(TContext context)
            {
                try
                {
                    await _command.ExecuteAsync(context, cancellationToken).ConfigureAwait(false);
                    await (options.SuccessHandler?.Invoke(context, cancellationToken) ?? Task.CompletedTask).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    await (options.FailureHandler?.Invoke(context, e, cancellationToken) ?? Task.CompletedTask).ConfigureAwait(false);
                }
            }

            await (options.AsyncSequence?.ForEachAsync(InvokeCommandAsync, options.MaxDegreeOfParallelism, cancellationToken) ?? Task.CompletedTask).ConfigureAwait(false);
            await (options.Sequence?.ForEachAsync(InvokeCommandAsync, options.MaxDegreeOfParallelism, cancellationToken) ?? Task.CompletedTask).ConfigureAwait(false);
        }
    }
}