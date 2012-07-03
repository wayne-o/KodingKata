// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBus.cs" company="Project Attack Ltd">
//   2012 Project Attack Ltd
// </copyright>
// <summary>
//   The i bus.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Infrastructure
{
    using System;

    public interface IBus
    {
        void RegisterHandler<T>(Action<T> handler) where T : class, IDomainEvent;

        void Send<T>(T command) where T : class, ICommand;
    }
}
