using HslTravelSharp.Helpers;
using System;

namespace HslTravelSharp.Models
{
    public class SingleTicket
    {
        public byte ApplicationVersion { get; private set; }
        public String ApplicationInstanceId { get; private set; }
        public byte PlatformType { get; private set; }
        public RawETicket ValueTicket { get; private set; }

        public SingleTicket(byte[] appInfoBytes, byte[] eTicketBytes)
        {
            ReadApplicationInfo(appInfoBytes);
            ValueTicket = new RawETicket(eTicketBytes, true);
        }

        internal void ReadApplicationInfo(byte[] appInfoBytes)
        {
            ApplicationVersion = (byte)(appInfoBytes[16] & 0xF0);

            byte[] temp = new byte[5];
            Array.Copy(appInfoBytes, 17, temp, 0, 5);            
            ApplicationInstanceId = ConversionUtil.GetHexString(temp);

            uint num = (uint)((appInfoBytes[1] ^ appInfoBytes[5]) & 0x7F);
            num = (uint)((num << 8) + ((appInfoBytes[2] ^ appInfoBytes[6]) & 0xFF));
            num = (uint)((num << 8) + ((appInfoBytes[4] ^ appInfoBytes[7]) & 0xFF));

            ApplicationInstanceId = $"{ApplicationInstanceId}{num.ToString("D7")}{appInfoBytes[22] & 0xF0 >> 4}";
            PlatformType = (byte)(appInfoBytes[22] & 0x0E);
        }
    }
}
