using System.IO;
using UnityEngine;

namespace ThuyKieu.Core
{
    public class InkStatePersistence : MonoBehaviour
    {
        public static InkStatePersistence Instance { get; private set; }
        public string StateJson { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        private void OnDestroy() { if (Instance == this) Instance = null; }
        public void Save(Ink.Runtime.Story story)
        {
            StateJson = story.state.ToJson();
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "Chapter1.inkstate.json"), StateJson);
        }
    }
}
