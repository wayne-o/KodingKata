namespace Infrastructure
{
    using System;

    public interface IDispatchCommits : IDisposable
    {
        void Dispatch(Commit commit);
    }
}