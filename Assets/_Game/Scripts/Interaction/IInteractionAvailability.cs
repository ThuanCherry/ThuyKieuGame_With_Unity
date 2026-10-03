namespace ThuyKieu.Interaction
{
    /// <summary>Opt-in range detection and availability for interactions with progression requirements.</summary>
    public interface IInteractionAvailability
    {
        bool IsAvailable { get; }
    }
}
