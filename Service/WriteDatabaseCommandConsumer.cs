using Common.Logging;
using Messages;
using Rhino.ServiceBus;
using System;
using System.Collections.Generic;

namespace Service
{
    /// <summary>
    /// Data Access Layer for writing to the database
    /// </summary>
    public class WriteDatabaseCommandConsumer : 
        ConsumerOf<WriteDatabaseCommand>,
        ConsumerOf<SagaEndedEvent>
    {
        private ILog _logger = LogManager.GetCurrentClassLogger();

        static Dictionary<string, string> _database;
        /// <summary>
        /// Initializes the <see cref="WriteDatabaseCommandConsumer"/> class.
        /// </summary>
        static WriteDatabaseCommandConsumer()
        {
            _database = new Dictionary<string, string>();
        }

        /// <summary>Consumes the specified message.</summary>
        /// <param name="message">The message.</param>
        public void Consume(WriteDatabaseCommand message)
        {
            lock (_database)
            {
                // write record to the "database"
                if (_database.ContainsKey(message.Database) == false) 
                    _database.Add(message.Database, message.Data);
                else _database[message.Database] += message.Data;
                _logger.InfoFormat("Writing record to {0} for {1}",
                    message.Database,
                    message.Data);
            }
        }

        /// <summary>Consumes the specified message.</summary>
        /// <param name="message">The message.</param>
        public void Consume(SagaEndedEvent message)
        {
            // output database to screen for debugging
            foreach(var database in _database)
            {
                _logger.InfoFormat("{0}: {1}", database.Key, database.Value);
            }
        }
    }
}
