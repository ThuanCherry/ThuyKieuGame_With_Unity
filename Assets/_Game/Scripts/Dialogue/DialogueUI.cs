using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ThuyKieu.Dialogue
{
    /// <summary>
    /// Functional placeholder view for DialogueManager. Intentionally plain uGUI:
    /// Developer 6 can rebuild the visuals and only has to keep the serialized fields
    /// and the three public methods below wired.
    /// </summary>
    public class DialogueUI : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject dialoguePanel;

        [Header("Texts")]
        [SerializeField] private Text npcNameText;
        [SerializeField] private Text dialogueText;

        [Header("Choices")]
        [Tooltip("Parent of the choice buttons. Buttons are pooled from choiceButtonPrefab.")]
        [SerializeField] private RectTransform choicesRoot;
        [SerializeField] private Button choiceButtonPrefab;

        [Header("Continue")]
        [SerializeField] private Button continueButton;

        [Header("Input")]
        [Tooltip("Also advance with Space / Enter and pick choices with number keys.")]
        [SerializeField] private bool allowKeyboardShortcuts = true;

        private readonly List<Button> spawnedChoices = new List<Button>();
        private DialogueManager manager;

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
            }

            if (continueButton != null)
            {
                continueButton.onClick.RemoveListener(OnContinueClicked);
            }
        }

        private void Update()
        {
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
                        manager.SelectChoice(i);
                        return;
                    }
                }

                return;
            }

            if (keyboard.spaceKey.wasPressedThisFrame || keyboard.enterKey.wasPressedThisFrame)
            {
                manager.Advance();
            }
        }

        /// <summary>Hooked to the Continue button.</summary>
        public void OnContinueClicked()
        {
            if (manager != null)
            {
                manager.Advance();
            }
        }

        /// <summary>Hooked to a choice button. Also callable from tests.</summary>
        public void OnChoiceClicked(int index)
        {
            if (manager != null)
            {
                manager.SelectChoice(index);
            }
        }

        public bool IsPanelVisible => dialoguePanel != null && dialoguePanel.activeSelf;

        public string CurrentName => npcNameText != null ? npcNameText.text : string.Empty;

        public string CurrentText => dialogueText != null ? dialogueText.text : string.Empty;

        public int VisibleChoiceCount => spawnedChoices.Count;

        private void HandleDialogueStarted(DialogueData data)
        {
            SetPanelVisible(true);
        }

        private void HandleLineChanged(string speaker, string text)
        {
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
                if (continueButton != null)
                {
                    continueButton.gameObject.SetActive(manager != null && manager.IsDialogueActive);
                }

                return;
            }

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
                if (label != null)
                {
                    label.text = (i + 1) + ". " + choices[i].ChoiceText;
                }

                spawnedChoices.Add(button);
            }
        }

        private void HandleDialogueEnded()
        {
            ClearChoices();
            SetPanelVisible(false);
        }

        private void ClearChoices()
        {
            for (int i = 0; i < spawnedChoices.Count; i++)
            {
                if (spawnedChoices[i] != null)
                {
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
    }
}
