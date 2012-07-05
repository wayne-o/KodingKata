// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDispatchCommits.cs" company="Project Attack Ltd">
//   2012 Project Attack Ltd
// </copyright>
// <summary>
//   The i dispatch commits.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Infrastructure
{
    using System;

    /// <summary>
    /// The i dispatch commits.
    /// </summary>
    public interface IDispatchCommits : IDisposable
    {
        #region Public Methods and Operators

        /// <summary>
        /// The dispatch.
        /// </summary>
        /// <param name="commit">
        /// The commit.
        /// </param>
        void Dispatch(Commit commit);

        #endregion
    }
}