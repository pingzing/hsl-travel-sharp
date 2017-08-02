namespace HslTravelSharp.Core.Models
{
    /// <summary>
    /// Type of the transaction recorded in a history entry.
    /// </summary>
    public enum HistoryTransactionType : byte
    {
        /// <summary>
        /// Indicates that the <see cref="History"/> containing this transaction represents a season pass transaction.
        /// </summary>
        SeasonPass,
        
        /// <summary>
        /// Indicates that the <see cref="History"/> containing this transaction represents a value ticket transaction.
        /// </summary>
        ValueTicket
    }
}
