using System;
using System.Collections;
using ThuyKieu.Dialogue;
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
        public enum Stage { Opening, Mother, Negotiation, Contract, Exit, Complete }
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
        public Stage CurrentStage { get; private set; }
        public bool DialogueActive => _dialogue.IsDialogueActive;
        public event Action<string, string> CueReceived;

        private IEnumerator Start()
        {
            _dialogue.SetInkStory(new Ink.Runtime.Story(_compiledInk.text));
            _dialogue.InkPauseBeforeLine = PauseBeforeLine;
            _dialogue.InkStopBeforeLine = tags => Array.Exists(tags, t => t.Trim() == "chapter:2");
            _dialogue.InkTagsChanged += RouteTags;
            _dialogue.DialogueStarted += OnDialogueStarted;
            _dialogue.DialogueEnded += OnDialogueEnded;
            _completionButton.onClick.AddListener(ContinueAfterChapter);
            _completionPanel.SetActive(false);
            _maGiamSinh.SetActive(false);
            SetLocked(true);
            yield return null; // Let existing dialogue views subscribe before opening.
            for (float t = 0; t < 1; t += Time.deltaTime)
            {
                _fade.alpha = 1 - t;
                yield return null;
            }
            _fade.alpha = 0;
            _fade.blocksRaycasts = false;
            _dialogue.ResumeInk();
        }

        private bool PauseBeforeLine(string[] tags)
        {
            if (CurrentStage == Stage.Opening && Array.Exists(tags, t => t == "speaker:MeKieu"))
            { SetStage(Stage.Mother, "Đến bên Mẹ Kiều · E để nói chuyện"); return true; }
            if (CurrentStage == Stage.Negotiation && Array.Exists(tags, t => t == "scene:VuongGia_BanKyKhe"))
            { SetStage(Stage.Contract, "Đến bàn ký khế · E để xem tờ khế"); return true; }
            if (CurrentStage == Stage.Contract && Array.Exists(tags, t => t == "camera:Hero_Kieu_Rain"))
            { SetStage(Stage.Exit, "Đến cửa chính · E để lên đường"); return true; }
            return false;
        }

        public void Interact(Stage stage, Transform target)
        {
            if (stage != CurrentStage || DialogueActive) return;
            Vector3 direction = target.position - _player.transform.position;
            direction.y = 0;
            if (direction.sqrMagnitude > 0.01f) _player.transform.rotation = Quaternion.LookRotation(direction);
            if (stage == Stage.Mother) SetStage(Stage.Negotiation, "Trao đổi với Mã Giám Sinh · chọn câu trả lời");
            _dialogue.ResumeInk(stage == Stage.Exit ? CompleteChapter : (Action)null);
        }

        private void RouteTags(string[] tags)
        {
            var playerAnimator = _player.GetComponentInChildren<Animator>();
            string speaker = "Narrator";
            string emotion = string.Empty;
            foreach (string tag in tags)
            {
                int colon = tag.IndexOf(':');
                if (colon < 0) continue;
                string key = tag.Substring(0, colon).Trim();
                string value = tag.Substring(colon + 1).Trim();
                CueReceived?.Invoke(key, value);
                if (key == "speaker") speaker = value;
                if (key == "emotion") emotion = value;
            }
            if (speaker == "MaGiamSinh") _maGiamSinh.SetActive(true);
            int gesture = EmotionGesture(emotion);
            SetAnimation(playerAnimator, speaker == "Kieu", gesture);
            SetAnimation(_motherAnimator, speaker == "MeKieu", gesture);
            SetAnimation(_maAnimator, speaker == "MaGiamSinh", gesture);
            if (Array.Exists(tags, tag => tag.Trim() == "camera:POV_Kieu_MaHands")) SetAnimation(playerAnimator, true, 5);
            if (Array.Exists(tags, tag => tag.Trim() == "camera:Table_Contract")) SetAnimation(playerAnimator, true, 6);
        }

        private static int EmotionGesture(string emotion)
        {
            switch (emotion)
            {
                case "crying": return 1;
                case "irritated_hidden": case "annoyed": case "cold": return 2;
                case "satisfied": return 3;
                case "defensive": case "caught_off_guard": case "determined": return 4;
                case "thoughtful": return 5;
                default: return 0;
            }
        }

        private static void SetAnimation(Animator animator, bool active, int emotion)
        {
            if (animator == null || !animator.gameObject.activeInHierarchy) return;
            animator.SetBool("DialogueTalking", active);
            // Keep compatibility with earlier Chapter 1 controllers that only expose DialogueTalking.
            foreach (var parameter in animator.parameters)
                if (parameter.name == "DialogueEmotion") { animator.SetInteger("DialogueEmotion", active ? emotion : 0); break; }
        }

        private void OnDialogueStarted(DialogueData data) { SetLocked(true); }
        private void OnDialogueEnded()
        {
            var animator = _player.GetComponentInChildren<Animator>();
            SetAnimation(animator, false, 0);
            SetAnimation(_motherAnimator, false, 0);
            _camera.SetDialogueCue(null);
            SetAnimation(_maAnimator, false, 0);
            SetLocked(CurrentStage == Stage.Complete);
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
            if (Application.CanStreamedLevelBeLoaded("Chapter02_CaiGiaCuaMotConNguoi"))
                SceneManager.LoadScene("Chapter02_CaiGiaCuaMotConNguoi");
            else SceneManager.LoadScene("Chapter02_Placeholder");
            Debug.Log("Chapter 1 Complete");
        }
        private void OnDestroy()
        {
            if (_dialogue == null) return;
            _dialogue.InkPauseBeforeLine = null;
            _dialogue.InkStopBeforeLine = null;
            _dialogue.InkTagsChanged -= RouteTags;
            _dialogue.DialogueStarted -= OnDialogueStarted;
            _dialogue.DialogueEnded -= OnDialogueEnded;
        }
    }
}
