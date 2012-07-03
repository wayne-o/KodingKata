// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAggregateRootFactory.cs" company="Project Attack Ltd">
//   2012 Project Attack Ltd
// </copyright>
// <summary>
//   The i aggregate root factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Domain
{
    using System;

    using CommonDomain;

    /// <summary>
    /// The i aggregate root factory.
    /// </summary>
    public interface IAggregateRootFactory
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
        IAggregateRoot Build(Type type, Guid id, IMemento snapshot);

        #endregion
    }
}