namespace TaxiDC2.Models.Enums
{
    public enum TripState
    {
        NewOrder = 0,
        AcceptedDiver = 1,
        Running = 2,
        SMS1sended = 3,
        SMS2sended = 4,
        Call = 5,
        ForwardToDiver = 70,
        RejectedByDiver = 80,
        Comleted = 100,
        NewWWW = 201,
        Canceled = 999
    }
}
