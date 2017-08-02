namespace HslTravelSharp.Core.Models
{
    /// <summary>
    /// This enum is pure speculation--the underlying value is a single bit. What else _could_ it mean?
    /// </summary>
    public enum BoardingDirection : byte
    {
        /// <summary>
        /// Indicates that at the time of boarding, the transit medium 
        /// was headed toward the end of its route.
        /// </summary>
        TowardEnd,

        /// <summary>
        /// Indicates that at the time of boarding, the transit medium 
        /// was headed toward the start of its route.
        /// </summary>
        TowardStart
    }
}
