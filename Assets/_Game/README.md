# Thuy Kieu - Core Project

Foundation only. Quest, dialogue, UI and final art are intentionally NOT implemented here.

## Folders

| Folder | Owner / content |
| --- | --- |
| `Scenes/` | `TestScene.unity` - sandbox to validate core systems |
| `Scripts/Core/` | Shared, gameplay-agnostic code (`InputReader`) |
| `Scripts/Player/` | Movement, interaction driver, third person camera |
| `Scripts/Interaction/` | `IInteractable` contract + test implementation |
| `Scripts/Dialogue/` | EMPTY - dialogue team |
| `Scripts/Quest/` | EMPTY - quest team |
| `Scripts/UI/` | EMPTY - UI team |
| `Prefabs/` | `Player/Player.prefab`, interaction prefabs |
| `Characters/ Environment/ Animations/ Materials/ Audio/ UI/` | Art buckets |

## Rules

1. One responsibility per class. `PlayerMovement` never talks to UI, `PlayerInteraction`
   never knows what an interactable does.
2. New interactables implement `ThuyKieu.Interaction.IInteractable` (optionally
   `IInteractableDisplayName` for a prompt). Never add NPC/item/door branches inside
   `PlayerInteraction`.
3. Read input through `ThuyKieu.Core.InputReader`, not through `Keyboard.current` directly.
   The project uses the new Input System only (old `Input.GetAxis` is disabled).
4. Serialize references with `[SerializeField]`; use `GameObject.Find` only as a fallback.

## Controls

WASD move, Left Shift run, mouse look, E interact, Esc release/lock cursor.
