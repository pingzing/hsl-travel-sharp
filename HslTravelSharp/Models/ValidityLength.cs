namespace HslTravelSharp.Models
{
    public class ValidityLength
    {
        public ValidityLengthType LengthType { get; set; }
        public byte Length { get; set; }

        /// <summary>
        /// A length value, and a piece of metadata about how to interpret that value.
        /// </summary>
        /// <param name="type">How the length data should be interpreted.</param>
        /// <param name="length">The value of the length.</param>
        internal ValidityLength(ValidityLengthType type, byte length)
        {
            LengthType = type;
            Length = length;
        }
    }
}
