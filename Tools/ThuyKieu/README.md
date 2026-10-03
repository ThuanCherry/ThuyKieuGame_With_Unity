# Thuy Kieu playable setup

Open `Assets/_Game/Scenes/ThuyKieu_Playground.unity` and press Play. Click the Game view if keyboard focus is elsewhere.

- WASD: camera-relative walking (2.5 m/s).
- Left Shift + WASD: running (5.5 m/s).
- Mouse: existing third-person orbit camera.
- Escape: release/lock cursor.
- C: reserved; no crouch animation FBX exists in this project yet.
- E: reserved for the existing interaction system; no automatic pickup binding was added.

Prefab: `Assets/_Game/Prefabs/Player/ThuyKieu.prefab`.
Movement: existing `Assets/_Game/Scripts/Player/PlayerMovement.cs`.
Controller: `Assets/_Game/Animations/ThuyKieu/ThuyKieu.controller`.
Material: `Assets/_Game/Art/Characters/ThuyKieu/Materials/ThuyKieu_URP.mat`, using the existing colour texture and normal map.

Speed thresholds are 0 / 0.5 / 1 for TK-Idle / TK-Walking / TK-Running. Movement measures actual CharacterController displacement. The normalization setting is enabled on the new prefab and leaves other controllers' existing speed units compatible. Root motion is disabled. FBX animation importers copy the rigged model's valid Humanoid avatar; locomotion roots are baked and looping is enabled. Pickup is not looping. Talk/Pickup triggers select existing clips. Look Around, Sitting Idle and Sitting Talking are available as controller states for later gameplay integration.

Editor tools are under `Assets/_Game/Scripts/Player/Editor/`. They only run explicitly. Setup refuses to replace an existing controller, prefab or scene. To repeat integration checks, enter Play Mode in the playground and choose `ThuyKieu > Player > Validate playground in Play Mode`. This injects keyboard state temporarily, restores input/camera settings afterwards and writes `Tools/ThuyKieu/Validation.json`.

The pre-existing unsaved Editor scene was preserved as `Assets/_Game/Scenes/Dev_ThuyKieuEditorBackup.unity`. Existing home/CoreTest/dev scenes have missing player prefab references from before this work; they have not been replaced. Use the new playground for the verified playable character. No files were deleted, no packages were changed, and no full player build was required.
