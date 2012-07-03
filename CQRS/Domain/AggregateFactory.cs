// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AggregateFactory.cs" company="Project Attack Ltd">
//   2012 Project Attack Ltd
// </copyright>
// <summary>
//   The aggregate factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Domain
{
    using System;

    using CommonDomain;

    /// <summary>
    /// The aggregate factory.
    /// </summary>
    public class AggregateFactory : IAggregateRootFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="snapshot">
        /// The snapshot.
        /// </param>
        /// <returns>
        /// </returns>
        public IAggregateRoot Build(Type type, Guid id, IMemento snapshot)
        {
            return Activator.CreateInstance(type) as IAggregateRoot;
        }

        #endregion
    }
}