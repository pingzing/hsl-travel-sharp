using HslTravelSharp.Core.Helpers;
using System;

namespace HslTravelSharp.Core.Models
{
    public class RawTravelCard
    {        
        private enum ProductNumber : byte
        {
            One,
            Two
        }

        //HSL data file lengths        
        internal const int AppInfoLength = 11;
        internal const int PeriodPassLength = 32;
        internal const int StoredValueLength = 12;
        internal const int ETicketLength = 26;
        internal const int HistoryMaxLength = 96;

        //Member variables for data extracted from ApplicationInformation file
        public byte ApplicationVersion { get; set; }
        public String ApplicationInstanceId { get; set; }
        public byte PlatformType { get; set; }        

        //Member variables for data extracted from PeriodPass file

        //First period product
        public ushort ProductCode1 { get; set; }
        public byte ValidityAreaType1 { get; set; }
        public byte ValidityArea1 { get; set; }
        public DateTimeOffset PeriodStartDate1 { get; set; }
        public DateTimeOffset PeriodEndDate1 { get; set; }
        public ushort PeriodLength1 { get; set; }                        

        //Second period product
        public ushort ProductCode2 { get; set; }
        public byte ValidityAreaType2 { get; set; }
        public byte ValidityArea2 { get; set; }       
        public DateTimeOffset PeriodStartDate2 { get; set; }
        public DateTimeOffset PeriodEndDate2 { get; set; }
        public ushort PeriodLength2 { get; set; }                       

        //Season pass info     
        public ushort LoadedPeriodProduct { get; set; }        
        public DateTimeOffset PeriodLoadingDate { get; set; }        
        public ushort LoadedPeriodLength { get; set; }        
        public uint LoadedPeriodPrice { get; set; }        
        public ushort PeriodLoadingOrganisation { get; set; }        
        public ushort PeriodLoadingDeviceNumber { get; set; }

        //Season pass last boarding info        
        public DateTimeOffset BoardingDate { get; set; }        
        public ushort BoardingVehicle { get; set; }        
        public byte BoardingLocationNumType { get; set; }        
        public ushort BoardingLocationNum { get; set; }        
        public byte BoardingDirection { get; set; }        
        public byte BoardingArea { get; set; }        

        //Stored value (i.e. money) info      
        public uint ValueCounter { get; set; }        
        public DateTimeOffset LoadingDate { get; set; }        
        public byte LoadingTime { get; set; }        
        public uint LoadedValue { get; set; }        
        public ushort LoadingOrganisationID { get; set; }        
        public ushort LoadingDeviceNumber { get; set; }

        //Member variables for data extracted from History file        
        public History[] HistoryFields { get; set; } = new History[8];        
        public byte HistoryIndex { get; set; }

        //Member variable for error status of the travel card        
        public CardStatus Status { get; set; } = 0;
       
        public ETicket FriendlyTicket { get; set; }
        public RawETicket RawTicket { get; set; }

        /// <summary>
        /// This constructor mostly exists as a convenience method for 
        /// serializers like Json.NET. Using it directly
        /// is not recommended.
        /// </summary>
        public RawTravelCard()
        {

        }

        /// <summary>
        /// Creates a travel card object with the given byte arrays.
        /// </summary>       
        public RawTravelCard(byte[] appInfoBytes, byte[] periodPassBytes, byte[] storedValueBytes, byte[] eTicketBytes, byte[] historyBytes)
        {
            // Check length of files, but don't check length of history file
            if ((appInfoBytes.Length >= AppInfoLength) 
                && (periodPassBytes.Length >= PeriodPassLength) 
                && (storedValueBytes.Length >= StoredValueLength)
                && (eTicketBytes.Length >= ETicketLength))
            {
                ReadApplicationInfo(appInfoBytes);
                ReadPeriodPass(periodPassBytes);
                ReadStoredValues(storedValueBytes);
                RawTicket = new RawETicket(eTicketBytes, false);
                FriendlyTicket = new ETicket(RawTicket);
                ReadHistory(historyBytes, historyBytes.Length);
            }
            else
            {
                Status = CardStatus.HslCardDataFailure;
            }            
        }

        private void ReadApplicationInfo(byte[] appInfoBytes)
        {
            ApplicationVersion = (byte)(appInfoBytes[0] & 0xF0);

            //Card number
            byte[] temp = new byte[9];
            Array.Copy(appInfoBytes, 1, temp, 0, 9);
            ApplicationInstanceId = ConversionUtil.GetHexString(temp);

            PlatformType = (byte)(appInfoBytes[10] & 0xE0);
        }

        private void ReadPeriodPass(byte[] periodPassBytes)
        {
            // We'll be reusing these as temp holder variables
            ushort date1;
            ushort date2;            

            //Read PERIOD PASS 1 data
            ProductCode1 = (ushort)(((periodPassBytes[0] & 0xFF) << 6) | ((periodPassBytes[1] & 0xFC) >> 2));
            ValidityAreaType1 = (byte)((periodPassBytes[1] & 0x02) >> 1);
            ValidityArea1 = (byte)(((periodPassBytes[1] & 0x01) << 3) | ((periodPassBytes[2] & 0xE0) >> 5));

            date1 = (ushort)(((periodPassBytes[2] & 0x1F) << 9) | ((periodPassBytes[3] & 0xFF) << 1) | ((periodPassBytes[4] & 0x80) >> 7));
            PeriodStartDate1 = ConversionUtil.FromEn1545Date(date1);

            date2 = (ushort)(((periodPassBytes[4] & 0x7F) << 7) | ((periodPassBytes[5] & 0xFE) >> 1));
            PeriodEndDate1 = ConversionUtil.FromEn1545Date(date2);
            //Add a day minus a single tick to get us to the VERY end of the given day. (Note, this only works because the PeriodEndDate explicitly has a time of 00:00)
            PeriodEndDate1.Add(TimeSpan.FromDays(1)).Add(TimeSpan.FromTicks(-1));
            //store period length
            PeriodLength1 = (ushort)(date2 - date1 + 1);

            //Read PERIOD PASS 2 data
            ProductCode2 = (ushort)(((periodPassBytes[6] & 0xFF) << 8) | (periodPassBytes[7] & 0xFC));
            ProductCode2 >>= 2;
            ValidityAreaType2 = (byte)(periodPassBytes[7] & 0x02);
            ValidityArea2 = (byte)(((periodPassBytes[7] & 0x01) << 3) | ((periodPassBytes[8] & 0xE0) >> 5));

            date1 = (ushort)(((periodPassBytes[8] & 0x1F) << 9) | ((periodPassBytes[9] & 0xFF) << 1) | ((periodPassBytes[10] & 0x80) >> 7));
            PeriodStartDate2 = ConversionUtil.FromEn1545Date(date1);

            date2 = (ushort)(((periodPassBytes[10] & 0x7F) << 7) | ((periodPassBytes[11] & 0xFE) >> 1));
            PeriodEndDate2 = ConversionUtil.FromEn1545Date(date2);
            //Add a day minus a single tick to get us to the VERY end of the given day. (Note, this only works because the PeriodEndDate explicitly has a time of 00:00)
            PeriodEndDate2.Add(TimeSpan.FromDays(1)).Add(TimeSpan.FromTicks(-1));

            //store period length
            PeriodLength2 = (ushort)(date2 - date1 + 1);

            //LAST LOADING
            LoadedPeriodProduct = (ushort)(((periodPassBytes[12] & 0xFF) << 6) | ((periodPassBytes[13] & 0xFC) >> 2));
            date1 = (ushort)(((periodPassBytes[13] & 0x03) << 12) | ((periodPassBytes[14] & 0xFF) << 4) | ((periodPassBytes[15] & 0xF0) >> 4));
            ushort time1 = (ushort)(((periodPassBytes[15] & 0x0F) << 7) | ((periodPassBytes[16] & 0xFE) >> 1));
            PeriodLoadingDate = ConversionUtil.FromEn1545DateAndTime(date1, time1);
            LoadedPeriodLength = (ushort)(((periodPassBytes[16] & 0x01) << 8) | (periodPassBytes[17] & 0xFF));
            LoadedPeriodPrice = (uint)(periodPassBytes[18] & 0xFF) << 12 | ((uint)(periodPassBytes[19] & 0xFF) << 4) | (uint)(periodPassBytes[20] & 0xF0) >> 4;
            PeriodLoadingOrganisation = (ushort)(((periodPassBytes[20] & 0x0F) << 10) | ((periodPassBytes[21] & 0xFF) << 2) | ((periodPassBytes[22] & 0xC0) >> 6));
            PeriodLoadingDeviceNumber = (ushort)(((periodPassBytes[22] & 0x3F) << 8) | (periodPassBytes[23] & 0xFF));

            //LAST USE (BOARDING DATA)
            date1 = (ushort)(((periodPassBytes[24] & 0xFF) << 6) | ((periodPassBytes[25] & 0xFC) >> 2));
            time1 = (ushort)(((periodPassBytes[25] & 0x03) << 9) | ((periodPassBytes[26] & 0xFF) << 1) | ((periodPassBytes[27] & 0x80) >> 7));
            BoardingDate = ConversionUtil.FromEn1545DateAndTime(date1, time1);
            BoardingVehicle = (ushort)(((periodPassBytes[27] & 0x7F) << 7) | (periodPassBytes[28] & 0xFE) >> 1);
            BoardingLocationNumType = (byte)(((periodPassBytes[28] & 0x01) << 1) | ((periodPassBytes[29] & 0x80) >> 7));
            BoardingLocationNum = (ushort)(((periodPassBytes[29] & 0x7F) << 7) | ((periodPassBytes[30] & 0xFE) >> 1));
            BoardingDirection = (byte)(periodPassBytes[30] & 0x01);
            BoardingArea = (byte)((periodPassBytes[31] & 0xF0) >> 4);
        }

        private void ReadStoredValues(byte[] storedValueBytes)
        {
            //Value
            ValueCounter = (uint)(((storedValueBytes[0] & 0xFF) << 12) | ((storedValueBytes[1] & 0xFF) << 4) | ((storedValueBytes[2] & 0xF0) >> 4));
            //Last value loading
            ushort date1 = (ushort)(((storedValueBytes[2] & 0x0F) << 10) | ((storedValueBytes[3] & 0xFF) << 2) | ((storedValueBytes[4] & 0xC0) >> 6));
            ushort time1 = (ushort)(((storedValueBytes[4] & 0x3F) << 5) | ((storedValueBytes[5] & 0xF8) >> 3));
            LoadingDate = ConversionUtil.FromEn1545DateAndTime(date1, time1);
            LoadedValue = (uint)(((storedValueBytes[5] & 0x07) << 17) | ((storedValueBytes[6] & 0xFF) << 9) | ((storedValueBytes[7] & 0xFF) << 1) | ((storedValueBytes[8] & 0x80) >> 7));
            LoadingOrganisationID = (ushort)(((storedValueBytes[8] & 0x7F) << 4) | ((storedValueBytes[9] & 0xFE) >> 1));
            LoadingDeviceNumber = (ushort)(((storedValueBytes[9] & 0x01) << 13) | ((storedValueBytes[10] & 0xFF) << 5) | ((storedValueBytes[11] & 0xF8) >> 3));
        }

        private void ReadHistory(byte[] historyBytes, int length)
        {
            // Count history data fields
            int dataCount = length / 12;

            // Set history count initially to zero 
            HistoryIndex = 0;

            for (int i = 0; i < dataCount; i++)
            {
                // Check if current field seems to contain data (some date and time bytes are not zeroes)
                if ((historyBytes[i * 12 + 1] != (byte)0) || (historyBytes[i * 12 + 2] != (byte)0) || (historyBytes[i * 12 + 3] != (byte)0) || (historyBytes[i * 12 + 4] != (byte)0))
                {
                    // Allocate memory for new history field
                    HistoryFields[HistoryIndex] = new History()
                    {
                        // Store transaction type 
                        TransactionType = (HistoryTransactionType)((byte)((historyBytes[i * 12 + 0] & 0x80) >> 7))
                    };

                    ushort boardingDate = (ushort)(((historyBytes[i * 12 + 0] & 0x7F) << 7) | ((historyBytes[i * 12 + 1] & 0xFE) >> 1));
                    ushort boardingTime = (ushort)(((historyBytes[i * 12 + 1] & 0x01) << 10) | ((historyBytes[i * 12 + 2] & 0xFF) << 2) | ((historyBytes[i * 12 + 3] & 0xC0) >> 6));                    

                    ushort transferEndDate = (ushort)(((historyBytes[i * 12 + 3] & 0x3F) << 8) | (historyBytes[i * 12 + 4] & 0xFF));
                    ushort transferEndTime = (ushort)(((historyBytes[i * 12 + 5] & 0xFF) << 3) | ((historyBytes[i * 12 + 6] & 0xE0) >> 5));

                    // If transfer end time is before boarding time, the day has changed after boarding 
                    // and we have to subtract one day from the transfer end date to get real boarding date
                    if (transferEndTime < boardingTime)
                    {
                        transferEndDate -= 1;
                    }

                    HistoryFields[HistoryIndex].BoardingDateTime = ConversionUtil.FromEn1545DateAndTime(boardingDate, boardingTime);
                    HistoryFields[HistoryIndex].TransferEndDateTime = ConversionUtil.FromEn1545DateAndTime(transferEndDate, transferEndTime);                    
                    HistoryFields[HistoryIndex].GroupSize = (byte)((historyBytes[i * 12 + 8] & 0x7C) >> 2);                    
                    HistoryFields[HistoryIndex].TicketFare = (ushort)(((historyBytes[i * 12 + 6] & 0x1F) << 9) | ((historyBytes[i * 12 + 7] & 0xFF) << 1) | ((historyBytes[i * 12 + 8] & 0x80) >> 7));
                    HistoryFields[HistoryIndex].RemainingValue = (uint)(((historyBytes[i * 12 + 8] & 0x03) << 18) | (historyBytes[i * 12 + 9] << 9) | ((historyBytes[i * 12 + 10] & 0xC0) >> 6));

                    //increment counter
                    HistoryIndex++;
                }
            }
        }

        public ProductValidity GetProduct1Validity()
        {
            return GetProductValidity(ProductNumber.One);
        }
        
        public ProductValidity GetProduct2Validity()
        {
            return GetProductValidity(ProductNumber.Two);
        }

        private ProductValidity GetProductValidity(ProductNumber product)
        {
            DateTimeOffset startDate = product == ProductNumber.One ? PeriodStartDate1 : PeriodStartDate2;
            DateTimeOffset endDate = product == ProductNumber.One ? PeriodStartDate2 : PeriodEndDate2;
            DateTimeOffset now = DateTimeOffset.UtcNow;

            // If we don't have a validity date for this product, its data on the card is zeroes, and its date is 0, aka the En1545 zero date.
            if (startDate > ConversionUtil.En1545ZeroDate)
            {
                // If start date is valid, but we have no end date
                if (startDate < now && endDate == ConversionUtil.En1545ZeroDate)
                {
                    return ProductValidity.ValidIndefinitelyNow;
                }
                // Standard valid
                else if (startDate < now && endDate > now)
                {
                    return ProductValidity.ValidNow;
                }
                // Start date in the future
                else if (startDate > now && endDate > now)
                {
                    return ProductValidity.ValidNotStarted;
                }
                // Start date in the future, but no end date
                else if (startDate > now && endDate == ConversionUtil.En1545ZeroDate)
                {
                    return ProductValidity.ValidIndefinitelyNotStarted;
                }
                else if (endDate < now)
                {
                    return ProductValidity.Expired;
                }
                // Unknown state, ahhhh
                else
                {
                    return ProductValidity.Unknown;
                }
            }
            // StartDate is zero, so probably no product in this slot
            else
            {
                return ProductValidity.Unknown;
            }
        }
    }
}
