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

    public class SimpleCommand : ICommand
    {
        public Guid AggregateId { get; set; }

        public uint Version { get; set; }

        public string Message { get; set; }
    }
}