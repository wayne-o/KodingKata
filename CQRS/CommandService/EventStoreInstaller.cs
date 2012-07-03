// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventStoreInstaller.cs" company="Project Attack Ltd">
//   2012 Project Attack Ltd
// </copyright>
// <summary>
//   Installs Jonathan Oliver's Event Store with a JsonSerializer and an asynchronous dispatcher.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CommandService
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    using EventStore;
    using EventStore.Dispatcher;
    using EventStore.Persistence;

    using Magnum.Extensions;
    using Magnum.Policies;

    using NLog;

    public class EventStoreInstaller : IWindsorInstaller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                 Component.For<ExceptionPolicy>()
                     .Named("eventstore")
                     .LifestyleTransient()
                     .Instance(
                         ExceptionPolicy.InCaseOf<StorageUnavailableException>()
                             .CircuitBreak(4.Seconds(), 5)),
                 Component.For<IStoreEvents>()
                     .UsingFactoryMethod(k =>
                     {
                         var policy = k.Resolve<ExceptionPolicy>("eventstore");

                         var wup = Wireup.Init()
                            .LogToOutputWindow()
                             .UsingAsynchronousDispatchScheduler(k.Resolve<IDispatchCommits>())
                             .UsingSqlPersistence("SQLiteEventStore")
                             .InitializeStorageEngine()
                             .UsingJsonSerialization()
                             .Compress();

                         while (true)
                         {
                             try
                             {
                                 return policy.Do(() => wup.Build());
                             }
                             catch (StorageUnavailableException exc)
                             {
                                 Logger.Error("Event Store unavailable, retrying with '{0} {1}'", policy, exc.Message);
                                 Logger.Error(exc.Message);
                                 Logger.Error(exc.StackTrace);
                             }
                         }
                     }));
        }
    }
}
