using System;
using System.Collections.Generic;

namespace HslTravelSharp.Core.Models
{
    /// <summary>
    /// A friendly representation of the values contained on a travel card.
    /// A travel card can contain two "period" (aka season pass) products:
    /// Period1, and Period2. Usually, they are the two most-recently purchased products.
    /// </summary>
    public class TravelCard
    {        
        public RawTravelCard RawValues { get; private set; }
        
        public string CardNumber => RawValues.ApplicationInstanceId;

        // Season pass 1
        public DateTimeOffset PeriodStartDate1 => RawValues.PeriodStartDate1;
        public DateTimeOffset PeriodEndDate1 => RawValues.PeriodEndDate1;        
        public ValidityArea ValidityArea1 { get; private set; }

        // Season pass 2
        public DateTimeOffset PeriodStartDate2 => RawValues.PeriodStartDate2;
        public DateTimeOffset PeriodEndDate2 => RawValues.PeriodEndDate2;
        public ValidityArea ValidityArea2 { get; private set; }

        // Season pass previous boarding info
        /// <summary>
        /// The date of the most recent boarding.
        /// </summary>
        public DateTimeOffset LastBoardingDate => RawValues.BoardingDate;

        /// <summary>
        /// The ID of the vehicle boarded at the most recent boarding.
        /// </summary>
        public ushort LastBoardingVehicleId => RawValues.BoardingVehicle;

        /// <summary>
        /// Indicator of how to interpret <see cref="LastBoardingLocationNumber"/>.
        /// </summary>
        public BoardingNumberLocationType LastBoardingType => (BoardingNumberLocationType)RawValues.BoardingLocationNumType;
        /// <summary>
        /// Number of the last boarding location. This value should be interpreted 
        /// according to the LastBoardingType enum. It might be a bus number, 
        /// train number or platform number.
        /// </summary>
        public ushort LastBoardingLocationNumber => RawValues.BoardingLocationNum;

        /// <summary>
        /// The direction the method of transit was heading when it was boarded at the most recent boarding.
        /// </summary>
        public BoardingDirection LastBoardingDirection => (BoardingDirection)RawValues.BoardingDirection;

        /// <summary>
        /// The zone which the most recent boarding ticket was purchased for.
        /// </summary>
        public ValidityArea LastBoardingArea { get; private set; }

        // Stored value (i.e. money) info
        /// <summary>
        /// Total monetary value on the card expressed in Euro cents.
        /// </summary>
        public decimal ValueTotalCents => RawValues.ValueCounter;
        public DateTimeOffset ValueLoadDateTime => RawValues.LoadingDate.AddMinutes(RawValues.LoadingTime);
        public decimal ValueLoadedCents => RawValues.LoadedValue;

        public IEnumerable<History> History => RawValues.HistoryFields;

        /// <summary>
        /// The currently (or most recently) active value ticket on the card.
        /// </summary>
        public ETicket ValueTicket => RawValues.FriendlyTicket;

        public TravelCard(RawTravelCard rawCard)
        {
            RawValues = rawCard;

            var valid1Type = (ValidityAreaType)RawValues.ValidityAreaType1;
            ValidityArea1 = valid1Type == ValidityAreaType.Zone
                ? new ValidityArea((Zone)RawValues.ValidityArea1)
                : new ValidityArea((Vehicle)RawValues.ValidityArea1);

            var valid2Type = (ValidityAreaType)RawValues.ValidityAreaType2;
            ValidityArea2 = valid2Type == ValidityAreaType.Zone
                ? new ValidityArea((Zone)RawValues.ValidityArea2)
                : new ValidityArea((Vehicle)RawValues.ValidityArea2);

            LastBoardingArea = new ValidityArea((Vehicle)RawValues.BoardingArea);
        }
    }
}
