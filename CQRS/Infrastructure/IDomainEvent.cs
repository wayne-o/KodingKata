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

    public interface IDomainEvent
    {
        Guid AggregateId { get; set; }

        uint Version { get; set; }
    }
}
