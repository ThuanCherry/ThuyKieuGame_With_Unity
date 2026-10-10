using System;
using System.Linq;
using ThuyKieu.Player;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThuyKieu.Environment.Editor
{
    public static class KieuBedroomSetup
    {
        [MenuItem("ThuyKieu/Chapter 1/Polish Kieu bedroom")]
        public static void Apply()
        {
            var scene = SceneManager.GetActiveScene();
            if (Application.isPlaying || scene.name != "Chapter01_GiaBien")
                throw new InvalidOperationException("Open Chapter01_GiaBien in Edit Mode.");
            var house = GameObject.Find("VuongGia/MainHouse").transform;
            var room = house.Find("KieuRoom");
            if (room == null)
            {
                room = house.Find("SideArea");
                if (room == null) room = new GameObject("KieuRoom").transform;
                room.name = "KieuRoom";
                room.SetParent(house, true);
            }
            var wood = AssetDatabase.LoadAssetAtPath<Material>("Assets/_Game/Materials/Chapter01Environment/DarkWood.mat");
            var plaster = AssetDatabase.LoadAssetAtPath<Material>("Assets/_Game/Materials/Chapter01Environment/LimePlaster.mat");
            var screen = room.Find("DecorativeScreen");
            if (screen != null) screen.gameObject.SetActive(false);
            // The existing shell supplies the west/back walls, ceiling and continuous floor.
            Block(room, "EastPartition", new Vector3(-2.1f, 1.6f, 4.4f), new Vector3(.16f, 3.2f, 3.8f), plaster);
            Block(room, "FrontWallLeft", new Vector3(-5.1f, 1.6f, 2.5f), new Vector3(1.6f, 3.2f, .16f), plaster);
            Block(room, "FrontWallRight", new Vector3(-2.6f, 1.6f, 2.5f), new Vector3(1f, 3.2f, .16f), plaster);
            Block(room, "DoorLintel", new Vector3(-3.7f, 2.9f, 2.5f), new Vector3(1.2f, .6f, .18f), wood);
            Block(room, "Door", new Vector3(-4.25f, 1.2f, 2.95f), new Vector3(.08f, 2.4f, .85f), wood);
            var bed = room.Find("Bed");
            if (bed != null) bed.position = new Vector3(-4.9f, 0, 4.95f);
            Block(room, "SmallTable", new Vector3(-2.75f, .64f, 5.65f), new Vector3(.7f, .08f, .6f), wood);
            foreach (int x in new[] {-1, 1}) foreach (int z in new[] {-1, 1})
                Block(room, "TableLeg" + x + z, new Vector3(-2.75f + x*.27f, .3f, 5.65f + z*.22f), new Vector3(.065f, .6f, .065f), wood);
            Block(room, "Stool", new Vector3(-2.8f, .22f, 4.8f), new Vector3(.42f, .44f, .42f), wood);
            Block(room, "Chest", new Vector3(-5.25f, .28f, 3.15f), new Vector3(.75f, .56f, .5f), wood);
            var lamp = room.Find("OilLamp");
            if (lamp == null)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Game/Prefabs/Props/OilLamp.prefab");
                lamp = ((GameObject)PrefabUtility.InstantiatePrefab(prefab, room)).transform;
                lamp.name = "OilLamp";
            }
            lamp.position = new Vector3(-2.75f, .68f, 5.65f);
            foreach (var collider in lamp.GetComponentsInChildren<Collider>()) collider.enabled = false;
            var glow = room.Find("WarmLamp");
            if (glow == null) { glow = new GameObject("WarmLamp").transform; glow.SetParent(room); }
            glow.position = new Vector3(-2.9f, 1.65f, 5.2f);
            var light = glow.GetComponent<Light>();
            if (light == null) light = glow.gameObject.AddComponent<Light>();
            light.type = LightType.Point; light.color = new Color(1, .66f, .32f);
            light.intensity = 1.15f; light.range = 4; light.shadows = LightShadows.None;
            var decoration = room.Find("DecorativeProps");
            if (decoration == null) { decoration = new GameObject("DecorativeProps").transform; decoration.SetParent(room); }
            Block(decoration, "FoldedCloth", new Vector3(-5.25f, .575f, 3.15f), new Vector3(.45f, .025f, .28f), plaster, false);
            var start = room.Find("PlayerStart");
            if (start == null) { start = new GameObject("PlayerStart").transform; start.SetParent(room); }
            start.SetPositionAndRotation(new Vector3(-3.7f, .05f, 4.5f), Quaternion.Euler(0, 180, 0));
            var player = UnityEngine.Object.FindAnyObjectByType<PlayerMovement>();
            player.transform.SetPositionAndRotation(start.position, start.rotation);
            var spawn = GameObject.Find("PlayerSpawn");
            if (spawn != null) spawn.transform.SetPositionAndRotation(start.position, start.rotation);
            var hallway = house.Find("Hallway");
            if (hallway == null) { hallway = new GameObject("Hallway").transform; hallway.SetParent(house); }
            hallway.position = new Vector3(-3.7f, 0, 1.8f);
            var camera = Camera.main;
            camera.transform.SetPositionAndRotation(new Vector3(-3.7f, 2.1f, 5.8f), Quaternion.Euler(18, 180, 0));
            Physics.SyncTransforms();
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
        }

        private static void Block(Transform parent, string name, Vector3 position, Vector3 size, Material material, bool solid = true)
        {
            var existing = parent.Find(name);
            var go = existing != null ? existing.gameObject : GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name; go.transform.SetParent(parent, true); go.layer = 8;
            go.transform.position = position; go.transform.localScale = size;
            go.GetComponent<Renderer>().sharedMaterial = material;
            go.GetComponent<Collider>().enabled = solid;
        }
    }
}
