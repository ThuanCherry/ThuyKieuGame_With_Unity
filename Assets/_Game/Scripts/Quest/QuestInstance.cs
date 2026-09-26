using System.Collections.Generic;

namespace ThuyKieu.Quest
{
    /// <summary>
    /// Runtime state of one quest. Keeps per-objective progress outside the
    /// ScriptableObject so quest assets stay immutable between play sessions.
    /// </summary>
    public class QuestInstance
    {
        private readonly bool[] completedObjectives;

        public QuestInstance(QuestData data)
        {
            Data = data;
            Status = QuestStatus.Active;
            completedObjectives = new bool[data.Objectives.Count];
        }

        public QuestData Data { get; }
        public QuestStatus Status { get; private set; }

        public bool IsObjectiveCompleted(int index)
        {
            return index >= 0 && index < completedObjectives.Length && completedObjectives[index];
        }

        public IEnumerable<string> GetObjectiveSummaries()
        {
            for (int i = 0; i < Data.Objectives.Count; i++)
            {
                yield return (completedObjectives[i] ? "[x] " : "[ ] ") + Data.Objectives[i].Description;
            }
        }

        /// <summary>Marks the first matching, still open objective. Returns its index, or -1.</summary>
        public int TryCompleteObjective(ObjectiveType type, string targetId)
        {
            if (Status != QuestStatus.Active)
            {
                return -1;
            }

            for (int i = 0; i < Data.Objectives.Count; i++)
            {
                if (completedObjectives[i] || !Data.Objectives[i].Matches(type, targetId))
                {
                    continue;
                }

                completedObjectives[i] = true;
                return i;
            }

            return -1;
        }

        public bool AllObjectivesCompleted()
        {
            for (int i = 0; i < completedObjectives.Length; i++)
            {
                if (!completedObjectives[i])
                {
                    return false;
                }
            }

            return true;
        }

        public void MarkCompleted()
        {
            Status = QuestStatus.Completed;
        }
    }
}
