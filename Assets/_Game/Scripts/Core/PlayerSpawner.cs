using System;
using UnityEngine;

namespace ThuyKieu.Core
{
    /// <summary>
    /// Places the player on the spawn point requested by the previous scene.
    /// Sits in the scene, touches nothing inside the Player prefab.
    /// </summary>
    public class PlayerSpawner : MonoBehaviour
    {
        [Serializable]
        public class SpawnEntry
        {
            [Tooltip("Id a door passes to SceneTransition.Travel.")]
            public string id = "Default";
            public Transform point;
        }

        [SerializeField] private SpawnEntry[] spawnPoints = new SpawnEntry[0];

        [Tooltip("Used when no spawn id was requested, for example when the scene is opened directly.")]
        [SerializeField] private Transform defaultSpawn;

        [SerializeField] private string playerTag = "Player";

        private void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag(playerTag);
            if (player == null)
            {
                Debug.LogWarning("[PlayerSpawner] No object tagged " + playerTag + " in this scene.", this);
                return;
            }

            Transform target = ResolveSpawn(SceneTransition.ConsumeSpawnId());
            if (target == null)
            {
                return;
            }

            Place(player, target);
        }

        private Transform ResolveSpawn(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    if (spawnPoints[i] != null && spawnPoints[i].point != null
                        && string.Equals(spawnPoints[i].id, id, StringComparison.OrdinalIgnoreCase))
                    {
                        return spawnPoints[i].point;
                    }
                }

                Debug.LogWarning("[PlayerSpawner] Unknown spawn id: " + id, this);
            }

            return defaultSpawn;
        }

        /// <summary>CharacterController overrides transform writes, so disable it while teleporting.</summary>
        private void Place(GameObject player, Transform target)
        {
            CharacterController controller = player.GetComponent<CharacterController>();
            bool hadController = controller != null && controller.enabled;

            if (hadController)
            {
                controller.enabled = false;
            }

            player.transform.SetPositionAndRotation(target.position, target.rotation);

            if (hadController)
            {
                controller.enabled = true;
            }
        }
    }
}
