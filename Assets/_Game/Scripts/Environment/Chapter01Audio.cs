using System.Collections;
using System.Collections.Generic;
using ThuyKieu.Core;
using ThuyKieu.Dialogue;
using ThuyKieu.Player;
using UnityEngine;

namespace ThuyKieu.Environment
{
    // Scene-local presentation; audio follows existing narrative and interaction cues.
    public sealed class Chapter01Audio : MonoBehaviour
    {
        [SerializeField] private Chapter01Director _director;
        [SerializeField] private DialogueUI _dialogueUI;
        [SerializeField] private PlayerMovement _player;
        [SerializeField] private AudioSource _musicSource, _rainSource, _windSource, _sfxSource, _uiSource, _footstepSource;
        [SerializeField] private AudioSource _cryingSource;
        [SerializeField, Range(0, 1)] private float _cryingBedroomVolume = .18f;
        [SerializeField, Range(0, 1)] private float _cryingHallVolume = .30f;
        private Coroutine _cryingFade;
        private float _cryingFadeGain = 1;
        [SerializeField] private AudioSource _thunderSource;
        [SerializeField] private Transform _cryingAnchor;
        [SerializeField] private AudioClip _paper, _doorOpen, _doorClose, _uiConfirm, _woodStep, _stoneStep;
        [SerializeField] private AudioClip _knock, _crying;
        [SerializeField] private Transform _knockAnchor;
        [SerializeField] private AudioSource _knockSource;
        private float _knockDuckUntil;
        private Coroutine _knockRoutine;
        public int KnockHitsPlayed { get; private set; }
        private readonly HashSet<string> _missingCues = new HashSet<string>();
        private float _stepDistance;
        private float _musicVolume = .18f;
        private CharacterController _controller;
        private Coroutine _thunderRoutine;
        private AudioClip _thunderClip;
        [SerializeField] private AudioClip _dialogueLetter;
        [SerializeField, Range(0f, 1f)] private float _dialogueLetterVolume = .95f;

        private void OnEnable()
        {
            if (_director != null) _director.CueReceived += OnCue;
            if (_dialogueUI != null) _dialogueUI.InteractionConfirmed += PlayUI;
            if (_dialogueUI != null) _dialogueUI.CharacterRevealed += PlayLetter;
        }
        private void Start()
        {
            if (_player != null) _controller = _player.GetComponent<CharacterController>();
            StartLoop(_musicSource); StartLoop(_rainSource); StartLoop(_windSource);
            if (_thunderSource != null) _thunderRoutine = StartCoroutine(ThunderAmbience());
        }
        private void OnDisable()
        {
            if (_director != null) _director.CueReceived -= OnCue;
            if (_dialogueUI != null) _dialogueUI.InteractionConfirmed -= PlayUI;
            if (_dialogueUI != null) _dialogueUI.CharacterRevealed -= PlayLetter;
            foreach (var source in new[]{_musicSource, _rainSource, _windSource, _cryingSource}) if (source != null) source.Stop();
            if (_cryingFade != null) StopCoroutine(_cryingFade);
            _cryingFade = null;
            _cryingFadeGain = 1;
            if (_thunderRoutine != null) StopCoroutine(_thunderRoutine);
            if (_knockRoutine != null) StopCoroutine(_knockRoutine);
            _knockRoutine = null;
            if (_knockSource != null) _knockSource.Stop();
        }
        private static void StartLoop(AudioSource source)
        { if (source != null && source.clip != null && !source.isPlaying) { source.loop = true; source.Play(); } }
        private void OnCue(string key, string value)
        {
            if (key == "music")
            {
                if (value == "sad_strings" || value == "sad_strings_low" || value == "sad_strings_end")
                { _musicVolume = value == "sad_strings_low" ? .08f : value == "sad_strings_end" ? .14f : .16f; StartLoop(_musicSource); }
                else Missing(value);
            }
            if (key == "sfx")
            {
                switch (value)
                {
                    case "rain_muffled": case "rain_room": case "rain_exterior": StartLoop(_rainSource); break;
                    case "door_knock": BeginKnock(1); break;
                    case "door_knock_double": BeginKnock(2); break;
                    case "door_open": break; // The actual door interaction supplies the sound.
                    case "woman_crying_distant":
                        StartCrying();
                        break;
                    case "footsteps_outside": PlayAt(_stoneStep, new Vector3(0, 0, -4)); break;
                    default: Missing(value); break;
                }
            }
            if (key != "camera") return;
            if (value == "Reveal_MaGiamSinh") _musicVolume = .21f;
            if (value == "Table_Contract") { _musicVolume = .23f; PlayAt(_paper, new Vector3(0, .85f, 3.1f)); }
            if (value == "Insert_Signing") PlayAt(_paper, new Vector3(0, .85f, 3.1f));
            if (value == "Hero_Kieu_Rain") _musicVolume = .16f;
        }
        private void Update()
        {
            if (_player == null) return;
            bool inside = Mathf.Abs(_player.transform.position.x) < 6.3f && _player.transform.position.z > -3.2f;
            bool bedroom = _player.transform.position.x < -2.1f && _player.transform.position.z > 2.5f;
            float blend = 1 - Mathf.Exp(-3 * Time.deltaTime);
            float musicVolume = _cryingSource != null && _cryingSource.isPlaying ? _musicVolume * .55f : _musicVolume;
            if (Time.time < _knockDuckUntil) musicVolume *= .3f;
            if (_musicSource != null) _musicSource.volume = Mathf.Lerp(_musicSource.volume, musicVolume, blend);
            if (_rainSource != null) _rainSource.volume = Mathf.Lerp(_rainSource.volume, bedroom ? .035f : inside ? .065f : .16f, blend);
            if (_windSource != null) _windSource.volume = Mathf.Lerp(_windSource.volume, inside ? .01f : .03f, blend);
            if (_cryingSource != null)
            {
                if (_cryingAnchor != null) _cryingSource.transform.position = _cryingAnchor.position + Vector3.up * 1.25f;
                float cryingVolume = bedroom ? _cryingBedroomVolume : inside ? _cryingHallVolume : _cryingBedroomVolume;
                _cryingSource.volume = Mathf.Lerp(_cryingSource.volume, cryingVolume * _cryingFadeGain, blend);
                if (_cryingFade != null) _cryingSource.volume = cryingVolume * _cryingFadeGain;
            }
            if (_player.ControlLocked || _controller == null || !_controller.isGrounded || _player.CurrentSpeed < .1f)
            { _stepDistance = 0; return; }
            _stepDistance += _player.CurrentSpeed * Time.deltaTime;
            if (_stepDistance < (_player.IsRunning ? 1.6f : 1.05f)) return;
            _stepDistance = 0;
            AudioClip step = inside ? _woodStep : _stoneStep;
            if (step != null && _footstepSource != null)
            { _footstepSource.transform.position = _player.transform.position; _footstepSource.PlayOneShot(step); }
        }
        public void PlayDoor(bool open, Vector3 position) { PlayAt(open ? _doorOpen : _doorClose, position); }
        public void StopCrying()
        {
            if (_cryingSource == null || !_cryingSource.isPlaying) return;
            if (_cryingFade == null) _cryingFade = StartCoroutine(FadeOutCrying());
        }
        private void StartCrying()
        {
            if (_crying == null) { Missing("woman_crying_distant"); return; }
            if (_cryingSource == null) { PlayAt(_crying, _cryingAnchor != null ? _cryingAnchor.position : Vector3.zero); return; }
            if (_cryingFade != null) StopCoroutine(_cryingFade);
            _cryingFade = null;
            _cryingFadeGain = 1;
            _cryingSource.clip = _crying;
            _cryingSource.loop = _crying.length > 3f;
            _cryingSource.spatialBlend = 1f;
            _cryingSource.minDistance = 3f;
            _cryingSource.maxDistance = 18f;
            _cryingSource.dopplerLevel = 0;
            if (!_cryingSource.isPlaying) _cryingSource.Play();
        }
        private IEnumerator FadeOutCrying()
        {
            for (float elapsed = 0; elapsed < .65f; elapsed += Time.deltaTime)
            {
                _cryingFadeGain = 1 - elapsed / .65f;
                yield return null;
            }
            _cryingSource.volume = 0;
            _cryingSource.Stop();
            _cryingFade = null;
            _cryingFadeGain = 1;
        }
        private IEnumerator Knock(int count)
        {
            if (_knock == null || _knockSource == null) { Missing("door_knock"); yield break; }
            _knockSource.transform.position = _knockAnchor != null ? _knockAnchor.position + Vector3.up * 1.2f : new Vector3(0, 1.2f, -2.5f);
            for (int i = 0; i < count; i++)
            {
                _knockDuckUntil = Time.time + .75f;
                // Duck immediately: a short transient can finish before the normal music fade.
                if (_musicSource != null) _musicSource.volume = Mathf.Min(_musicSource.volume, _musicVolume * .3f);
                _knockSource.PlayOneShot(_knock);
                KnockHitsPlayed++;
                yield return new WaitForSeconds(.35f);
            }
            _knockRoutine = null;
        }
        private void BeginKnock(int count)
        {
            if (_knockRoutine != null) return;
            _knockRoutine = StartCoroutine(Knock(count));
        }
        private IEnumerator ThunderAmbience()
        {
            yield return new WaitForSeconds(Random.Range(5f, 9f));
            while (enabled)
            {
                PlayThunder();
                yield return new WaitForSeconds(Random.Range(19f, 34f));
            }
        }
        private void PlayThunder()
        {
            if (_thunderSource == null) return;
            if (_thunderClip == null) _thunderClip = CreateThunderClip();
            _thunderSource.PlayOneShot(_thunderClip, .16f);
        }
        private static AudioClip CreateThunderClip()
        {
            const int sampleRate = 22050;
            const float duration = 3.8f;
            var samples = new float[(int)(sampleRate * duration)];
            float seed = Random.value * 1000f;
            for (int i = 0; i < samples.Length; i++)
            {
                float time = i / (float)sampleRate;
                float attack = Mathf.Clamp01(time / .18f);
                float rumble = Mathf.Exp(-time * .72f);
                float noise = Mathf.PerlinNoise(seed + time * 11f, .42f) * 2f - 1f;
                float lowTone = Mathf.Sin(time * 18f) * .32f + Mathf.Sin(time * 31f) * .16f;
                samples[i] = (noise * .58f + lowTone) * attack * rumble;
            }
            var clip = AudioClip.Create("ProceduralNightThunder", samples.Length, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }
        private void Missing(string cue)
        { if (_missingCues.Add(cue)) Debug.LogWarning("Chapter 1 audio cue unavailable: " + cue + ". Continuing with ambience and Ink narration.", this); }
        private void PlayAt(AudioClip clip, Vector3 position)
        { if (clip != null && _sfxSource != null) { _sfxSource.transform.position = position; _sfxSource.PlayOneShot(clip); } }
        private void PlayUI() { if (_uiConfirm != null && _uiSource != null) _uiSource.PlayOneShot(_uiConfirm); }
        private void PlayLetter() { if (_dialogueLetter != null && _uiSource != null) _uiSource.PlayOneShot(_dialogueLetter, _dialogueLetterVolume); }
    }
}
