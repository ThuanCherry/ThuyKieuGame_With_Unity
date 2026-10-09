using System;
using System.Linq;
using ThuyKieu.Core;
using ThuyKieu.Dialogue;
using ThuyKieu.Interaction;
using ThuyKieu.Player;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ThuyKieu.Environment.Editor
{
    public static class Chapter01ReworkSetup
    {
        private static Material _wood, _paper, _ink, _stone;
        private static Chapter01Director _director;
        [MenuItem("ThuyKieu/Chapter 1/Apply story rework")]
        public static void Apply()
        {
            var scene = SceneManager.GetActiveScene();
            if (Application.isPlaying || scene.path != ThuyKieu.Core.Editor.Chapter01Setup.ScenePath)
                throw new InvalidOperationException("Open Chapter01_GiaBien in Edit Mode.");
            _wood = Material("DarkWood"); _paper = Material("WarmPaper");
            _ink = Material("Ink"); _stone = Material("WetStone");
            _director = UnityEngine.Object.FindAnyObjectByType<Chapter01Director>();
            var house = GameObject.Find("VuongGia/MainHouse").transform;
            var hall = house.Find("MainHall");
            var room = house.Find("KieuRoom");
            Block(room, "EastTimberBase", new Vector3(-2.2f,.35f,4.4f), new Vector3(.06f,.7f,3.7f), _wood, false);
            Block(room, "EastTimberCrown", new Vector3(-2.2f,2.75f,4.4f), new Vector3(.08f,.15f,3.7f), _wood, false);
            foreach (float x in new[]{-4.3f,-3.1f})
                Block(room,"DoorFrame"+x,new Vector3(x,1.3f,2.39f),new Vector3(.09f,2.6f,.1f),_wood,false);
            var props = Node(hall, "SearchTraces");
            // Disturbance stays off the main navigation route.
            var chair = Node(props, "FallenChair");
            chair.SetPositionAndRotation(new Vector3(-4.6f, .35f, .2f), Quaternion.Euler(0, 25, 75));
            Block(chair, "Seat", new Vector3(0, 0, 0), new Vector3(.6f, .08f, .55f), _wood, true, true);
            Block(chair, "Back", new Vector3(0, .35f, .24f), new Vector3(.6f, .65f, .07f), _wood, true, true);
            foreach (int x in new[] {-1, 1}) foreach (int z in new[] {-1, 1})
                Block(chair, "Leg"+x+z, new Vector3(x*.22f, -.25f, z*.2f), new Vector3(.06f, .5f, .06f), _wood, true, true);
            for (int i = 0; i < 5; i++)
            {
                var shard = Block(props, "BrokenCup"+i, new Vector3(-3.1f + i*.13f, .055f, -.3f + (i%2)*.16f), new Vector3(.12f, .025f, .075f), _paper, false);
                shard.localRotation = Quaternion.Euler(0, i*47, i%2*16);
            }
            for (int i = 0; i < 9; i++)
            {
                var mark = Block(props, "MudFootprint"+i, new Vector3(-.4f-(i%2)*.27f, .03f, -2.25f+i*.4f), new Vector3(.12f, .009f, .25f), _ink, false);
                mark.rotation = Quaternion.Euler(0, -18, 0);
            }
            var clues = Node(hall, "StoryInteractables");
            var debt = Block(clues, "DebtPaper", new Vector3(-.85f, .795f, 2.45f), new Vector3(.5f, .015f, .35f), _paper, false);
            for (int i = 0; i < 5; i++)
                Block(clues, "DebtInk"+i, new Vector3(-.85f, .805f, 2.33f+i*.05f), new Vector3(.34f, .003f, .005f), _ink, false);
            Clue(debt, "c1_clue_debt", "c1_xem_to_trat", "Xem tờ trát");
            var bag = Node(clues, "MoneyBag"); bag.position = new Vector3(.85f, .92f, 2.45f);
            var bagMesh = Block(bag, "Pouch", Vector3.zero, new Vector3(.25f, .22f, .25f), _wood, false, true);
            var tie = Block(bag, "Tie", new Vector3(0, .13f, 0), new Vector3(.13f, .045f, .13f), _paper, false, true);
            Clue(bag, "c1_clue_money", "c1_xem_tui_bac", "Kiểm tra túi bạc");
            var jewelry = Node(clues, "JewelryBox"); jewelry.position = new Vector3(.8f, .86f, 3.65f);
            Block(jewelry, "Base", Vector3.zero, new Vector3(.45f, .16f, .3f), _wood, false, true);
            var lid = Block(jewelry, "OpenLid", new Vector3(0, .15f, .15f), new Vector3(.45f, .025f, .3f), _wood, false, true);
            lid.localRotation = Quaternion.Euler(-65, 0, 0);
            for (int i = 0; i < 3; i++)
                Block(jewelry, "Ornament"+i, new Vector3(-.12f+i*.12f, .09f, 0), new Vector3(.025f, .016f, .19f), _paper, false, true);
            Clue(jewelry, "c1_clue_jewelry", "c1_xem_nu_trang", "Xem hộp nữ trang");

            var story = Node(house, "StoryPositions");
            var porch = Node(story, "MaPorch"); porch.position = new Vector3(.9f, 0, -3.45f);
            var maHall = Node(story, "MaHall"); maHall.position = new Vector3(2.4f, 0, 1.3f);
            var messenger = Node(story, "MessengerOffscreen"); messenger.position = new Vector3(-.7f, 0, -3.65f);
            var ma = new SerializedObject(_director).FindProperty("_maGiamSinh").objectReferenceValue as GameObject;
            ma.transform.SetPositionAndRotation(porch.position, Quaternion.Euler(0, 0, 0));
            // Keep inactive in the saved scene; it is revealed only by opening the door.
            ma.SetActive(false);
            var mother = GameObject.Find("MeKieu").transform;
            var chest = Block(story, "MaMoneyChest", new Vector3(1.8f, .3f, -.2f), new Vector3(.65f, .6f, .4f), _wood, true);
            chest.gameObject.SetActive(false);
            Set(_director, "_mother", mother); Set(_director, "_maPorch", porch); Set(_director, "_maHall", maHall);
            Set(_director, "_messengerAnchor", messenger); Set(_director, "_moneyChest", chest.gameObject);

            var door = house.Find("MainDoor").GetComponent<ProximityDoor>();
            var blocker = Node(door.transform, "StoryDoorBlocker");
            blocker.position = new Vector3(0, 1.15f, -2.5f); blocker.gameObject.layer = 8;
            var box = Get<BoxCollider>(blocker.gameObject); box.size = new Vector3(2.7f, 2.3f, .16f); box.enabled = true;
            Set(door, "_storyControlled", true); Set(door, "_storyBlocker", box); Set(_director, "_mainDoor", door);
            var doorTarget = house.Find("MainDoorInteraction");
            doorTarget.position = new Vector3(0, 0, -1.7f);
            Interactable(doorTarget, Chapter01Director.Stage.Door, "Mở cửa");
            var exit = Node(house, "ChapterExit"); exit.position = new Vector3(0, 0, -6.2f);
            var exitCollider = Get<BoxCollider>(exit.gameObject); exitCollider.center = Vector3.up; exitCollider.size = new Vector3(2, 2, .5f); exitCollider.isTrigger = true;
            Interactable(exit, Chapter01Director.Stage.Exit, "Rời Vương gia");
            // Static labels expose spoilers and obscure the new subtle interaction prompts.
            foreach (var name in new[] {"ContractLabel", "MainDoorInteraction/Label"})
            {
                var label = hall.Find(name) ?? house.Find(name);
                if (label != null) label.gameObject.SetActive(false);
            }
            foreach (var label in mother.GetComponentsInChildren<TMP_Text>(true)) label.gameObject.SetActive(false);
            foreach (var label in ma.GetComponentsInChildren<TMP_Text>(true)) label.gameObject.SetActive(false);

            var canvas = UnityEngine.Object.FindAnyObjectByType<DialogueUI>().transform;
            var fade = canvas.Find("OpeningFade"); fade.SetAsFirstSibling();
            var fadeGroup = fade.GetComponent<CanvasGroup>(); fadeGroup.alpha = 1; fadeGroup.blocksRaycasts = false;
            Set(_director, "_gameplayHints", canvas.Find("Controls").gameObject);
            var font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Fonts/VietnameseTMP.asset");
            var promptRoot = canvas.Find("InteractionPrompt");
            var oldPrompt = promptRoot.GetComponentInChildren<Text>(true);
            if (oldPrompt != null) oldPrompt.gameObject.SetActive(false);
            var prompt = Node(promptRoot, "VietnamesePrompt");
            var promptText = Get<TextMeshProUGUI>(prompt.gameObject);
            var promptRect = promptText.rectTransform;
            promptRect.anchorMin = Vector2.zero; promptRect.anchorMax = Vector2.one;
            promptRect.offsetMin = new Vector2(12, 0); promptRect.offsetMax = new Vector2(-12, 0);
            promptText.fontSize = 25; promptText.alignment = TextAlignmentOptions.Center; promptText.color = Color.white;
            Set(canvas.GetComponent<ThuyKieu.UI.InteractionPromptUI>(), "_tmpText", promptText);
            foreach (var text in canvas.GetComponentsInChildren<TMP_Text>(true))
            {
                text.font = font; text.fontSharedMaterial = font.material;
                text.textWrappingMode = TextWrappingModes.Normal;
            }
            var audio = UnityEngine.Object.FindAnyObjectByType<Chapter01Audio>();
            Set(audio, "_knock", AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/_Game/Audio/Chapter01/DoorKnock.wav"));
            Set(audio, "_dialogueLetterVolume", .22f);
            ConfigureCameras(mother, porch, messenger);
            ThuyKieu.Dialogue.Editor.InkStoryCompiler.Compile();
            Set(_director, "_compiledInk", AssetDatabase.LoadAssetAtPath<TextAsset>(ThuyKieu.Dialogue.Editor.InkStoryCompiler.Output));
            var scenes = EditorBuildSettings.scenes;
            foreach (var entry in scenes)
                if (!System.IO.File.Exists(entry.path))
                {
                    string resolved = AssetDatabase.GUIDToAssetPath(entry.guid.ToString());
                    if (!string.IsNullOrEmpty(resolved)) entry.path = resolved;
                }
            EditorBuildSettings.scenes = scenes.OrderBy(s => s.path == scene.path ? 0 : 1).ToArray();
            Physics.SyncTransforms();
            EditorSceneManager.MarkSceneDirty(scene); EditorSceneManager.SaveScene(scene); AssetDatabase.SaveAssets();
        }

        private static void ConfigureCameras(Transform mother, Transform porch, Transform messenger)
        {
            var router = _director.GetComponent<DialogueTagRouter>();
            var root = Node(null, "ReworkCameraPresets");
            Camera.main.fieldOfView = 65;
            var hall = Shot(root, "HallTwoShot", new Vector3(-.8f, 1.8f, -.6f), new Vector3(-2.25f, 1.2f, 1.35f));
            var motherShot = Shot(root, "Mother", new Vector3(-1, 1.6f, .25f), mother.position + Vector3.up*1.3f);
            var maShot = Shot(root, "MaPorch", new Vector3(-.6f, 1.65f, -1), porch.position + Vector3.up*1.35f);
            var offer = Shot(root, "HallOffer", new Vector3(0, 2f, -2.15f), new Vector3(0, 1.25f, 1.4f));
            var table = Shot(root, "Contract", new Vector3(.1f, 2.4f, 1.1f), new Vector3(0, .85f, 3));
            var messengerShot = Shot(root, "MessengerDoor", new Vector3(1.2f, 1.7f, -.7f), messenger.position + Vector3.up*1.2f);
            var ending = Shot(root, "RainExit", new Vector3(-2, 2, -7.9f), new Vector3(0, 1.1f, -5.8f));
            var lookBack = Shot(root, "LookBack", new Vector3(0, 1.8f, -3.9f), mother.position + Vector3.up*1.2f);
            string[] names = {"WS_VuongGia","WS_MainHall","MCU_MeKieu","MCU_Kieu","CU_Kieu","CU_Kieu_Eyes","MCU_Kieu_Me","MCU_MeKieu_Kieu","CU_Kieu_Ma","WS_MainHall_Ma","Door_Reveal","Reveal_MaGiamSinh","POV_Kieu_MaHands","Table_Contract","Insert_DebtPaper","Insert_MoneyBag","Insert_Jewelry","Insert_Signing","Insert_Hairpin","Door_Messenger","Hero_Kieu_Rain","Kieu_LookBack","CU_Kieu_Rain"};
            Transform[] shots = {hall,hall,motherShot,hall,hall,hall,hall,hall,offer,offer,maShot,maShot,offer,table,table,table,table,table,hall,messengerShot,ending,lookBack,ending};
            var serialized = new SerializedObject(router);
            var cues = serialized.FindProperty("_cameraCues"); cues.arraySize = names.Length;
            for (int i=0;i<names.Length;i++) { var cue = cues.GetArrayElementAtIndex(i); cue.FindPropertyRelative("Name").stringValue = names[i]; cue.FindPropertyRelative("Preset").objectReferenceValue = shots[i]; }
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static Transform Shot(Transform parent, string name, Vector3 position, Vector3 look)
        { var shot = Node(parent,name); shot.position = position; shot.LookAt(look); return shot; }
        private static Material Material(string name) => AssetDatabase.LoadAssetAtPath<Material>("Assets/_Game/Materials/Chapter01Environment/"+name+".mat");
        private static Transform Node(Transform parent, string name)
        {
            Transform node = parent != null ? parent.Find(name) : GameObject.Find(name)?.transform;
            if (node == null) { node = new GameObject(name).transform; node.SetParent(parent, false); }
            return node;
        }
        private static Transform Block(Transform parent, string name, Vector3 position, Vector3 scale, Material material, bool solid, bool local = false)
        {
            var node = parent.Find(name);
            if (node == null) { node = GameObject.CreatePrimitive(PrimitiveType.Cube).transform; node.name=name; node.SetParent(parent,false); }
            if (local) node.localPosition=position; else node.position=position;
            node.localScale=scale; node.gameObject.layer=8;
            node.GetComponent<Renderer>().sharedMaterial=material; node.GetComponent<Collider>().enabled=solid;
            return node;
        }
        private static T Get<T>(GameObject go) where T: Component
        { var component = go.GetComponent<T>(); return component != null ? component : go.AddComponent<T>(); }
        private static void Clue(Transform node, string knot, string flag, string label)
        {
            var collider = Get<BoxCollider>(node.gameObject);
            // Trigger is local to the mesh/root; visual proportions are preserved.
            collider.enabled = true; collider.isTrigger = true;
            collider.size = node.name == "DebtPaper" ? new Vector3(1, 20, 1) : new Vector3(.55f, .4f, .4f);
            var clue = Get<ChapterClueInteractable>(node.gameObject);
            Set(clue,"_director",_director); Set(clue,"_knot",knot); Set(clue,"_flag",flag); Set(clue,"_label",label);
        }
        private static void Interactable(Transform node, Chapter01Director.Stage stage, string label)
        {
            var item = Get<ChapterInteractable>(node.gameObject);
            Set(item,"_director",_director); Set(item,"_stage",(int)stage); Set(item,"_label",label);
        }
        private static void Set(UnityEngine.Object target, string field, object value)
        {
            var serialized = new SerializedObject(target); var property = serialized.FindProperty(field);
            if (value is UnityEngine.Object obj) property.objectReferenceValue=obj;
            else if (value is bool b) property.boolValue=b;
            else if (value is int n) property.intValue=n;
            else if (value is float f) property.floatValue=f;
            else if (value is string text) property.stringValue=text;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
