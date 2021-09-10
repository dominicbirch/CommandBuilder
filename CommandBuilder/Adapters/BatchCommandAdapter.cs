using System;
using System.Threading;

namespace CommandBuilder.Adapters
{
    public class BatchCommandAdapter<TBatchArgs, TContext> : ICommand<TBatchArgs>
    {
        private readonly ICommand<TContext> _command;
        private readonly Iterator<TBatchArgs, TContext> _argsToContextsMapping;
        private readonly ContextHandler<TContext>? _successHandler;
        private readonly ContextHandler<TContext, Exception>? _failureHandler;

        public BatchCommandAdapter(ICommand<TContext> command, Iterator<TBatchArgs, TContext> argsToContextsMapping, ContextHandler<TContext>? successHandler = default, ContextHandler<TContext, Exception>? failureHandler = default)
        {
            _command = command;
            _argsToContextsMapping = argsToContextsMapping;
            _successHandler = successHandler;
            _failureHandler = failureHandler;
        }


        public void Execute(TBatchArgs args, CancellationToken cancellationToken = default)
        {
            foreach (var context in _argsToContextsMapping(args, cancellationToken))
                try
                {
                    _command.Execute(context, cancellationToken);
                    _successHandler?.Invoke(context, cancellationToken);
                }
                catch (Exception e)
                {
                    _failureHandler?.Invoke(context, e, cancellationToken);
                }
        }
    }
}