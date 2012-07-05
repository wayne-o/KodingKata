// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDomainEvent.cs" company="Project Attack Ltd">
//   2012 Project Attack Ltd
// </copyright>
// <summary>
//   Denotes an event in the domain model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Infrastructure
{
    using System;

    /// <summary>
    /// The i domain event.
    /// </summary>
    public interface IDomainEvent
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets AggregateId.
        /// </summary>
        Guid AggregateId { get; set; }

        /// <summary>
        /// Gets or sets Version.
        /// </summary>
        uint Version { get; set; }

        #endregion
    }
}