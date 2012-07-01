// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Project Attack Ltd">
//   2012 Project Attack Ltd
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EventService
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.Windsor.Configuration.Interpreters;

    using Infrastructure;

    using MassTransit;

    using Topshelf;

    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        private IWindsorContainer _container;
        private IBus _bus;

        /// <summary>
        /// The main.
        /// </summary>
        private static void Main()
        {
            Thread.CurrentThread.Name = "Domain Service Main Thread";
            HostFactory.Run(x =>
            {
                x.Service<Program>(s =>
                {
                    s.ConstructUsing(name => new Program());
                    s.WhenStarted(p => p.Start());
                    s.WhenStopped(p => p.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("Handles the domain logic for the Application.");
                x.SetDisplayName("Domain Service");
                x.SetServiceName("Domain.Service");
            });
        }

        private void Stop()
        {
            _container.Release(_bus);
            _container.Dispose();
            Process.GetCurrentProcess().Kill();
        }

        private void Start()
        {
            SetupContainer();
        }

        private IEndpoint GetDomainService()
        {
            var bus = _container.Resolve<IServiceBus>();
            var domainService = bus.GetEndpoint(new Uri(Keys.DomainServiceEndpoint));
            return domainService;
        }

        private void SetupContainer()
        {
            _container = new WindsorContainer(new XmlInterpreter("Windsor.config"));

            _container.Register(Component.For<IWindsorContainer>().Instance(_container));

            _container.Install(new BusInstaller(Keys.CommandServiceEndpoint));

            _bus = _container.Resolve<IBus>();
        }
    }
}