// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAggregateRoot.cs" company="Project Attack Ltd">
//   2012 Project Attack Ltd
// </copyright>
// <summary>
//   The i aggregate root.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Domain
{
    using System;

    /// <summary>
    /// The i aggregate root.
    /// </summary>
    public interface IAggregateRoot
    {
        #region Public Properties

        /// <summary>
        /// Gets the id of the aggregate root
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the current aggregate root version. This correspond to the event sequence number.
        /// </summary>
        uint Version { get; }

        #endregion
    }
}