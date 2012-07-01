// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Keys.cs" company="Project Attack Ltd">
//   2012 Project Attack Ltd
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Infrastructure
{
    public static class Keys
    {
        public static readonly string CommandServiceEndpoint = "rabbitmq://localhost/Sonatribe.App";

        public static readonly string DomainServiceEndpoint = "rabbitmq://localhost/Sonatribe.Domain.Service";

        public static readonly string ClientEndpoint = "rabbitmq://localhost/Sonatribe.Web";
    }
}
