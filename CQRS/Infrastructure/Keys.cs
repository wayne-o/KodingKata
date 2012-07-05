// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Keys.cs" company="Project Attack Ltd">
//   2012 Project Attack Ltd
// </copyright>
// <summary>
//   The keys.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Infrastructure
{
    /// <summary>
    /// The keys.
    /// </summary>
    public static class Keys
    {
        #region Constants and Fields

        /// <summary>
        /// The client endpoint.
        /// </summary>
        public static readonly string ClientEndpoint = "rabbitmq://localhost/Sonatribe.Web";

        /// <summary>
        /// The command service endpoint.
        /// </summary>
        public static readonly string CommandServiceEndpoint = "rabbitmq://localhost/Sonatribe.App";

        /// <summary>
        /// The domain service endpoint.
        /// </summary>
        public static readonly string DomainServiceEndpoint = "rabbitmq://localhost/Sonatribe.Domain.Service";

        #endregion
    }
}