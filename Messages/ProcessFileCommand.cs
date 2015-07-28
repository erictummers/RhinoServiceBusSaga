using Rhino.ServiceBus.Sagas;
using System;

namespace Messages
{
    public class ProcessFileCommand : ISagaMessage
    {
        public Guid CorrelationId { get; set; }
        public string PathToFile { get; set; }
    }
}
