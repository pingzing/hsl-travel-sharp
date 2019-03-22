using HslTravelSharp.Core.Commands;
using System.Linq;

namespace HslTravelSharp.Core.Helpers
{
    public static class ByteArrayExtensions
    {
        public static bool HasMore(this byte[] response)
        {
            return response != null
                && response.Length >= 2
                && response.Skip(response.Length - 2).ToArray().SequenceEqual(HslCommands.MoreData);
        }
    }
}
