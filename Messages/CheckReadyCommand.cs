using Rhino.ServiceBus.Sagas;
using System;

namespace Messages
{
    public class CheckReadyCommand : ISagaMessage
    {
        public Guid CorrelationId { get; set; }
    }
}
