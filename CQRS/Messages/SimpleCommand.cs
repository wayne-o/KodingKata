// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleCommand.cs" company="Project Attack Ltd">
//   2012 Project Attack Ltd
// </copyright>
// <summary>
//   The class 1.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Messages
{
    using System;

    using Infrastructure;

    /// <summary>
    /// The simple command.
    /// </summary>
    public class SimpleCommand : ICommand
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets AggregateId.
        /// </summary>
        public Guid AggregateId { get; set; }

        /// <summary>
        /// Gets or sets Message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets Version.
        /// </summary>
        public uint Version { get; set; }

        #endregion
    }
}