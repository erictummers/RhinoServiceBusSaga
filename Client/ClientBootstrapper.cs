using Common.Logging;
using Rhino.ServiceBus;
using Rhino.ServiceBus.StructureMap;
using StructureMap;
using System;

namespace Client
{
    /// <summary>
    /// Used to Bootstrap the client service bus
    /// </summary>
    /// <remarks>
    /// Rhino.ServiceBus.Host.exe will load this as it is the only
    /// implementation of AbstractBootStrapper
    /// </remarks>
    public class ClientBootstrapper : StructureMapBootStrapper
    {
        private ILog _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientBootstrapper"/> class.
        /// </summary>
        public ClientBootstrapper() : base() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientBootstrapper"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public ClientBootstrapper(IContainer container) : base(container) { }

        // StructureMap configuration
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            _logger.Debug("ConfigureContainer");
            // scan current assembly for consumers and interface/implementation pairs
            Container.Configure(x =>
            {
                x.Scan(s =>
                {
                    s.AssemblyContainingType<ClientBootstrapper>();
                    s.WithDefaultConventions();
                });
            });
        }
    }
}
