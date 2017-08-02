namespace HslTravelSharp.Core.Models
{
    /// <summary>
    /// An HSL vehicle type.
    /// </summary>
    public enum Vehicle : byte
    {
        NotDefined  = 0,
        Bus         = 1,
        Tram        = 5,
        Metro       = 6,
        Train       = 7,
        Ferry       = 8,
        ULine       = 9
    }
}
