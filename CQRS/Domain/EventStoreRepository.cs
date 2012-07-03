namespace Domain
{
    using System;
    using System.Collections.Generic;

    using EventStore;

    using Infrastructure;

    using Magnum.Policies;

    using MassTransit.Util;

    using NLog;

    /// <summary>
    /// The event store repository.
    /// </summary>
    public class EventStoreRepository : IDomainRepository
    {
        #region Constants and Fields

        /// <summary>
        /// The _logger.
        /// </summary>
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The _event store.
        /// </summary>
        private readonly IStoreEvents _eventStore;

        /// <summary>
        /// The _factory.
        /// </summary>
        private readonly IAggregateRootFactory _factory;

        /// <summary>
        /// The _retry policy.
        /// </summary>
        private readonly ExceptionPolicy _retryPolicy;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStoreRepository"/> class.
        /// </summary>
        /// <param name="eventStore">
        /// The event store.
        /// </param>
        /// <param name="factory">
        /// The factory.
        /// </param>
        /// <param name="retryPolicy">
        /// The retry policy.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public EventStoreRepository(
            [NotNull] IStoreEvents eventStore, 
            [NotNull] IAggregateRootFactory factory, 
            [NotNull] ExceptionPolicy retryPolicy)
        {
            if (eventStore == null)
            {
                throw new ArgumentNullException("eventStore");
            }

            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            if (retryPolicy == null)
            {
                throw new ArgumentNullException("retryPolicy");
            }

            this._eventStore = eventStore;
            this._factory = factory;
            this._retryPolicy = retryPolicy;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get by id.
        /// </summary>
        /// <param name="aggregateId">
        /// The aggregate id.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        public T GetById<T>(Guid aggregateId, uint version) where T : class, IAggregateRoot, IEventAccessor
        {
            try
            {
                IEventStream stream = this._eventStore.OpenStream(aggregateId, 0, checked((int)version));
                var ar = this._factory.Build(typeof(T), aggregateId, null) as T;

                if (ar != null)
                {
                    foreach (EventMessage evt in stream.CommittedEvents)
                    {
                        ar.Events.ApplyEvent(evt.Body as IDomainEvent);
                    }

                    return ar;
                }

                return null;
            }
            catch (OverflowException e)
            {
                _logger.Error(
                    "Congratulations, your domain object has over two billion events; "
                    + "you should consider customizing the EventStore library for your purposes.", 
                    e);
                throw;
            }
        }

        /// <summary>
        /// The save.
        /// </summary>
        /// <param name="aggregate">
        /// The aggregate.
        /// </param>
        /// <param name="commitId">
        /// The commit id.
        /// </param>
        /// <param name="headers">
        /// The headers.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        public void Save<T>(T aggregate, Guid commitId, IDictionary<string, string> headers)
            where T : class, IAggregateRoot, IEventAccessor
        {
            this._retryPolicy.Do(
                () =>
                    {
                        IEventStream stream = this._eventStore.OpenStream(aggregate.Id, 0, int.MaxValue);
                        IEventAccessor accessor = aggregate;
                        accessor.WriteToStream(stream);
                        try
                        {
                            stream.CommitChanges(commitId);
                        }
                        catch (DuplicateCommitException)
                        {
                            // ignore, we're OK!
                        }
                        catch (ConcurrencyException)
                        {
                            // possible merge?
                        }
                        catch (Exception exc)
                        {
                            _logger.Error(exc);
                        }
                    });
        }

        #endregion
    }
}