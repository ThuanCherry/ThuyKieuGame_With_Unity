# CODEX PROMPT — DỰNG PLAYABLE CHƯƠNG 1: GIA BIẾN

Bạn đang làm việc trực tiếp trong Unity project game **Thúy Kiều: Kiếm tiền có gì khó nhỉ?**

## Mục tiêu

Hoàn thiện **toàn bộ Chương 1 — Gia Biến** thành một scene/playable sequence có thể bấm Play và chơi từ đầu đến cuối.

Không dùng voice/audio thoại. Tất cả hội thoại phải hiển thị bằng **khung hội thoại UI để người chơi đọc**.

Nguồn kịch bản:
- `Main.ink`
- `Chapter1_GiaBien.ink`

Không tự ý viết lại nội dung cốt truyện trừ khi cần chỉnh typo/format kỹ thuật để Ink chạy.

---

## Quyền làm việc

Bạn được phép tự động:
- đọc toàn project;
- tạo/sửa C# scripts;
- tạo/sửa scene;
- tạo prefab;
- tạo Canvas/UI;
- tạo Animator Controller;
- cấu hình Humanoid animation;
- tạo Collider/Trigger/CharacterController;
- tạo Material đơn giản;
- đọc Editor.log/Console;
- build/test;
- chạy các bước kiểm tra cần thiết;
- sửa compile/runtime error;
- tiếp tục lặp cho đến khi Chapter 1 playable.

**Không được xóa file/folder mà chưa hỏi người dùng trước.**

Không phá các hệ thống hiện có. Nếu project đã có PlayerController, Camera, Dialogue, Ink integration hoặc Interaction system thì tái sử dụng thay vì tạo bản trùng.

---

# 1. ĐẦU TIÊN: AUDIT PROJECT

Trước khi code:
1. Xác định Unity version.
2. Xác định render pipeline.
3. Xác định Input System đang dùng.
4. Tìm player Thúy Kiều hiện tại.
5. Tìm model/rig/avatar của:
   - Thúy Kiều
   - Mẹ Kiều (nếu chưa có model, cho phép dùng placeholder humanoid/capsule có label rõ)
   - Mã Giám Sinh
6. Tìm các animation Humanoid có sẵn:
   - Idle
   - Walk
   - Run
   - Talking
   - Look Around
   - Sitting/other interaction clips
7. Xác định Ink runtime/Ink Unity Integration có sẵn hay chưa.
8. Xác định scene gameplay hiện tại.
9. Đọc Console và sửa blocker trước.

Không dừng ở bước báo cáo. Sau audit phải tiếp tục triển khai.

---

# 2. SCENE CHƯƠNG 1

Tạo hoặc hoàn thiện scene:

`Chapter01_GiaBien.unity`

Scene cần tối thiểu:

- Player Thúy Kiều.
- Mẹ Kiều.
- Mã Giám Sinh.
- Nhà Vương gia dạng blockout/playable:
  - gian chính;
  - bàn ký khế;
  - cửa chính;
  - khoảng sân/cửa ra ngoài;
  - collider để player không xuyên tường/sàn.
- Lighting đêm.
- Mưa đơn giản bằng Particle System nếu phù hợp.
- Một bàn có vị trí cho "tờ khế".
- Spawn point cho player.
- Trigger/collider cho các bước cốt truyện.

Không cần dựng kiến trúc nghệ thuật hoàn chỉnh. Ưu tiên gameplay chạy được.

---

# 3. FLOW GAMEPLAY CHƯƠNG 1

## State A — Opening

Khi Play:
- Fade in.
- Player control tạm khóa.
- Hiện narrator dialogue:
  "Mưa quất lên mái ngói Vương gia..."
- Tiếp tục các dòng mở đầu.
- Không cần audio voice.
- Người chơi bấm:
  - `Space`
  - hoặc click nút `Tiếp`
  để sang câu tiếp theo.

Sau đoạn mở đầu:
- trả điều khiển cho player.

## State B — Nói chuyện với Mẹ Kiều

Player có thể đi tới Mẹ Kiều.

Khi vào Interaction Range:
- hiện prompt:
  `E — Nói chuyện`

Nhấn E:
- khóa movement;
- player quay về phía NPC nếu hệ thống cho phép;
- mở Dialogue Panel;
- chạy đúng nội dung Ink tương ứng;
- đóng dialogue thì trả movement.

## State C — Mã Giám Sinh xuất hiện

Theo flow của Ink:
- sau đoạn với Mẹ Kiều, Mã Giám Sinh xuất hiện/được enable hoặc trigger vào scene;
- mở sequence hội thoại;
- player được đưa tới lựa chọn Ink:

1. Hỏi thẳng lai lịch.
2. Im lặng quan sát.
3. Hỏi tiền đến tay cha trước hay sau.

Hiển thị mỗi choice thành một button trong Dialogue UI.

Click choice:
- gửi choice index vào Ink;
- update biến Ink;
- tiếp tục dialogue;
- không dùng voice.

## State D — Bàn ký khế

Sau đoạn hội thoại:
- player tới bàn hoặc sequence tự chuyển camera nhẹ;
- hiện interaction:
  `E — Xem tờ khế`

Khi bấm:
- mở dialogue/choice:
  - Đọc kỹ tờ khế trước khi ký.
  - Ký ngay để tiền được đưa đi cứu cha.

Có thể dùng cùng Dialogue Panel, không cần UI inventory riêng.

## State E — Kết chương

Chạy đoạn:
- Mẹ Kiều khóc;
- Kiều khẳng định quyết định;
- Mã Giám Sinh bảo lên đường;
- Kiều bước ra cửa.

Sau câu:
"Và nàng quyết định: trước khi cánh cửa ấy khép lại, nàng phải tìm được chìa khóa."

Hiện:
`CHƯƠNG 1 HOÀN THÀNH`

Có button:
`Tiếp tục`

Trong giai đoạn hiện tại:
- button có thể load Chapter 2 nếu scene đã tồn tại;
- nếu chưa có, load một placeholder/return menu và log `Chapter 1 Complete`.

---

# 4. DIALOGUE UI — BẮT BUỘC

Tạo Canvas/prefab, ví dụ:

`DialogueCanvas`
- `DialoguePanel`
  - `SpeakerNameText` (TextMeshProUGUI)
  - `DialogueBodyText` (TextMeshProUGUI)
  - `ContinueButton`
  - `ChoicesContainer`
    - runtime choice buttons
  - optional portrait slot
  - optional continue indicator

Yêu cầu UX:
- nền bán trong suốt, dễ đọc;
- speaker name khác màu/weight với nội dung;
- text tiếng Việt hiển thị đúng Unicode;
- auto resize/wrap hợp lý;
- có padding;
- choice button rõ trạng thái hover;
- không cần audio voice;
- có thể click hoặc Space để tiếp tục;
- khi choices hiện ra, không cho continue vượt qua choice.

Speaker map:
- `Narrator` → hiển thị "Dẫn chuyện" hoặc ẩn tên speaker.
- `Kieu` → "Thúy Kiều"
- `MeKieu` → "Mẹ Kiều"
- `MaGiamSinh` → "Mã Giám Sinh"

---

# 5. INK INTEGRATION

Ưu tiên dùng Ink runtime hiện có.

Cần hỗ trợ:
- `Continue()`
- `currentText`
- `currentChoices`
- `ChooseChoiceIndex(index)`
- đọc tag từ `currentTags`

Các tag có trong script:
- `# speaker:`
- `# emotion:`
- `# camera:`
- `# scene:`
- `# music:`
- `# chapter:`

Tối thiểu Chapter 1 phải xử lý:
- `speaker` để đổi tên người nói.
- `camera` có thể log hoặc gọi camera cue đơn giản.
- `emotion` có thể log/để hook cho Animator sau.
- các tag khác không được gây crash.

Nếu chưa có facial animation:
- không cần implement facial expression hoàn chỉnh;
- tạo interface/hook để sau này mở rộng.

Không viết parser Ink giả nếu package Ink đã tồn tại.

---

# 6. HỆ THỐNG TƯƠNG TÁC

Player:
- WASD di chuyển.
- Shift chạy nếu hệ thống hiện có hỗ trợ.
- E tương tác.

Tạo interface hoặc component tương đương:
`IInteractable`
hoặc
`InteractableNPC`

Khi player vào vùng NPC/object:
- hiện Interaction Prompt.
- bấm E để gọi interaction.

Khi Dialogue đang mở:
- khóa player movement.
- khóa interaction spam.
- có thể giữ camera.
- kết thúc dialogue → unlock.

---

# 7. ANIMATION

Thúy Kiều:
- Idle
- Walk
- Run
- Talk nếu có.

NPC:
- Idle mặc định.
- Talking animation dùng chung Humanoid nếu có.
- không cần animation riêng cho từng NPC nếu avatar Humanoid retarget được.

Mã Giám Sinh:
- Idle
- Talk
- optional smug/look animation.

Mẹ Kiều:
- Idle
- Talk
- nếu chưa có crying animation, dùng Talk/Idle + dialogue text; không block progress.

Không dùng root motion cho locomotion nếu PlayerController đang điều khiển vị trí.

---

# 8. CAMERA

Nếu project đã có camera third-person:
- giữ lại.

Trong dialogue:
- có thể dùng camera hiện tại hoặc một DialogueCameraController đơn giản.
- Không cần Cinemachine nếu project chưa dùng.
- Không thêm dependency nặng chỉ cho Chương 1.

Các tag `# camera:` có thể ánh xạ tối thiểu:
- `WS_VuongGia`
- `MCU_MeKieu`
- `CU_Kieu`
- `Reveal_MaGiamSinh`
- `Table_Contract`
- `Hero_Kieu_Rain`

Nếu chưa đủ thời gian:
- implement camera cue bằng Transform presets.
- fallback an toàn về main camera.

---

# 9. GAME STATE CHƯƠNG 1

Giữ state Ink:
- `tinh_tao`
- `tu_trong`
- các biến global trong Main.ink

Khi kết Chapter 1:
- lưu Ink state JSON hoặc GameState object nếu project đã có save system.
- tối thiểu state phải tồn tại để Chapter 2 có thể tiếp tục.

Nếu chưa có Save System:
- tạo `GameSessionState`/`InkStatePersistence` đơn giản, DontDestroyOnLoad.

---

# 10. DEFINITION OF DONE

Chapter 1 chỉ được coi là hoàn thành khi:

1. Scene load không lỗi.
2. Player Thúy Kiều xuất hiện.
3. Player đi được bằng WASD.
4. Không xuyên sàn/tường.
5. Idle/Walk/Run hoạt động.
6. Có Interaction Prompt.
7. Nói chuyện mở khung dialogue.
8. Không có voice audio.
9. Text tiếng Việt hiển thị đúng.
10. Continue hoạt động.
11. Ba lựa chọn đầu tiên của Mã Giám Sinh hoạt động.
12. Lựa chọn tại tờ khế hoạt động.
13. Ink variable thay đổi theo choice.
14. Sau dialogue player điều khiển lại được.
15. Chapter 1 có ending state rõ ràng.
16. Không có compile error.
17. Không có runtime exception blocker.
18. Không tạo duplicate player/model khi dialogue chạy.
19. Không xóa file cũ.
20. Build hoặc Play Mode test thành công.

---

# 11. FILE ORGANIZATION GỢI Ý

Không bắt buộc đổi nếu project đã có kiến trúc tốt.

```text
Assets/
  Ink/
    Main.ink
    Chapter1_GiaBien.ink
    Chapter2_CaiGiaCuaMotConNguoi.ink
    Chapter3_KiemTienCoGiKho.ink
    Chapter4_KhongAiDuocDinhGiaTa.ink

  Scenes/
    Chapters/
      Chapter01_GiaBien.unity

  Scripts/
    Dialogue/
      InkDialogueController.cs
      DialogueUI.cs
      DialogueTagRouter.cs

    Interaction/
      IInteractable.cs
      NPCInteractable.cs
      InteractionPromptUI.cs

    Player/
      PlayerInteraction.cs

    Game/
      Chapter01Director.cs
      GameSessionState.cs

  Prefabs/
    UI/
      DialogueCanvas.prefab
      InteractionPrompt.prefab
```

---

# 12. KHI HOÀN THÀNH

Báo ngắn gọn:
- scene nào mở để test;
- file nào đã tạo/sửa;
- player controls;
- NPC interaction controls;
- Dialogue UI prefab ở đâu;
- Ink asset ở đâu;
- animation đang dùng;
- những phần dùng placeholder;
- warning còn lại nếu có.

Không chỉ đưa hướng dẫn. Hãy trực tiếp triển khai, compile, test và sửa đến khi Chapter 1 playable.
