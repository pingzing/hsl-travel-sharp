namespace HslTravelSharp.Core.Models
{
    /// <summary>
    /// Status response when reading the travel card.
    /// </summary>
    public enum CardStatus : uint
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Ok = 0,
        NoHslCard = 1,
        HslCardDataFailure = 2,
        CardReadFailure = 3,
        HslCardnumberFailure = 4
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
