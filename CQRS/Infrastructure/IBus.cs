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

    /// <summary>
    /// The i bus.
    /// </summary>
    public interface IBus
    {
        #region Public Methods and Operators

        /// <summary>
        /// The register handler.
        /// </summary>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        void RegisterHandler<T>(Action<T> handler) where T : class, IDomainEvent;

        /// <summary>
        /// The send.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        void Send<T>(T command) where T : class, ICommand;

        #endregion
    }
}