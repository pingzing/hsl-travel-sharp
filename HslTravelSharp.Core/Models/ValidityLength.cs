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

        /// <summary>
        /// This constructor mostly exists as a convenience method for 
        /// serializers like Json.NET. Using it directly
        /// is not recommended.
        /// </summary>
        public ValidityLength()
        {

        }

        internal ValidityLength(ValidityLengthType type, byte length)
        {
            LengthType = type;
            Length = length;
        }
    }
}
