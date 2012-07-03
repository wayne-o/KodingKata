// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventAccessor.cs" company="Project Attack Ltd">
//   2012 Project Attack Ltd
// </copyright>
// <summary>
//   The i event accessor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Domain
{
    /// <summary>
    /// The i event accessor.
    /// </summary>
    public interface IEventAccessor
    {
        #region Public Properties

        /// <summary>
        /// Gets Events.
        /// </summary>
        EventRouter Events { get; }

        #endregion
    }
}