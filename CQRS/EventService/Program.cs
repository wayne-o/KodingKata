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
        #region Constants and Fields

        /// <summary>
        /// The _bus.
        /// </summary>
        private IBus _bus;

        /// <summary>
        /// The _container.
        /// </summary>
        private IWindsorContainer _container;

        #endregion

        #region Methods

        /// <summary>
        /// The main.
        /// </summary>
        private static void Main()
        {
            Thread.CurrentThread.Name = "Domain Service Main Thread";
            HostFactory.Run(
                x =>
                    {
                        x.Service<Program>(
                            s =>
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

        /// <summary>
        /// The get domain service.
        /// </summary>
        /// <returns>
        /// </returns>
        private IEndpoint GetDomainService()
        {
            var bus = this._container.Resolve<IServiceBus>();
            IEndpoint domainService = bus.GetEndpoint(new Uri(Keys.DomainServiceEndpoint));
            return domainService;
        }

        /// <summary>
        /// The setup container.
        /// </summary>
        private void SetupContainer()
        {
            this._container = new WindsorContainer(new XmlInterpreter("Windsor.config"));

            this._container.Register(Component.For<IWindsorContainer>().Instance(this._container));

            this._container.Install(new BusInstaller(Keys.CommandServiceEndpoint));

            this._bus = this._container.Resolve<IBus>();
        }

        /// <summary>
        /// The start.
        /// </summary>
        private void Start()
        {
            this.SetupContainer();
        }

        /// <summary>
        /// The stop.
        /// </summary>
        private void Stop()
        {
            this._container.Release(this._bus);
            this._container.Dispose();
            Process.GetCurrentProcess().Kill();
        }

        #endregion
    }
}