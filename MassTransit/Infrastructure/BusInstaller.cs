// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BusInstaller.cs" company="Project Attack Ltd">
//   2012 Project Attack Ltd
// </copyright>
// <summary>
//   Installs the service bus into the container.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Infrastructure
{
    using System.Linq;

    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    using MassTransit;
    using MassTransit.NLogIntegration;

    /// <summary>
    /// Installs the service bus into the container.
    /// </summary>
    public class BusInstaller : IWindsorInstaller
    {
        private readonly string _endpointUri;

        public BusInstaller(string endpointUri)
        {
            this._endpointUri = endpointUri;
        }

        public void Install(IWindsorContainer container, 
                            IConfigurationStore store)
        {
            // for factory
            if (container.Kernel.GetFacilities().All(x => x.GetType() != typeof(TypedFactoryFacility)))
            {
                container.AddFacility<TypedFactoryFacility>();
            }

            container.Register(
                Component
                .For<IServiceBus>()
                .UsingFactoryMethod(() =>
                    ServiceBusFactory.New(sbc =>
                                            {
                                                sbc.ReceiveFrom(this._endpointUri);
                                                sbc.UseRabbitMqRouting();
                                                sbc.UseNLog();
                                                sbc.SetPurgeOnStartup(true);
                                                sbc.Subscribe(c => c.LoadFrom(container));
                                            })).LifeStyle.Singleton);

            ////container.Register(
            ////    Component
            ////    .For<IBus>()
            ////    .UsingFactoryMethod((k, c) =>
            ////        new MassTransitPublisher(k.Resolve<IServiceBus>()))
            ////        .Forward<IDispatchCommits>().LifeStyle.Singleton);
        }
    }
}
