using Android.Nfc.Tech;
using HslTravelSharp.Core.Commands;
using HslTravelSharp.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HslTravelSharp.Android
{
    /// <summary>
    /// Basic operations for interacting with an HSL Travel Card.
    /// </summary>
    public class CardOperations
    {
        /// <summary>
        /// Reads the travel card data from HSL Mifare DESFire card.
        /// </summary>
        /// <param name="card">The card to try to read.</param>
        /// <returns>A deserialized Travel Card object if the card was valid, otherwise null.</returns>
        public static async Task<TravelCard> ReadTravelCardAsync(IsoDep card)
        {            
            try
            {
                card.Connect();
                byte[] selection = card.Transceive(HslCommands.SelectHslCommand);

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

                    appInfo = await card.TransceiveAsync(HslCommands.ReadAppInfoCommand);
                    periodPass = await card.TransceiveAsync(HslCommands.ReadPeriodPassCommand);
                    storedValue = await card.TransceiveAsync(HslCommands.ReadStoredValueCommand);
                    eTicket = await card.TransceiveAsync(HslCommands.ReadETicketCommand);
                    hist1 = await card.TransceiveAsync(HslCommands.ReadHistoryCommand);

                    // If we have more history, the last two bytes of the history array will contain the MORE_DATA bytes.
                    if (hist1.Skip(Math.Max(0, hist1.Length - 2)).ToArray() == HslCommands.MoreData)
                    {
                        hist2 = await card.TransceiveAsync(HslCommands.ReadNextCommand);
                    }

                    // Combine the two history chunks into a single array, minus their last two MORE_DATA bytes
                    history = hist1.Take(hist1.Length - 2)
                                     .Concat(hist2.Take(hist2.Length - 2)).ToArray();

                    var rawCard = new RawTravelCard(appInfo, periodPass, storedValue, eTicket, history);
                    if (rawCard.Status == CardStatus.HslCardDataFailure)
                    {
                        System.Diagnostics.Debug.WriteLine("Failed to construct travel card. Data appears to be invalid.");
                        return null;
                    }
                    else
                    {
                        return new TravelCard(rawCard);
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Failed to construct travel card. This doesn't seem to be an HSL Travel card.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Failed to construct travel card. Reason: " + ex.ToString() + ".\nStack trace:" + ex.StackTrace);
                return null;
            }
            finally
            {
                card.Close();                
            }

        }
    }
}
