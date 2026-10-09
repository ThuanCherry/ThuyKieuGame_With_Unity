# Chapter 1 rework — implementation and verification

Status: implemented and verified, with the supported asset fallbacks below.
This report supersedes the earlier partial bedroom/source-missing report.

## Scene and story

Playable scene: Assets/_Game/Scenes/Chapter01/Chapter01_GiaBien.unity.
It is first in Build Settings. Scene paths were repaired through their existing GUIDs.

The newly supplied narrative is at Assets/_Game/Data/Dialogue/Ink/Chapter1_GiaBien.ink.
Main.ink remains the shared entry point; Main.json was recompiled. The originally
requested Assets/Ink paths do not exist. No Chapter 2–4 narrative was rewritten.

The authored Chapter 1 dialogue, four probing branches, three contract branches,
emotional decision, Messenger report and farewell are retained. Technical changes:

- Explicit gate tags pause the existing DialogueManager at physical gameplay boundaries.
- Exploration choice bodies became callable clue knots, preserving their text and flags.
- Guards prevent repeated clue knots from incrementing c1_clues twice.
- Empty/tag-only Ink output now dispatches inventory/end cues and can pause at a gate.
- Chapter end uses DONE; Unity owns the scene load and chooses chapter_2 after saving.

## Gameplay and environment

The full path runs in one scene: black/rain opening, bedroom fade, walk to hallway,
Mother conversation, two of three clues, return to Mother, knock, E at MainDoor,
Ma reveal, walk back into the hall, four probing choices, E at contract table,
three contract choices, objection/signing, eight-second waiting gate, off-screen
Messenger, hairpin/farewell, walk into the rain, ending and Chapter Complete.

Movement stays on PlayerMovement, Input System and CharacterController.
The player is never teleported by Chapter01Director. Dialogue locks movement,
interaction and camera input. Door/story/clue prompts appear through the existing
single-target interaction system. The third clue remains available after two.

KieuRoom reuses the existing bed/side area and house shell. Added partitions, open
bedroom door, small table, oil lamp, stool, chest, decorative folded cloth and
PlayerStart. Its doorway connects to Hallway and MainHall. Existing floor/roof and
new simple furniture/wall colliders retain the camera collision layer.

DebtPaper, MoneyBag and JewelryBox sit around the family table. FallenChair,
BrokenCup pieces and muddy footprints communicate the search without blocking
the primary route. MainDoor is ajar but gated until the explicit opening interaction.
Ma starts inactive under the porch, appears at the door, and walks along the east
aisle to the hall. His path waits if the player stands in front of him.
Vuong Ong and Vuong Quan are never spawned.

No reusable prefab was replaced or modified. Existing bed, lamp, table and other
environment prefabs remain reused as scene instances; new simple props are scene objects.

## Presentation and state

- Camera: a small shared set maps all Chapter 1 camera tags; scene tags only update
  logical scene state. Collision-safe camera smoothing and cuts across blocked
  partitions prevent the lens getting stuck between rooms.
- Animation: existing Humanoid controllers supply Idle/Walk/Talk/Crying/gestures.
  Mother uses a standing distressed fallback. Signing uses an interaction gesture,
  a paper close-up and the authored narration.
- Lighting/VFX: warm bedroom lamp, warm hall/cool exterior, existing bounded
  courtyard rain; no interior rain emitter or extra shadow-casting lamp.
- Audio: existing rain/wind/music/paper/door/footstep clips reused. DoorKnock.wav
  is an original locally synthesized clip with a standard-library reproduction script.
  Single/double knocks follow Ink. Bedroom rain and letter ticks are quieter;
  low/end music tags are supported.
- UI: existing Canvas and DialogueUI retained. All scene TMP text uses
  Assets/Fonts/VietnameseTMP.asset with its compatible material, including E prompts.
  Choice buttons measure wrapped text instead of assuming a fixed one-line height.
  Full Ink paragraphs remain intact. Contract clauses use the readable dialogue UI.
- State: one Ink Story contains all global/Chapter 1 variables. The hairpin is
  c1_co_tram_gia_dinh; inventory:add:tram_gia_dinh safely updates it. Existing
  InkStatePersistence saves the complete JSON state and survives scene loading.
- Chapter 2: configured Chapter02_Placeholder loads through the director after fade.
  Its narrative resume point is chapter_2 and the saved state retains choices/items.
  A real Chapter02_CaiGiaCuaMotConNguoi scene is preferred if later configured.
  If neither scene exists, Chapter Complete remains open safely.

## Important scripts

Created under Assets/_Game/Scripts:

- Core/Editor/Chapter01InkReworkCheck.cs
- Core/Editor/Chapter01ReworkPlayCheck.cs
- Environment/Editor/Chapter01ReworkSetup.cs
- Environment/Editor/KieuBedroomSetup.cs
- Environment/Editor/KieuBedroomPlayCheck.cs
- Interaction/ChapterClueInteractable.cs
- UI/Editor/Chapter01ReworkUICheck.cs

Modified:

- Core/Chapter01Director.cs — gate/staging/wait/ending orchestration.
- Core/Editor/Chapter01Setup.cs — corrected scene path.
- Dialogue/DialogueManager.cs — knot entry, tag-only handling, Messenger display name.
- Dialogue/DialogueTagRouter.cs — safe cue routing/fallbacks.
- Dialogue/DialogueUI.cs — multiline choice layout and resize handling.
- Environment/Chapter01Audio.cs — new cues, mix and missing-cue fallback.
- Interaction/ChapterInteractable.cs — stage-aware interaction availability.
- Interaction/ProximityDoor.cs — story-controlled opening and portal collider.
- Player/ThirdPersonCameraController.cs — collision-safe camera placement.
- UI/InteractionPromptUI.cs — optional TMP presentation.
- Legacy Chapter01PlayModeCheck, Chapter01EnvironmentCheck and
  DialoguePresentationCheck entry points now invoke the current rework suites.

Tools/Chapter01 contains apply/compile/inspection scripts, generated reports,
screenshots, the dialogue test corpus and GenerateKnock.py.

## Verification

- Unity recompile: zero errors and zero compiler warnings.
- Ink: 36 combinations (three clue pairs × four probes × three contract choices).
  74 assertions pass, including expected stat/flag changes, payment condition,
  optional third clue, duplicate-clue protection and state restore into Chapter 2.
- Full Play Mode path: 47 assertions pass using actual WASD/E Input System input,
  existing movement/collisions, real UI choices and the actual scene transition.
  Includes model/avatar references, opening camera clearance, rain confinement,
  bounded shadow lights, gameplay gates and hairpin persistence.
- UI: all 221 distinct authored lines plus choice groups tested in actual GameView
  resolutions 1920×1080, 1600×900, 1366×768 and 2560×1440. 24 assertions pass for
  text bounds, screen bounds, Vietnamese glyphs and non-overlapping wrapped choices.
- No runtime errors captured. One expected warning identifies the unavailable
  woman_crying_distant clip.
- Source whitespace checks pass. Unity-generated scene YAML retains its normal
  blank-field trailing spaces; it was saved through Editor tooling, not hand-edited.
- No full standalone player build was requested or performed.

Reports: Tools/Chapter01/ReworkInkReport.json, ReworkPlayReport.json, ReworkUIReport.json.
The full-path test accelerates dialogue clicks; its measured duration is not a
measurement of normal player reading time.

## Supported fallbacks and limits

- No suitable recorded crying clip exists; the hook warns once and continues with
  rain/narration. Messenger reports from the doorway off-screen, without a new model.
- Exact bed-rise, seated staging and pen-signing animations are not required;
  standing beside the bed, distressed/talk poses and an interaction gesture are used.
- Some furniture, document/bag/jewelry and search traces use primitive placeholders.
- Chapter 2 remains its existing placeholder; no playable Chapter 2 was built.
- The room/scene visual style is a functional polish pass using existing assets,
  not a replacement environment asset pack.

No manual Unity Editor setup step is required. Open the saved Chapter01_GiaBien
scene and press Play. Controls: WASD, mouse, E; Space/Enter or buttons for dialogue.
