namespace HslTravelSharp.Core.Models
{
    /// <summary>
    /// Metadata indcating how a <see cref="BoardingLocationNumber "/> should be interpreted.
    /// </summary>
    public enum BoardingNumberLocationType : byte
    {
        /// <summary>
        /// Unused.
        /// </summary>
        Reserved = 0,

        /// <summary>
        /// <see cref="BoardingLocationNumber"/> should be interpreted as a bus number.
        /// </summary>
        BusNumber = 1,

        /// <summary>
        /// <see cref="BoardingLocationNumber"/> should be interpreted as a train number.
        /// </summary>
        TrainNumber = 2,

        /// <summary>
        /// <see cref="BoardingLocationNumber"/> should be interpreted as a platform number.
        /// </summary>
        PlatformNumber = 3
    }
}
