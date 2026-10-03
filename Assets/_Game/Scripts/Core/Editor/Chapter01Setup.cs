using System;
using System.IO;
using System.Linq;
using ThuyKieu.Dialogue;
using ThuyKieu.Dialogue.Editor;
using ThuyKieu.Interaction;
using ThuyKieu.Player;
using ThuyKieu.UI;
using TMPro;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;

namespace ThuyKieu.Core.Editor
{
    public static class Chapter01Setup
    {
        public const string ScenePath = "Assets/_Game/Scenes/Chapter01_GiaBien.unity";
        public const string UIPath = "Assets/_Game/Prefabs/UI/Chapter01DialogueCanvas.prefab";
        private const string ControllerPath = "Assets/_Game/Animations/ThuyKieu/Chapter01.controller";
        private static TMP_FontAsset _font;
        private static Material _wood, _plaster, _stone;

        [MenuItem("ThuyKieu/Chapter 1/Create playable scene")]
        public static void Create()
        {
            if (Application.isPlaying) throw new InvalidOperationException("Stop Play Mode before scene setup.");
            if (File.Exists(ScenePath)) throw new InvalidOperationException("Chapter scene exists; use targeted edits instead of replacing it.");
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                var open = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
                if (open.isDirty)
                    EditorSceneManager.SaveScene(open, string.IsNullOrEmpty(open.path)
                        ? AssetDatabase.GenerateUniqueAssetPath("Assets/_Game/Scenes/BeforeChapter01.unity") : open.path);
            }
            InkStoryCompiler.Compile();
            PrepareFont();
            PrepareController();
            _wood = Material("Chapter01Wood", new Color(0.22f, 0.10f, 0.06f));
            _plaster = Material("Chapter01Plaster", new Color(0.46f, 0.39f, 0.29f));
            _stone = Material("Chapter01Courtyard", new Color(0.16f, 0.22f, 0.25f));
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.18f, 0.23f, 0.33f);
            RenderSettings.skybox = null;
            var systems = new GameObject("Chapter01Systems");
            var manager = systems.AddComponent<DialogueManager>();
            var director = systems.AddComponent<Chapter01Director>();
            if (InkStatePersistence.Instance == null) new GameObject("InkSession").AddComponent<InkStatePersistence>();

            var player = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Game/Prefabs/Player/ThuyKieu.prefab"));
            player.name = "ThuyKieu";
            player.transform.position = new Vector3(0, 0.05f, -1.8f);
            var spawn = new GameObject("PlayerSpawn");
            spawn.transform.position = player.transform.position;
            var movement = player.GetComponent<PlayerMovement>();
            var interaction = player.GetComponent<PlayerInteraction>() ?? player.AddComponent<PlayerInteraction>();
            var playerAnimator = player.GetComponentInChildren<Animator>();
            playerAnimator.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(ControllerPath);
            playerAnimator.applyRootMotion = false;

            var cameraObject = new GameObject("Main Camera") { tag = "MainCamera" };
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.025f, 0.035f, 0.065f);
            camera.nearClipPlane = 0.08f;
            camera.farClipPlane = 100;
            cameraObject.AddComponent<AudioListener>();
            cameraObject.transform.SetPositionAndRotation(new Vector3(0, 2.8f, -5.4f), Quaternion.Euler(18, 0, 0));
            var follow = cameraObject.AddComponent<ThirdPersonCameraController>();
            Set(follow, "target", player.transform.Find("CameraTarget"));
            Set(follow, "distance", 3.4f);
            Set(follow, "_collisionMask", 1 << 8);
            Set(movement, "cameraTransform", cameraObject.transform);
            systems.AddComponent<DialogueCursorController>();

            Block("CourtyardFloor", new Vector3(0, -0.25f, -2), new Vector3(20, 0.5f, 24), _stone);
            Block("MainHallFloor", new Vector3(0, -0.10f, 2), new Vector3(12, 0.2f, 9), _wood);
            Block("BackWall", new Vector3(0, 1.6f, 6.5f), new Vector3(12, 3.2f, 0.3f), _plaster);
            Block("WestWall", new Vector3(-6, 1.6f, 2), new Vector3(0.3f, 3.2f, 9), _plaster);
            Block("EastWall", new Vector3(6, 1.6f, 2), new Vector3(0.3f, 3.2f, 9), _plaster);
            Block("FrontWallLeft", new Vector3(-3.8f, 1.6f, -2.5f), new Vector3(4.4f, 3.2f, 0.3f), _plaster);
            Block("FrontWallRight", new Vector3(3.8f, 1.6f, -2.5f), new Vector3(4.4f, 3.2f, 0.3f), _plaster);
            Block("DoorLintel", new Vector3(0, 2.9f, -2.5f), new Vector3(3.2f, 0.6f, 0.3f), _wood);
            Block("CourtyardWest", new Vector3(-9.5f, 1, -7), new Vector3(0.3f, 2, 12), _stone);
            Block("CourtyardEast", new Vector3(9.5f, 1, -7), new Vector3(0.3f, 2, 12), _stone);
            Block("CourtyardGate", new Vector3(0, 1, -13.8f), new Vector3(20, 2, 0.3f), _stone);
            // Open ceiling keeps the existing orbit camera usable in a compact blockout.
            for (int i = -1; i <= 1; i++) Block("RoofBeam", new Vector3(i * 4.5f, 3.3f, 2), new Vector3(0.18f, 0.2f, 9), _wood);

            var mother = Character("MeKieu", "Assets/_Game/Art/Characters/MeThuyKieu/Models/MeThuyKieu_Rigged.fbx", new Vector3(-2.5f, 0, 1.5f), 1.62f);
            mother.transform.rotation = Quaternion.Euler(0, 150, 0);
            var motherAnimator = mother.GetComponentInChildren<Animator>();
            if (motherAnimator != null && motherAnimator.avatar != null && motherAnimator.avatar.isHuman)
                motherAnimator.runtimeAnimatorController = playerAnimator.runtimeAnimatorController;
            bool maHasRig = File.Exists(MaGiamSinhAnimationSetup.ModelPath);
            var ma = Character(maHasRig ? "MaGiamSinh" : "MaGiamSinh · model chưa rig",
                maHasRig ? MaGiamSinhAnimationSetup.ModelPath : "Assets/_Game/Art/Characters/MaGiamSinh/Models/4fc6d0ee9e41c2998a0772c3819720a1.obj", new Vector3(2.5f, 0, 0.5f), 1.78f);
            var maAnimator = ma.GetComponentInChildren<Animator>();
            if (maAnimator != null && maAnimator.avatar != null && maAnimator.avatar.isHuman)
                maAnimator.runtimeAnimatorController = MaGiamSinhAnimationSetup.GetController();
            ma.transform.rotation = Quaternion.Euler(0, 230, 0);
            Label(mother.transform, "Mẹ Kiều", new Vector3(0, 1.95f, 0));
            Label(ma.transform, "Mã Giám Sinh", new Vector3(0, 2.1f, 0));
            var table = Block("ContractTable", new Vector3(0, 0.78f, 3.1f), new Vector3(2.3f, 0.16f, 1.2f), _wood);
            foreach (float x in new[] { -0.95f, 0.95f }) foreach (float z in new[] { 2.65f, 3.55f })
                Block("TableLeg", new Vector3(x, 0.38f, z), new Vector3(0.12f, 0.76f, 0.12f), _wood);
            Block("Tờ khế", new Vector3(0, 0.88f, 3.1f), new Vector3(0.65f, 0.015f, 0.45f), Material("Chapter01Paper", new Color(0.8f, 0.72f, 0.48f)));
            Label(table.transform, "Tờ khế", new Vector3(0, 0.85f, 0));
            var exit = new GameObject("MainDoorInteraction");
            exit.transform.position = new Vector3(0, 0, -3.4f);
            var exitCollider = exit.AddComponent<BoxCollider>();
            exitCollider.center = Vector3.up;
            exitCollider.size = new Vector3(2.6f, 2, 1);
            exitCollider.isTrigger = true;
            Label(exit.transform, "Cửa chính", new Vector3(0, 2.5f, 0));
            Interactable(mother, director, Chapter01Director.Stage.Mother, "Nói chuyện với Mẹ Kiều");
            Interactable(table, director, Chapter01Director.Stage.Contract, "Xem tờ khế");
            Interactable(exit, director, Chapter01Director.Stage.Exit, "Lên đường");

            Light("Moonlight", LightType.Directional, new Vector3(0, 8, 0), new Color(0.45f, 0.6f, 1), 0.65f).transform.rotation = Quaternion.Euler(45, -30, 0);
            Light("OilLamp", LightType.Point, new Vector3(-2, 2.5f, 2), new Color(1, 0.62f, 0.27f), 6).range = 12;
            Light("ContractLamp", LightType.Point, new Vector3(2, 2.5f, 4), new Color(1, 0.72f, 0.4f), 5).range = 10;
            Rain();

            var canvas = CreateUI(manager, interaction, director);
            PrefabUtility.SaveAsPrefabAsset(canvas, UIPath);
            Set(director, "_compiledInk", AssetDatabase.LoadAssetAtPath<TextAsset>(InkStoryCompiler.Output));
            Set(director, "_dialogue", manager);
            Set(director, "_player", movement);
            Set(director, "_interaction", interaction);
            Set(director, "_camera", follow);
            Set(director, "_maGiamSinh", ma);
            Set(director, "_motherAnimator", motherAnimator);
            Set(director, "_maAnimator", maAnimator);
            Chapter01CameraSetup.Configure();
            EditorSceneManager.SaveScene(scene, ScenePath);
            if (File.Exists(Chapter01SharedAnimationSetup.ControllerPath)) Chapter01SharedAnimationSetup.Apply();
            CreateChapter2Placeholder();
            var existing = EditorBuildSettings.scenes.ToList();
            foreach (string path in new[] { ScenePath, "Assets/_Game/Scenes/Chapter02_Placeholder.unity" })
                if (!existing.Any(s => s.path == path)) existing.Add(new EditorBuildSettingsScene(path, true));
            EditorBuildSettings.scenes = existing.ToArray();
            AssetDatabase.SaveAssets();
            EditorSceneManager.SetActiveScene(scene);
            Selection.activeGameObject = player;
        }

        private static void PrepareFont()
        {
            if (Resources.Load<TMP_Settings>("TMP Settings") == null)
            {
                var package = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(TMP_Settings).Assembly);
                string essentials = Path.Combine(package.resolvedPath, "Package Resources/TMP Essential Resources.unitypackage");
                UnityEditor.AssetPackage.Package.Import(essentials, false);
                AssetDatabase.Refresh();
            }
            const string path = "Assets/_Game/UI/Fonts/ChapterVietnamese.asset";
            _font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(path);
            if (_font != null) return;
            _font = TMP_FontAsset.CreateFontAsset(AssetDatabase.LoadAssetAtPath<Font>("Assets/_Game/UI/Fonts/ChapterVietnamese.ttf"), 64, 8, GlyphRenderMode.SDFAA, 2048, 2048, AtlasPopulationMode.Dynamic);
            _font.name = "ChapterVietnamese";
            _font.isMultiAtlasTexturesEnabled = true;
            AssetDatabase.CreateAsset(_font, path);
            AssetDatabase.AddObjectToAsset(_font.material, _font);
            foreach (var texture in _font.atlasTextures) AssetDatabase.AddObjectToAsset(texture, _font);
            string corpus = string.Join("", Directory.GetFiles("Assets/_Game/Data/Dialogue/Ink", "*.ink").Select(File.ReadAllText));
            corpus += "CHƯƠNG 1 HOÀN THÀNH Tiếp tục Đến bên Mẹ Kiều E để nói chuyện Đến bàn ký khế xem tờ khế Đến cửa chính Lên đường WASD Shift Chuột Dẫn chuyện 0123456789 · —";
            if (!_font.TryAddCharacters(corpus, out string missing))
                UnityEngine.Debug.LogWarning("Missing font glyphs: " + missing);
            EditorUtility.SetDirty(_font);
        }

        private static void PrepareController()
        {
            if (File.Exists(ControllerPath)) return;
            AssetDatabase.CopyAsset("Assets/_Game/Animations/ThuyKieu/ThuyKieu.controller", ControllerPath);
            var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(ControllerPath);
            controller.AddParameter("DialogueTalking", AnimatorControllerParameterType.Bool);
            var machine = controller.layers[0].stateMachine;
            var talk = machine.AddState("DialogueTalk");
            talk.motion = AssetDatabase.LoadAllAssetsAtPath("Assets/_Game/Animations/ThuyKieu/TK-Talking.fbx").OfType<AnimationClip>().First(c => !c.name.StartsWith("__preview__"));
            var enter = machine.AddAnyStateTransition(talk);
            enter.hasExitTime = false; enter.duration = 0.15f; enter.canTransitionToSelf = false;
            enter.AddCondition(AnimatorConditionMode.If, 0, "DialogueTalking");
            var leave = talk.AddTransition(machine.defaultState);
            leave.hasExitTime = false; leave.duration = 0.15f;
            leave.AddCondition(AnimatorConditionMode.IfNot, 0, "DialogueTalking");
            EditorUtility.SetDirty(controller);
        }

        private static GameObject Character(string name, string path, Vector3 position, float height)
        {
            var root = new GameObject(name);
            var importer = AssetImporter.GetAtPath(path) as ModelImporter;
            if (path.EndsWith(".fbx") && importer.animationType != ModelImporterAnimationType.Human)
            { importer.animationType = ModelImporterAnimationType.Human; importer.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel; importer.SaveAndReimport(); }
            var model = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(path));
            model.transform.SetParent(root.transform, false);
            Bounds bounds = model.GetComponentsInChildren<Renderer>()[0].bounds;
            foreach (var renderer in model.GetComponentsInChildren<Renderer>()) bounds.Encapsulate(renderer.bounds);
            float scale = height / bounds.size.y;
            model.transform.localScale *= scale;
            model.transform.localPosition = new Vector3(-bounds.center.x * scale, -bounds.min.y * scale, -bounds.center.z * scale);
            var animator = model.GetComponent<Animator>();
            if (animator != null) { animator.applyRootMotion = false; animator.cullingMode = AnimatorCullingMode.AlwaysAnimate; }
            string folder = path.Substring(0, path.IndexOf("/Models/"));
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(folder + "/Tectures/texture_pbr_20250901.png");
            var material = Material(name.Replace(" · model chưa rig", "") + "Chapter01", Color.white);
            if (texture != null) material.SetTexture("_BaseMap", texture);
            foreach (var renderer in model.GetComponentsInChildren<Renderer>()) renderer.sharedMaterials = Enumerable.Repeat(material, renderer.sharedMaterials.Length).ToArray();
            root.transform.position = position;
            var collider = root.AddComponent<CapsuleCollider>();
            collider.height = height; collider.center = Vector3.up * height / 2; collider.radius = 0.3f;
            return root;
        }

        private static GameObject CreateUI(DialogueManager manager, PlayerInteraction interaction, Chapter01Director director)
        {
            var go = new GameObject("DialogueCanvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            go.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = go.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080); scaler.matchWidthOrHeight = 0.5f;
            var ui = go.AddComponent<DialogueUI>();
            var panel = Panel(go.transform, "DialoguePanel", new Vector2(0.04f, 0.035f), new Vector2(0.96f, 0.42f), new Color(0.025f, 0.035f, 0.065f, 0.95f));
            var speaker = Text(panel, "SpeakerNameText", "", 30, new Vector2(0.03f, 0.81f), new Vector2(0.9f, 0.97f)); speaker.color = new Color(1, 0.77f, 0.4f); speaker.fontStyle = FontStyles.Bold;
            var body = Text(panel, "DialogueBodyText", "", 29, new Vector2(0.03f, 0.45f), new Vector2(0.97f, 0.80f));
            var next = Button(panel, "ContinueButton", "Tiếp · Space", new Vector2(0.78f, 0.06f), new Vector2(0.96f, 0.23f));
            var choices = Rect(panel, "ChoicesContainer", new Vector2(0.03f, 0.03f), new Vector2(0.97f, 0.44f));
            var layout = choices.gameObject.AddComponent<VerticalLayoutGroup>(); layout.spacing = 7; layout.childControlHeight = true; layout.childForceExpandHeight = false; layout.childControlWidth = true;
            var template = Button(choices, "ChoiceButtonTemplate", "", Vector2.zero, Vector2.one);
            var size = template.gameObject.AddComponent<LayoutElement>(); size.preferredHeight = 53; size.minHeight = 53;
            template.gameObject.SetActive(false);
            Set(ui, "dialoguePanel", panel.gameObject); Set(ui, "_speakerText", speaker); Set(ui, "_bodyText", body);
            Set(ui, "continueButton", next); Set(ui, "choicesRoot", choices); Set(ui, "choiceButtonPrefab", template);
            ThuyKieu.UI.Editor.Chapter01DialogueStyle.Apply(ui);

            var objective = Text(go.transform, "Objective", "CHƯƠNG 1 — GIA BIẾN", 30, new Vector2(0.03f, 0.90f), new Vector2(0.8f, 0.97f));
            Text(go.transform, "Controls", "WASD: đi · Shift: chạy · Chuột: camera · E: tương tác", 22, new Vector2(0.03f, 0.85f), new Vector2(0.9f, 0.9f));
            Set(director, "_objective", objective);
            var prompt = Panel(go.transform, "InteractionPrompt", new Vector2(0.31f, 0.46f), new Vector2(0.69f, 0.53f), new Color(0, 0, 0, 0.75f));
            var promptTextObject = new GameObject("PromptText", typeof(RectTransform), typeof(UnityEngine.UI.Text));
            promptTextObject.transform.SetParent(prompt, false);
            Stretch(promptTextObject.GetComponent<RectTransform>(), Vector2.zero, Vector2.one);
            var promptText = promptTextObject.GetComponent<UnityEngine.UI.Text>();
            promptText.font = AssetDatabase.LoadAssetAtPath<Font>("Assets/_Game/UI/Fonts/ChapterVietnamese.ttf");
            promptText.fontSize = 25; promptText.alignment = TextAnchor.MiddleCenter; promptText.color = Color.white;
            var promptView = go.AddComponent<InteractionPromptUI>();
            Set(promptView, "promptRoot", prompt.gameObject); Set(promptView, "promptText", promptText); Set(promptView, "playerInteraction", interaction);
            var completion = Panel(go.transform, "ChapterComplete", new Vector2(0.2f, 0.3f), new Vector2(0.8f, 0.7f), new Color(0.03f, 0.05f, 0.1f, 0.98f));
            Text(completion, "Title", "CHƯƠNG 1 HOÀN THÀNH", 46, new Vector2(0.07f, 0.55f), new Vector2(0.95f, 0.9f));
            var completeButton = Button(completion, "ContinueChapter", "Tiếp tục", new Vector2(0.3f, 0.15f), new Vector2(0.7f, 0.4f));
            Set(director, "_completionPanel", completion.gameObject); Set(director, "_completionButton", completeButton);
            var fade = Panel(go.transform, "OpeningFade", Vector2.zero, Vector2.one, Color.black).gameObject.AddComponent<CanvasGroup>();
            Set(director, "_fade", fade);
            var events = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
            return go;
        }

        private static void CreateChapter2Placeholder()
        {
            const string path = "Assets/_Game/Scenes/Chapter02_Placeholder.unity";
            if (File.Exists(path)) return;
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
            EditorSceneManager.SetActiveScene(scene);
            var camera = new GameObject("Main Camera", typeof(Camera)); camera.tag = "MainCamera";
            camera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            camera.GetComponent<Camera>().backgroundColor = new Color(0.03f, 0.05f, 0.1f);
            var canvas = new GameObject("Chapter2Canvas", typeof(Canvas), typeof(CanvasScaler)); canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            Text(canvas.transform, "Chapter2Placeholder", "CHƯƠNG 2\nCÁI GIÁ CỦA MỘT CON NGƯỜI\n\nChương 1 đã hoàn thành. Trạng thái lựa chọn đã được lưu.\nChương 2 đang chờ triển khai.", 32, new Vector2(0.1f, 0.2f), new Vector2(0.9f, 0.8f));
            EditorSceneManager.SaveScene(scene, path);
            EditorSceneManager.CloseScene(scene, true);
        }

        private static void Rain()
        {
            var go = new GameObject("CourtyardRain"); go.transform.position = new Vector3(0, 7, -7);
            var rain = go.AddComponent<ParticleSystem>(); var main = rain.main;
            main.startLifetime = 0.75f; main.startSpeed = 10; main.startSize = 0.008f; main.maxParticles = 1200;
            main.startColor = new Color(0.55f, 0.7f, 0.9f, 0.45f); main.simulationSpace = ParticleSystemSimulationSpace.World;
            var shape = rain.shape; shape.shapeType = ParticleSystemShapeType.Box; shape.scale = new Vector3(17, 10, 0.1f);
            go.transform.rotation = Quaternion.Euler(90, 0, 0);
            var emission = rain.emission; emission.rateOverTime = 550;
            var renderer = rain.GetComponent<ParticleSystemRenderer>(); renderer.renderMode = ParticleSystemRenderMode.Stretch; renderer.lengthScale = 1; renderer.velocityScale = 0.008f;
            var material = new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit")); material.color = new Color(0.55f, 0.7f, 0.9f, 0.4f);
            AssetDatabase.CreateAsset(material, "Assets/_Game/Materials/Chapter01Rain.mat"); renderer.sharedMaterial = material;
        }

        private static Material Material(string name, Color color)
        {
            string path = "Assets/_Game/Materials/" + name + ".mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material != null) return material;
            material = new Material(Shader.Find("Universal Render Pipeline/Lit")); material.color = color;
            AssetDatabase.CreateAsset(material, path); return material;
        }
        private static GameObject Block(string name, Vector3 position, Vector3 scale, Material material)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube); go.name = name; go.layer = 8;
            go.transform.position = position; go.transform.localScale = scale; go.GetComponent<Renderer>().sharedMaterial = material; return go;
        }
        private static Light Light(string name, LightType type, Vector3 position, Color color, float intensity)
        {
            var go = new GameObject(name); go.transform.position = position; var light = go.AddComponent<Light>(); light.type = type; light.color = color; light.intensity = intensity; light.shadows = LightShadows.Soft; return light;
        }
        private static void Interactable(GameObject go, Chapter01Director director, Chapter01Director.Stage stage, string label)
        {
            var interaction = go.AddComponent<ChapterInteractable>(); Set(interaction, "_director", director); Set(interaction, "_stage", (int)stage); Set(interaction, "_label", label);
        }
        private static void Label(Transform parent, string text, Vector3 position)
        {
            var go = new GameObject("Label", typeof(TextMeshPro)); go.transform.SetParent(parent, false); go.transform.localPosition = position; go.transform.localRotation = Quaternion.Euler(0, 180, 0);
            var label = go.GetComponent<TextMeshPro>(); label.font = _font; label.text = text; label.fontSize = 2; label.alignment = TextAlignmentOptions.Center; label.rectTransform.sizeDelta = new Vector2(3, 0.6f);
            go.AddComponent<WorldLabelBillboard>();
        }
        private static RectTransform Rect(Transform parent, string name, Vector2 min, Vector2 max)
        {
            var go = new GameObject(name, typeof(RectTransform)); go.transform.SetParent(parent, false); var rect = go.GetComponent<RectTransform>(); Stretch(rect, min, max); return rect;
        }
        private static void Stretch(RectTransform rect, Vector2 min, Vector2 max) { rect.anchorMin = min; rect.anchorMax = max; rect.offsetMin = rect.offsetMax = Vector2.zero; }
        private static RectTransform Panel(Transform parent, string name, Vector2 min, Vector2 max, Color color)
        { var rect = Rect(parent, name, min, max); rect.gameObject.AddComponent<Image>().color = color; return rect; }
        private static TMP_Text Text(Transform parent, string name, string value, float size, Vector2 min, Vector2 max)
        {
            var rect = Rect(parent, name, min, max); var text = rect.gameObject.AddComponent<TextMeshProUGUI>(); text.font = _font; text.text = value; text.fontSize = size; text.color = Color.white; text.raycastTarget = false; text.margin = new Vector4(10, 5, 10, 5); return text;
        }
        private static Button Button(Transform parent, string name, string label, Vector2 min, Vector2 max)
        {
            var rect = Panel(parent, name, min, max, new Color(0.16f, 0.23f, 0.32f)); var button = rect.gameObject.AddComponent<Button>();
            var colors = button.colors; colors.highlightedColor = new Color(0.75f, 0.85f, 1); colors.pressedColor = new Color(0.6f, 0.7f, 0.85f); button.colors = colors;
            var text = Text(rect, "Label", label, 24, Vector2.zero, Vector2.one); text.alignment = TextAlignmentOptions.MidlineLeft; return button;
        }
        private static void Set(UnityEngine.Object target, string property, object value)
        {
            var serialized = new SerializedObject(target); var field = serialized.FindProperty(property);
            if (field == null) throw new InvalidOperationException(target.name + ": " + property);
            if (value is UnityEngine.Object unityObject) field.objectReferenceValue = unityObject;
            else if (value is string text) field.stringValue = text;
            else if (value is float number) field.floatValue = number;
            else if (value is int integer) field.intValue = integer;
            else if (value == null) field.objectReferenceValue = null;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
