// -----------------------------------------------------------------------
// <copyright file="DomainInfrastructureInstaller.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace CommandService
{
    using System;

    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    using Domain;

    using Magnum.Policies;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DomainInfrastructureInstaller : IWindsorInstaller
    {
        public static readonly ExceptionPolicy DefaultRepoExceptionPolicy = ExceptionPolicy.InCaseOf<Exception>().Retry(1);

        public void Install(IWindsorContainer container,
                            IConfigurationStore store)
        {
            container.Register(
                C<IDomainRepository, Domain.EventStoreRepository>().DependsOn((a, b) => a.Resolve<ExceptionPolicy>("eventstore")),
                C<IAggregateRootFactory, AggregateFactory>());
        }

        private static ComponentRegistration<TS> C<TS, TC>()
            where TC : TS
            where TS : class
        {
            return Component.For<TS>().ImplementedBy<TC>().LifeStyle.Transient;
        }
    }
}
