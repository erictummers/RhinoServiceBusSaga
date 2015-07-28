using Common.Logging;
using Events;
using Messages;
using Rhino.ServiceBus;
using System;

namespace Service
{
    /// <summary>
    /// Sends command to the Saga when File is received
    /// Starts a new Saga when the existing has ended
    /// </summary>
    public class ReceivedFileEventConsumer : 
        ConsumerOf<ReceivedFileEvent>,
        ConsumerOf<SagaEndedEvent>
    {
        private ILog _logger = LogManager.GetCurrentClassLogger();
        private readonly IServiceBus _bus;

        static Guid _correlationId;
        /// <summary>
        /// Initializes the <see cref="ReceivedFileEventConsumer"/> class.
        /// </summary>
        static ReceivedFileEventConsumer()
        {
            _correlationId = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceivedFileEventConsumer"/> class.
        /// </summary>
        /// <param name="bus">The bus.</param>
        public ReceivedFileEventConsumer(IServiceBus bus)
        {
            _bus = bus;
        }

        /// <summary>Consumes the specified message.</summary>
        /// <param name="message">The message.</param>
        public void Consume(ReceivedFileEvent message)
        {
            _logger.DebugFormat("ReceivedFileEvent with {0}", message.PathToFile);

            // Send command to the Saga
            _bus.Send(new ProcessFileCommand
            {
                CorrelationId = _correlationId,
                PathToFile = message.PathToFile
            });
        }

        /// <summary>Consumes the specified message.</summary>
        /// <param name="message">The message.</param>
        public void Consume(SagaEndedEvent message)
        {
            // Saga ended, generate a new correlationId for the 
            // next Saga
            _correlationId = Guid.NewGuid();
        }
    }
}
