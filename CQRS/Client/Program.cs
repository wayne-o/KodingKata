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

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using Infrastructure;

    using Magnum;

    using MassTransit;

    using Messages;

    /// <summary>
    /// The program.
    /// </summary>
    internal class Program
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
            var p = new Program();
            p.Start();
            p.Stop();
        }

        /// <summary>
        /// The call service bus.
        /// </summary>
        private void CallServiceBus()
        {
            this._bus.Send(new SimpleCommand { AggregateId = CombGuid.Generate(), Message = "test", Version = 0 });
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
            this._container = new WindsorContainer();

            this._container.Register(Component.For<IWindsorContainer>().Instance(this._container));

            this._container.Install(new BusInstaller(Keys.ClientEndpoint));

            this._bus = this._container.Resolve<IBus>();
        }

        /// <summary>
        /// The start.
        /// </summary>
        private void Start()
        {
            this.SetupContainer();

            this.CallServiceBus();
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