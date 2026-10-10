using System;
using System.Collections;
using ThuyKieu.Dialogue;
using ThuyKieu.Environment;
using ThuyKieu.Interaction;
using ThuyKieu.Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ThuyKieu.Core
{
    public class Chapter01Director : MonoBehaviour
    {
        // Keep original serialized stage values.
        public enum Stage { Opening, Mother, Negotiation, Contract, Exit, Complete, Hallway, Explore, Door, Offer, Waiting }
        [SerializeField] private TextAsset _compiledInk;
        [SerializeField] private DialogueManager _dialogue;
        [SerializeField] private PlayerMovement _player;
        [SerializeField] private PlayerInteraction _interaction;
        [SerializeField] private ThirdPersonCameraController _camera;
        [SerializeField] private GameObject _maGiamSinh;
        [SerializeField] private Animator _motherAnimator;
        [SerializeField] private Animator _maAnimator;
        [SerializeField] private GameObject _completionPanel;
        [SerializeField] private Button _completionButton;
        [SerializeField] private TMP_Text _objective;
        [SerializeField] private CanvasGroup _fade;
        [SerializeField] private ProximityDoor _mainDoor;
        [SerializeField] private Transform _mother, _maPorch, _maHall, _messengerAnchor;
        [SerializeField] private GameObject _moneyChest;
        [SerializeField] private GameObject _messenger;
        [SerializeField] private GameObject _gameplayHints;
        [SerializeField] private Button _standUpButton;
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private Chapter01Audio _audio;
        [SerializeField] private Transform _openingCamera;
        [SerializeField] private Transform _standingAfterSeat;
        [SerializeField] private Chapter01ContractPresentation _contractPresentation;
        [SerializeField, Min(5)] private float _waitSeconds = 8;
        private Coroutine _fadeRoutine, _maWalk;
        private bool _chapterEndRequested, _transitioning, _maMoving, _openingStandComplete, _standingUp;
        private float _waitUntil;
        private string _speaker = "Narrator";
        private Vector3 _maDestination;
        private Transform _reactionFocus;
        private float _reactionUntil;

        public Stage CurrentStage { get; private set; }
        public string LogicalScene { get; private set; }
        public bool DialogueActive => _dialogue != null && _dialogue.IsDialogueActive;
        public bool MotherConversation { get; private set; }
        public bool MaConversation { get; private set; }
        public int ClueCount => _dialogue.InkStory == null ? 0 : (int)_dialogue.InkStory.variablesState["c1_clues"];
        public event Action<string, string> CueReceived;

        private void Awake()
        {
            SetLocked(true);
            if (_playerAnimator == null) _playerAnimator = _player.GetComponentInChildren<Animator>();
            SetSitting(_playerAnimator, true);
            _player.PositionLocked = true;
            SetSitting(_motherAnimator, true);
            if (_openingCamera != null) _camera.BeginCinematicDialogue(_openingCamera);
            _fade.alpha = 1;
            // Fade sits behind dialogue so the opening narration remains readable on black.
            _fade.blocksRaycasts = false;
            _completionPanel.SetActive(false);
            if (_gameplayHints != null) _gameplayHints.SetActive(false);
            _maGiamSinh.SetActive(false);
            if (_messenger != null) _messenger.SetActive(false);
            if (_moneyChest != null) _moneyChest.SetActive(false);
            if (_standUpButton != null)
            {
                _standUpButton.gameObject.SetActive(false);
                _standUpButton.onClick.AddListener(StandUp);
            }
        }

        private IEnumerator Start()
        {
            _dialogue.SetInkStory(new Ink.Runtime.Story(_compiledInk.text));
            _dialogue.InkPauseBeforeLine = PauseBeforeLine;
            _dialogue.InkTagsChanged += RouteTags;
            _dialogue.DialogueStarted += OnDialogueStarted;
            _dialogue.DialogueEnded += OnDialogueEnded;
            _completionButton.onClick.AddListener(ContinueAfterChapter);
            if (InkStatePersistence.Instance == null) new GameObject("InkSession").AddComponent<InkStatePersistence>();
            SetAnimation(_motherAnimator, true, 1);
            yield return null;
            // Existing rain sources start before the first line appears.
            yield return new WaitForSeconds(1.5f);
            _dialogue.ResumeInk();
        }

        private void Update()
        {
            if (_contractPresentation != null && _contractPresentation.IsActing) return;
            if (!_openingStandComplete && _standUpButton != null && _standUpButton.gameObject.activeInHierarchy
                && InputReader.InteractPressedThisFrame)
                StandUp();
            if (CurrentStage == Stage.Waiting && !DialogueActive && Time.time >= _waitUntil)
                ResumeGate();
            if (!_maMoving && _maGiamSinh.activeInHierarchy &&
                (DialogueActive || CurrentStage == Stage.Offer || CurrentStage == Stage.Contract || CurrentStage == Stage.Waiting))
                Face(_maGiamSinh.transform, _player.transform.position, 180);
            if (!DialogueActive || _maMoving) return;
            if (_reactionFocus != null && Time.time < _reactionUntil)
            {
                Face(_player.transform, _reactionFocus.position, 240);
                return;
            }
            Transform focus = MotherConversation ? _mother : MaConversation ? _maGiamSinh.transform : _speaker == "MaGiamSinh" ? _maGiamSinh.transform
                : _speaker == "MeKieu" ? _mother : _speaker == "Messenger" ? _messengerAnchor : _player.transform;
            if (focus != null && focus != _player.transform) Face(_player.transform, focus.position, 130);
            // A seated actor keeps the chair's orientation instead of rotating through its geometry.
            if (_mother != null && focus != _mother && (_motherAnimator == null || !_motherAnimator.GetBool("IsSitting")))
                Face(_mother, focus.position, 95);
        }

        private bool PauseBeforeLine(string[] tags)
        {
            foreach (string raw in tags)
            {
                string tag = raw.Trim();
                if (!tag.StartsWith("gate:")) continue;
                switch (tag.Substring(5).Trim())
                {
                    case "hallway": SetStage(Stage.Hallway, "Tìm Mẹ Kiều"); break;
                    case "mother": SetStage(Stage.Mother, "Tìm Mẹ Kiều · E để nói chuyện"); break;
                    case "explore":
                        SetStage(Stage.Explore, ClueCount >= 2 ? "Trở lại bên Mẹ Kiều · có thể xem thêm manh mối" : "Tìm hiểu tình hình Vương gia · " + ClueCount + "/2");
                        break;
                    case "door": SetStage(Stage.Door, "Ra mở cửa"); break;
                    case "offer":
                        SetStage(Stage.Offer, "Trở lại gian chính · nói chuyện với Mẹ Kiều");
                        WalkMa(_maHall.position);
                        break;
                    case "contract": SetStage(Stage.Contract, "Đến bàn · E để xem tờ khế"); break;
                    case "waiting":
                        SetStage(Stage.Waiting, "Chờ tin");
                        _waitUntil = Time.time + _waitSeconds;
                        if (_moneyChest != null) _moneyChest.SetActive(false);
                        break;
                    case "exit":
                        SetStage(Stage.Exit, "Tạm biệt Mẹ · bước ra ngoài hiên");
                        if (_messenger != null) _messenger.SetActive(false);
                        WalkMa(_maPorch.position);
                        break;
                    default: Debug.LogWarning("Unknown Chapter 1 gate: " + tag, this); return false;
                }
                return true;
            }
            return false;
        }

        public bool CanInteract(Stage stage)
        {
            if (DialogueActive || _transitioning || _standingUp || !_openingStandComplete) return false;
            if (stage == Stage.Mother)
                return CurrentStage == Stage.Mother || CurrentStage == Stage.Offer && !_maMoving || CurrentStage == Stage.Explore && ClueCount >= 2;
            if (stage == Stage.Exit && _player.transform.position.z > -4.8f) return false;
            return stage == CurrentStage && CurrentStage != Stage.Waiting;
        }

        public void Interact(Stage stage, Transform target)
        {
            if (!CanInteract(stage)) return;
            MotherConversation = stage == Stage.Mother &&
                (CurrentStage == Stage.Mother || CurrentStage == Stage.Explore);
            MaConversation = CurrentStage == Stage.Door || CurrentStage == Stage.Offer || CurrentStage == Stage.Contract;
            if (CurrentStage == Stage.Hallway && target.TryGetComponent(out ProximityDoor roomDoor))
            {
                roomDoor.SetStoryOpen(true);
                ResumeGate();
                return;
            }
            if (CurrentStage == Stage.Explore)
            {
                SetStage(Stage.Negotiation, "");
                _dialogue.StartInkAt("c1_before_knock");
                return;
            }
            if (CurrentStage == Stage.Door)
            {
                _mainDoor.SetStoryOpen(true);
                _maGiamSinh.transform.SetPositionAndRotation(_maPorch.position, _maPorch.rotation);
                _maGiamSinh.SetActive(true);
                if (_maAnimator != null) _maAnimator.Update(0);
                Vector3 facing = _player.transform.position - _maGiamSinh.transform.position; facing.y = 0;
                if (facing.sqrMagnitude > .01f) _maGiamSinh.transform.rotation = Quaternion.LookRotation(facing);
                if (_moneyChest != null) _moneyChest.SetActive(true);
            }
            if (CurrentStage == Stage.Mother && _audio != null) _audio.StopCrying();
            ResumeGate();
        }

        private void StandUp()
        {
            if (_openingStandComplete || _standingUp || _standUpButton == null || !_standUpButton.gameObject.activeInHierarchy) return;
            _standingUp = true;
            _standUpButton.gameObject.SetActive(false);
            StartCoroutine(PlayStandUp());
        }

        private IEnumerator PlayStandUp()
        {
            Vector3 startPosition = _player.transform.position;
            Vector3 endPosition = _standingAfterSeat != null ? _standingAfterSeat.position : startPosition;
            var controller = _player.GetComponent<CharacterController>();
            // The authored seated capsule overlaps the mattress; prevent depenetration while
            // transferring to the clear standing marker, then restore the controller once.
            controller.enabled = false;
            if (_playerAnimator != null)
            {
                _playerAnimator.ResetTrigger("Stand");
                _playerAnimator.SetTrigger("Stand");
                yield return null;
                float duration = 1f;
                foreach (var clip in _playerAnimator.runtimeAnimatorController.animationClips)
                    if (clip.name == "Sit To Stand") { duration = Mathf.Max(duration, clip.length); break; }
                for (float elapsed = 0; elapsed < duration + .1f; elapsed += Time.deltaTime)
                {
                    float progress = Mathf.SmoothStep(0, 1, Mathf.InverseLerp(duration * .55f, duration, elapsed));
                    _player.transform.position = Vector3.Lerp(startPosition, endPosition, progress);
                    yield return null;
                }
                SetSitting(_playerAnimator, false);
                _playerAnimator.Play("Idle", 0);
            }
            _openingStandComplete = true;
            if (_standingAfterSeat != null)
                _player.transform.SetPositionAndRotation(_standingAfterSeat.position, _standingAfterSeat.rotation);
            controller.enabled = true;
            _standingUp = false;
            _player.PositionLocked = false;
            _camera.SetDialogueCue(null);
            SetStage(Stage.Hallway, "Tìm Mẹ Kiều");
            if (_gameplayHints != null) _gameplayHints.SetActive(true);
            SetLocked(false);
        }

        private void ResumeGate()
        {
            if (CurrentStage == Stage.Waiting) MaConversation = true;
            SetStage(Stage.Negotiation, "");
            _dialogue.ResumeInk();
        }

        public bool CanInspect(string flag) => CurrentStage == Stage.Explore && !DialogueActive
            && _dialogue.InkStory.variablesState[flag] is bool read && !read;

        public void InspectClue(string knot, string flag)
        {
            if (!CanInspect(flag)) return;
            SetStage(Stage.Negotiation, "");
            _dialogue.StartInkAt(knot);
        }

        private void RouteTags(string[] tags)
        {
            string emotion = "";
            bool greeting = false;
            foreach (string raw in tags)
            {
                int colon = raw.IndexOf(':');
                if (colon < 0) continue;
                string key = raw.Substring(0, colon).Trim(), value = raw.Substring(colon + 1).Trim();
                CueReceived?.Invoke(key, value);
                switch (key)
                {
                    case "scene": LogicalScene = value; break;
                    case "gameplay":
                        // Control is released by the matching gate after the dialogue view closes.
                        if (value != "enable_player") Debug.LogWarning("Unknown gameplay cue: " + value, this);
                        break;
                    case "objective":
                        // Gate objectives remain visible during gameplay, never over the conversation.
                        if (!DialogueActive) _objective.text = ObjectiveLabel(value);
                        break;
                    case "transition":
                        if (value == "short_time_pass") StartCoroutine(ShortTimePass());
                        else Debug.LogWarning("Unknown transition cue: " + value, this);
                        break;
                    case "speaker":
                        _speaker = value;
                        if (value == "Messenger" && _messenger != null)
                        {
                            _messenger.SetActive(true);
                            Vector3 direction = _player.transform.position - _messenger.transform.position; direction.y = 0;
                            if (direction.sqrMagnitude > .01f) _messenger.transform.rotation = Quaternion.LookRotation(direction);
                        }
                        break;
                    case "staging":
                        if (value == "ma_greeting") { if (_contractPresentation != null) _contractPresentation.Bow(); else greeting = true; break; }
                        if (value == "present_contract") { if (_contractPresentation != null) _contractPresentation.Present(); break; }
                        _reactionFocus = value == "look_at_door" ? _mainDoor.transform
                            : value == "look_at_mother" ? _mother : value == "look_at_ma" ? _maGiamSinh.transform : null;
                        if (_reactionFocus == null) { Debug.LogWarning("Unknown staging cue: " + value, this); break; }
                        float hold = 1f;
                        foreach (string cue in tags)
                            if (cue.StartsWith("wait:") && float.TryParse(cue.Substring(5), System.Globalization.NumberStyles.Float,
                                System.Globalization.CultureInfo.InvariantCulture, out float seconds)) hold = seconds;
                        _reactionUntil = Time.time + hold;
                        break;
                    case "emotion": emotion = value; break;
                    case "camera":
                        if (value == "Fade_Black") FadeTo(1, 0);
                        if (value == "FadeIn_KieuRoom") FadeTo(0, 1.5f);
                        break;
                    case "inventory":
                        if (value == "add:tram_gia_dinh")
                            _dialogue.InkStory.variablesState["c1_co_tram_gia_dinh"] = true;
                        else Debug.LogWarning("Unknown story inventory cue: " + value, this);
                        break;
                    case "chapter_end":
                        if (value == "1") _chapterEndRequested = true;
                        break;
                }
            }
            int gesture = EmotionGesture(emotion);
            SetAnimation(_player.GetComponentInChildren<Animator>(), _speaker == "Kieu", gesture);
            bool awaitingMother = CurrentStage == Stage.Opening || CurrentStage == Stage.Hallway || CurrentStage == Stage.Mother;
            SetAnimation(_motherAnimator, awaitingMother || _speaker == "MeKieu", awaitingMother ? 1 : gesture);
            if (!_maMoving) SetAnimation(_maAnimator, _speaker == "MaGiamSinh", greeting ? 3 : 0);
            if (_messenger != null) SetAnimation(_messenger.GetComponentInChildren<Animator>(), _speaker == "Messenger", 0);
            if (Array.Exists(tags, t => t.Trim() == "camera:Insert_Signing"))
            {
                SetAnimation(_player.GetComponentInChildren<Animator>(), false, 0);
                if (_contractPresentation != null) _contractPresentation.Sign();
            }
        }

        private static int EmotionGesture(string emotion)
        {
            if (emotion.Contains("crying") || emotion.StartsWith("broken")) return 1;
            if (emotion.Contains("annoyed") || emotion == "irritated_hidden") return 2;
            if (emotion == "satisfied" || emotion == "gently_smiling") return 3;
            if (emotion == "defensive" || emotion == "caught_off_guard") return 4;
            return 0;
        }

        private static string ObjectiveLabel(string value)
        {
            switch (value)
            {
                case "Tim_Me_Kieu": return "Tìm Mẹ Kiều";
                case "Tim_hieu_tinh_hinh_Vuong_Gia": return "Tìm hiểu tình hình Vương gia";
                case "Ra_mo_cua": return "Ra mở cửa";
                case "Cho_tin": return "Chờ tin";
                default: return "";
            }
        }

        private IEnumerator ShortTimePass()
        {
            FadeTo(.6f, .6f);
            yield return new WaitForSeconds(.8f);
            FadeTo(0, .8f);
        }

        private static void SetAnimation(Animator animator, bool active, int emotion)
        {
            if (animator == null || !animator.gameObject.activeInHierarchy) return;
            foreach (var parameter in animator.parameters)
            {
                if (parameter.name == "DialogueTalking") animator.SetBool(parameter.nameHash, active);
                if (parameter.name == "DialogueEmotion") animator.SetInteger(parameter.nameHash, active ? emotion : 0);
            }
        }

        private void WalkMa(Vector3 destination)
        {
            if (_maWalk != null) StopCoroutine(_maWalk);
            _maDestination = destination;
            _maWalk = StartCoroutine(MoveMa());
        }

        private IEnumerator MoveMa()
        {
            _maMoving = true;
            SetAnimation(_maAnimator, false, 0);
            // A clear route along the east side of the table and doorway.
            Vector3[] route = _maDestination.z > 0
                ? new[] { new Vector3(.8f, 0, -1.25f), new Vector3(2.4f, 0, -.5f), _maDestination }
                : new[] { new Vector3(2.4f, 0, -.5f), new Vector3(.8f, 0, -1.25f), _maDestination };
            foreach (var waypoint in route)
            {
                while (Vector3.Distance(_maGiamSinh.transform.position, waypoint) > .025f)
                {
                    var delta = waypoint - _maGiamSinh.transform.position;
                    // Do not push through the player; the objective asks them to return to the hall.
                    if (Vector3.Distance(_player.transform.position, _maGiamSinh.transform.position + delta.normalized * .6f) > .55f)
                    {
                        Face(_maGiamSinh.transform, waypoint, 160);
                        _maGiamSinh.transform.position = Vector3.MoveTowards(_maGiamSinh.transform.position, waypoint, 1.35f * Time.deltaTime);
                        if (_maAnimator != null) _maAnimator.SetFloat("Speed", .5f, .12f, Time.deltaTime);
                    }
                    else if (_maAnimator != null) _maAnimator.SetFloat("Speed", 0);
                    yield return null;
                }
            }
            if (_maAnimator != null) _maAnimator.SetFloat("Speed", 0);
            for (float elapsed = 0; elapsed < 1.5f; elapsed += Time.deltaTime)
            {
                Vector3 facing = _player.transform.position - _maGiamSinh.transform.position; facing.y = 0;
                if (facing.sqrMagnitude < .01f || Vector3.Angle(_maGiamSinh.transform.forward, facing) < 3) break;
                Face(_maGiamSinh.transform, _player.transform.position, 180);
                yield return null;
            }
            SetAnimation(_maAnimator, DialogueActive && _speaker == "MaGiamSinh", 0);
            _maMoving = false;
            _maWalk = null;
        }

        private static void Face(Transform actor, Vector3 position, float speed)
        {
            Vector3 direction = position - actor.position; direction.y = 0;
            if (direction.sqrMagnitude > .05f)
                actor.rotation = Quaternion.RotateTowards(actor.rotation, Quaternion.LookRotation(direction), speed * Time.deltaTime);
        }

        private void OnDialogueStarted(DialogueData data)
        {
            SetLocked(true); _objective.text = "";
            if (_gameplayHints != null) _gameplayHints.SetActive(false);
        }
        private void OnDialogueEnded()
        {
            _reactionFocus = null;
            MotherConversation = false;
            MaConversation = false;
            SetAnimation(_player.GetComponentInChildren<Animator>(), false, 0);
            bool awaitingMother = CurrentStage == Stage.Hallway || CurrentStage == Stage.Mother;
            SetAnimation(_motherAnimator, awaitingMother, awaitingMother ? 1 : 0);
            SetAnimation(_maAnimator, false, 0);
            if (CurrentStage != Stage.Hallway || _openingStandComplete) _camera.SetDialogueCue(null);
            if (_chapterEndRequested) { CompleteChapter(); return; }
            if (CurrentStage == Stage.Hallway && !_openingStandComplete)
            {
                if (_standUpButton != null) _standUpButton.gameObject.SetActive(true);
                return;
            }
            if (_gameplayHints != null) _gameplayHints.SetActive(true);
            SetLocked(CurrentStage == Stage.Complete);
        }

        private static void SetSitting(Animator animator, bool sitting)
        {
            if (animator == null) return;
            foreach (var parameter in animator.parameters)
                if (parameter.name == "IsSitting") animator.SetBool(parameter.nameHash, sitting);
            if (sitting)
            {
                int seatedState = Animator.StringToHash("Base Layer.SittingIdle");
                if (animator.HasState(0, seatedState)) animator.Play(seatedState, 0, 0);
            }
        }

        private void SetLocked(bool locked)
        {
            _player.ControlLocked = locked;
            _interaction.ControlLocked = locked;
            _camera.ControlLocked = locked;
            _camera.SetCursorLocked(!locked);
        }

        private void SetStage(Stage stage, string objective) { CurrentStage = stage; _objective.text = objective; }

        private void CompleteChapter()
        {
            SetStage(Stage.Complete, "CHƯƠNG 1 HOÀN THÀNH");
            InkStatePersistence.Instance.Save(_dialogue.InkStory);
            _completionPanel.SetActive(true);
            SetLocked(true);
        }

        public void ContinueAfterChapter()
        {
            if (CurrentStage != Stage.Complete || _transitioning) return;
            string next = Application.CanStreamedLevelBeLoaded("Chapter02_CaiGiaCuaMotConNguoi") ? "Chapter02_CaiGiaCuaMotConNguoi"
                : Application.CanStreamedLevelBeLoaded("Chapter02_Placeholder") ? "Chapter02_Placeholder" : null;
            if (next == null)
            {
                Debug.LogWarning("Chapter 2 is not configured. Chapter 1 is complete and its Ink state is saved.", this);
                return;
            }
            StartCoroutine(Transition(next));
        }

        private IEnumerator Transition(string scene)
        {
            _transitioning = true;
            _completionButton.interactable = false;
            _dialogue.InkStory.ChoosePathString("chapter_2");
            InkStatePersistence.Instance.Save(_dialogue.InkStory);
            _fade.transform.SetAsLastSibling();
            FadeTo(1, 1);
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(scene);
        }

        private void FadeTo(float alpha, float seconds)
        {
            if (_fadeRoutine != null) StopCoroutine(_fadeRoutine);
            _fadeRoutine = StartCoroutine(Fade(alpha, seconds));
        }

        private IEnumerator Fade(float alpha, float seconds)
        {
            float start = _fade.alpha;
            for (float elapsed = 0; elapsed < seconds; elapsed += Time.unscaledDeltaTime)
            {
                _fade.alpha = Mathf.Lerp(start, alpha, elapsed / seconds);
                yield return null;
            }
            _fade.alpha = alpha;
        }

        private void OnDestroy()
        {
            if (_dialogue == null) return;
            _dialogue.InkPauseBeforeLine = null;
            _dialogue.InkTagsChanged -= RouteTags;
            _dialogue.DialogueStarted -= OnDialogueStarted;
            _dialogue.DialogueEnded -= OnDialogueEnded;
            if (_completionButton != null) _completionButton.onClick.RemoveListener(ContinueAfterChapter);
            if (_standUpButton != null) _standUpButton.onClick.RemoveListener(StandUp);
        }
    }
}
