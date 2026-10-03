# AGENTS.md

## Project

Unity game project: **ThuyKieu**

- Unity: `6000.6.0f1`
- Language: C#
- Render Pipeline: URP `17.6.0`
- Input System: `1.20.0`
- AI Navigation: `2.0.14`
- Timeline: `6.6.0`
- Unity Test Framework: `1.8.0`
- Unity Pipeline: `0.7.0-exp.1`
- Unity MCP package is installed
- Primary game content root: `Assets/_Game`

Do not assume a package or framework is installed unless it exists in `Packages/manifest.json`.

In particular, do not assume Ink is installed unless it is added to the project.

---

# 1. Primary Project Structure

Treat this as the canonical game structure:

```text
Assets/_Game/
├── Animations/
├── Art/
├── Audio/
├── Characters/
├── Data/
│   ├── Dialogue/
│   └── Quest/
├── Environment/
├── Materials/
├── Prefabs/
│   ├── Architecture/
│   ├── Interaction/
│   ├── NPC/
│   ├── Player/
│   ├── Props/
│   ├── UI/
│   └── Vegetation/
├── Scenes/
├── Scripts/
│   ├── Core/
│   ├── Dialogue/
│   ├── Interaction/
│   ├── Player/
│   ├── Quest/
│   └── UI/
└── UI/
```

All new gameplay C# code should normally go under:

```text
Assets/_Game/Scripts/
```

Choose the narrowest appropriate module.

Examples:

```text
GameManager       -> Scripts/Core/
ChapterManager    -> Scripts/Core/
PlayerController  -> Scripts/Player/
DialogueManager   -> Scripts/Dialogue/
Interactable      -> Scripts/Interaction/
QuestManager      -> Scripts/Quest/
UIManager         -> Scripts/UI/
```

Do not create another competing scripts architecture unless explicitly requested.

---

# 2. Codex Context / Token Efficiency

Minimize repository scanning and context usage.

## Before editing

Do NOT recursively read the entire project.

Start from the smallest relevant scope.

For C# tasks, search primarily inside:

```text
Assets/_Game/Scripts
```

Use targeted search such as:

```powershell
rg "ClassName|MethodName" Assets/_Game/Scripts
rg --files Assets/_Game/Scripts
```

Read only files relevant to the requested feature and their direct dependencies.

Do not repeatedly reopen unchanged files.

Do not repeat project structure, package lists, or previously established facts in reasoning/output unless necessary.

For small tasks, do not produce a large implementation plan. Inspect, modify, verify, and report.

For large cross-system changes, first identify the affected modules and keep the plan concise.

## Avoid expensive/unnecessary inspection

Do not scan these directories unless the task specifically requires them:

```text
Library/
Temp/
Logs/
obj/
UserSettings/
Assets/TutorialInfo/
Assets/_Recovery/
```

Do not inspect large art directories for programming tasks:

```text
Assets/_Game/Art/
Assets/_Game/Audio/
Assets/_Game/Materials/
Assets/_Game/Environment/
Assets/_Game/Animations/
```

Do not read binary/model/texture assets such as:

```text
.fbx
.png
.jpg
.wav
.mp3
.psd
.blend
```

unless the task explicitly concerns those assets.

Do not inspect `.meta` files unless investigating GUID, missing-reference, import, or asset-move problems.

Do not dump entire `.unity` scenes or `.prefab` YAML files into context unless absolutely necessary. Prefer Unity-aware tools when inspecting or changing Scene/Prefab state.

When checking Git changes, prefer targeted output:

```powershell
git status --short
git diff --stat
git diff -- <relevant-file>
```

Avoid dumping a huge repository-wide diff unless required.

---

# 3. Unity CLI Skill

A global `unity-cli` Codex skill is installed.

Use that skill when a task requires Unity CLI.

Do NOT duplicate or reload the entire Unity CLI documentation into project context unless required to solve a specific problem.

Prefer discovering exact command syntax through the installed skill or:

```powershell
unity <command> --help
```

instead of searching broadly.

---

# 4. Coding Rules

Use C# for gameplay systems.

Naming:

```text
Classes / Structs / Enums / Methods / Properties -> PascalCase
Local variables / parameters                     -> camelCase
Private fields                                   -> _camelCase
Interfaces                                       -> IName
```

Prefer:

```csharp
[SerializeField] private GameObject _target;
```

over exposing fields publicly only for Inspector access.

Keep one primary class per `.cs` file and match filename to class name.

Prefer small classes with clear responsibilities.

Avoid creating a global manager for every feature.

Avoid unnecessary static state and singletons.

Do not introduce new dependencies when existing Unity/project functionality is sufficient.

Do not rewrite unrelated code while implementing a feature.

Preserve existing public APIs unless changing them is necessary.

When changing an API, update its known usages.

Do not leave dead code, commented-out experimental implementations, or debug spam after completing a task.

Use comments for intent or non-obvious behavior, not to narrate simple code.

---

# 5. Architecture Responsibilities

Use these boundaries unless the existing implementation establishes otherwise.

## Core

Cross-game orchestration and state.

Examples:

```text
GameManager
ChapterManager
GameState
Save/Load coordination
```

Core should not contain feature-specific UI implementation.

## Player

Player movement, player input, player state, and player-facing gameplay behavior.

Use Unity Input System rather than introducing a second input architecture.

## Interaction

Reusable interaction contracts and interaction logic.

Prefer interfaces/components so NPCs, props, doors, quest objects, etc. can share interaction behavior.

## Dialogue

Dialogue flow and dialogue runtime state.

Keep dialogue data separate from presentation where practical.

Dialogue data belongs under:

```text
Assets/_Game/Data/Dialogue/
```

Do not add an Ink dependency unless Ink is actually installed or the user explicitly requests installation.

## Quest

Quest state, objectives, progression, and quest-related domain logic.

Quest data belongs under:

```text
Assets/_Game/Data/Quest/
```

## UI

UI presentation and input forwarding.

Do not place core game rules directly inside UI components when the logic belongs to another system.

---

# 6. Scene / Prefab / Asset Safety

C# source files may be created and edited without additional approval when required by the task.

Creating normal project folders under `Assets/_Game` is allowed when necessary.

Ask before:

- deleting any file or folder;
- deleting or replacing a Scene;
- deleting or replacing a Prefab;
- destructive bulk asset moves;
- changing many serialized assets automatically;
- removing packages;
- upgrading/downgrading Unity packages;
- changing Unity Editor version;
- destructive Git commands.

Do not manually edit Scene or Prefab YAML for ordinary tasks if Unity Editor / Unity CLI / Unity-aware tooling can perform the change more safely.

When moving Unity assets, preserve their `.meta` files so GUID references are not broken.

Never manually modify generated content in:

```text
Library/
Temp/
Logs/
obj/
```

---

# 7. Unity MCP vs Unity CLI

Use the right tool for the job.

Use normal file editing for C# source code.

Use **Unity CLI** for tasks such as:

- compile verification;
- tests;
- builds;
- Unity project diagnostics;
- Editor automation supported by Unity Pipeline.

Use **Unity-aware Editor/MCP tooling** when a task specifically requires inspecting or manipulating live Unity Editor state, GameObjects, Components, Scenes, or serialized Editor objects.

Do not invoke MCP/Editor automation for a simple C# edit when file editing plus compilation is sufficient.

This reduces unnecessary tool calls and context.

---

# 8. Verification Workflow

Do not claim a C# task is complete without appropriate verification.

For normal C# changes:

```text
edit
→ inspect changed files
→ Unity recompile
→ fix compile errors caused by the change
→ run relevant tests when applicable
→ inspect git diff
→ report concise result
```

When a Unity Editor instance is available, prefer:

```powershell
unity recompile
```

after C# changes rather than performing a full game build.

Use stricter compilation only when warnings are relevant to the task.

Run tests when:

- tests already exist for the affected system;
- behavior is non-trivial;
- the task explicitly asks for tests;
- the change risks regression.

Prefer affected/relevant tests instead of unnecessarily running every test for tiny changes.

Do NOT perform a full player build after every C# edit.

Run `unity build` only when:

- the user requests a build;
- build behavior is part of the task;
- release/CI validation requires it;
- compile/test checks are insufficient for the change.

If Unity CLI cannot reach the Editor, report that clearly instead of pretending verification succeeded.

---

# 9. Error Handling

When compilation/test fails:

1. Read the actual error.
2. Locate the specific affected file.
3. Fix the smallest root cause.
4. Re-run the failed verification.
5. Avoid unrelated refactors.

Do not repeatedly run the same failing command without changing anything unless retry is justified by an Editor/connection startup condition.

Do not hide remaining Unity errors.

Distinguish:

```text
error caused by current change
existing project error
tool/environment error
```

---

# 10. Package Management

Before using a Unity package API, check `Packages/manifest.json` if package availability matters.

Do not install, remove, or upgrade packages without explicit user approval unless package installation is itself the requested task.

Current important packages include:

```text
com.unity.inputsystem
com.unity.ai.navigation
com.unity.render-pipelines.universal
com.unity.pipeline
com.unity.test-framework
com.unity.timeline
com.unity.ugui
com.unity.visualscripting
com.coplaydev.unity-mcp
```

Prefer already-installed packages over adding equivalent third-party dependencies.

---

# 11. Git Safety

Before substantial edits, check:

```powershell
git status --short
```

Assume existing uncommitted changes belong to the user or another team member.

Do not overwrite or revert unrelated changes.

Allowed without asking:

```text
git status
git diff
git log
git branch --show-current
```

Ask before:

```text
git commit
git push
git pull with potentially conflicting changes
git merge
git rebase
git reset
git clean
branch deletion
force operations
```

Never use:

```powershell
git reset --hard
git clean -fd
git push --force
```

without explicit approval.

---

# 12. Completion Response

Keep the final response concise.

Report:

```text
- what was changed;
- important files changed;
- compile/test result;
- any remaining issue or manual Unity Editor step.
```

Do not paste entire source files into the response unless requested.

Do not provide a long explanation of unchanged project structure.

If everything succeeded, state that directly.

If verification could not be performed, state exactly what was not verified.

---

# 13. General Decision Rule

When uncertain, follow this priority:

```text
Preserve user work
→ make the smallest correct change
→ follow existing project architecture
→ avoid unnecessary context/tool usage
→ compile
→ test relevant behavior
→ summarize concisely
```
