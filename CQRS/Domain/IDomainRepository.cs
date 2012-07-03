// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDomainRepository.cs" company="Project Attack Ltd">
//   2012 Project Attack Ltd
// </copyright>
// <summary>
//   The i domain repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Domain
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The i domain repository.
    /// </summary>
    public interface IDomainRepository
    {
        #region Public Methods and Operators

        T GetById<T>(Guid aggregateId, uint version) where T : class, IAggregateRoot, IEventAccessor;

        void Save<T>(T aggregate, Guid commitId, IDictionary<string, string> headers)
            where T : class, IAggregateRoot, IEventAccessor;

        #endregion
    }
}