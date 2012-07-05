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

    /// <summary>
    /// The i command.
    /// </summary>
    public interface ICommand
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