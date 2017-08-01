namespace HslTravelSharp.Models
{
    public enum CardStatus : uint
    {
        Ok = 0,
        NoHslCard = 1,
        HslCardDataFailure = 2,
        CardReadFailure = 3,
        HslCardnumberFailure = 4
    }
}
