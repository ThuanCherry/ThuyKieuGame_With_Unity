Bạn đang làm việc trực tiếp trong Unity project game:

THÚY KIỀU: KIẾM TIỀN CÓ GÌ KHÓ NHỈ?

NHIỆM VỤ:
Hoàn thiện môi trường Chapter 1 — GIA BIẾN.

Mục tiêu cuối cùng:
Khi tôi mở Chapter01_GiaBien.unity và bấm Play,
tôi phải có một căn nhà Vương Gia cổ trang đẹp, có interior playable,
ánh sáng đúng không khí đêm mưa của cốt truyện,
có ambience/music/SFX bằng những audio assets đang có trong project,
Thúy Kiều đi lại được và có thể tương tác với NPC + bàn ký khế.

==================================================
QUYỀN LÀM VIỆC
==================================================

Bạn được tự động:

- đọc toàn bộ Unity project
- tìm model/prefab/material/texture
- tìm audio/music/SFX
- tìm particle/VFX
- tạo GameObject
- tạo Prefab
- tạo Material
- tạo/sửa Scene
- tạo/sửa C# scripts
- tạo/sửa lighting
- tạo AudioSource
- tạo AudioMixer nếu cần
- tạo Particle System
- thêm Collider
- cấu hình Unity assets
- chạy compile
- chạy test
- đọc Console/Editor.log
- tự sửa compile/runtime errors
- lặp lại cho đến khi Chapter 1 playable

Không cần hỏi tôi đối với thao tác development bình thường.

QUY TẮC BẮT BUỘC:

KHÔNG ĐƯỢC XÓA FILE HOẶC FOLDER nếu chưa hỏi tôi trước.

Không được dùng:
rm
del
Remove-Item
rmdir
git clean
hoặc cách khác để xóa dữ liệu
mà chưa được tôi đồng ý.

==================================================

1. # ĐỌC CONTEXT TRƯỚC

Trước khi làm, hãy đọc đầy đủ:

AGENTS.md

docs/codex/CHAPTER1_CODEX_PROMPT.md

docs/codex/INK_SETUP_README.md

Assets/Ink/Main.ink

Assets/Ink/Chapter1_GiaBien.ink

Sau đó audit toàn bộ Unity project.

Không dừng lại để báo cáo audit.
Audit xong phải tiếp tục thực hiện nhiệm vụ.

================================================== 2. AUDIT ENVIRONMENT ASSETS
==================================================

Tìm toàn bộ assets có thể sử dụng cho môi trường:

- ancient house
- Asian house
- Chinese/Vietnamese historical architecture
- wooden house
- roof
- tiles
- walls
- floors
- columns
- doors
- windows
- lanterns
- tables
- chairs
- screens
- fences
- gates
- stone paths
- trees
- plants
- props
- furniture

Ưu tiên tuyệt đối:
TÁI SỬ DỤNG asset đang có trong project.

Không tạo duplicate model nếu đã có prefab/model tương đương.

Nếu project không có nhà hoàn chỉnh:
hãy dựng modular house từ các asset đang có.

Nếu vẫn không có đủ asset:
dùng Unity primitives làm phần còn thiếu,
nhưng tổ chức chúng thành prefab/module để sau này dễ thay bằng model đẹp.

================================================== 3. THIẾT KẾ VƯƠNG GIA
==================================================

Tạo:

Assets/Scenes/Chapters/Chapter01_GiaBien.unity

hoặc sử dụng đúng folder Scenes hiện tại nếu project đã có convention.

Môi trường cần có:

VƯƠNG GIA
|
|-- MainHouse
| |
| |-- MainHall
| | |
| | |-- ContractTable
| | |-- Chairs
| | |-- Lanterns
| | |-- DecorativeScreens
| | `-- InteriorProps
|    |
|    |-- MainDoor
|    |
|    `-- SideArea
|
|-- Courtyard
| |
| |-- StonePath
| |-- Plants
| |-- Rain
| `-- Puddles / wet decoration nếu khả thi
|
|-- MainGate
|
|-- EnvironmentLighting
|
|-- EnvironmentAudio
|
|-- PlayerSpawn
|
|-- NPC_MeKieu
|
`-- NPC_MaGiamSinh

Phong cách:

- nhà gỗ cổ trang
- sang nhưng không phải cung điện hoàng gia
- gia đình có học thức
- màu gỗ nâu trầm
- cột gỗ
- mái ngói
- đèn/lồng đèn ánh sáng ấm
- bàn ghế gỗ
- màn/vách trang trí vừa phải
- không quá nhiều chi tiết Trung Hoa cung đình
- phù hợp không khí Truyện Kiều

Ưu tiên cảm giác:
"nhà từng yên bình nhưng đêm nay đang gặp đại biến".

================================================== 4. SCALE GAMEPLAY
==================================================

Mọi thứ phải được dựng theo scale phù hợp humanoid Unity.

Player Thúy Kiều phải:

- đi qua cửa dễ dàng
- không bị mắc collider
- đi quanh bàn được
- tiếp cận Mẹ Kiều
- tiếp cận Mã Giám Sinh
- tiếp cận ContractTable
- đi từ trong nhà ra sân
- đi tới cổng

Không tạo hành lang quá hẹp.

Không để furniture chắn navigation.

================================================== 5. COLLISION
==================================================

Thêm collider hợp lý cho:

- Floor
- Walls
- Columns
- Table
- Door frames
- Gate
- major props

Ưu tiên BoxCollider / primitive collider.

Không dùng một MeshCollider cực lớn cho toàn bộ căn nhà nếu không cần.

Player không được:

- xuyên sàn
- xuyên tường
- rơi khỏi map
- mắc vào props nhỏ

================================================== 6. LIGHTING — CHAPTER 1
==================================================

Cốt truyện diễn ra vào ĐÊM MƯA.

Mood:

buồn
căng thẳng
bất an
nhưng vẫn giữ cảm giác gia đình và hy vọng.

Thiết kế lighting theo nguyên tắc:

# NGOÀI TRỜI

ánh sáng lạnh.

# TRONG NHÀ

ánh sáng ấm.

==================================================

EXTERIOR LIGHTING

Tạo ánh sáng đêm:

- moon/ambient light xanh xám nhẹ
- không quá sáng
- courtyard tối hơn interior
- rain phải nhìn thấy được
- bóng đổ mềm vừa phải

Màu tổng thể exterior:
blue-gray / desaturated.

Không biến scene thành tối đen.

Player và NPC vẫn phải đọc được silhouette.

==================================================

INTERIOR LIGHTING

Dùng:

- lanterns
- candles
- warm lights

Màu:
warm amber / vàng ấm.

Main Hall phải đủ sáng để:

- thấy khuôn mặt NPC
- đọc được nhân vật
- thấy ContractTable
- phân biệt interior và exterior

Tạo contrast:

Exterior:
cold

Interior:
warm

Điều này phải khiến người chơi cảm thấy:

"bên trong là gia đình,
nhưng nguy hiểm đang tiến vào từ bên ngoài."

================================================== 7. LIGHTING THEO STORY MOMENT
==================================================

OPENING:

Không khí:
buồn + bất an.

Lighting:
đêm mưa, ánh sáng thấp,
interior hơi tối.

KHI MÃ GIÁM SINH XUẤT HIỆN:

Nếu hệ thống hiện tại cho phép,
nhấn nhẹ ánh sáng tại cửa hoặc thay đổi framing,
để entrance của hắn rõ ràng hơn.

Không dùng spotlight sân khấu quá lộ.

CONTRACT SCENE:

ContractTable phải trở thành visual focus.

Tăng nhẹ ánh sáng local quanh:

- tờ khế
- mặt Thúy Kiều
- Mã Giám Sinh

Nhưng vẫn giữ background tối.

ENDING:

Khi Kiều bước ra ngoài:

giữ mưa lạnh.

Không chuyển thành cảnh sáng vui.

Mood:
determined / uncertain freedom.

================================================== 8. RAIN VFX
==================================================

Audit project trước:

Tìm:

Rain
Rain Particle
Weather
Storm
Water
Splash

Nếu có prefab/particle phù hợp:
reuse nó.

Nếu không có:
tạo Particle System rain đơn giản.

Rain cần:

- rơi chủ yếu ngoài sân
- không mưa xuyên mái nhà
- density vừa phải
- không quá nặng GPU
- hướng hơi nghiêng nếu đẹp

Optional nếu project dễ hỗ trợ:

- ground splash
- wet ground impression
- mist nhẹ

Không dành quá nhiều thời gian cho realistic water simulation.

================================================== 9. AUDIO AUDIT
==================================================

Scan toàn bộ project để tìm:

.wav
.mp3
.ogg

và AudioClip assets.

Phân loại những audio đang có thành:

MUSIC
AMBIENCE
SFX
UI

Không tải audio bên ngoài.
Không dùng network.
Không tự thêm copyrighted music.

Ưu tiên sử dụng AUDIO ĐANG CÓ.

Nếu không có asset phù hợp:
tạo hook/AudioSource placeholder,
nhưng không tạo audio giả.

================================================== 10. CHAPTER 1 AUDIO DESIGN
==================================================

Chapter 1 KHÔNG DÙNG VOICE ACTING.

Dialogue chỉ đọc bằng Dialogue UI.

Audio chỉ gồm:

Music
Ambience
SFX
UI sounds

==================================================

AMBIENCE:

Nếu có audio phù hợp:

Rain Loop
→ chạy liên tục ngoài trời.

Wind ambience
→ volume thấp.

Interior room tone
→ optional.

Nếu có thunder:
chỉ sử dụng rất nhẹ và thưa.

Không để ambience át dialogue reading experience.

================================================== 11. MUSIC
==================================================

Kịch bản sử dụng mood:

sad_strings

Tìm music đang có trong project phù hợp với:

- sad
- emotional
- strings
- drama
- tense
- historical
- ambient

Chọn track phù hợp nhất.

Không dùng music quá epic.

Opening:
sad / restrained.

Mã Giám Sinh:
có thể tăng tension nhẹ.

Contract:
tension tăng nhẹ.

Ending:
không triumph.
Giữ cảm giác chưa biết tương lai.

Nếu project không có music phù hợp:
để Music AudioSource sẵn sàng nhưng không tự tải file ngoài.

================================================== 12. SFX
==================================================

Tìm và sử dụng SFX đang có cho:

RAIN
rain loop

FOOTSTEP
nếu đã có player footsteps system thì reuse.

DOOR
main door open/close.

PAPER
paper / parchment / page / document sound
cho ContractTable nếu có.

UI
click / confirm / select.

NPC ENTRY
không cần âm thanh đặc biệt trừ khi asset hợp lý.

Nếu không có SFX tương ứng:
không fabricate audio.
Chỉ để hook để sau này bổ sung.

================================================== 13. AUDIO ARCHITECTURE
==================================================

Tạo EnvironmentAudio hierarchy tương tự:

EnvironmentAudio
|
|-- MusicSource
|
|-- RainSource
|
|-- WindSource
|
`-- SFXSource

Hoặc reuse hệ thống AudioManager hiện tại.

Nếu project đã có AudioManager:
KHÔNG tạo AudioManager thứ hai.

Nếu cần AudioMixer:

Master
|
|-- Music
|
|-- Ambience
|
|-- SFX
|
`-- UI

Volume mặc định phải cân bằng.

Dialogue đọc bằng text nên:
Music và ambience phải đủ nhỏ để không gây mệt.

================================================== 14. AUDIO SPATIALIZATION
==================================================

Rain:
có thể 2D ambience hoặc mixture phù hợp.

Door SFX:
3D spatial.

Environment localized SFX:
3D.

Music:
2D.

UI:
2D.

Không đặt tất cả audio thành 3D.

================================================== 15. CONTRACT TABLE
==================================================

ContractTable phải rõ ràng về mặt visual.

Tạo:

ContractTable
|
`-- ContractInteractable

Khi player lại gần:

[E] Xem tờ khế

Nhấn E:

- player movement lock
- mở Dialogue UI
- chạy Ink choice tương ứng
- optional paper SFX nếu project có
- kết thúc interaction → unlock movement

Có thể đặt một parchment/plane/model trên bàn.

================================================== 16. NPC PLACEMENT
==================================================

Mẹ Kiều:

đặt trong Main Hall.

Mood:
buồn / lo lắng.

Mã Giám Sinh:

ban đầu có thể disable hoặc đứng gần entrance.

Khi story tới entry:
enable / move / trigger sequence.

Không spawn duplicate NPC.

================================================== 17. ANIMATION
==================================================

Reuse Humanoid animation hiện có.

Player:
Idle
Walk
Run

NPC:
Idle
Talking
Listening nếu có

Mã Giám Sinh:
Idle
Talking

Mẹ Kiều:
Idle
Talking

Không cần tạo animation mới nếu chưa có.

Không dùng animation requirement làm blocker cho Chapter 1.

================================================== 18. CAMERA
==================================================

Reuse third-person camera hiện tại.

Nếu project có dialogue camera:
reuse.

Nếu không:
tạo system nhẹ.

Khi dialogue:

- giữ NPC/player rõ trong frame
- không để camera xuyên tường
- không cần cinematic system phức tạp

ContractTable:
camera có thể focus nhẹ vào bàn.

Ending:
camera có thể nhìn Kiều bước ra sân.

================================================== 19. PERFORMANCE
==================================================

Đây là DEMO.

Không cần ultra graphics.

Ưu tiên:

- playable
- đẹp vừa đủ
- stable
- dễ sửa
- modular

Avoid:

- quá nhiều realtime shadow lights
- particle density quá cao
- unnecessary 4K textures
- duplicated models/materials
- huge mesh colliders

================================================== 20. TEST
==================================================

Sau khi hoàn thành:

Mở Chapter01_GiaBien.

Test:

1. Không compile error.
2. Không runtime blocker.
3. Thúy Kiều spawn đúng.
4. WASD hoạt động.
5. Walk/Run animation hoạt động.
6. Nhà có interior.
7. Player không xuyên sàn.
8. Player không xuyên tường.
9. Door/courtyard accessible.
10. Lighting đêm mưa hoạt động.
11. Interior warm / exterior cold rõ ràng.
12. Rain nhìn được.
13. Rain không rơi xuyên interior nếu có thể tránh.
14. Existing music hoạt động.
15. Rain ambience loop đúng.
16. SFX đang có được reuse phù hợp.
17. NPC đứng đúng vị trí.
18. Interaction Prompt hoạt động.
19. ContractTable hoạt động.
20. Dialogue vẫn dùng TEXT, không voice.
21. Không có duplicate AudioManager.
22. Không có duplicate Player.
23. Console sạch các lỗi nghiêm trọng.

Nếu gặp lỗi:
đọc log,
sửa,
test lại.

Tiếp tục cho đến khi scene playable.

================================================== 21. KẾT QUẢ CUỐI
==================================================

Khi hoàn thành, báo:

SCENE:
đường dẫn scene.

ENVIRONMENT:
những prefab/model đã reuse.

LIGHTING:
những light đã tạo/chỉnh.

AUDIO:
music clip nào đang dùng.
rain clip nào đang dùng.
SFX nào đang dùng.

VFX:
Rain implementation.

PLAYER:
spawn location.

NPC:
Mẹ Kiều location.
Mã Giám Sinh location.

INTERACTION:
ContractTable location.

FILES CREATED:
liệt kê.

FILES MODIFIED:
liệt kê.

PLACEHOLDERS:
những phần chưa có asset thật.

WARNINGS:
warning còn lại.

Không chỉ viết kế hoạch.
Hãy trực tiếp thực hiện toàn bộ nhiệm vụ.
