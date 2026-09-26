namespace ThuyKieu.Interaction
{
    /// <summary>
    /// Optional add-on for an <see cref="IInteractable"/> that wants to show a prompt
    /// ("Talk to Kim Trong", "Open door"). The UI team can read this without the
    /// interaction system knowing anything about UI.
    /// </summary>
    public interface IInteractableDisplayName
    {
        string DisplayName { get; }
    }
}
