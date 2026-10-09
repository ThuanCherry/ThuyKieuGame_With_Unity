using System;
using System.Linq;
using ThuyKieu.Dialogue;
using ThuyKieu.Environment;
using ThuyKieu.Interaction;
using ThuyKieu.Player;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ThuyKieu.Core.Editor
{
    public static class Chapter01OpeningPolishSetup
    {
        [MenuItem("ThuyKieu/Chapter 1/Apply opening polish")]
        public static void Apply()
        {
            var scene = SceneManager.GetActiveScene();
            if (Application.isPlaying || scene.path != Chapter01Setup.ScenePath)
                throw new InvalidOperationException("Open Chapter01_GiaBien in Edit Mode.");

            var director = UnityEngine.Object.FindAnyObjectByType<Chapter01Director>();
            var player = UnityEngine.Object.FindAnyObjectByType<PlayerMovement>();
            var playerAnimator = player.GetComponentInChildren<Animator>();
            var house = GameObject.Find("VuongGia/MainHouse").transform;
            var room = house.Find("KieuRoom");
            var bed = room.Find("Bed");
            var mother = GameObject.Find("MeKieu").transform;
            var audio = UnityEngine.Object.FindAnyObjectByType<Chapter01Audio>();

            PositionKieuAtBed(player, bed);
            SetSitting(playerAnimator, true);
            CreateRoomDoor(room, player.transform, director, audio);
            PositionMotherOnExistingSeat(house.Find("MainHall"), mother);
            CreateDisturbance(house.Find("MainHall"));
            var motherAnimator = mother.GetComponentInChildren<Animator>();
            motherAnimator.runtimeAnimatorController = playerAnimator.runtimeAnimatorController;
            SetSitting(motherAnimator, true);
            ConfigureStandButton(director);
            ConfigureCryingAudio(audio, mother);
            ConfigureThunderAudio(audio);
            ConfigureOpeningCamera(director, player.transform, house);
            Set(director, "_playerAnimator", playerAnimator);
            Set(director, "_audio", audio);

            Physics.SyncTransforms();
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Chapter01PresentationFix.Apply();
        }

        private static void PositionKieuAtBed(PlayerMovement player, Transform bed)
        {
            var bounds = bed.GetComponentsInChildren<Renderer>(true).Aggregate(new Bounds(bed.position, Vector3.zero), (current, renderer) =>
            {
                current.Encapsulate(renderer.bounds);
                return current;
            });
            var marker = FindOrCreate(bed.parent, "KieuSittingStart");
            marker.SetPositionAndRotation(new Vector3(bounds.max.x - .10f, .05f, bounds.center.z - .35f), Quaternion.Euler(0, 90, 0));
            var controller = player.GetComponent<CharacterController>();
            controller.enabled = false;
            player.transform.SetPositionAndRotation(marker.position, marker.rotation);
            controller.enabled = true;
            var spawn = GameObject.Find("PlayerSpawn");
            if (spawn != null) spawn.transform.SetPositionAndRotation(marker.position, marker.rotation);
            var standing = FindOrCreate(bed.parent, "KieuStandingAfterSeat");
            standing.SetPositionAndRotation(new Vector3(bounds.max.x + .40f, .05f, bounds.center.z - .35f), Quaternion.Euler(0, 90, 0));
            var director = UnityEngine.Object.FindAnyObjectByType<Chapter01Director>();
            Set(director, "_standingAfterSeat", standing);
        }

        private static void CreateRoomDoor(Transform room, Transform player, Chapter01Director director, Chapter01Audio audio)
        {
            var oldDoor = room.Find("Door");
            if (oldDoor != null) oldDoor.gameObject.SetActive(false);
            var root = FindOrCreate(room, "KieuRoomDoor");
            root.SetPositionAndRotation(new Vector3(-4.27f, 0, 2.5f), Quaternion.identity);
            var leaf = FindOrCreate(root, "Leaf");
            leaf.localPosition = Vector3.zero;
            leaf.localRotation = Quaternion.identity;
            var panel = FindOrCreate(leaf, "Panel");
            panel.localPosition = new Vector3(.56f, 1.2f, 0);
            panel.localRotation = Quaternion.identity;
            panel.localScale = new Vector3(1.12f, 2.4f, .09f);
            panel.gameObject.layer = 8;
            var renderer = panel.GetComponent<MeshRenderer>();
            if (renderer == null)
            {
                var primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
                primitive.name = "Panel";
                primitive.transform.SetParent(leaf, false);
                primitive.transform.localPosition = panel.localPosition;
                primitive.transform.localScale = panel.localScale;
                UnityEngine.Object.DestroyImmediate(panel.gameObject);
                panel = primitive.transform;
                panel.gameObject.layer = 8;
            }
            var material = AssetDatabase.LoadAssetAtPath<Material>("Assets/_Game/Materials/Chapter01Environment/DarkWood.mat");
            panel.GetComponent<Renderer>().sharedMaterial = material;
            var collider = panel.GetComponent<BoxCollider>();
            collider.enabled = true;
            var door = Get<ProximityDoor>(root.gameObject);
            Set(door, "_player", player);
            Set(door, "_left", leaf);
            Set(door, "_right", null);
            Set(door, "_audio", audio);
            Set(door, "_storyControlled", true);
            Set(door, "_ajarAngle", 0f);
            Set(door, "_storyBlocker", collider);
            var interactable = Get<ChapterInteractable>(root.gameObject);
            Set(interactable, "_director", director);
            Set(interactable, "_stage", (int)Chapter01Director.Stage.Hallway);
            Set(interactable, "_label", "Mở cửa");
        }

        private static void PositionMotherOnExistingSeat(Transform hall, Transform mother)
        {
            var chair = hall.Find("MotherChair");
            if (chair != null) Undo.DestroyObjectImmediate(chair.gameObject);
            mother.SetPositionAndRotation(new Vector3(1.15f, .05f, 3.1f), Quaternion.Euler(0, 270, 0));
        }

        private static void ConfigureStandButton(Chapter01Director director)
        {
            var canvas = UnityEngine.Object.FindAnyObjectByType<DialogueUI>().transform;
            var root = canvas.Find("OpeningStandButton");
            if (root == null)
            {
                root = new GameObject("OpeningStandButton", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button)).transform;
                root.SetParent(canvas, false);
            }
            var rect = (RectTransform)root;
            rect.anchorMin = rect.anchorMax = new Vector2(.5f, .18f);
            rect.sizeDelta = new Vector2(240, 60);
            rect.anchoredPosition = Vector2.zero;
            var image = root.GetComponent<Image>();
            image.color = new Color(.18f, .11f, .07f, .92f);
            var button = root.GetComponent<Button>();
            var textTransform = root.Find("Text");
            if (textTransform == null)
            {
                textTransform = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI)).transform;
                textTransform.SetParent(root, false);
            }
            var textRect = (RectTransform)textTransform;
            textRect.anchorMin = Vector2.zero; textRect.anchorMax = Vector2.one;
            textRect.offsetMin = textRect.offsetMax = Vector2.zero;
            var label = textTransform.GetComponent<TextMeshProUGUI>();
            label.text = "[E]  Ngồi dậy";
            label.fontSize = 28;
            label.alignment = TextAlignmentOptions.Center;
            label.color = Color.white;
            label.font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/_Game/UI/Fonts/ChapterVietnamese.asset");
            Set(director, "_standUpButton", button);
        }

        private static void ConfigureCryingAudio(Chapter01Audio audio, Transform mother)
        {
            var sourceTransform = FindOrCreate(audio.transform, "CryingSource");
            var source = Get<AudioSource>(sourceTransform.gameObject);
            source.playOnAwake = false;
            source.spatialBlend = 1;
            source.rolloffMode = AudioRolloffMode.Logarithmic;
            Set(audio, "_cryingSource", source);
            Set(audio, "_cryingAnchor", mother);
            Set(audio, "_crying", AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/_Game/Audio/Chapter01/crying.mp3"));
        }

        private static void ConfigureOpeningCamera(Chapter01Director director, Transform player, Transform house)
        {
            var camera = FindOrCreate(house, "OpeningKieuBedCamera");
            // Stay inside the room, ahead of Kiều's seated pose; the doorway wall is farther forward.
            var position = player.position + new Vector3(1.15f, 0f, -.8f);
            position.y = 1.45f;
            camera.SetPositionAndRotation(position, Quaternion.identity);
            camera.LookAt(player.position + Vector3.up * .88f);
            Set(director, "_openingCamera", camera);
        }

        private static void ConfigureThunderAudio(Chapter01Audio audio)
        {
            var sourceTransform = FindOrCreate(audio.transform, "ThunderSource");
            var source = Get<AudioSource>(sourceTransform.gameObject);
            source.playOnAwake = false;
            source.loop = false;
            source.spatialBlend = 0;
            source.volume = 1;
            Set(audio, "_thunderSource", source);
        }

        private static void CreateDisturbance(Transform hall)
        {
            var root = FindOrCreate(hall, "OpeningDisturbance");
            var wood = AssetDatabase.LoadAssetAtPath<Material>("Assets/_Game/Materials/Chapter01Environment/DarkWood.mat");
            var paper = AssetDatabase.LoadAssetAtPath<Material>("Assets/_Game/Materials/Chapter01Environment/WarmPaper.mat");
            Prop(root, "OverturnedStoolSeat", PrimitiveType.Cube, new Vector3(3.7f, .3f, 2.15f), new Vector3(.58f, .09f, .5f), Quaternion.Euler(14, 28, 78), wood);
            Prop(root, "OverturnedStoolLegA", PrimitiveType.Cube, new Vector3(3.52f, .17f, 2.1f), new Vector3(.06f, .42f, .06f), Quaternion.Euler(12, 34, 78), wood);
            Prop(root, "OverturnedStoolLegB", PrimitiveType.Cube, new Vector3(3.88f, .17f, 2.22f), new Vector3(.06f, .42f, .06f), Quaternion.Euler(12, 34, 78), wood);
            Prop(root, "FallenVase", PrimitiveType.Cylinder, new Vector3(-4.35f, .2f, 2.2f), new Vector3(.19f, .32f, .19f), Quaternion.Euler(82, 8, -24), wood);
            Prop(root, "VaseShardA", PrimitiveType.Cube, new Vector3(-4.07f, .035f, 2.14f), new Vector3(.18f, .025f, .09f), Quaternion.Euler(0, 31, 8), paper);
            Prop(root, "VaseShardB", PrimitiveType.Cube, new Vector3(-4.23f, .035f, 1.91f), new Vector3(.1f, .02f, .15f), Quaternion.Euler(0, -40, 12), paper);
            Prop(root, "ScatteredScrollA", PrimitiveType.Cube, new Vector3(3.15f, .028f, .35f), new Vector3(.28f, .012f, .18f), Quaternion.Euler(0, 24, 0), paper);
            Prop(root, "ScatteredScrollB", PrimitiveType.Cube, new Vector3(3.48f, .028f, .08f), new Vector3(.22f, .012f, .16f), Quaternion.Euler(0, -48, 0), paper);
            Prop(root, "BrokenFrame", PrimitiveType.Cube, new Vector3(4.55f, .5f, 3.45f), new Vector3(.62f, .09f, .42f), Quaternion.Euler(71, 12, 18), wood);
        }

        private static void Prop(Transform parent, string name, PrimitiveType type, Vector3 position, Vector3 scale, Quaternion rotation, Material material)
        {
            var node = parent.Find(name);
            if (node == null)
            {
                node = GameObject.CreatePrimitive(type).transform;
                node.name = name;
                node.SetParent(parent, true);
            }
            node.SetPositionAndRotation(position, rotation);
            node.localScale = scale;
            node.gameObject.layer = 8;
            node.GetComponent<Renderer>().sharedMaterial = material;
            node.GetComponent<Collider>().enabled = false;
        }

        private static Transform FindOrCreate(Transform parent, string name)
        {
            var found = parent.Find(name);
            if (found != null) return found;
            var result = new GameObject(name).transform;
            result.SetParent(parent, false);
            return result;
        }

        private static void Block(Transform parent, string name, Vector3 localPosition, Vector3 localScale)
        {
            var node = FindOrCreate(parent, name);
            if (node.GetComponent<MeshRenderer>() == null)
            {
                var primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
                primitive.name = name;
                primitive.transform.SetParent(parent, false);
                UnityEngine.Object.DestroyImmediate(node.gameObject);
                node = primitive.transform;
            }
            node.localPosition = localPosition;
            node.localScale = localScale;
            node.gameObject.layer = 8;
            node.GetComponent<Renderer>().sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/_Game/Materials/Chapter01Environment/DarkWood.mat");
        }

        private static T Get<T>(GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponent<T>();
            return component != null ? component : gameObject.AddComponent<T>();
        }

        private static void SetSitting(Animator animator, bool value)
        {
            if (animator == null) return;
            foreach (var parameter in animator.parameters)
                if (parameter.name == "IsSitting") animator.SetBool(parameter.nameHash, value);
            if (value)
            {
                int seatedState = Animator.StringToHash("Base Layer.SittingIdle");
                if (animator.HasState(0, seatedState)) animator.Play(seatedState, 0, 0);
            }
        }

        private static void Set(UnityEngine.Object target, string field, object value)
        {
            var property = new SerializedObject(target).FindProperty(field);
            if (property == null) throw new InvalidOperationException("Missing serialized field: " + field);
            if (value == null) property.objectReferenceValue = null;
            else if (value is UnityEngine.Object reference) property.objectReferenceValue = reference;
            else if (value is bool boolean) property.boolValue = boolean;
            else if (value is int integer) property.intValue = integer;
            else if (value is float number) property.floatValue = number;
            else if (value is string text) property.stringValue = text;
            property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
