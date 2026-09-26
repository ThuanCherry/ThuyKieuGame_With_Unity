namespace ThuyKieu.Interaction
{
    /// <summary>
    /// Anything the player can interact with: NPC, item, door, quest object...
    /// Kept deliberately minimal so every system can implement it on its own terms.
    /// </summary>
    public interface IInteractable
    {
        void Interact();
    }
}
