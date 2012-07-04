// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserCreated.cs" company="Project Attack Ltd">
//   2012 Project Attack Ltd
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Messages
{
    using System;

    using Infrastructure;

    public interface IUserCreated : IDomainEvent
    {
        Guid ActivationKey { get; }

        bool SendActivationEmail { get; }

        string Email { get; }

        string Password { get; }

        string UserName { get; }
    }

    /// <summary>
    /// The user created.
    /// </summary>
    [Serializable]
    public class UserCreated : IUserCreated
    {
        public Guid ActivationKey { get; set; }

        public bool SendActivationEmail { get; set; }

        public Guid AggregateId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

        public uint Version { get; set; }
    }
}
