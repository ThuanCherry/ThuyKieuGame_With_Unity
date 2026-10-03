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

## Audio audit and hooks

The complete Assets filename scan and Unity AssetDatabase AudioClip scan found **zero audio clips**.
Consequently no music, rain recording, wind, door, paper, footstep or UI sound is currently played.
No audio was downloaded or synthesized. There is no voice acting.

`VuongGia/EnvironmentAudio` contains one scene-local Chapter01Audio component and six sources:

| Source | Purpose | Defaults |
| --- | --- | --- |
| MusicSource | Assign restrained sad/strings music | 2D loop, volume 0.18 |
| RainSource | Assign rain loop | 2D loop, 0.13 indoors / 0.28 outdoors |
| WindSource | Assign wind ambience | 2D loop, 0.025 indoors / 0.07 outdoors |
| SFXSource | Door open/close and contract paper | 3D, volume 0.4 |
| UISource | Continue and choice confirmation | 2D, volume 0.2 |
| FootstepSource | Wood/stone footsteps while grounded | 3D, volume 0.25 |

Music/rain/wind can be assigned to their source clip slots. Other clips are exposed on
Chapter01Audio. Hooks already follow Ink music/camera cues, door proximity, UI input, player
grounding and distance walked. Empty slots are silent. No global AudioManager was introduced.

## Files created

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
**43/43 checks** and the Chapter 1 regression test passed **100/100 checks** on this scene.
The completed regression run reached Chapter02_Placeholder. No runtime errors were reported.
No standalone player build was performed.

Results are recorded in `Tools/Chapter01/EnvironmentReport.json` and `PlayModeReport.json`.
The environment check traverses both NPC approaches, all sides of the table, the side area,
door, courtyard and gate using the actual Input System and CharacterController, then verifies
rain bounds, lighting cues and camera collision. The existing chapter check covers all six
choice branches, animation, dialogue locks, state persistence and Chapter 2 transition.

To repeat: enter Play Mode, then call `Chapter01EnvironmentCheck.Start()` using Editor tooling.
Start a fresh Play session for `Chapter01PlayModeCheck.Start()`.

## Remaining placeholders and limits

- All audio requires real clips; these are explicit empty hooks permitted by the environment brief.
- Main-hall roof, trim, screens, paving, lanterns and parchment use modular primitive geometry.
- Puddles suggest wetness; there is no water simulation or planar reflection.
- Imported environment models are relatively dense (roughly 0.3M vertices for many source props);
  source meshes were preserved. One shadow light and bounded particle density keep the demo modest.
- Chapter 2 remains the existing placeholder.

No file or folder was deleted; no package, source FBX, source prefab or Ink story was replaced.
