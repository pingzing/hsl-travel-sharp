using System;

namespace HslSharp.Models
{
    /// <summary>
    /// The History class represents one history record from the History file. History file holds up to 7 records of transaction history.
    /// </summary>
    public class History
    {
        /// <summary>
        /// Transaction type (season journey or value ticket).
        /// </summary>
        public HistoryTransactionType TransactionType { get; set; }

        /// <summary>
        ///  Date and time of boarding.
        /// </summary>
        public DateTimeOffset BoardingDateTime { get; set; }

        /// <summary>
        /// The date at time at which the transfer ticket ends. This will be the expiration date of the season pass
        /// if this was a season pass transaction.
        /// </summary>
        public DateTimeOffset TransferEndDateTime { get; set; }

        /// <summary>
        /// The price of the ticket (used only with value tickets) in cents. Zero if this was a season pass transaction.
        /// </summary>
        public decimal TicketFare { get; set; }

        /// <summary>
        /// Amount of tickets included in the value ticket (used only with value tickets). Will always be one if this
        /// was a season pass transaction.
        /// </summary>
        public uint GroupSize { get; set; }

        /// <summary>
        /// The balance remaining on the card after this transaction. Set to zero if this was a season pass transaction.
        /// </summary>
        public decimal RemainingValue { get; set; }
    }
}
