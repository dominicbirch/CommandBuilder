﻿namespace CommandBuilder
{
    public interface ICommandBuilder<T>
    {
        ICommandBuilder<T> Add(ICommand<T> command);
        ICommand<T> Build();
    }
}
