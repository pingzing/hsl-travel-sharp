namespace HslTravelSharp.Core.Models
{
    /// <summary>
    /// A length value, and a piece of metadata about how to interpret that value.
    /// </summary>
    public class ValidityLength
    {
        /// <summary>
        /// How the length data should be interpreted.
        /// </summary>
        public ValidityLengthType LengthType { get; set; }

        /// <summary>
        /// The value of the length.
        /// </summary>
        public byte Length { get; set; }

        public ValidityLength(ValidityLengthType lengthType, byte length)
        {
            LengthType = lengthType;
            Length = length;
        }
    }
}
