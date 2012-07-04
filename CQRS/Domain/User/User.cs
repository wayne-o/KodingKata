// --------------------------------------------------------------------------------------------------------------------
// <copyright file="User.cs" company="Project Attack Ltd">
//   Project Attack Ltd
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Domain
{
    using System;

    using Magnum;

    using MassTransit.Util;

    using Messages;

    using NLog;

    using Sonatribe.Domain;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class User : IAggregateRoot, IEventAccessor
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly EventRouter _eventRouter;

        // for construction in tests
        public User()
        {
            this._eventRouter = EventRouter.For(this);
        }

        private User(Guid id,
                     UserName userName,
                     EmailAddress emailAddress,
                     string password,
                     Guid activationKey,
                     bool sendActivationEmail)
            : this()
        {
            this.Raise<User, IUserCreated>(new UserCreated
            {
                ActivationKey = activationKey,
                Email = emailAddress.Email,
                SendActivationEmail = sendActivationEmail,
                Version = this.Version + 1,
                UserName = userName.Name,
                AggregateId = id,
                Password = password
            });
        }

        public EventRouter Events
        {
            get
            {
                return this._eventRouter;
            }
        }

        public Guid Id { get; private set; }

        public uint Version { get; set; }

        public static User CreateNew(Guid id,
                                     UserName userName,
                                     EmailAddress emailAddress,
                                     string password,
                                     Guid activationKey,
                                     bool sendActivationEmail)
        {
            return new User(id, userName, emailAddress, password, activationKey, sendActivationEmail);
        }

        [UsedImplicitly]
        public void Apply(IUserCreated @event)
        {
            this.Id = @event.AggregateId;
        }
    }
}