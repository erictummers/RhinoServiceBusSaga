using Common.Logging;
using Messages;
using Rhino.ServiceBus;

namespace Service
{
    /// <summary>
    /// Ship the database somewhere
    /// </summary>
    public class ShipDatabaseCommandComsumer : ConsumerOf<ShipDatabaseCommand>
    {
        private ILog _logger = LogManager.GetCurrentClassLogger();

        /// <summary>Consumes the specified message.</summary>
        /// <param name="message">The message.</param>
        public void Consume(ShipDatabaseCommand message)
        {
            // just log for now, but we could send an email with the
            // datbase attached
            _logger.InfoFormat("Sending {0}", message.PathToDatabase);
        }
    }
}
