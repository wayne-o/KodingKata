// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommand.cs" company="Project Attack Ltd">
//   2012 Project Attack Ltd
// </copyright>
// <summary>
//   The i command.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Infrastructure
{
    using System;

    public interface ICommand
    {
        Guid AggregateId { get; set; }

        uint Version { get; set; }
    }
}
