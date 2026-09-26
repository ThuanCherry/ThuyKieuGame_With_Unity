using System;
using UnityEngine;

namespace ThuyKieu.Quest
{
    /// <summary>
    /// One step of a quest. This is pure authored data: runtime completion is tracked
    /// per play session by QuestInstance, so the asset is never mutated at runtime.
    /// </summary>
    [Serializable]
    public class QuestObjective
    {
        [SerializeField] private string description = "New objective";
        [SerializeField] private ObjectiveType type = ObjectiveType.TalkToNPC;

        [Tooltip("Id of the NPC or object that satisfies this objective. Must match NPCInteractable.NpcId or QuestObjectInteractable.ObjectId.")]
        [SerializeField] private string targetId = string.Empty;

        public string Description => description;
        public ObjectiveType Type => type;
        public string TargetId => targetId;

        public bool Matches(ObjectiveType reportedType, string reportedId)
        {
            return reportedType == type && string.Equals(reportedId, targetId, StringComparison.OrdinalIgnoreCase);
        }
    }
}
