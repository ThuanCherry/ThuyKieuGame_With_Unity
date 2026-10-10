# CHAPTER 1 — STORY, CINEMATIC & GAMEPLAY REWORK

## ROLE

Bạn đóng đồng thời 3 vai trò:

- Senior Unity Gameplay Developer
- Senior Narrative Game Designer
- Cinematic Game Director

Dự án: **Thúy Kiều — Game phiêu lưu kể chuyện góc nhìn thứ ba**, sử dụng Unity, C#, Ink và Unity MCP.

**Mục tiêu:** Đánh giá, phản biện, cải thiện và triển khai trực tiếp Chapter 1 đang hoạt động trong project.

Không chỉ lập kế hoạch. Hãy audit → đánh giá → sửa → test → khắc phục.

---

## 1. XÁC ĐỊNH ĐÚNG CHAPTER 1

Trước khi sửa, bắt buộc:

1. Đọc `AGENTS.md` và hướng dẫn project liên quan.
2. Sử dụng Unity MCP để xác định project và scene hiện mở.
3. Tìm `Main.ink`, `Chapter1_GiaBien.ink`, Ink runtime và các file JSON được biên dịch.
4. Truy vết Ink entrypoint → INCLUDE → compiled story → DialogueManager → Unity scene.
5. Xác định file Chapter 1 nào thực sự đang được runtime sử dụng.
6. Đọc các script PlayerController, CameraController, Interaction, Animator và Dialogue liên quan.
7. Kiểm tra scene `Chapter01_GiaBien.unity` nếu tồn tại, nhưng không mặc định đây là scene active nếu chưa xác minh.

**Không sửa file Ink cũ, backup, tài liệu tham khảo hoặc bản không được runtime sử dụng.**

Nếu không xác định được chính xác runtime đang dùng file nào, dừng chỉnh sửa và hỏi tôi.

Trước khi thực hiện, kiểm tra Git status. Không ghi đè thay đổi của thành viên khác.

---

## 2. ĐÁNH GIÁ VÀ PHẢN BIỆN

Đọc toàn bộ Chapter 1, bao gồm các lựa chọn, knot, tag, biến và gameplay gate.

Đánh giá:

- Cốt truyện có hợp lý và nhất quán không?
- Nhân vật có hành động đúng động cơ và tính cách không?
- Hội thoại có tự nhiên, cảm xúc và chiều sâu không?
- Narrator có đang giải thích quá nhiều điều người chơi đã nhìn thấy?
- Tình tiết nào bị kéo dài hoặc lặp ý?
- Camera và animation có giúp kể chuyện tốt hơn không?
- Các lựa chọn có thật sự khiến người chơi phải suy nghĩ không?
- Tiết tấu có phù hợp Chapter 1 của game 4 chương, tổng thời lượng 15–25 phút không?
- Những yêu cầu của tôi có điểm nào chưa hợp lý hoặc quá tốn chi phí triển khai không?

**Bạn phải phản biện khách quan, không mặc định tôi luôn đúng.**

Đối với vấn đề quan trọng, trình bày ngắn:

- Vấn đề
- Nguyên nhân
- Đề xuất tốt hơn
- Quyết định triển khai và lý do

Được phép tự cải thiện các vấn đề kỹ thuật và dàn cảnh trong phạm vi nhiệm vụ. Hỏi tôi trước khi thay đổi bước ngoặt cốt truyện, kết quả lựa chọn hoặc ý nghĩa nhân vật.

---

## 3. REWORK CỐT TRUYỆN INK — GIẢM NARRATOR

Mục tiêu là để người chơi **trải nghiệm câu chuyện, không phải đọc một cuốn tiểu thuyết qua Dialogue Box**.

Yêu cầu:

- Giảm tối đa Narrator mô tả khung cảnh, hành động, nét mặt và âm thanh.
- Ưu tiên camera, animation, SFX và environment kể chuyện.
- Giữ Narrator ngắn ở những thời điểm thật sự cần thiết.
- Không cắt ngắn hội thoại một cách máy móc.
- Giữ lời thoại có chiều sâu nhưng tự nhiên, đúng người và đúng hoàn cảnh.
- Không để một ý được nhắc đi nhắc lại nhiều lần.
- Chỉ dẫn diễn xuất có thể chuyển thành camera/animation/event tags tương thích với runtime đang có.
- Không tạo tag mới mà runtime không xử lý rồi xem như đã hoàn thành.
- Không loại bỏ thông tin thiết yếu nếu Unity chưa có cách thể hiện thay thế.
- Không để dialogue trống, câu thoại mất speaker hoặc chuyển cảnh sai.

### Giữ nguyên các tình tiết cốt lõi

1. Kiều nghe Mẹ Kiều khóc giữa đêm mưa.
2. Kiều rời phòng, thấy nhà bị lục soát.
3. Mẹ Kiều kể Vương Ông và Vương Quan đều bị áp giải.
4. Gia đình cần 400 lạng trước sáng.
5. Kiều tìm hiểu tờ trát, túi bạc và nữ trang.
6. Tiếng gõ cửa vang lên và Mã Giám Sinh xuất hiện.
7. Mã đề nghị giúp với điều kiện gắn với hôn ước.
8. Kiều thăm dò Mã và kiểm tra tờ khế.
9. Mẹ phản đối; Kiều tự đưa ra quyết định.
10. Bạc được chuyển đến quan phủ, có tin cha và em sẽ được tạm thả.
11. Mẹ trao trâm gia đình cho Kiều.
12. Kiều rời nhà trong mưa, kết thúc Chapter 1.

Vương Ông và Vương Quan **không xuất hiện ở nhà ngay sau khi nhận tin tạm thả**.

Bảo toàn các biến, lựa chọn, gameplay gates và điều kiện chuyển chapter.

---

## 4. FIX CẢNH MÃ GIÁM SINH XUẤT HIỆN

Kiểm tra các đoạn Ink tương ứng với:

- `c1_before_knock`
- `c1_ma_arrives`
- `c1_offer`

### A. Tiếng gõ cửa

Khi cuộc trò chuyện giữa Kiều và Mẹ Kiều bị ngắt:

1. Phát tiếng gõ cửa từ vị trí cửa chính bằng AudioSource 3D nếu phù hợp.
2. Ưu tiên một nhịp gõ, sau đó nhịp gõ tiếp theo nếu đã được kịch bản quy định.
3. Kiều và Mẹ có phản ứng tự nhiên.
4. Player nhận objective ra mở cửa.
5. Người chơi đến gần và nhấn E.
6. Cửa thực sự mở bằng hệ thống tương tác hiện có.

Không mở cửa trước khi player tương tác.

Không để âm thanh gõ lặp vô hạn hoặc phát hai lần do Ink và Unity cùng kích hoạt.

Nếu thiếu audio asset, báo rõ và dùng giải pháp tạm phù hợp nếu có; không giả vờ đã có âm thanh.

### B. Vị trí Mã Giám Sinh

Khi Kiều mở cửa:

- Mã Giám Sinh phải đứng ở vị trí trung tâm hợp lý của lối vào, bên ngoài ngưỡng cửa.
- Không đứng lệch, quay mặt vào tường hoặc đè lên collider.
- Mã đối diện Kiều và có khoảng cách hội thoại tự nhiên.
- Khi được mời vào, Mã di chuyển tới vị trí trung tâm hợp lý trong MainHall, ở chỗ có thể đối thoại với hai mẹ con.
- Không teleport bất ngờ nếu có thể sử dụng di chuyển ngắn hoặc chuyển cảnh điện ảnh.
- Kiểm tra collision, ground position và character scale.

### C. Animation của Mã Giám Sinh

Kiểm tra những animation đã có trong project.

Ưu tiên:

- Idle tự nhiên.
- Đi bộ khi vào nhà.
- Chào hoặc cúi nhẹ người.
- Talking gesture khi hội thoại.
- Chuyển từ lịch sự sang khó chịu khi bị Kiều chất vấn.
- Đưa tờ khế hoặc hướng tay về bàn khi thích hợp.

Sử dụng Humanoid retargeting và animation có sẵn nếu tương thích.

Không tự tạo animation phức tạp hoặc tải asset ngoài khi chưa cần thiết. Nếu thiếu animation, lựa chọn cách diễn xuất đơn giản hơn và đề xuất asset cần bổ sung.

Không để animation loop sai, chân trượt, root motion làm nhân vật rời khỏi vị trí hoặc tay xuyên người.

---

## 5. DIALOGUE CAMERA — KIỀU VÀ MÃ GIÁM SINH

Triển khai hệ thống camera hội thoại tái sử dụng, phù hợp hệ thống hiện có.

Kiểm tra Cinemachine đã cài chưa. Nếu chưa, đánh giá việc dùng camera hiện tại hay cài thêm; không tự tạo hệ thống camera thứ hai gây xung đột.

### Camera A: Two-Shot

- Thấy Kiều và Mã Giám Sinh.
- Dùng lúc gặp mặt, giới thiệu và trao đổi thông thường.
- Bố cục không bị cửa, bàn ghế hoặc nhân vật khác che mất.

### Camera B: Kiều

- Medium Close-Up hoặc Over-the-Shoulder.
- Tập trung khuôn mặt và ánh mắt.
- Dùng khi Kiều chất vấn Mã hoặc đưa ra quyết định.
- Không che khuôn mặt bằng tóc, vai hoặc vật thể.

### Camera C: Mã Giám Sinh

- Medium Close-Up.
- Ban đầu tạo cảm giác lịch sự.
- Khi Mã bị chất vấn hoặc lộ sơ hở, dùng góc gần hơn vừa đủ.
- Không lạm dụng hiệu ứng zoom gây khó chịu.

### Chuyển camera

- GameplayCamera → DialogueCamera blend mượt.
- Ưu tiên explicit Ink camera tag, fallback theo speaker.
- Không đổi góc liên tục theo mỗi lượt bấm Continue.
- Giữ một góc trong nhiều câu thoại nếu phù hợp.
- Khi kết thúc, blend về gameplay.
- Khôi phục đúng movement và mouse-look.
- Không để nhiều Camera/AudioListener hoạt động xung đột.
- Không để Dialogue Box che mặt hoặc cảm xúc nhân vật.

Phải tích hợp với DialogueManager/Ink hiện có, không hard-code nội dung thoại trong C#.

---

## 6. FIX LỖI KIỀU NHẤN E NGỒI DẬY

**BUG HIỆN TẠI:**

Ở đầu Chapter 1, Kiều đang ngồi trên giường. Khi nhấn E để ngồi dậy, góc nhìn camera bị lag, giật hoặc mắc vào giường.

Yêu cầu:

1. Reproduce lỗi trong Play Mode nếu có thể.
2. Kiểm tra spawn position, giường, collider và camera collision.
3. Kiểm tra Animator, Sit To Stand transition và root motion.
4. Kiểm tra việc PlayerController và animation cùng điều khiển Transform.
5. Kiểm tra Cinemachine/custom camera follow, damping, update order.
6. Kiểm tra camera có đang bị đẩy bởi collider giường hoặc camera pivot nằm sai vị trí không.
7. Xác định root cause dựa trên code/scene thực tế, không đoán rồi sửa bừa.

### Kết quả mong muốn

- Ban đầu Kiều ngồi đúng vị trí trên giường.
- Nhấn E một lần.
- Animation Sit To Stand phát đúng.
- Trong khi đứng dậy, camera ổn định, không giật/xuyên giường.
- Kiều không teleport, trượt chân hay mắc collider.
- Khi animation kết thúc, chuyển sang Idle.
- Movement và Third-Person Camera được kích hoạt lại đúng thời điểm.
- Không cho spam E gây trigger animation lặp.

Nếu cần, triển khai camera transition riêng trong lúc đứng dậy và blend về GameplayCamera. Nhưng phải giải quyết nguyên nhân gốc, không chỉ che lỗi.

---

## 7. QUY TẮC CODE VÀ TÍCH HỢP

- Reuse kiến trúc hiện tại.
- Không tạo manager/script trùng chức năng.
- Không sửa Chapter 2, 3, 4 nếu không thật sự cần và chưa được tôi đồng ý.
- Không sửa các asset mà đồng đội đang làm nếu không cần.
- Không xóa file, prefab, scene hoặc asset khi chưa hỏi.
- Không đổi tên public API, Ink knot, biến hoặc tag đang được sử dụng mà không cập nhật và kiểm chứng mọi nơi tham chiếu.
- Không phá Inspector references, prefab links hoặc `.meta`.
- Chỉ đọc các file liên quan để tiết kiệm token.
- Nếu MCP không hỗ trợ một thao tác, dùng phương pháp Unity Editor/script phù hợp hoặc báo hạn chế; không giả định thao tác đã thành công.
- Tránh thay đổi package/dependency nếu không bắt buộc.

---

## 8. KIỂM TRA BẮT BUỘC

### Ink

- Compile từ đúng entrypoint.
- Không còn lỗi biến `tinh_tao`, `tu_trong`.
- Không lỗi knot, divert, choice, INCLUDE hoặc tag.
- Các gameplay gate vẫn hoạt động.
- Các lựa chọn quan trọng không bị mất.
- Không có Narrator rỗng hoặc dialogue bị kẹt.

### Gameplay

Test theo trình tự:

1. Kiều bắt đầu ở giường.
2. Nhấn E ngồi dậy, camera không lag.
3. Kiều đi tới Mẹ.
4. Hội thoại Kiều – Mẹ hoạt động.
5. Hoàn tất tìm hiểu các manh mối cần thiết.
6. Tiếng gõ cửa phát đúng.
7. Kiều nhấn E mở cửa.
8. Mã xuất hiện đúng vị trí với animation.
9. Hội thoại Kiều – Mã đổi camera tự nhiên.
10. Kiều kiểm tra khế và ký.
11. Người đưa tin xuất hiện.
12. Mẹ trao trâm.
13. Chapter 1 kết thúc đúng.

Kiểm tra Console, Animator, colliders, camera, audio và UI.

Không báo Play Mode đã thành công nếu chưa kiểm chứng thực tế.

---

## 9. QUY TRÌNH LÀM VIỆC

Thực hiện theo thứ tự:

**Phase 1 — Audit:** Xác định runtime active, đọc code/Ink/scene, phát hiện lỗi và đánh giá cốt truyện.

**Phase 2 — Critique:** Ghi nhận ngắn các điểm chưa hợp lý và đề xuất cách cải thiện. Phản biện khi yêu cầu của tôi có rủi ro. Chỉ hỏi khi cần quyết định làm thay đổi cốt truyện chính.

**Phase 3 — Implement:** Sửa từng nhóm chức năng nhỏ, đúng phạm vi.

**Phase 4 — Verify:** Compile Ink/C#, chạy kiểm tra qua Unity MCP và Play Mode nếu khả thi.

**Phase 5 — Review:** Kiểm tra Git diff, bảo đảm chỉ các file cần thiết bị thay đổi.

## 10. BÁO CÁO CUỐI

Trả lời ngắn gọn bằng tiếng Việt:

1. Đường dẫn Ink và scene runtime thực sự sử dụng.
2. Đánh giá và phản biện cốt truyện.
3. Các lỗi đã tìm thấy và nguyên nhân.
4. Những gì đã sửa.
5. Các file thay đổi.
6. Kết quả Ink compile / C# compile / Play Mode.
7. Những hạn chế hoặc đề xuất cần tôi quyết định.

**Không chỉ nói “đã hoàn thành” khi chưa thực sự kiểm tra. Bắt đầu audit ngay.**
