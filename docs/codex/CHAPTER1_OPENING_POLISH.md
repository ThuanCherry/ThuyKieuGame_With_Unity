Tiếp tục chỉnh Chapter 1 hiện tại.

Scene:
Assets/\_Game/Scenes/Chapter01/Chapter01_GiaBien.unity

Animation dùng chung đã có tại:
Assets/\_Game/Animations/Shared

Trong đó đã có animation:

- Sit To Stand
- Stand To Sit

QUAN TRỌNG:

- Audit project hiện tại trước.
- Reuse hệ thống/code/Animator đang có.
- Không tạo hệ thống trùng.
- Không làm lại bedroom vì bedroom hiện tại đã hoàn thành.
- Không download asset ngoài.
- Không giải thích dài; ưu tiên trực tiếp implement.
- Chỉ đọc các file thật sự liên quan để tiết kiệm token.
- Sau khi xong chỉ báo cáo ngắn file nào đã sửa + còn lỗi gì.
- Full access cho các thao tác development bình thường.
- Không xóa asset/file đang dùng.

MỤC TIÊU:
Hoàn thiện opening Chapter 1 như sau:

Màn hình tối
→ nghe mưa
→ nghe tiếng Mẹ Kiều khóc vọng từ MainHall
→ fade vào phòng Kiều
→ Kiều đang NGỒI trên mép giường
→ UI hiện nút "Ngồi dậy"
→ user nhấn nút
→ play animation Sit To Stand
→ animation kết thúc
→ Kiều chuyển sang Idle
→ player mới được phép điều khiển
→ Objective: "Tìm Mẹ Kiều"
→ player đi tới cửa phòng
→ cửa phòng ban đầu ĐÓNG
→ E - Mở cửa
→ cửa mở có animation + door creak SFX nếu asset có
→ player tự đi ra ngoài, không teleport
→ càng đi gần MainHall thì tiếng khóc của Mẹ Kiều càng rõ
→ nhìn thấy Mẹ Kiều đang NGỒI trên ghế khóc
→ tới gần Mẹ Kiều
→ bắt đầu dialogue Chapter 1 theo Ink.

==================================================

1. # KIỀU NGỒI TRÊN GIƯỜNG

Không để player đứng sẵn cạnh giường nữa.

Khi Chapter bắt đầu:

- Kiều ở đúng vị trí mép giường.
- dùng Sitting Idle hiện có nếu project đã có.
- player movement disabled.
- camera framing phù hợp.
- Animator ở trạng thái Sitting.

Sau đoạn Kiều nghe tiếng mẹ khóc:

Hiện UI button:

[ Ngồi dậy ]

Không dùng phím movement để đứng dậy.

Khi user nhấn:

1. disable button
2. play Sit To Stand từ:
   Assets/\_Game/Animations/Shared
3. tránh animation bị loop
4. chờ animation hoàn thành
5. chuyển Animator về Idle
6. đảm bảo chân Kiều đứng đúng trên sàn
7. enable PlayerController
8. hiện objective "Tìm Mẹ Kiều"

Nếu Sit To Stand bị root motion làm Kiều trượt:

- ưu tiên giữ character position ổn định
- cấu hình root motion/import/Animator hợp lý
- không phá movement hiện tại.

Stand To Sit chưa bắt buộc sử dụng trong opening,
nhưng import/configure Humanoid đúng để có thể tái sử dụng sau này.

================================================== 2. BUTTON "NGỒI DẬY"
==================================================

Reuse Canvas/UI hiện có nếu phù hợp.

Không tạo thêm Canvas chính nếu không cần.

Button:

- TextMeshPro
- dùng VietnameseTMP.asset
- text: "Ngồi dậy"
- dễ nhìn
- không che nhân vật
- responsive

Button chỉ xuất hiện tại opening.

Sau khi nhấn:

- hide button
- không xuất hiện lại.

Không cho player điều khiển Kiều trước khi Sit To Stand kết thúc.

================================================== 3. CỬA PHÒNG KIỀU
==================================================

Hiện tại phòng đã có cửa nhưng cần hoàn thiện đóng/mở.

Khi game bắt đầu:

- cửa phòng Kiều phải ĐÓNG.

Khi player đứng đủ gần:
hiện:

E - Mở cửa

Khi nhấn E:

- mở cửa tự nhiên khoảng 80–100 độ
- pivot phải nằm ở bản lề
- không xoay quanh tâm sai vị trí
- player không xuyên qua cửa khi đang đóng
- sau khi mở, player đi qua bình thường.

Ưu tiên:

- Animator hoặc simple DoorController
- reuse interaction system hiện tại

Nếu có door creak SFX trong project:

- dùng nó.

Nếu không có:

- implementation vẫn phải hoạt động
- không download audio ngoài.

Không teleport player qua cửa.

================================================== 4. ÂM THANH MẸ KIỀU KHÓC
==================================================

Mẹ Kiều ở MainHall.

Cần tạo/reuse AudioSource cho tiếng khóc.

Ưu tiên 3D positional audio:

Khi Kiều còn trong phòng:

- tiếng khóc nghe nhỏ/muffled.

Khi mở cửa:

- nghe rõ hơn.

Khi Kiều đi gần MainHall:

- tiếng khóc rõ dần.

Không để tiếng khóc quá lớn hoặc át nhạc/mưa.

Nếu project đã có female crying audio:

- reuse.

Nếu chưa có clip tiếng khóc:

- vẫn setup AudioSource/logic hoàn chỉnh
- ghi rõ asset audio còn thiếu trong final report
- không tự download.

Đừng loop một đoạn khóc quá ngắn theo kiểu khó chịu.
Nếu clip phù hợp loop được thì loop nhẹ.
Nếu không, play theo khoảng hợp lý.

================================================== 5. MẸ KIỀU
==================================================

Khi Kiều ra MainHall:

Mẹ Kiều phải:

- ngồi trên ghế
- gần bàn có tờ trát / túi bạc / nữ trang
- đầu/cơ thể thể hiện buồn
- không đứng chờ player.

Ưu tiên animation:

- Sitting Idle
- Sitting Sad/Crying nếu project có
- Sitting Talking khi dialogue nếu có

Audit animation hiện tại trước.
Không yêu cầu download animation mới.

Nếu không có Sitting Crying:

- dùng Sitting Idle/Sitting Talking
- kết hợp pose/camera + crying audio
- không block implementation.

Mẹ Kiều không được:

- floating
- chìm xuống ghế
- chân xuyên sàn quá rõ
- quay lưng với player.

================================================== 6. TRIGGER NÓI CHUYỆN
==================================================

Không bắt đầu dialogue ngay khi Kiều mở cửa phòng.

Player phải tự đi tới MainHall.

Khi player tới gần Mẹ Kiều:

- hoàn thành objective "Tìm Mẹ Kiều"
- stop/fade crying audio hợp lý
- lock PlayerController
- Mẹ Kiều chuyển từ crying/sitting state sang dialogue state nếu cần
- bắt đầu đúng đoạn Ink tương ứng với cuộc gặp Mẹ Kiều.

Không hard-code toàn bộ thoại vào C#.
Ink vẫn là source of truth.

================================================== 7. AUDIO OPENING
==================================================

Giữ:

- rain ambience
- sad_strings nếu hiện có.

Mix:
dialogue > crying > rain > music

Tránh nhiều AudioListener.
Phải có đúng 1 AudioListener active.

================================================== 8. TEST BẮT BUỘC
==================================================

Test trực tiếp flow:

Start Chapter
→ Kiều ngồi trên giường
→ nghe khóc
→ button Ngồi dậy xuất hiện
→ click
→ Sit To Stand chạy đúng
→ Kiều Idle
→ movement hoạt động
→ cửa phòng đang đóng
→ E mở cửa
→ cửa mở đúng pivot
→ đi qua được
→ tiếng khóc tăng dần khi tới MainHall
→ thấy Mẹ Kiều ngồi khóc
→ tới gần
→ dialogue bắt đầu.

Kiểm tra:

- không compile error
- không Animator error
- không NullReference
- không player movement trong animation
- không button trigger nhiều lần
- không cửa mở nhiều lần sai
- không audio tiếp tục khóc đè lên dialogue nếu không phù hợp
- không teleport player

Nếu phát hiện lỗi:
tự sửa rồi test lại.

==================================================
DONE
==================================================

Chỉ coi task hoàn thành khi opening trên chạy được từ đầu tới lúc bắt đầu dialogue với Mẹ Kiều.

Cuối cùng báo cáo NGẮN:

- file đã sửa/tạo
- Animator thay đổi gì
- cửa dùng hệ thống nào
- crying audio có asset hay đang thiếu
- test result
- manual step nếu thật sự cần

Không viết báo cáo dài.
Bắt đầu implement ngay, không chỉ lập kế hoạch.
