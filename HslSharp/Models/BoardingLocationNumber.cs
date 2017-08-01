namespace HslSharp.Models
{
    public class BoardingLocationNumber
    {
        public BoardingNumberLocationType NumberType { get; private set; }
        public ushort BoardingNumber { get; private set; }

        /// <summary>
        /// The number of a boarded element, and a piece of metadata indicating how to interpret that number.
        /// </summary>
        /// <param name="numberType">The type of thing that was boarded.</param>
        /// <param name="boardingNumber">A number describing a bus, train or platform number.</param>
        internal BoardingLocationNumber(BoardingNumberLocationType numberType, ushort boardingNumber)
        {
            NumberType = numberType;
            BoardingNumber = boardingNumber;
        }
    }
}
