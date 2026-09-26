using System.Collections.Generic;
using UnityEngine;

namespace ThuyKieu.Quest
{
    /// <summary>
    /// Authored quest definition. Designers create these assets, code never edits them.
    /// </summary>
    [CreateAssetMenu(fileName = "Quest_", menuName = "ThuyKieu/Quest", order = 10)]
    public class QuestData : ScriptableObject
    {
        [Tooltip("Stable id used for saving and for cross-references. Keep unique.")]
        [SerializeField] private string questId = "quest_id";
        [SerializeField] private string title = "New Quest";
        [SerializeField, TextArea(2, 4)] private string description = string.Empty;
        [SerializeField] private List<QuestObjective> objectives = new List<QuestObjective>();

        public string QuestId => questId;
        public string Title => title;
        public string Description => description;
        public IReadOnlyList<QuestObjective> Objectives => objectives;
    }
}
