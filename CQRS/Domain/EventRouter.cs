// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventRouter.cs" company="Project Attack Ltd">
//   2012 Project Attack Ltd
// </copyright>
// <summary>
//   The event router.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Domain
{
    using System;
    using System.Collections.Generic;

    using Infrastructure;

    using MassTransit.Util;

    /// <summary>
    /// The event router.
    /// </summary>
    public class EventRouter
    {
        #region Constants and Fields

        /// <summary>
        /// The _instance.
        /// </summary>
        private readonly dynamic _instance;

        /// <summary>
        /// The _raised events.
        /// </summary>
        private readonly List<IDomainEvent> _raisedEvents = new List<IDomainEvent>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EventRouter"/> class.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Throws if instance is null.
        /// </exception>
        private EventRouter([NotNull] IAggregateRoot instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            this._instance = PrivateReflectionDynamicObject.WrapObjectIfNeeded(instance);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The for.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        public static EventRouter For<T>(T instance) where T : IAggregateRoot
        {
            return new EventRouter(instance);
        }

        /// <summary>
        /// The apply event.
        /// </summary>
        /// <param name="evt">
        /// The evt.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        public void ApplyEvent<T>(T evt) where T : IDomainEvent
        {
            this._instance.Apply(evt);
        }

        /// <summary>
        /// The get uncommitted.
        /// </summary>
        /// <returns>
        /// </returns>
        [NotNull]
        public IEnumerable<IDomainEvent> GetUncommitted()
        {
            return new List<IDomainEvent>(this._raisedEvents);
        }

        /// <summary>
        /// The raise event.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <exception cref="ArgumentException">
        /// </exception>
        public void RaiseEvent<T>(T e) where T : IDomainEvent
        {
            if (e.Version != this._instance.Version + 1)
            {
                throw new ArgumentException("The event needs to increment the aggregate's version by one.");
            }

            this._raisedEvents.Add(e);
            this._instance.Apply(e);
        }

        #endregion
    }
}