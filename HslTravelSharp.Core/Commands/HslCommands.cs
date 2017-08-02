namespace HslTravelSharp.Core.Commands
{
    /// <summary>
    /// Contains definitions of common HSL MiFare DESFire command and response codes.
    /// </summary>
    public static class HslCommands
    {
        // Commands

        /// <summary>
        /// DESFire Select Application command for selecting the HSL application on the card.
        /// Returns <see cref="OkResponse"/> on success.
        /// </summary>
        public static readonly byte[] SelectHslCommand = { 0x90, 0x5A, 0x00, 0x00, 0x03, 0x11, 0x20, 0xEF, 0x00 };

        /// <summary>
        /// Command to read app info file, which contains application version, card name, etc.
        /// </summary>
        public static readonly byte[] ReadAppInfoCommand = { 0x90, 0xBD, 0x00, 0x00, 0x07, 0x08, 0x00, 0x00, 0x00, 0x0B, 0x00, 0x00, 0x00 };

        /// <summary>
        /// Command to read the season pass file on the card.
        /// </summary>
        public static readonly byte[] ReadPeriodPassCommand = { 0x90, 0xBD, 0x00, 0x00, 0x07, 0x01, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00 };

        /// <summary>
        /// Command to read the stored value on the card.
        /// </summary>
        public static readonly byte[] ReadStoredValueCommand = { 0x90, 0xBD, 0x00, 0x00, 0x07, 0x02, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00 };

        /// <summary>
        /// Command to read the active eTicket on the card.
        /// </summary>

        public static readonly byte[] ReadETicketCommand = { 0x90, 0xBD, 0x00, 0x00, 0x07, 0x03, 0x00, 0x00, 0x00, 0x1A, 0x00, 0x00, 0x00 };
        /// <summary>
        /// Command to read the 8 most recent transactions on the card.
        /// </summary>
        public static readonly byte[] ReadHistoryCommand = { 0x90, 0xBB, 0x00, 0x00, 0x07, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        /// <summary>
        /// Reads the remaining bytes-to-be-sent if a read request returned <see cref="MoreData"/>.
        /// </summary>
        public static readonly byte[] ReadNextCommand = { 0x90, 0xAF, 0x00, 0x00, 0x00 };

        // Responses
        /// <summary>
        /// DESFire OPERATION_OK response.
        /// </summary>
        public static readonly byte[] OkResponse = { 0x91, 0x00 };

        /// <summary>
        /// DESFire ADDTIONAL_FRAME response. Indicates that more data is expected to be sent.
        /// </summary>
        public static readonly byte[] MoreData = { 0x91, 0xAF };
    }
}
