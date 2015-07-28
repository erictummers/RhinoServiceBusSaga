using Common.Logging;
using Messages;
using Rhino.ServiceBus;
using Rhino.ServiceBus.Sagas;
using System;

namespace Service
{
    /// <summary>
    /// Saga to bundle Files in a database and
    /// ship that database when 30 minutes no new
    /// files are received
    /// </summary>
    public class GroupingSaga : ISaga<GroupSagaState>,
        InitiatedBy<ProcessFileCommand>,
        Orchestrates<CheckReadyCommand>
    {
        private ILog _logger = LogManager.GetCurrentClassLogger();
        private readonly IServiceBus _bus;

        /// <summary>Gets or sets the state.</summary>
        public GroupSagaState State { get; set; }
        /// <summary>Gets or sets the identifier.</summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is completed.
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupingSaga"/> class.
        /// </summary>
        /// <param name="bus">The bus.</param>
        public GroupingSaga(IServiceBus bus)
        {
            _bus = bus;
            State = new GroupSagaState();
        }

        /// <summary>Consumes the specified message.</summary>
        /// <param name="message">The message.</param>
        public void Consume(ProcessFileCommand message)
        {
            _logger.DebugFormat("ProcessFileCommand with {0}", message.PathToFile);

            // first call to the saga, initialize the state
            if (State.IsInitialized == false)
            {
                _logger.InfoFormat("Initializing {0}", Id.ToString());
                State.Database = string.Format(@"d:\temp\sea\{0}.mdb", Id.ToString());
                State.IsInitialized = true;
            }
            // save the last action for checking the timeout
            State.LastAction = DateTime.Now;

            // remind myself to check the last action after 30 minutes
            var thirtyMinuteDelay = State.LastAction.Add(TimeSpan.FromSeconds(10));
            _bus.DelaySend(thirtyMinuteDelay,
                new CheckReadyCommand { CorrelationId = Id });

            // save file to database
            _bus.Send(new WriteDatabaseCommand { Database = State.Database, Data = "*" });
        }

        /// <summary>Consumes the specified message.</summary>
        /// <param name="message">The message.</param>
        public void Consume(CheckReadyCommand message)
        {
            _logger.Debug("CheckReadyCommand");

            // check last action (in state) was 30 minutes ago
            var ThirtyMinutesAgo = DateTime.Now.Subtract(TimeSpan.FromSeconds(10));
            if (State.LastAction <= ThirtyMinutesAgo)
            {
                // ship the database
                _bus.Send(new ShipDatabaseCommand { PathToDatabase = State.Database });
                
                // the saga ended
                _bus.Publish(new SagaEndedEvent());

                _logger.Info("Saga completed");
                IsCompleted = true;
            }
            else
            {
                // there has been activity, don't end the saga
                _logger.InfoFormat("Saga not completed {0} > {1}",
                    State.LastAction, ThirtyMinutesAgo);
            }
        }
    }
}
