# Chapter 1 — Vương gia environment

Scene: `Assets/_Game/Scenes/Chapter01_GiaBien.unity`.
Open it and press Play. Existing Ink, dialogue choices, player animations and progression remain in use.
Controls: WASD, Shift, mouse, E; Space or buttons for dialogue.

## Environment and assets

`VuongGia` organizes MainHouse/MainHall, SideArea, MainDoor, Courtyard, MainGate,
EnvironmentLighting and EnvironmentAudio. Player and NPC objects retain their existing identities.

Reused prefabs from `Assets/_Game/Prefabs`:

- Architecture: KieuHouse_Exterior, DoorLeaf, Fence.
- Props: TableSet (including benches and tea set), Altar, Cabinet, Bed, CeramicJar, OilLamp, Well.
- Vegetation: Bamboo, ArecaPalm, BananaTree.

The imported house is a closed exterior mesh with a box collider covering the entire building.
It is used as an adjacent family wing. The accessible main hall retains the proven floor/wall
collision layout, with timber columns, planks, wainscot, lattice windows, decorative screen,
lanterns, a ceiling and a modular pitched tile roof. Roof channels share a combined render mesh.
The gate reuses the same roof module, existing doors and fence assets.

The existing ContractTable interaction remains attached to its original object; its blockout
renderer and legs are disabled. TableSet provides the visible furniture. The parchment has ink
lines and a red seal, positioned on the clear front area of the table. The main door opens on
approach and returns closed after the player leaves its proximity. Door frames retain collision;
moving leaves are visual so they cannot trap the player.

Primitive colliders cover floors, walls, columns, table, screen, major props and map bounds.
Small trim, paving overlays, paper and lamps do not obstruct movement.

## Locations (world coordinates, metres)

| Object | Position |
| --- | --- |
| PlayerSpawn / Thúy Kiều | (0, 0.05, -1.8) |
| Mẹ Kiều | (-2.5, 0, 1.5) |
| Mã Giám Sinh | (2.5, 0, 0.5), revealed by existing Ink flow |
| ContractTable interaction | (0, 0.78, 3.1) |
| Parchment | (-0.22, 0.781, 2.4) |
| Main doorway | (0, 0, -2.5) |
| Existing departure interaction | (0, 0, -3.4) |
| Main gate | Around (0, 0, -13) |

## Lighting and VFX

- Moonlight: cool blue-gray directional light, soft shadows, intensity 0.5.
- OilLamp: warm hall illumination; ContractLamp: warm local table focus.
- EntranceGlow, PorchGlow, GateGlow and CourtyardMoonBounce: local unshadowed fill.
- Only the moon casts realtime shadows. Existing scene textures are at most 2048 pixels.
- Opening is restrained; Mã's entrance raises local light; the contract cue raises the table
  light; the ending retains the cold exterior. Changes interpolate through existing Ink cues.
- The existing CourtyardRain particle system is reused, capped at 650 particles, emitting
  550/second with short lifetimes. Its emitter stays beyond the porch, with slight lateral tilt.
  Particles die on environment collision. No drops are emitted inside the house.
- Stone paving, simple smooth dark puddle surfaces and distance fog suggest wet ground.

The existing third-person camera and dialogue presets remain. The wide shot now sits inside
the covered hall. Dialogue camera movement also checks environment obstruction, including
player-relative close-up/ending presets.

## Audio implementation

The initial environment pass contained no audio clips. The audio pass now adds nine original
locally synthesized WAV clips under `Assets/_Game/Audio/Chapter01`, assigned and saved in the
Chapter 1 scene. No audio was downloaded and there is no voice acting.
These are designed effects and a synthesized bowed pad, rather than recorded instruments or ambience.

`VuongGia/EnvironmentAudio` contains one scene-local Chapter01Audio component and six sources:

| Source | Purpose | Defaults |
| --- | --- | --- |
| MusicSource | SadStrings: restrained D-minor pad and melody | 2D loop, volume 0.18 |
| RainSource | Rain: diffuse rain and roof droplets | 2D loop, 0.13 indoors / 0.28 outdoors |
| WindSource | Wind: filtered gust ambience | 2D loop, 0.025 indoors / 0.07 outdoors |
| SFXSource | Door open/close and contract paper | 3D, volume 0.4 |
| UISource | Continue and choice confirmation | 2D, volume 0.2 |
| FootstepSource | Wood/stone footsteps while grounded | 3D, volume 0.25 |

Music/rain/wind are assigned to their source clip slots. Paper, DoorOpen, DoorClose,
UIConfirm, WoodStep and StoneStep are assigned on Chapter01Audio. Hooks follow Ink music/camera
cues, door proximity, UI input, player grounding and distance walked. Repeated music cues
preserve playback instead of restarting the loop. No global AudioManager was introduced.

`Tools/Chapter01/GenerateAudio.py` reproduces the 44.1 kHz / 16-bit mono WAV files with NumPy.
No Python dependency is needed by Unity or the player. `Chapter01AudioSetup` and the menu
`ThuyKieu/Chapter 1/Assign audio` restore assignments without rebuilding the environment.
The environment setup also retains these assignments when reapplied.

### Compact dialogue presentation

Dialogue uses a dark navy background with opacity 0.62, separate bright edge borders,
light text and TMP SDF underlay drop shadows. Separate borders avoid compositing copies
of the whole translucent panel as Unity's Image Outline would do.
The panel occupies 76% of the screen width and sizes its height to the line and choices.
Text reveals at 48 characters/second. Space/Enter or Continue first completes a revealing
line, then advances on the next press. Choice buttons and number shortcuts remain available.
A tenth clip, `DialogueLetter.wav`, plays through UISource for letters/digits,
limited to one tick per 55 ms. Spaces and punctuation are silent; skipping or closing a
line stops the reveal. The tick is 50 ms at 880 Hz, peak 0.8, with `_dialogueLetterVolume`
set to 0.95 (Inspector adjustable). This raises the effective peak from 0.0225 to 0.152
with the existing UISource volume 0.2, making it clearer above the ambience.
`Chapter01DialogueStyle` supplies the saved scene style and setup defaults;
`Assets/_Game/UI/Materials/DialogueTextShadow.mat` supplies the shadow without editing the font material.
Presentation checks and previews: `Tools/Chapter01/DialoguePresentationReport.json`,
`DialogueCompact.png`, `DialogueNavyChoices.png`.
The presentation check passed 15/15 cases covering Vietnamese reveal, letter audio,
skip/advance behavior, three choices, text bounds and closing the panel.
The audio check measured UISource PCM output during reveal (peak sampled RMS 0.0500),
as well as validating the clip and source gain. Physical speaker/headphone playback is not captured.

## Files created

- `Assets/_Game/Audio/Chapter01/`: nine WAV clips and provenance/reproduction README.
- `Assets/_Game/Scripts/Environment/Editor/Chapter01AudioSetup.cs`
- `Tools/Chapter01/GenerateAudio.py`, `ApplyAudio.cs`, `CheckAudio.cs`,
  `AudioVerificationState.cs`, `AudioReport.json`.
- `Assets/_Game/Scripts/Environment/Chapter01Atmosphere.cs`
- `Assets/_Game/Scripts/Environment/Chapter01Audio.cs`
- `Assets/_Game/Scripts/Environment/Editor/Chapter01EnvironmentSetup.cs`
- `Assets/_Game/Scripts/Environment/Editor/Chapter01EnvironmentCheck.cs`
- `Assets/_Game/Scripts/Interaction/ProximityDoor.cs`
- `Assets/_Game/Prefabs/Architecture/Chapter01Modules/VuongGiaRoof.prefab`
- `Assets/_Game/Prefabs/Architecture/Chapter01Modules/VuongGiaRoofMesh.asset`
- `Assets/_Game/Materials/Chapter01Environment/`: DarkWood, WoodTrim, WetStone, RoofTile,
  WarmPaper, LanternGlow, LimePlaster, Ink, SealRed, Puddles.
- `Tools/Chapter01/`: AuditEnvironment.cs, PreviewEnvironmentAssets.cs, ApplyEnvironment.cs,
  RenderEnvironment.cs, InspectEnvironmentDetails.cs, StartEnvironmentCheck.cs;
  audit/test JSON and environment/asset preview PNGs.
- This report and Unity-generated `.meta` files for new assets.

## Files modified

- `Assets/_Game/Scenes/Chapter01_GiaBien.unity`
- `Assets/_Game/Materials/Chapter01Rain.mat`
- `Assets/_Game/Scripts/Dialogue/DialogueUI.cs`: optional confirmation event shared by mouse/keyboard.
- `Assets/_Game/Scripts/Player/ThirdPersonCameraController.cs`: dialogue camera obstruction checks.
- `Assets/_Game/Scripts/Core/Editor/Chapter01PlayModeCheck.cs`: permit environment audio while checking no voice sources.
- `docx/codex/CHAPTER1_IMPLEMENTATION.md`: link to this environment pass.

## Verification

Unity recompilation passed with zero errors and warnings. The environment test passed
**45/45 checks** and the Chapter 1 regression test passed **103/103 checks** after the audio pass.
The dedicated audio check passed **9/9 checks**, covering the listener, three loops, door opening
and closing, UI confirmation and the contract paper cue. Environment traversal additionally
confirmed wood and stone footsteps playing through the actual player movement system.
All nine PCM files were checked for nonzero audio samples. Playback checks verify Unity state;
perceived sound quality still needs listening on the user's speakers/headphones.
The completed regression run reached Chapter02_Placeholder. No runtime errors were reported.
No standalone player build was performed.

Results are recorded in `Tools/Chapter01/AudioReport.json`, `EnvironmentReport.json` and `PlayModeReport.json`.
The environment check traverses both NPC approaches, all sides of the table, the side area,
door, courtyard and gate using the actual Input System and CharacterController, then verifies
rain bounds, lighting cues and camera collision. The existing chapter check covers all six
choice branches, animation, dialogue locks, state persistence and Chapter 2 transition.

To repeat: enter Play Mode, then call `Chapter01EnvironmentCheck.Start()` using Editor tooling.
Start a fresh Play session for `Chapter01PlayModeCheck.Start()`.

## Remaining placeholders and limits

- Audio uses original synthesized assets; recorded instruments, field ambience and voice acting are not included.
- Main-hall roof, trim, screens, paving, lanterns and parchment use modular primitive geometry.
- Puddles suggest wetness; there is no water simulation or planar reflection.
- Imported environment models are relatively dense (roughly 0.3M vertices for many source props);
  source meshes were preserved. One shadow light and bounded particle density keep the demo modest.
- Chapter 2 remains the existing placeholder.

No file or folder was deleted; no package, source FBX, source prefab or Ink story was replaced.
