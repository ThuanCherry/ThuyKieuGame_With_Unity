# CHAPTER 1 REWORK — FULL CODEX IMPLEMENTATION PROMPT

## Project
**THÚY KIỀU: KIẾM TIỀN CÓ GÌ KHÓ NHỈ?**

## Objective

Rework the entire playable **Chapter 1 — Gia Biến** in the current Unity project so that the gameplay, Ink dialogue, scene staging, character placement, interaction, camera, lighting, audio, UI, animation and chapter transition all match the new Chapter 1 story.

Do not only analyze or write a plan.

**Audit → Implement → Play/Test → Fix → Report.**

---

# 1. WORKING PERMISSIONS

You have **FULL ACCESS** to this Unity project for normal development work.

You may directly:

- inspect the whole project
- edit C# scripts
- create C# scripts
- edit Unity scenes
- create GameObjects
- edit/create prefabs
- edit Animator Controllers
- edit materials
- edit lighting
- edit VFX
- edit audio configuration
- edit UI
- edit Ink integration
- edit interaction systems
- edit camera systems
- edit project configuration when needed
- move/rename files when references can be safely preserved
- create editor/helper tools
- compile
- run tests
- open/run Unity if available
- enter Play Mode if available
- fix compile/runtime errors
- refactor existing systems when it materially improves correctness
- save all modified scenes/assets/prefabs

Do not stop and ask for permission for ordinary development work.

Prefer reusing and improving existing systems/assets rather than creating duplicates.

Do not break Chapter 2, Chapter 3, Chapter 4, or shared systems.

Avoid destructive deletion unless truly necessary. If an existing asset/script/prefab is referenced elsewhere, preserve it or migrate references safely.

---

# 2. SOURCE OF TRUTH

Before changing anything, read:

- `AGENTS.md`
- `Assets/Ink/Main.ink`
- `Assets/Ink/Chapter1_GiaBien.ink`
- relevant files under `docs/codex/`
- all existing Chapter 1 scripts
- all existing Chapter 1 prefabs
- the current playable Chapter 1 scene
- current managers for dialogue, Ink, interaction, audio, chapter progression and UI

The **new `Chapter1_GiaBien.ink` is the narrative source of truth** for:

- dialogue
- speakers
- choices
- story variables
- story order
- emotional beats
- chapter ending

Do not rewrite the story into a substantially different plot.

Minor technical edits are allowed only when required for correct Ink parsing or Unity integration.

---

# 3. IMPORTANT STORY LOGIC

The Chapter 1 logic must be:

- Vương Ông has already been arrested.
- Vương Quan has already been arrested.
- Both were taken away by government officers.
- At the beginning of Chapter 1, **Vương Ông must NOT be physically present in the house**.
- Vương Quan must NOT be physically present in the house.
- Only **Thúy Kiều and Mẹ Kiều** remain in the house at the beginning.
- Mẹ Kiều witnessed Vương Ông and Vương Quan being taken away.
- The house still shows signs of the arrest/search.
- The authorities demand **400 lạng** before morning so the case can be reconsidered and the two men can be temporarily released.
- Mã Giám Sinh knows suspiciously detailed information.
- Mã offers the money in exchange for marriage.
- Kiều must remain intelligent, observant and self-directed.
- Kiều chooses to save her family, but the story must clearly communicate that this does not mean Mã owns her life.

Do not reintroduce Vương Ông into the opening house scene.

---

# 4. UNITY SCENE STRATEGY

Prefer running the entire Chapter 1 inside **one main Unity scene**, for example:

`Chapter01_GiaBien.unity`

Ink tags such as:

- `#scene:VuongGia_DemMua`
- `#scene:VuongGia_BanKyKhe`

should normally be treated as:

- logical story states
- camera states
- lighting states
- gameplay zones
- story phases

Do not load a new Unity Scene every time a `#scene` tag appears unless the existing architecture absolutely requires it.

Suggested hierarchy:

```text
Chapter01_GiaBien
├── Systems
│   ├── ChapterManager
│   ├── DialogueManager
│   ├── InkManager
│   ├── InteractionManager
│   ├── AudioManager
│   └── ObjectiveManager
│
├── Player
│   └── ThuyKieu
│
├── Characters
│   ├── MeKieu
│   ├── MaGiamSinh
│   └── Messenger
│
├── Environment
│   └── VuongGia
│       ├── KieuRoom
│       ├── Hallway
│       ├── MainHall
│       ├── MainDoor
│       ├── ContractTable
│       ├── Exterior
│       └── Props
│
├── StoryInteractables
│   ├── DebtPaper
│   ├── MoneyBag
│   ├── JewelryBox
│   ├── BrokenCup
│   ├── FallenChair
│   └── MudFootprints
│
├── Cameras
├── Lighting
├── VFX
├── Audio
└── UI
```

Reuse equivalent existing hierarchy if it already exists.

---

# 5. FULL CHAPTER 1 PLAYABLE FLOW

The finished chapter must play in this order:

```text
BLACK SCREEN
↓
rain ambience
↓
distant crying from Mẹ Kiều
↓
fade into Kiều's bedroom
↓
Kiều wakes up
↓
player gains control
↓
objective: Tìm Mẹ Kiều
↓
player walks from bedroom toward main hall
↓
player sees traces left by government officers
↓
player reaches Mẹ Kiều
↓
long dialogue about Vương Ông, Vương Quan and 400 lạng
↓
player gains control
↓
objective: Tìm hiểu tình hình Vương gia
↓
player inspects at least 2 of 3 primary clues
    - tờ trát
    - túi bạc
    - hộp nữ trang
↓
return to Mẹ Kiều / story continues
↓
door knocking
↓
objective: Ra mở cửa
↓
player physically walks to the door
↓
press E
↓
door opens
↓
Mã Giám Sinh reveal
↓
dialogue inside main hall
↓
Mã proposes marriage in exchange for 400 lạng
↓
Mẹ Kiều rejects the idea
↓
Kiều takes control of the conversation
↓
player chooses one of four probing responses
↓
contract sequence
↓
player checks contract
↓
Mẹ Kiều emotionally opposes the signing
↓
Kiều explains her decision
↓
Kiều signs
↓
money is sent away
↓
short waiting sequence
↓
Messenger returns
↓
Vương Ông and Vương Quan are reported temporarily released
↓
Mã asks Kiều to leave
↓
farewell between Mẹ Kiều and Kiều
↓
Mẹ gives Kiều family hairpin
↓
Kiều exits into rain
↓
Chapter 1 ending narration
↓
CHAPTER 1 COMPLETE
↓
transition to Chapter 2
```

---

# 6. OPENING — BLACK SCREEN AND BEDROOM

The opening must not immediately start with a normal gameplay camera.

Sequence:

1. screen black
2. rain audible
3. distant crying from Mẹ Kiều
4. short pause
5. fade into Kiều's bedroom
6. Kiều is lying/sitting in bed depending on available animations
7. Kiều hears Mẹ
8. Ink dialogue completes
9. player movement becomes enabled

Use the actual Ink text.

Do not require a perfect bed-rise animation if one is unavailable.

A clean fade + character already standing beside the bed is acceptable if that is safer technically.

Priority:

**story readability > complicated animation**

---

# 7. FIRST GAMEPLAY OBJECTIVE

Display:

`Tìm Mẹ Kiều`

or a stylistically appropriate Vietnamese equivalent.

Player must physically walk from Kiều's room to the main hall.

Do not teleport the player directly to Mẹ Kiều.

Movement must continue using the project's current player controller.

Do not rewrite movement unless it is broken.

---

# 8. ENVIRONMENTAL STORYTELLING

Before meeting Mẹ Kiều, the player should visually understand that something violent or disruptive happened.

Use existing assets where possible.

Required/desired visual clues:

- main door partly open
- muddy footprints from outside
- a fallen chair
- broken cup or small object
- disturbed furniture
- rain/wind audible from entrance
- lighting slightly colder near exterior

These clues should not make the scene look destroyed like a battlefield.

The house was searched and the men were arrested, not completely demolished.

Optional interactions:

### Fallen chair
Kiều may comment that it belonged near her father's table.

### Mud footprints
Kiều may recognize that strangers entered recently.

### Broken cup
Kiều may recognize something her father normally used.

These optional environmental interactions must not block progression.

---

# 9. MẸ KIỀU SCENE

Mẹ Kiều must be alone in the main hall.

She should be:

- seated or standing near the main table
- visibly distressed
- near the government notice
- near the money bag
- near the jewelry box

Her staging must communicate that she has been counting assets and crying for some time.

The dialogue here is deliberately longer.

Do NOT split every sentence into awkward tiny UI clicks if the dialogue system supports displaying 2–4 related sentences together.

Respect the Ink grouping as much as possible.

During dialogue:

- disable player movement
- prevent accidental interactions
- use readable framing
- do not overuse hard cinematic cuts
- use subtle camera changes if the current camera system supports it
- re-enable gameplay only when Ink reaches a gameplay phase

---

# 10. STORY INTERACTIONS — THREE PRIMARY CLUES

After the main conversation with Mẹ Kiều, return control to the player.

Objective:

`Tìm hiểu tình hình Vương gia`

Player should inspect at least **2 of 3** primary clues.

## A. DebtPaper / Tờ trát

Interaction prompt:

`E — Xem tờ trát`

On interaction:

- lock movement
- play the relevant Ink content
- optionally show close-up camera/UI
- set `c1_xem_to_trat = true`
- increment `c1_clues`
- restore control afterward

## B. MoneyBag / Túi bạc

Prompt:

`E — Kiểm tra túi bạc`

On interaction:

- play the Ink content
- set `c1_xem_tui_bac = true`
- increment `c1_clues`

## C. JewelryBox / Hộp nữ trang

Prompt:

`E — Xem hộp nữ trang`

On interaction:

- play the Ink content
- set `c1_xem_nu_trang = true`
- increment `c1_clues`

After `c1_clues >= 2`, unlock the next story beat.

Do not force the player to inspect all three.

The third clue should remain optional if technically reasonable.

---

# 11. GAMEPLAY / INK SYNCHRONIZATION

The existing Ink text currently represents some gameplay transitions using tags such as:

- `#gameplay:enable_player`
- `#objective:...`
- `#camera:...`
- `#sfx:...`
- `#music:...`
- `#chapter_end:1`

Audit how the current Ink integration parses tags.

Implement safe handling for the required Chapter 1 tags.

At minimum support:

```text
speaker
emotion
camera
scene
music
sfx
gameplay
objective
inventory
chapter_end
transition
```

Unknown tags must fail safely.

Do not crash if an optional camera or audio cue is missing.

If a named cue cannot be resolved, log a useful warning and continue the story.

---

# 12. DO NOT LET INK AUTO-RUN THROUGH GAMEPLAY GATES

This is critical.

When Ink reaches a gameplay gate such as:

```text
#gameplay:enable_player
#objective:Tim_Me_Kieu
```

Unity must not immediately continue through all subsequent Ink lines automatically.

The narrative system needs a clear pause/resume mechanism.

Examples:

- wait until player reaches Mẹ Kiều
- wait until player has inspected enough clues
- wait until player opens the door
- wait until the Messenger event completes
- wait until player reaches chapter exit if needed

Use the current architecture if it already has a pause/resume system.

Otherwise add a small, clear system such as:

```text
StoryGate
StoryTrigger
InkResumePoint
ChapterStoryController
```

Do not create an overly complicated framework.

---

# 13. DOOR KNOCK SEQUENCE

After the clue phase and follow-up dialogue:

- play door knock SFX
- pause player briefly if needed for reaction
- show Mẹ Kiều's reaction
- set objective:

`Ra mở cửa`

Then return movement to player.

Player must walk to the real MainDoor.

Interaction:

`E — Mở cửa`

On interaction:

- disable movement
- play door opening SFX
- animate door if existing system permits
- otherwise rotate/open door cleanly via simple animation/script
- reveal Mã Giám Sinh
- start Mã's Ink sequence

Do not teleport Mã into view in a visibly jarring way.

---

# 14. MÃ GIÁM SINH ENTRANCE

Mã should look composed compared with the distressed household.

Desired staging:

- under the exterior roof/porch
- rain behind him
- he is protected from direct rain if possible
- warm interior light behind Kiều
- colder exterior light behind Mã

This should visually reinforce:

**inside = family / warmth / vulnerability**  
**outside = uncertainty / danger**

Mã should not look like a cartoon villain.

His initial presentation should be:

- polite
- controlled
- socially confident
- suspicious only through details

He becomes more unsettling through dialogue and behavior.

---

# 15. NPC PLACEMENT AND FACING

Check:

- Mẹ Kiều
- Mã Giám Sinh
- Messenger
- Kiều

No character should:

- float above floor
- sink into floor
- clip into table
- clip into chair
- face away during dialogue
- stand inside another NPC
- block player path
- block camera unnecessarily

If an NPC dialogue begins, rotate characters naturally toward the relevant speaker where appropriate.

Do not rotate violently/snapping every line.

---

# 16. MÃ GIÁM SINH DIALOGUE CHOICES

The player gets four major choices:

1. ask about Mã's identity/background
2. silently observe his preparation
3. question why he knows exactly about 400 lạng
4. require payment before Kiều leaves the house

Preserve Ink logic and variables:

```text
c1_hoi_lai_lich
c1_quan_sat_ma
c1_hoi_400
c1_tien_truoc
tinh_tao
```

These choices must NOT all feel cosmetically identical.

The selected branch should:

- play its own full dialogue
- update the relevant Ink state
- naturally return to the contract phase

Do not visually show raw variable names.

---

# 17. CONTRACT TABLE GAMEPLAY

The contract area is a major story focal point.

Use the existing table or create a simple polished area using existing props.

Required elements:

- table
- contract/paper
- suitable light source
- position for Mã
- position for Mẹ Kiều
- readable player approach
- interaction focus

Do not use arcade neon outlines.

Subtle highlight / UI prompt is acceptable.

Ink enters:

`c1_contract`

Possible player choices:

- read the full contract
- inspect only the payment clause
- sign quickly because time is running out

Preserve:

```text
c1_doc_ky_khe
c1_tien_truoc
tu_trong
tinh_tao
```

If `c1_tien_truoc` is already true, the contract dialogue must acknowledge the condition.

If false and the player checks the payment clause, allow the story to add it as defined by Ink.

---

# 18. CONTRACT PRESENTATION

If the project already has a document UI, reuse it.

Otherwise a lightweight contract UI may be created.

Requirements:

- readable Vietnamese
- supports `VietnameseTMP.asset`
- does not require player to read tiny world-space text
- can show relevant clause text
- closes cleanly
- does not block progression

Do not spend excessive time making a full parchment editor.

A clear close-up UI or camera plus TMP text is sufficient.

---

# 19. MẸ KIỀU'S OBJECTION / EMOTIONAL PEAK

This is one of the most important dialogue scenes.

Do not rush it.

Mẹ Kiều must emotionally oppose the agreement.

Kiều must admit that she is afraid.

Kiều must make the decision herself.

The core meaning must remain:

- she is not being ordered by her mother
- her family is not selling her
- she chooses to save them
- this choice does not surrender her future agency

Use restrained camera work.

Avoid constantly switching cameras for every sentence.

Prefer:

- medium two-shot
- close-up for key emotional line
- reaction shot
- return to stable framing

---

# 20. SIGNING

The signing should feel consequential.

If a writing/signing animation exists, use it.

If not:

- bring Kiều to signing position
- play a subtle hand/interaction animation if possible
- show contract close-up
- use Ink narration
- use a short paper/pen SFX if available

Do not block implementation because an exact signing animation is unavailable.

---

# 21. WAITING PHASE

After signing:

- Mã sends the money away
- use existing NPC/prop staging or a simple implied action
- reduce dialogue temporarily
- allow a short quiet beat

Optional:

- player regains limited control
- player can look around the house
- objective can show something subtle such as `Chờ tin`

Do not require a fake long timer.

Rough pacing:

5–20 seconds depending on implementation.

The goal is emotional breathing room, not waiting simulation.

---

# 22. MESSENGER

Use an existing suitable NPC if available.

If no dedicated messenger model exists:

- reuse a generic NPC if possible
- or stage the report from doorway/off-screen in a technically safe way

Do not delay the entire implementation just to create a unique model.

Messenger reports:

- money was received
- Vương Ông and Vương Quan will be temporarily released
- the case will be reconsidered

This does NOT mean they instantly appear in the house.

Do not spawn Vương Ông or Vương Quan at this moment unless the story is later explicitly changed.

---

# 23. FAMILY HAIRPIN

Mẹ Kiều gives Kiều a family hairpin during the farewell.

Ink sets:

```text
c1_co_tram_gia_dinh = true
```

and uses:

```text
# inventory:add:tram_gia_dinh
```

Implement this safely.

If a global inventory system already exists:

- add the story item there

If no inventory system exists:

- store the item in shared GameState / StoryState
- do not build an overcomplicated inventory system only for this

The item must persist beyond Chapter 1 if Chapter 2 later wants to reference it.

Suggested state:

```text
tram_gia_dinh = true
```

or equivalent.

---

# 24. SHARED GAME STATE

Audit how current chapter-to-chapter state persists.

Relevant existing/global story variables may include:

```text
tinh_tao
danh_tieng
tu_trong
tien
bang_chung
biet_cua_sau
da_dan
da_viet_thu
da_buon_vai
so_viec
```

Chapter 1 additional state:

```text
c1_hoi_lai_lich
c1_quan_sat_ma
c1_hoi_400
c1_tien_truoc
c1_doc_ky_khe
c1_co_tram_gia_dinh
```

Ensure story state is not lost when transitioning to Chapter 2.

Prefer Ink's story state plus the existing GameState/Session architecture.

Do not create duplicate competing save-state systems.

---

# 25. DIALOGUE UI

Audit the current dialogue UI.

Required:

- Vietnamese text renders correctly
- use `Assets/Fonts/VietnameseTMP.asset`
- long dialogue does not overflow
- choices do not overlap
- speaker names render correctly
- responsive at common resolutions

Test at least:

- 1920×1080
- 1600×900
- 1366×768
- 2560×1440

Recommended structure:

```text
DialogueCanvas
└── DialoguePanel
    ├── SpeakerNameText
    ├── DialogueBodyText
    ├── ContinueIndicator
    └── ChoicesContainer
```

ChoicesContainer should use a layout system that supports long choice text.

Do not force tiny single-line buttons for long Ink choices.

Use:

- Vertical Layout Group
- Layout Element
- Content Size Fitter where appropriate
- TMP word wrapping

Avoid broken UI when cloned to another computer.

---

# 26. DIALOGUE PAGING

The new script intentionally uses longer dialogue.

Do not automatically split every sentence into a separate click if the system can present a natural paragraph.

Aim for:

- one coherent thought per dialogue page
- 2–4 sentences when reasonable
- readable amount of text
- no giant wall of text

If the existing Ink system displays one Ink line at a time, preserve it unless changing it is safe.

But do not further split individual Ink lines into artificial sentence-by-sentence pages.

---

# 27. SPEAKER NAME MAPPING

Ensure these map correctly:

```text
Narrator -> Dẫn chuyện or hidden name
Kieu -> Thúy Kiều
MeKieu -> Mẹ Kiều
MaGiamSinh -> Mã Giám Sinh
Messenger -> Người đưa tin
```

If the current dialogue system already has localization/display-name mapping, reuse it.

Do not show raw internal IDs to the player.

---

# 28. EMOTION TAGS

Ink includes tags such as:

```text
# emotion:crying
# emotion:soft_firm
# emotion:calm_probe
# emotion:polite_fake
# emotion:caught_off_guard
```

Audit how emotions are currently used.

Use them when reasonable for:

- animator triggers
- facial expression systems
- body animation selection
- dialogue portraits if such system exists

If exact emotion animations do not exist:

- fall back gracefully to Idle/Talk/listening
- do not create compile errors
- do not block story progression

No need to build a full facial animation system if the project does not already have one.

---

# 29. CHARACTER ANIMATION

## Thúy Kiều

Reuse existing Humanoid animations when available:

- Idle
- Walk
- Run
- Talk
- interaction/pickup where useful

Do not break current movement.

## Mẹ Kiều

Minimum:

- Idle
- distressed/crying-like pose if available
- Talk
- subtle hand/body motion

## Mã Giám Sinh

Minimum:

- Idle
- Talk
- controlled posture

## Messenger

Minimum:

- Idle
- Talk

Use Unity Humanoid retargeting where applicable.

Avoid overengineering.

---

# 30. CAMERA

Camera should help story readability without turning the whole chapter into a cutscene.

Gameplay sections:

- normal third-person camera
- player retains control

Dialogue:

- lock or soften player camera control only when needed
- use existing dialogue camera system if available
- do not create dozens of unnecessary virtual cameras

Ink camera tags should be mapped where reasonable:

```text
Fade_Black
FadeIn_KieuRoom
WS_MainHall
MCU_MeKieu
MCU_Kieu
Insert_DebtPaper
Door_Reveal
Reveal_MaGiamSinh
CU_Kieu
POV_Kieu_MaHands
Table_Contract
Insert_Signing
Door_Messenger
Hero_Kieu_Rain
Kieu_LookBack
CU_Kieu_Rain
```

If some named cameras do not exist:

- create only the useful ones
- or map multiple tags to a smaller reusable camera set

No hard failure for missing optional camera cue.

---

# 31. LIGHTING

Art direction:

## Exterior
- cool blue/gray
- rainy night
- low but readable exposure

## Interior
- warm amber
- lantern/oil lamp feeling
- softer shadows
- clear character faces

Important visual contrast:

**outside = danger / uncertainty**  
**inside = family / warmth, but under pressure**

Do not make the house so dark that characters disappear.

Do not use excessive realtime lights.

Reuse current URP/lighting setup if present.

---

# 32. RAIN / VFX

Audit existing rain.

Requirements:

- clearly visible outside
- not overwhelmingly dense
- not constantly spawning through the interior roof
- should not obscure the player camera
- should not destroy performance

Rain must be audible from inside, softer/muffled if the current audio system allows it.

Optional:

- slight mist
- subtle splash
- wet visual feel

Only if existing systems/assets make this inexpensive.

---

# 33. AUDIO

Audit all existing `.wav`, `.mp3`, `.ogg` and AudioSources before creating new configuration.

Do not download audio from the internet.

Required audio moments:

- muffled rain in bedroom
- rain ambience in house
- distant crying cue if an appropriate clip exists
- door knock
- door open
- rain exterior
- subtle background music `sad_strings`
- optional paper/contract SFX
- optional footsteps
- Messenger arrival cue if appropriate

Balance:

**dialogue readability > ambience > music**

Do not let rain or music dominate.

If `sad_strings` does not exist, use the closest existing appropriate music or keep the hook without crashing.

---

# 34. OBJECTIVE UI

Support these Chapter 1 objectives:

```text
Tìm Mẹ Kiều
Tìm hiểu tình hình Vương gia
Ra mở cửa
Chờ tin
```

Objective UI must:

- be readable
- not cover dialogue
- update at correct story beats
- disappear or update when completed

Reuse the existing objective system if available.

If none exists, create a minimal reusable ObjectiveUI, not a huge quest framework.

---

# 35. INTERACTION SYSTEM

Reuse current interaction system.

Expected interaction prompt style:

```text
E — Xem
E — Nói chuyện
E — Mở cửa
E — Xem tờ trát
E — Kiểm tra túi bạc
```

Requirements:

- prompt appears only when in range
- only one relevant target at a time
- cannot repeatedly trigger an already completed story interaction unless intended
- dialogue cannot be accidentally retriggered while already open
- player input is locked appropriately during dialogue

---

# 36. CHAPTER END

At:

```text
# chapter_end:1
```

perform a clean Chapter 1 ending.

Desired:

- finishing narration
- chapter-complete presentation if project has one
- preserve Ink/GameState
- transition into Chapter 2 cleanly

Do not blindly load Chapter 2 if `Chapter02` scene is missing or not ready.

If Chapter 2 exists and is configured:
- load it through ChapterManager

If Chapter 2 is not ready:
- end Chapter 1 safely
- show a Chapter Complete state
- log/report what integration remains

Do not crash because Chapter 2 is unfinished.

---

# 37. CHAPTER TRANSITION ARCHITECTURE

Prefer:

```text
Ink reaches #chapter_end:1
        ↓
ChapterManager receives chapter end
        ↓
save/preserve story state
        ↓
fade out
        ↓
load Chapter 2 scene
        ↓
resume from chapter_2
```

Unity should own scene loading.

Ink should own story progression.

Avoid having Ink directly hard-code unsafe Unity scene changes.

---

# 38. HOUSE / ENVIRONMENT POLISH

The Vương family house should feel lived-in.

Reuse existing assets.

Improve composition if needed:

- bed/room for Kiều
- corridor from bedroom to main hall
- main family table
- chairs
- lantern/oil lamps
- contract table or usable main table
- decorative screens if present
- pottery
- small boxes
- books/scrolls
- curtains
- plants
- household props

Do not spam decorations.

Keep navigable space clear.

---

# 39. COLLIDERS

Audit:

- floor
- walls
- door
- main table
- bedroom furniture
- large props
- exterior boundary

Player must not:

- fall through floor
- pass through major walls
- get stuck between props
- clip through the main contract table
- get trapped in Kiều's room
- fail to reach the door

Prefer simple colliders over unnecessarily complex MeshColliders.

---

# 40. CHARACTER MATERIALS / VISIBILITY

Verify Thúy Kiều is visible and correctly rendered.

Check:

- SkinnedMeshRenderer
- Avatar
- materials
- textures
- Animator
- prefab references
- LFS-backed model assets
- shadows

Also check Mẹ Kiều and Mã Giám Sinh.

No character should be missing because of broken prefab/material references.

If an asset is missing, report the exact path/reference rather than silently substituting unrelated content.

---

# 41. FONT

Use:

`Assets/Fonts/VietnameseTMP.asset`

for Chapter 1 TextMeshPro / TextMeshProUGUI.

Do not use incompatible material presets from another TMP font.

Ensure Vietnamese characters display correctly.

Do not rely on fonts installed only on the local Windows machine.

---

# 42. SAVE AND PREFAB REFERENCES

When modifying scene objects:

- save the scene
- apply prefab overrides when they are intended to be shared
- preserve `.meta` relationships
- avoid creating broken duplicate prefabs

Any new reusable prefab should be saved into an appropriate project folder.

---

# 43. PERFORMANCE

Do not sacrifice performance for unnecessary visual effects.

Audit:

- particle count
- realtime shadow lights
- duplicate AudioSources
- duplicate canvases
- duplicate managers
- expensive colliders
- unnecessarily huge textures
- camera stack duplication

Target:

smooth enough for a normal student gaming/development laptop.

---

# 44. SAFE FALLBACKS

If an ideal asset is unavailable:

- use a simple existing prop
- use a primitive temporarily
- use a clean fade
- use Idle/Talk animation
- use a basic camera angle
- use a simple interaction

Do not block the entire Chapter because one exact animation, audio clip or prop is missing.

Gameplay and story correctness come first.

---

# 45. PLAYTEST CHECKLIST

After implementation, play Chapter 1 from the beginning.

Verify:

## Opening
- black screen works
- rain works
- crying cue works/fails safely
- fade works
- Kiều appears correctly

## Bedroom
- player control activates
- objective appears
- player can leave room

## Hallway
- environmental traces are visible
- no collision block

## Mẹ Kiều
- correct NPC
- Vương Ông not present
- Vương Quan not present
- long dialogue readable
- movement locked appropriately

## Clue phase
- all 3 interactables function
- at least 2 are enough
- flags update correctly
- no repeated count exploit

## Door
- knock occurs
- objective changes
- player opens door
- Mã reveal works

## Mã dialogue
- speaker names correct
- choices render correctly
- each branch runs
- Ink variables update

## Contract
- contract interaction works
- conditional payment-before-leaving logic works
- no broken dialogue state

## Emotional decision
- Mẹ Kiều dialogue plays
- Kiều decision plays
- signing sequence completes

## Waiting
- story pauses naturally
- Messenger arrives/reports
- no instant Vương Ông spawn

## Farewell
- hairpin state set
- dialogue readable
- Mã uses correct staging

## Ending
- Kiều exits
- rain visible
- narration plays
- chapter end triggers
- state persists
- Chapter 2 transition works or fails safely if Chapter 2 is not ready

---

# 46. ERROR CHECKING

After implementation:

- compile all C# scripts
- inspect Unity Console
- fix errors introduced by this work
- fix missing references introduced by this work
- check warnings that materially affect Chapter 1
- enter Play Mode
- test the full path

Do not stop after compile success.

The goal is a **playable Chapter 1**.

---

# 47. DO NOT DO THESE

Do not:

- rewrite the whole architecture without need
- replace working PlayerController just because a new one is easier
- create a second competing DialogueManager
- create a second competing Ink runtime
- create duplicate AudioManagers
- create duplicate main Canvas unless required
- rewrite Chapter 2
- remove shared assets used by other chapters
- change the core story
- put Vương Ông back in the house opening
- skip the exploration section
- skip the door interaction
- skip the contract logic
- automatically rush from signing to ending
- hard-code dialogue text in C# when it belongs in Ink
- make every `#scene` tag load a new Unity Scene
- depend on local-only files outside the project
- leave compile/runtime errors behind

---

# 48. DEFINITION OF DONE

Chapter 1 is complete only when:

- game starts from the intended Chapter 1 scene
- opening black screen/rain works
- Kiều wakes in her room
- player can move
- player can find Mẹ Kiều
- Vương Ông and Vương Quan are correctly absent
- story correctly explains their arrest
- 400 lạng problem is clear
- clue exploration works
- Mã arrives through the door
- player opens the door
- Mã dialogue works
- all four probing choices work
- contract sequence works
- conditional Ink variables work
- Mẹ Kiều's emotional objection works
- Kiều signs
- waiting beat works
- Messenger report works
- hairpin is stored
- farewell works
- final rain exit works
- `#chapter_end:1` works
- no compile errors
- no blocking runtime errors
- dialogue UI does not break Vietnamese text
- Kiều renders correctly
- Chapter 1 can be played from start to finish without manual editor intervention

---

# 49. FINAL REPORT

After finishing, give a concise implementation report with:

1. exact Chapter 1 scene path
2. scripts created
3. scripts modified
4. prefabs modified/created
5. Ink integration changes
6. objective/interactable implementation
7. camera changes
8. animation changes
9. lighting/VFX changes
10. audio changes
11. UI/font changes
12. GameState changes
13. Chapter 2 transition status
14. known limitations
15. files/assets still missing, if any

Also report the exact manual steps, if any, that I must perform in Unity Editor.

If no manual steps are required, explicitly say so.

---

# FINAL EXECUTION INSTRUCTION

Do not stop after reading this file.

Do not only produce a design plan.

Start by auditing the current project.

Then directly implement the Chapter 1 rework.

Use the existing project architecture where sensible.

Make the new Ink story playable from beginning to end.

**Audit → Implement → Compile → Play/Test → Fix → Report.**
