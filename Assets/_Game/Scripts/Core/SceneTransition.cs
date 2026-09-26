using UnityEngine.SceneManagement;

namespace ThuyKieu.Core
{
    /// <summary>
    /// Remembers which spawn point the player should appear at after a scene load.
    /// Static on purpose: no manager object has to survive the load.
    /// </summary>
    public static class SceneTransition
    {
        /// <summary>Spawn id requested by the last travel call, or null.</summary>
        public static string PendingSpawnId { get; private set; }

        /// <summary>Name of the scene we came from, so a door can send the player back.</summary>
        public static string PreviousScene { get; private set; }

        public static void Travel(string sceneName, string spawnId)
        {
            PendingSpawnId = spawnId;
            PreviousScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>Reads and clears the pending spawn id.</summary>
        public static string ConsumeSpawnId()
        {
            string id = PendingSpawnId;
            PendingSpawnId = null;
            return id;
        }
    }
}
