namespace HslTravelSharp.Core.Models
{
    /// <summary>
    /// Indicates how a <see cref="ValidityLength"/> should be interpreted.
    /// </summary>
    public enum ValidityLengthType : byte
    {
        Minutes = 0,
        Hours = 1,
        AllDay = 2,
        Days = 3
    }
}
