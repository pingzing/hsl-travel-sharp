namespace HslTravelSharp.Core.Models
{
    /// <summary>
    /// The number of a boarded element, and a piece of metadata indicating how to interpret that number.
    /// </summary>
    public class BoardingLocationNumber
    {
        /// <summary>
        /// The type of thing that was boarded.
        /// </summary>
        public BoardingNumberLocationType NumberType { get; private set; }

        /// <summary>
        /// A number describing a bus, train or platform number.
        /// </summary>
        public ushort BoardingNumber { get; private set; }
        
        public BoardingLocationNumber(BoardingNumberLocationType numberType, ushort boardingNumber)
        {
            NumberType = numberType;
            BoardingNumber = boardingNumber;
        }
    }
}
