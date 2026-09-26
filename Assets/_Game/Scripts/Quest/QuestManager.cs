using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThuyKieu.Quest
{
    /// <summary>
    /// Owns quest runtime state. Other systems only talk to it through
    /// StartQuest / ReportProgress / GetStatus, never by touching QuestInstance directly.
    /// </summary>
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; private set; }

        [Tooltip("Logs quest transitions to the Console. Handy while the quest UI does not exist yet.")]
        [SerializeField] private bool logTransitions = true;

        private readonly Dictionary<QuestData, QuestInstance> quests = new Dictionary<QuestData, QuestInstance>();

        /// <summary>Raised when a quest moves to Active.</summary>
        public event Action<QuestData> QuestStarted;

        /// <summary>Raised when an objective is ticked off. Int is the objective index.</summary>
        public event Action<QuestData, int> ObjectiveCompleted;

        /// <summary>Raised when every objective of a quest is done.</summary>
        public event Action<QuestData> QuestCompleted;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public QuestStatus GetStatus(QuestData quest)
        {
            if (quest == null)
            {
                return QuestStatus.NotStarted;
            }

            QuestInstance instance;
            return quests.TryGetValue(quest, out instance) ? instance.Status : QuestStatus.NotStarted;
        }

        public bool IsActive(QuestData quest)
        {
            return GetStatus(quest) == QuestStatus.Active;
        }

        public bool IsCompleted(QuestData quest)
        {
            return GetStatus(quest) == QuestStatus.Completed;
        }

        public QuestInstance GetInstance(QuestData quest)
        {
            QuestInstance instance;
            return quest != null && quests.TryGetValue(quest, out instance) ? instance : null;
        }

        /// <summary>Starts a quest if it has never been started. Returns true when it actually started.</summary>
        public bool StartQuest(QuestData quest)
        {
            if (quest == null || quests.ContainsKey(quest))
            {
                return false;
            }

            var instance = new QuestInstance(quest);
            quests.Add(quest, instance);

            if (logTransitions)
            {
                Debug.Log("[Quest] Started: " + quest.Title, this);
            }

            if (QuestStarted != null)
            {
                QuestStarted(quest);
            }

            // A quest with no objectives would otherwise stay Active forever.
            EvaluateCompletion(instance);
            return true;
        }

        /// <summary>
        /// Entry point for the rest of the game: an NPC was talked to, an object was used...
        /// Every active quest gets a chance to tick off a matching objective.
        /// </summary>
        public void ReportProgress(ObjectiveType type, string targetId)
        {
            if (string.IsNullOrEmpty(targetId))
            {
                return;
            }

            // ToArray-style copy: completing a quest may start another one via listeners.
            var active = new List<QuestInstance>(quests.Values);
            foreach (var instance in active)
            {
                int index = instance.TryCompleteObjective(type, targetId);
                if (index < 0)
                {
                    continue;
                }

                if (logTransitions)
                {
                    Debug.Log("[Quest] Objective done: " + instance.Data.Objectives[index].Description, this);
                }

                if (ObjectiveCompleted != null)
                {
                    ObjectiveCompleted(instance.Data, index);
                }

                EvaluateCompletion(instance);
            }
        }

        private void EvaluateCompletion(QuestInstance instance)
        {
            if (instance.Status != QuestStatus.Active || !instance.AllObjectivesCompleted())
            {
                return;
            }

            instance.MarkCompleted();

            if (logTransitions)
            {
                Debug.Log("[Quest] Completed: " + instance.Data.Title, this);
            }

            if (QuestCompleted != null)
            {
                QuestCompleted(instance.Data);
            }
        }
    }
}
