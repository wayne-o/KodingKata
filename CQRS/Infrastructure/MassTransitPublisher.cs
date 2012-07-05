// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MassTransitPublisher.cs" company="Project Attack Ltd">
//   2012 Project Attack Ltd
// </copyright>
// <summary>
//   The mass transit publisher.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Infrastructure
{
    using System;

    using MassTransit;

    /// <summary>
    /// The mass transit publisher.
    /// </summary>
    public class MassTransitPublisher : IBus, IDispatchCommits
    {
        #region Constants and Fields

        /// <summary>
        /// The _ bus.
        /// </summary>
        private readonly IServiceBus _Bus;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MassTransitPublisher"/> class.
        /// </summary>
        /// <param name="bus">
        /// The bus.
        /// </param>
        public MassTransitPublisher(IServiceBus bus)
        {
            this._Bus = bus;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this._Bus.Dispose();
        }

        #endregion

        #region Explicit Interface Methods

        /// <summary>
        /// The register handler.
        /// </summary>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        void IBus.RegisterHandler<T>(Action<T> handler)
        {
            this._Bus.SubscribeHandler(handler);
        }

        /// <summary>
        /// The send.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        void IBus.Send<T>(T command)
        {
            this._Bus.Publish(command);
        }

        /// <summary>
        /// The dispatch.
        /// </summary>
        /// <param name="commit">
        /// The commit.
        /// </param>
        void IDispatchCommits.Dispatch(Commit commit)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// The publish event.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        private void PublishEvent<T>(T message) where T : class
        {
            this._Bus.Publish(message);
        }

        #endregion
    }
}