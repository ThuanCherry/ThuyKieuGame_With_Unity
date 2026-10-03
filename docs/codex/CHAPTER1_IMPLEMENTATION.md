# Chapter 1 — playable implementation

Open `Assets/_Game/Scenes/Chapter01_GiaBien.unity` and press Play.

Controls: WASD movement, Left Shift run, mouse orbit camera, E interaction,
Space or the Tiếp button to advance dialogue. Click choices or press 1–3.
Escape releases/relocks the cursor outside dialogue.

Flow: fade/opening → walk to Mother → E → Mã Giám Sinh reveal and three choices
→ walk to contract table → E and two choices → decision dialogue → walk to front door
→ E and final narration → CHƯƠNG 1 HOÀN THÀNH → Tiếp tục → Chapter 2 placeholder.

## Assets and reused systems

- Player: existing `Assets/_Game/Prefabs/Player/ThuyKieu.prefab` instantiated once;
  valid Humanoid avatar, CharacterController, existing PlayerMovement and third-person camera.
- Existing DialogueManager, DialogueUI, PlayerInteraction, InputReader and InteractionPromptUI
  were extended/reused. Their original dialogue-data APIs remain available.
- UI variant: `Assets/_Game/Prefabs/UI/Chapter01DialogueCanvas.prefab`.
  TMP speaker/body/choice text uses `Assets/_Game/UI/Fonts/ChapterVietnamese.asset`.
- Ink source: `Assets/_Game/Data/Dialogue/Ink/Main.ink`, including all four original chapters.
  No story text was rewritten. Compiled asset: `Main.json` in the same directory.
- Official Ink 1.2.1 source and MIT license: `Assets/ThirdParty/Ink`.
  Editor-only compiler adapter: `Scripts/Dialogue/Editor/InkStoryCompiler.cs`.
- Base Animator for the three Chapter 1 actors: `Assets/_Game/Animations/Shared/Chapter01Shared.controller`.
  All 14 newly downloaded Shared clips are imported as Humanoid, with actor-specific avatars retained.
  Thúy Kiều's scene instance AND existing player prefab use `ThuyKieuLocomotion.overrideController`
  under `Animations/ThuyKieu`, retaining Shared Idle and dialogue gestures while overriding
  Walking/Running with the existing `TK-Walking` / `TK-Running` clips. These have a more compact
  gait and closer arm carriage on her rig. Movement is 2.1 m/s walking and 4.8 m/s running,
  with 0.13 s animation damping and 540 degrees/s turning. NPCs retain the Shared controller.
  Root motion is disabled; locomotion translation is baked into pose for CharacterController movement.
  Original controllers and animation files are preserved.
- Camera tags map to eight transform presets. Unknown tags are safe; emotion has an event hook.
- Chapter01Director owns progression; ChapterInteractable implements the existing interaction
  contract. InkStatePersistence keeps state across scenes and writes
  `Chapter1.inkstate.json` under `Application.persistentDataPath` on completion.
  Restoring the saved JSON into a new Story allows continuing at Chapter 2.
- The house/courtyard now have a furnished modular hall, tiled roof, automatic main doors,
  courtyard, gate, existing environment prefabs and story lighting. See
  [CHAPTER1_ENVIRONMENT_IMPLEMENTATION.md](CHAPTER1_ENVIRONMENT_IMPLEMENTATION.md)
  for the environment audit, locations, audio placeholders and verification.
- Mã Giám Sinh now uses `Art/Characters/MaGiamSinh/Models/MaGiamSinh_Rigged.fbx`
  with a valid Humanoid avatar and the Shared controller.
  Shared animation clips are retargeted to this avatar. Talking follows the
  MaGiamSinh speaker tag and returns to Idle for other speakers and when dialogue closes.
  Root motion is disabled. The original static visual is preserved but disabled in the scene.
- Chapter 2 scene is a placeholder, as permitted by the Chapter 1 prompt.

## Shared animation cues

The Chapter 1 dialogue uses Crying for `crying`, Arguing for `irritated_hidden`, `annoyed`,
and `cold`, Agreeing for `satisfied`, Disagree for `defensive`, `caught_off_guard` and
`determined`, and Looking Around for `thoughtful` and the observation camera cue.
The contract-table cue plays Picking Up Object on Kiều. Unknown emotions use Talking.
One-shot gestures return to neutral Talking; ending the conversation resets to Idle.

Sitting Idle/Talking and Stand To Sit/Sit To Stand are also configured in the controller
and verified in Play Mode. The existing Chapter 1 story has no seated scene, so those
clips are available without forcing a new sitting sequence into its dialogue.
For a seated interaction, set `IsSitting=true` and trigger `Sit`; trigger `Stand` to return.

## Audit and verification

The requested documents actually live in `docx/codex`, and the Ink files in the canonical
`Assets/_Game/Data/Dialogue/Ink` directory. Those files and AGENTS.md were read before edits.
Unity 6000.6.0f1, URP 17.6.0, Input System 1.20.0 were verified locally.
Ink runtime/integration was absent before this change; the official runtime/compiler were added.

`Tools/Chapter01/SceneAudit.json` records all ten scenes in the game and sample scene directories.
Chapter 1 contains exactly one player, one camera, one dialogue manager and no missing scripts.
Existing unrelated scenes CoreTest, KieuHome_Interior, KieuHome_Exterior and Dev_PlayerMovement
have Missing Prefab roots caused by pre-existing deleted player assets. Those scenes/assets
were preserved, and are not required by Chapter 1.

Unity recompilation passed. `Tools/Chapter01/PlayModeReport.json` records 100 passing checks,
including real Input System W/Shift/E/Space, animation speed, floor/wall collision, movement
lock/unlock, all 3×2 choice paths, variable values, Vietnamese glyphs, one player throughout,
final narration, state persistence and the Chapter 2 scene transition. The checks also verify
all 14 Shared Humanoid clips, Kiều's new Idle, dialogue gestures and seated transitions.
No runtime exceptions
were observed during the completed check. No standalone player build was performed.

To repeat the check: enter Play Mode in Chapter 1, then invoke
`ThuyKieu.Core.Editor.Chapter01PlayModeCheck.Start()` through Editor tooling.
The check controls a temporary virtual keyboard and writes its report outside Assets.

Known import/compiler warnings: some existing character meshes have no source normals
(Unity recalculates them); the unused upstream Ink compiler plugin loader can emit UAC0020.
They do not block Chapter 1. Setup-time errors from importing missing TMP Essentials and a
fixed camera compile typo were resolved before the passing Play Mode check.

No files/folders were deleted, no package versions changed, and no Git commit was made.
