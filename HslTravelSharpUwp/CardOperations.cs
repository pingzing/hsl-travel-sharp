using HslTravelSharp.Models;
using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.SmartCards;

namespace HslTravelSharpUwp
{
    public class CardOperations
    {
        // Commands
        private static readonly byte[] _selectHslCommand = { 0x90, 0x5A, 0x00, 0x00, 0x03, 0x11, 0x20, 0xEF, 0x00 };        
        private static readonly byte[] _readAppInfoCommand = { 0x90, 0xBD, 0x00, 0x00, 0x07, 0x08, 0x00, 0x00, 0x00, 0x0B, 0x00, 0x00, 0x00 };        
        private static readonly byte[] _readPeriodPassCommand = { 0x90, 0xBD, 0x00, 0x00, 0x07, 0x01, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00 };        
        private static readonly byte[] _readStoredValueCommand = { 0x90, 0xBD, 0x00, 0x00, 0x07, 0x02, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00 };        
        private static readonly byte[] _readETicketCommand = { 0x90, 0xBD, 0x00, 0x00, 0x07, 0x03, 0x00, 0x00, 0x00, 0x1A, 0x00, 0x00, 0x00 };        
        private static readonly byte[] _readHistoryCommand = { 0x90, 0xBB, 0x00, 0x00, 0x07, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };        
        private static readonly byte[] _readNextCommand = { 0x90, 0xAF, 0x00, 0x00, 0x00 };
        
        // Responses
        private static readonly byte[] _okResponse = { 0x91, 0x00 };        
        private static readonly byte[] _moreData = { 0x91, 0xAF };

        /// <summary>
        /// Reads the travel card data from HSL Mifare DESFire card.
        /// </summary>
        /// <param name="card">The card to try to read.</param>
        /// <returns>A deserialized Travel Card object if the card was valid, otherwise null.</returns>
        public static async Task<TravelCard> ReadTravelCardAsync(SmartCard card)
        {
            using (SmartCardConnection connection = await card.ConnectAsync())
            {
                byte[] selection = (await connection.TransmitAsync(_selectHslCommand.AsBuffer())).ToArray();
                if (selection != null 
                    && selection.Length > 0 
                    && selection.SequenceEqual(_okResponse))
                {
                    // Travel card info bytes
                    byte[] appInfo = null;
                    byte[] periodPass = null;
                    byte[] storedValue = null;
                    byte[] eTicket = null;
                    byte[] history = null;

                    // Temporary containers for history chunks
                    byte[] hist1 = new byte[2];
                    byte[] hist2 = new byte[2];

                    appInfo = (await connection.TransmitAsync(_readAppInfoCommand.AsBuffer())).ToArray();
                    periodPass = (await connection.TransmitAsync(_readPeriodPassCommand.AsBuffer())).ToArray();
                    storedValue = (await connection.TransmitAsync(_readStoredValueCommand.AsBuffer())).ToArray();
                    eTicket = (await connection.TransmitAsync(_readETicketCommand.AsBuffer())).ToArray();
                    hist1 = (await connection.TransmitAsync(_readHistoryCommand.AsBuffer())).ToArray();

                    // If we have more history, the last two bytes of the history array will contain the MORE_DATA bytes.
                    if (hist1.Skip(Math.Max(0, hist1.Length - 2)).ToArray() == _moreData)
                    {
                        hist2 = (await connection.TransmitAsync(_readNextCommand.AsBuffer())).ToArray();
                    }

                    // Combine the two history chunks into a single array, minus their last two MORE_DATA bytes
                    history = hist1.Take(hist1.Length - 2)
                                     .Concat(hist2.Take(hist2.Length - 2)).ToArray();

                    var rawCard = new RawTravelCard(appInfo, periodPass, storedValue, eTicket, history);
                    return new TravelCard(rawCard);
                }
                else
                {
                    return null;
                }
            }            
        }

        // TODO: we need to use the PCSC SDK at https://github.com/Microsoft/Windows-universal-samples/tree/master/Samples/Nfc for this, for talking to a MiFare Ultralight. Ain't no way I'm doing that by hand

        /// <summary>
        /// Read the single ticket card data from an HSL single ticket card (a Mifare 
        /// </summary>
        /// <param name="card">The card to try to read.</param>
        /// <returns>A deserialized Travel Card object if the card was valid, otherwise null.</returns>
        //public static async Task<SingleTicket> ReadSingleTicketAsync(SmartCardReader reader)
        //{
        //    // Array for all card data
        //    byte[] cardBytes = new byte[64];
        //    byte[] pages;
        //    //Arrays for application information and eTicket data
        //    byte[] appInfoBytes = new byte[23];
        //    byte[] eTicketData = new byte[41];
        //}        
    }
}
