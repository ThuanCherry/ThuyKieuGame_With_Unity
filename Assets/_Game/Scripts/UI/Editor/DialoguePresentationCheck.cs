using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ThuyKieu.Dialogue;
using ThuyKieu.Environment;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ThuyKieu.UI.Editor
{
    public static class DialoguePresentationCheck
    {
        [Serializable] private class Report { public bool completed; public bool passed = true; public List<string> checks = new List<string>(); }
        private static Report _report;
        private static DialogueUI _ui;
        private static TMP_Text _body;
        private static int _phase, _sounds, _soundsAtSkip;
        private static float _start;
        private static bool _background, _played;
        private static float _letterPeak;
        private static float _outputRms;
        private static readonly float[] OutputSamples = new float[1024];
        public static bool Running { get; private set; }
        public static void Start()
        {
            if (!Application.isPlaying || Running) throw new InvalidOperationException("Start once in Play Mode.");
            _ui = UnityEngine.Object.FindAnyObjectByType<DialogueUI>();
            _body = _ui.GetComponentsInChildren<TMP_Text>(true).First(t => t.name == "DialogueBodyText");
            _report = new Report(); _phase = 0; _sounds = 0; _played = false;
            _letterPeak = 0;
            _outputRms = 0;
            _start = Time.realtimeSinceStartup; _background = Application.runInBackground;
            Application.runInBackground = true; Running = true;
            _ui.CharacterRevealed += HeardLetter;
            EditorApplication.update += Tick;
        }
        private static void HeardLetter()
        {
            _sounds++;
            var source = UnityEngine.Object.FindAnyObjectByType<Chapter01Audio>().GetComponentsInChildren<AudioSource>().First(s => s.name == "UISource");
            _played |= source.isPlaying;
        }
        private static void Check(bool ok, string text) { _report.passed &= ok; _report.checks.Add((ok ? "PASS: " : "FAIL: ") + text); }
        private static void Invoke(string name, params object[] args) => typeof(DialogueUI).GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_ui, args);
        private static void Tick()
        {
            try
            {
                if (!Application.isPlaying) { Check(false, "Play Mode interrupted"); Finish(); return; }
                float elapsed = Time.realtimeSinceStartup - _start;
                if (_phase == 1)
                {
                    var output = UnityEngine.Object.FindAnyObjectByType<Chapter01Audio>().GetComponentsInChildren<AudioSource>().First(s => s.name == "UISource");
                    output.GetOutputData(OutputSamples, 0);
                    float energy = 0;
                    foreach (float sample in OutputSamples) energy += sample * sample;
                    _outputRms = Mathf.Max(_outputRms, Mathf.Sqrt(energy / OutputSamples.Length));
                }
                if (elapsed > 15) throw new InvalidOperationException("Presentation check timed out");
                switch (_phase)
                {
                    case 0:
                        if (!DialogueManager.Instance.IsDialogueActive) return;
                        Invoke("HandleLineChanged", "Thúy Kiều", "Một lời xin gửi mẹ cha, con xin ghi nhớ nghĩa nhà hôm nay.");
                        Invoke("HandleChoicesChanged", new object[] { Array.Empty<DialogueChoice>() });
                        Check(_ui.IsRevealing && _body.maxVisibleCharacters == 0, "New line starts hidden");
                        _phase = 1; _start = Time.realtimeSinceStartup; break;
                    case 1:
                        if (elapsed < .4f) return;
                        Check(_body.maxVisibleCharacters > 0 && _ui.IsRevealing, "Vietnamese characters reveal progressively");
                        Check(_sounds > 0 && _sounds <= 9 && _played, "Letter sounds play with rate limiting");
                        var audio = new SerializedObject(UnityEngine.Object.FindAnyObjectByType<Chapter01Audio>());
                        var clip = (AudioClip)audio.FindProperty("_dialogueLetter").objectReferenceValue;
                        var samples = new float[clip.samples * clip.channels];
                        bool loaded = clip.GetData(samples, 0);
                        float volume = audio.FindProperty("_dialogueLetterVolume").floatValue;
                        var source = UnityEngine.Object.FindAnyObjectByType<Chapter01Audio>().GetComponentsInChildren<AudioSource>().First(s => s.name == "UISource");
                        _letterPeak = samples.Max(sample => Mathf.Abs(sample)) * volume * source.volume;
                        Check(loaded && _letterPeak > .1f && !source.mute, "Letter audio has clear signal level through unmuted 2D source");
                        Check(_outputRms > .005f, "Letter sound produces measured PCM output during reveal: RMS=" + _outputRms.ToString("F4"));
                        string before = _ui.CurrentText;
                        _ui.OnContinueClicked();
                        Check(!_ui.IsRevealing && _ui.CurrentText == before, "First continue reveals all without advancing");
                        _soundsAtSkip = _sounds;
                        var panel = _body.transform.parent.GetComponent<RectTransform>();
                        var color = panel.GetComponent<Image>().color;
                        Check(color.b > color.r && color.r < .1f && color.a >= .4f && color.a <= .7f && _body.color.r > .9f, "Translucent navy panel at 40-70 percent opacity with light text");
                        Check(panel.Find("BorderTop").GetComponent<Image>().color.r > .6f && !panel.GetComponent<Outline>().enabled && _body.fontSharedMaterial.IsKeywordEnabled("UNDERLAY_ON"), "Light edge border preserves transparency and TMP drop shadow enabled");
                        Check(panel.rect.height < 300, "Ordinary line uses compact panel");
                        Check(!_body.isTextOverflowing, "Vietnamese line fits panel");
                        ScreenCapture.CaptureScreenshot("Tools/Chapter01/DialogueCompact.png");
                        _phase = 2; _start = Time.realtimeSinceStartup; break;
                    case 2:
                        if (elapsed < .3f) return;
                        Check(_sounds == _soundsAtSkip, "Skipping reveal stops letter sounds");
                        _ui.OnContinueClicked();
                        Check(_ui.CurrentText != "Một lời xin gửi mẹ cha, con xin ghi nhớ nghĩa nhà hôm nay.", "Second continue advances dialogue");
                        Invoke("HandleLineChanged", "Thúy Kiều", "Con xin nghĩ cho trọn chữ hiếu. Dẫu đường phía trước còn nhiều gian khó, con vẫn mong cha mẹ được bình an.");
                        Invoke("HandleChoicesChanged", new object[] { new[] { new DialogueChoice("Con xin nhận lời để cứu cha."), new DialogueChoice("Xin cho con thêm chút thời gian suy nghĩ."), new DialogueChoice("Con muốn hỏi rõ những điều trong khế ước.") } });
                        _phase = 3; _start = Time.realtimeSinceStartup; break;
                    case 3:
                        if (elapsed < 3) return;
                        Canvas.ForceUpdateCanvases();
                        _body.ForceMeshUpdate();
                        Check(_ui.VisibleChoiceCount == 3 && !_body.isTextOverflowing, "Three choices and long line fit");
                        var labels = _ui.GetComponentsInChildren<Button>().Where(b => b.name.Contains("Clone")).Select(b => b.GetComponentInChildren<TMP_Text>()).ToArray();
                        Check(labels.Length == 3 && labels.All(t => !t.isTextOverflowing), "Choice labels remain readable");
                        ScreenCapture.CaptureScreenshot("Tools/Chapter01/DialogueNavyChoices.png");
                        _phase = 4; _start = Time.realtimeSinceStartup; break;
                    case 4:
                        if (elapsed < .3f) return;
                        Invoke("HandleDialogueEnded");
                        Check(!_ui.IsRevealing && !_ui.IsPanelVisible, "Closing dialogue stops reveal");
                        Finish(); break;
                }
            }
            catch (Exception e) { Check(false, e.ToString()); Finish(); }
        }
        private static void Finish()
        {
            Running = false; EditorApplication.update -= Tick;
            if (_ui != null) _ui.CharacterRevealed -= HeardLetter;
            Application.runInBackground = _background;
            _report.completed = true;
            File.WriteAllText("Tools/Chapter01/DialoguePresentationReport.json", JsonUtility.ToJson(_report, true));
        }
    }
}
