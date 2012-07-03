namespace CommandService
{
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    using MassTransit;

    /// <summary>
    /// The command handler installer.
    /// </summary>
    public class CommandHandlerInstaller : IWindsorInstaller
    {
        /// <summary>
        /// The install.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <param name="store">
        /// The store.
        /// </param>
        public void Install(IWindsorContainer container, 
                            IConfigurationStore store)
        {
            container.Register(AllTypes.FromAssemblyContaining(this.GetType()).Where(x => x.GetInterface(typeof(Consumes<>.All).Name) != null));
        }
    }
}
