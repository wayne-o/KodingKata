namespace Messages
{
    using System;

    using Infrastructure;

    /// <summary>
    /// The create new user.
    /// </summary>
    [Serializable]
    public class CreateNewUser : ICommand
    {
        /// <summary>
        /// Gets or sets ActivationKey.
        /// </summary>
        public Guid ActivationKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to SendActivationEmail.
        /// </summary>
        public bool SendActivationEmail { get; set; }

        /// <summary>
        /// Gets or sets AggregateId.
        /// </summary>
        public Guid AggregateId { get; set; }

        /// <summary>
        /// Gets or sets Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets FirstName.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets LastName.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets UserName.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets Version.
        /// </summary>
        public uint Version { get; set; }

        public override string ToString()
        {
            return "{0} - {1}".With(this.AggregateId, this.UserName);
        }
    }
}
