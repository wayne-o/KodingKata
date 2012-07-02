// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Project Attack Ltd">
//   2012 Project Attack Ltd
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Client
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.Windsor.Configuration.Interpreters;

    using Infrastructure;

    using Magnum;

    using MassTransit;

    using Messages;

    using Topshelf;

    /// <summary>
    /// The program.
    /// </summary>
    internal class Program
    {
        private IWindsorContainer _container;
        private IBus _bus;

        /// <summary>
        /// The main.
        /// </summary>
        private static void Main()
        {
            var p = new Program();
            p.Start();
            p.Stop();
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

            CallServiceBus();
        }

        private void CallServiceBus()
        {
            _bus.Send(new SimpleCommand
                {
                     AggregateId = CombGuid.Generate(),
                     Message = "test",
                     Version = 0
                });
        }

        private IEndpoint GetDomainService()
        {
            var bus = _container.Resolve<IServiceBus>();
            var domainService = bus.GetEndpoint(new Uri(Keys.DomainServiceEndpoint));
            return domainService;
        }

        private void SetupContainer()
        {
            _container = new WindsorContainer();

            _container.Register(Component.For<IWindsorContainer>().Instance(_container));

            _container.Install(new BusInstaller(Keys.ClientEndpoint));

            _bus = _container.Resolve<IBus>();
        }
    }
}