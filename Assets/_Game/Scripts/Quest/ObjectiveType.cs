namespace ThuyKieu.Quest
{
    /// <summary>
    /// Kinds of objective the quest system understands.
    /// Add new members here and handle them in QuestManager.ReportProgress.
    /// Inventory / combat objectives are intentionally NOT part of this milestone.
    /// </summary>
    public enum ObjectiveType
    {
        TalkToNPC = 0,
        InteractWithObject = 1
    }
}
