using System;

namespace Service
{
    /// <summary>
    /// State information of the <see cref="GroupSaga"/>
    /// </summary>
    public class GroupSagaState
    {
        /// <summary>Gets or sets the database.</summary>
        public string Database { get; set; }
        /// <summary>Gets or sets the last action.</summary>
        public DateTime LastAction { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is initialized.
        /// </summary>
        public bool IsInitialized { get; set; }
    }
}
