using HslTravelSharp.Core.Helpers;
using System;

namespace HslTravelSharp.Core.Models
{
    /// <summary>
    /// Represents a single ticket data that is used both in HSL single tickets and in HSL travel card's value tickets.
    /// </summary>
    public class RawETicket
    {
        public ushort ProductCode { get; private set; }
        public byte Child { get; private set; }
        public byte LanguageCode { get; private set; }
        public byte ValidityLengthType { get; private set; }
        public byte ValidityLength { get; private set; }
        public byte ValidityAreaType { get; private set; }
        public byte ValidityArea { get; private set; }
        public DateTimeOffset SaleDate { get; private set; }
        public byte SaleTime { get; private set; }
        public byte GroupSize { get; private set; }
        public byte SaleStatus { get; private set; }
        public DateTimeOffset ValidityStartDate { get; private set; }
        public DateTimeOffset ValidityEndDate { get; private set; }
        public byte ValidityStatus { get; private set; }

        //Last boarding info
        public DateTimeOffset BoardingDate { get; private set; }
        public ushort BoardingVehicle { get; private set; }
        public byte BoardingLocationNumType { get; private set; }
        public ushort BoardingLocationNum { get; private set; }
        public byte BoardingDirection { get; private set; }
        public byte BoardingArea { get; private set; }

        public RawETicket(byte[] eTicketData, Boolean isSingleTicket)
        {            
            ProductCode = (ushort)((ushort)((eTicketData[0] & 0xFF) << 6) | ((eTicketData[1] & 0xFC) >> 2));
            Child = (byte)((eTicketData[1] & 0x02) >> 1);
            LanguageCode = (byte)(((eTicketData[1] & 0x01) << 1) | ((eTicketData[2] & 0x80) >> 7));
            ValidityLengthType = (byte)((eTicketData[2] & 0x60) >> 5);
            ValidityLength = (byte)(((eTicketData[2] & 0x1F) << 3) | ((eTicketData[3] & 0xE0) >> 5));
            ValidityAreaType = (byte)((eTicketData[3] & 0x10) >> 4);
            ValidityArea = (byte)(eTicketData[3] & 0x0F);

            ushort date1 = (ushort)(((eTicketData[4] & 0xFF) << 6) | ((eTicketData[5] & 0xFC) >> 2));
            SaleDate = ConversionUtil.FromEn1545Date(date1).UtcDateTime;
            SaleTime = (byte)(((eTicketData[5] & 0x03) << 3) | ((eTicketData[6] & 0xE0) >> 5));
            GroupSize = (byte)((eTicketData[10] & 0x3E) >> 1);
            //sale status is relevant only in value tickets on desfire cards
            SaleStatus = (byte)(eTicketData[10] & 0x01);

            int count = 0;

            //if we're reading separate eTicket (single ticket) skip over some data
            if (isSingleTicket)
            {
                count = 6;
            }

            //read validity start datestamp
            date1 = (ushort)(((eTicketData[11 + count] & 0xFF) << 6) | ((eTicketData[12 + count] & 0xFC) >> 2));
            //read validity start timestamp
            ushort time1 = (ushort)(((eTicketData[12 + count] & 0x03) << 9) | ((eTicketData[13 + count] & 0xFF) << 1) | ((eTicketData[14 + count] & 0x80) >> 7));
            ValidityStartDate = ConversionUtil.FromEn1545DateAndTime(date1, time1);

            //read validity end datestamp
            date1 = (ushort)(((eTicketData[14 + count] & 0x7F) << 7) | ((eTicketData[15 + count] & 0xFF) >> 1));
            //read validity end timestamp
            time1 = (ushort)(((eTicketData[15 + count] & 0x01) << 10) | ((eTicketData[16 + count] & 0xFF) << 2) | ((eTicketData[17 + count] & 0xC0) >> 6));
            ValidityEndDate = ConversionUtil.FromEn1545DateAndTime(date1, time1);

            //validity status is relevant only in value tickets on desfire cards
            ValidityStatus = (byte)(eTicketData[17 + count] & 0x01);

            //LAST USE (BOARDING)

            date1 = (ushort)(((eTicketData[18 + count] & 0xFF) << 6) | ((eTicketData[19 + count] & 0xFC) >> 2));
            time1 = (ushort)(((eTicketData[19 + count] & 0x03) << 9) | ((eTicketData[20 + count] & 0xFF) << 1) | ((eTicketData[21 + count] & 0x80) >> 7));
            BoardingDate = ConversionUtil.FromEn1545DateAndTime(date1, time1);
            BoardingVehicle = (ushort)(((eTicketData[21 + count] & 0x7F) << 7) | (eTicketData[22 + count] & 0xFE) >> 1);
            BoardingLocationNumType = (byte)(((eTicketData[22 + count] & 0x01) << 1) | ((eTicketData[23 + count] & 0x80) >> 7));
            BoardingLocationNum = (ushort)(((eTicketData[23 + count] & 0x7F) << 7) | ((eTicketData[24 + count] & 0xFE) >> 1));
            BoardingDirection = (byte)(eTicketData[24 + count] & 0x01);
            BoardingArea = (byte)((eTicketData[25 + count] & 0xF0) >> 4);
        }
    }
}
