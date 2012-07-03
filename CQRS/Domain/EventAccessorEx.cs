namespace Domain
{
    using System.Collections.Generic;
    using System.Linq;

    using EventStore;

    using Infrastructure;

    /// <summary>
    /// The event accessor ex.
    /// </summary>
    public static class EventAccessorEx
    {
        #region Public Methods and Operators

        /// <summary>
        /// The write to stream.
        /// </summary>
        /// <param name="accessor">
        /// The accessor.
        /// </param>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <param name="headers">
        /// The headers.
        /// </param>
        public static void WriteToStream(
            this IEventAccessor accessor, IEventStream stream, IDictionary<string, object> headers = null)
        {
            foreach (IDomainEvent e in accessor.Events.GetUncommitted())
            {
                var toSave = new EventMessage { Body = e };
                if (headers != null)
                {
                    headers.ToList().ForEach(kv => toSave.Headers.Add(kv.Key, kv.Value));
                }

                stream.Add(toSave);
            }
        }

        #endregion
    }
}