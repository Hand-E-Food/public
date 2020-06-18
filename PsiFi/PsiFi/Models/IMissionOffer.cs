namespace PsiFi.Models
{
    interface IMissionOffer
    {
        string Name { get; }

        IMission CreateMission();
    }
}
