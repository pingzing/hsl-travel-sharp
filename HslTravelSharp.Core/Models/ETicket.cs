using System;

namespace HslTravelSharp.Core.Models
{
    public class ETicket
    {
        public RawETicket RawValues { get; private set; }

        /// <summary>
        /// Whether or not this ticket was purchased at a child-rate fare.
        /// </summary>
        public bool IsChildTicket => RawValues.Child == 1;
        public LanguageCode Language => (LanguageCode)RawValues.LanguageCode;        
        public ValidityLength ValidityLength { get; private set; }
        public ValidityArea ValidityArea { get; private set; }
        public DateTimeOffset SaleDateTime => RawValues.SaleDate.AddMinutes(RawValues.SaleTime);

        /// <summary>
        /// The size of the group this ticket was purchased for.
        /// </summary>
        public byte GroupSize => RawValues.GroupSize;

        /// <summary>
        /// Indicates that the ticket has been successfully purchased (???)
        /// </summary>
        public bool IsSaleValid => RawValues.SaleStatus == 1;

        public DateTimeOffset ValidityStartDate => RawValues.ValidityStartDate;
        public DateTimeOffset ValidityEndDate => RawValues.ValidityEndDate;

        /// <summary>
        /// Indicates whether or not the ticket is currently valid.
        /// </summary>
        public bool IsValid => RawValues.ValidityStatus == 1;

        public DateTimeOffset BoardingDate => RawValues.BoardingDate;

        /// <summary>
        /// The ID of the vehicle this ticket was used to board.
        /// </summary>
        public ushort BoardingVehicle => RawValues.BoardingVehicle;
        public BoardingLocationNumber BoardingLocationNumber { get; private set; }
        public BoardingDirection BoardingDirection => (BoardingDirection)RawValues.BoardingDirection;
        public ValidityArea BoardingArea { get; private set; }

        /// <summary>
        /// A friendly representation of the data contained on a given <see cref="RawETicket"/>.
        /// </summary>        
        public ETicket(RawETicket rawValues)
        {
            RawValues = rawValues;

            var validAreaType = (ValidityAreaType)RawValues.ValidityAreaType;
            ValidityArea = validAreaType == ValidityAreaType.Zone
                ? new ValidityArea((Zone)RawValues.ValidityArea)
                : new ValidityArea((Vehicle)RawValues.ValidityArea);

            ValidityLength = new ValidityLength((ValidityLengthType)RawValues.ValidityLengthType, RawValues.ValidityLength);
            BoardingLocationNumber = new BoardingLocationNumber((BoardingNumberLocationType)RawValues.BoardingLocationNumType, RawValues.BoardingLocationNum);
            BoardingArea = new ValidityArea((Zone)RawValues.BoardingArea);
        }
    }
}
