using HslTravelSharp.Core.Commands;
using HslTravelSharp.Core.Models;
using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.SmartCards;

namespace HslTravelSharpUwp
{
    public class CardOperations
    {        
        /// <summary>
        /// Reads the travel card data from HSL Mifare DESFire card.
        /// </summary>
        /// <param name="card">The card to try to read.</param>
        /// <returns>A deserialized Travel Card object if the card was valid, otherwise null.</returns>
        public static async Task<TravelCard> ReadTravelCardAsync(SmartCard card)
        {
            using (SmartCardConnection connection = await card.ConnectAsync())
            {
                byte[] selection = (await connection.TransmitAsync(HslCommands.SelectHslCommand.AsBuffer())).ToArray();
                if (selection != null 
                    && selection.Length > 0 
                    && selection.SequenceEqual(HslCommands.OkResponse))
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

                    appInfo = (await connection.TransmitAsync(HslCommands.ReadAppInfoCommand.AsBuffer())).ToArray();
                    periodPass = (await connection.TransmitAsync(HslCommands.ReadPeriodPassCommand.AsBuffer())).ToArray();
                    storedValue = (await connection.TransmitAsync(HslCommands.ReadStoredValueCommand.AsBuffer())).ToArray();
                    eTicket = (await connection.TransmitAsync(HslCommands.ReadETicketCommand.AsBuffer())).ToArray();
                    hist1 = (await connection.TransmitAsync(HslCommands.ReadHistoryCommand.AsBuffer())).ToArray();

                    // If we have more history, the last two bytes of the history array will contain the MORE_DATA bytes.
                    if (hist1.Skip(Math.Max(0, hist1.Length - 2)).ToArray() == HslCommands.MoreData)
                    {
                        hist2 = (await connection.TransmitAsync(HslCommands.ReadNextCommand.AsBuffer())).ToArray();
                    }

                    // Combine the two history chunks into a single array, minus their last two MORE_DATA bytes
                    history = hist1.Take(hist1.Length - 2)
                                     .Concat(hist2.Take(hist2.Length - 2)).ToArray();

                    var rawCard = new RawTravelCard(appInfo, periodPass, storedValue, eTicket, history);
                    if (rawCard.Status == CardStatus.HslCardDataFailure)
                    {
                        return null;
                    }
                    else
                    {
                        return new TravelCard(rawCard);
                    }
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
