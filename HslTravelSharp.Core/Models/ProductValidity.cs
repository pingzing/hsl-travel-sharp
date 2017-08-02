namespace HslTravelSharp.Core.Models
{
    /// <summary>
    /// Enum for the various validity states that a travel card can be in.
    /// </summary>
    public enum ProductValidity
    {
        /// <summary>
        /// The travel card's validity is in an invalid or unknown state.
        /// </summary>
        Unknown,

        /// <summary>
        /// The travel card is currently valid, and will remain valid indefinitely.
        /// </summary>
        ValidIndefinitelyNow,

        /// <summary>
        /// The travel card is not yet valid, but once activated, will remain valid indefinitely.
        /// </summary>
        ValidIndefinitelyNotStarted,

        /// <summary>
        /// The travel card is currently valid.
        /// </summary>
        ValidNow,

        /// <summary>
        /// The travel card is valid, but is not yet active.
        /// </summary>
        ValidNotStarted,

        /// <summary>
        /// The travel card has expired, and is no longer valid.
        /// </summary>
        Expired
    }
}
