using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

namespace ThuyKieu.Dialogue
{
    /// <summary>
    /// Dialogue presentation, progressive text reveal and input forwarding.
    /// </summary>
    public class DialogueUI : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject dialoguePanel;

        [Header("Texts")]
        [SerializeField] private Text npcNameText;
        [SerializeField] private Text dialogueText;
        [SerializeField] private TMP_Text _speakerText;
        [SerializeField] private TMP_Text _bodyText;

        [Header("Choices")]
        [Tooltip("Parent of the choice buttons. Buttons are pooled from choiceButtonPrefab.")]
        [SerializeField] private RectTransform choicesRoot;
        [SerializeField] private Button choiceButtonPrefab;

        [Header("Continue")]
        [SerializeField] private Button continueButton;

        [Header("Input")]
        [Tooltip("Also advance with Space / Enter and pick choices with number keys.")]
        [SerializeField] private bool allowKeyboardShortcuts = true;
        [SerializeField, Min(1)] private float _charactersPerSecond = 48f;
        private float _revealProgress;
        private int _characterCount;
        private float _nextLetterSound;
        public bool IsRevealing => _bodyText != null && _bodyText.maxVisibleCharacters < _characterCount;
        public event System.Action CharacterRevealed;

        private readonly List<Button> spawnedChoices = new List<Button>();
        private DialogueManager manager;
        public event System.Action InteractionConfirmed;

        private void Start()
        {
            manager = DialogueManager.Instance;

            if (manager == null)
            {
                Debug.LogWarning("[DialogueUI] No DialogueManager found in the scene.", this);
            }
            else
            {
                manager.DialogueStarted += HandleDialogueStarted;
                manager.LineChanged += HandleLineChanged;
                manager.ChoicesChanged += HandleChoicesChanged;
                manager.DialogueEnded += HandleDialogueEnded;
                manager.PresentationCueChanged += HandlePresentationCue;
            }

            if (continueButton != null)
            {
                continueButton.onClick.AddListener(OnContinueClicked);
            }

            SetPanelVisible(false);
        }

        private void OnDestroy()
        {
            if (manager != null)
            {
                manager.DialogueStarted -= HandleDialogueStarted;
                manager.LineChanged -= HandleLineChanged;
                manager.ChoicesChanged -= HandleChoicesChanged;
                manager.DialogueEnded -= HandleDialogueEnded;
                manager.PresentationCueChanged -= HandlePresentationCue;
            }

            if (continueButton != null)
            {
                continueButton.onClick.RemoveListener(OnContinueClicked);
            }
        }

        private void Update()
        {
            RevealCharacters();
            if (!allowKeyboardShortcuts || manager == null || !manager.IsDialogueActive)
            {
                return;
            }

            Keyboard keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return;
            }

            if (manager.IsAtChoicePoint)
            {
                // 1..9 pick a branch.
                for (int i = 0; i < spawnedChoices.Count && i < 9; i++)
                {
                    if (keyboard[Key.Digit1 + i].wasPressedThisFrame)
                    {
                        OnChoiceClicked(i);
                        return;
                    }
                }

                return;
            }

            if (keyboard.spaceKey.wasPressedThisFrame || keyboard.enterKey.wasPressedThisFrame)
            {
                OnContinueClicked();
            }
        }

        /// <summary>Hooked to the Continue button.</summary>
        public void OnContinueClicked()
        {
            if (IsRevealing)
            {
                CompleteReveal();
                return;
            }
            if (manager != null && manager.IsDialogueActive && !manager.IsAtChoicePoint)
            {
                InteractionConfirmed?.Invoke();
                manager.Advance();
            }
        }

        /// <summary>Hooked to a choice button. Also callable from tests.</summary>
        public void OnChoiceClicked(int index)
        {
            if (manager != null && manager.IsAtChoicePoint && index >= 0 && index < spawnedChoices.Count)
            {
                CompleteReveal();
                InteractionConfirmed?.Invoke();
                manager.SelectChoice(index);
            }
        }

        public bool IsPanelVisible => dialoguePanel != null && dialoguePanel.activeSelf;

        public string CurrentName => _speakerText != null ? _speakerText.text : npcNameText != null ? npcNameText.text : string.Empty;

        public string CurrentText => _bodyText != null ? _bodyText.text : dialogueText != null ? dialogueText.text : string.Empty;

        public int VisibleChoiceCount => spawnedChoices.Count;

        private void HandleDialogueStarted(DialogueData data)
        {
            SetPanelVisible(false);
        }

        private void HandlePresentationCue(bool active)
        {
            if (active) SetPanelVisible(false);
        }

        private void HandleLineChanged(string speaker, string text)
        {
            SetPanelVisible(true);
            if (_speakerText != null) _speakerText.text = speaker;
            if (_bodyText != null) _bodyText.text = text;
            if (_bodyText != null)
            {
                _bodyText.maxVisibleCharacters = int.MaxValue;
                _bodyText.ForceMeshUpdate();
                _characterCount = _bodyText.textInfo.characterCount;
                _bodyText.maxVisibleCharacters = 0;
                _revealProgress = 0;
                _nextLetterSound = 0;
            }
            if (npcNameText != null)
            {
                npcNameText.text = speaker;
            }

            if (dialogueText != null)
            {
                dialogueText.text = text;
            }
        }

        private void HandleChoicesChanged(DialogueChoice[] choices)
        {
            ClearChoices();

            if (choices == null || choices.Length == 0)
            {
                UpdatePanelLayout(0);
                if (continueButton != null)
                {
                    continueButton.gameObject.SetActive(manager != null && manager.IsDialogueActive);
                }

                return;
            }

            SetPanelVisible(true);

            if (continueButton != null)
            {
                continueButton.gameObject.SetActive(false);
            }

            if (choicesRoot == null || choiceButtonPrefab == null)
            {
                Debug.LogWarning("[DialogueUI] Choices root or button prefab missing.", this);
                return;
            }

            for (int i = 0; i < choices.Length; i++)
            {
                int index = i;
                Button button = Instantiate(choiceButtonPrefab, choicesRoot);
                button.gameObject.SetActive(true);
                button.onClick.AddListener(delegate { OnChoiceClicked(index); });

                Text label = button.GetComponentInChildren<Text>();
                TMP_Text tmpLabel = button.GetComponentInChildren<TMP_Text>();
                if (tmpLabel != null) tmpLabel.text = (i + 1) + ". " + choices[i].ChoiceText;
                if (label != null)
                {
                    label.text = (i + 1) + ". " + choices[i].ChoiceText;
                }

                spawnedChoices.Add(button);
            }
            UpdatePanelLayout(spawnedChoices.Count);
        }

        private void HandleDialogueEnded()
        {
            CompleteReveal();
            ClearChoices();
            SetPanelVisible(false);
        }

        private void ClearChoices()
        {
            for (int i = 0; i < spawnedChoices.Count; i++)
            {
                if (spawnedChoices[i] != null)
                {
                    spawnedChoices[i].gameObject.SetActive(false);
                    Destroy(spawnedChoices[i].gameObject);
                }
            }

            spawnedChoices.Clear();
        }

        private void SetPanelVisible(bool visible)
        {
            if (dialoguePanel != null)
            {
                dialoguePanel.SetActive(visible);
            }

            if (continueButton != null && visible)
            {
                continueButton.gameObject.SetActive(true);
            }
        }

        private void CompleteReveal()
        {
            if (_bodyText != null) _bodyText.maxVisibleCharacters = int.MaxValue;
        }

        private void OnDisable() => CompleteReveal();

        private void RevealCharacters()
        {
            if (!IsPanelVisible || !IsRevealing) return;
            int previous = _bodyText.maxVisibleCharacters;
            _revealProgress += Time.unscaledDeltaTime * _charactersPerSecond;
            int visible = Mathf.Min(_characterCount, Mathf.FloorToInt(_revealProgress));
            _bodyText.maxVisibleCharacters = visible;
            for (int i = previous; i < visible; i++)
            {
                if (!char.IsLetterOrDigit(_bodyText.textInfo.characterInfo[i].character)) continue;
                if (Time.unscaledTime >= _nextLetterSound)
                {
                    CharacterRevealed?.Invoke();
                    _nextLetterSound = Time.unscaledTime + .055f;
                }
                break;
            }
        }

        private void UpdatePanelLayout(int choiceCount)
        {
            if (_bodyText == null || dialoguePanel == null || choicesRoot == null) return;
            Canvas.ForceUpdateCanvases();
            var panel = dialoguePanel.GetComponent<RectTransform>();
            float choiceHeight = choiceCount > 0 ? (choiceCount - 1) * 8f : 42f;
            foreach (var button in spawnedChoices)
            {
                var label = button.GetComponentInChildren<TMP_Text>();
                if (label == null) continue;
                label.textWrappingMode = TextWrappingModes.Normal;
                float height = Mathf.Max(52, label.GetPreferredValues(label.text, Mathf.Max(100, panel.rect.width - 88), 0).y + 20);
                var layout = button.GetComponent<LayoutElement>();
                if (layout == null) layout = button.gameObject.AddComponent<LayoutElement>();
                layout.minHeight = height;
                layout.preferredHeight = height;
                choiceHeight += height;
            }
            float bodyHeight = Mathf.Max(64f, _bodyText.GetPreferredValues(_bodyText.text, Mathf.Max(100, panel.rect.width - 56), 0).y + 12);
            panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 64f + bodyHeight + choiceHeight + 36f);
            _bodyText.rectTransform.anchorMin = new Vector2(0, 1);
            _bodyText.rectTransform.anchorMax = Vector2.one;
            _bodyText.rectTransform.pivot = new Vector2(.5f, 1);
            _bodyText.rectTransform.offsetMin = new Vector2(28, -60 - bodyHeight);
            _bodyText.rectTransform.offsetMax = new Vector2(-28, -60);
            choicesRoot.anchorMin = Vector2.zero;
            choicesRoot.anchorMax = new Vector2(1, 0);
            choicesRoot.offsetMin = new Vector2(28, 20);
            choicesRoot.offsetMax = new Vector2(-28, 20 + choiceHeight);
        }

        private Vector2 _lastCanvasSize;
        private void LateUpdate()
        {
            if (!IsPanelVisible) return;
            var canvas = GetComponentInParent<Canvas>();
            if (canvas == null) return;
            Vector2 size = canvas.GetComponent<RectTransform>().rect.size;
            if (size == _lastCanvasSize) return;
            _lastCanvasSize = size;
            UpdatePanelLayout(spawnedChoices.Count);
        }
    }
}
