# Chapter 1 — audit và kết quả sửa

## Phase 1 — Runtime active

- Unity MCP xác nhận project `D:/ThuyKieuKiemTien/thuykieu2.0`, Scene `Assets/_Game/Scenes/Chapter01/Chapter01_GiaBien.unity`.
- `Chapter01Director._compiledInk` trỏ `Assets/_Game/Data/Dialogue/Ink/Main.json`.
- `InkStoryCompiler.Source` là `Assets/_Game/Data/Dialogue/Ink/Main.ink`; entrypoint `chapter_1`, INCLUDE `Chapter1_GiaBien.ink` cùng thư mục. `DialogueManager.InkStory` được Director tạo từ JSON này.
- Kiểm tra Play ban đầu cho thấy Story đã khởi tạo trước khi asset được biên dịch lại: dialogue cũ vẫn đang chạy dù TextAsset đã được cập nhật. Các lượt xác minh sau đều khởi động Play mới.
- Bản giảm Narrator của người dùng và các sửa animation/font/camera trước nhiệm vụ được giữ lại. Không sửa Chapter 2–4; không xóa asset.

## Phase 2 — Phản biện

- Động cơ mẹ bảo vệ con, Kiều cứu gia đình nhưng giữ quyền tự quyết, Mã che giấu giao dịch đều nhất quán. Giữ lời thoại, biến, lựa chọn và kết quả.
- Giảm Narrator là phù hợp, nhưng comment `STAGING` không tự tạo hành động trong Unity. Không đánh đồng comment với cảnh đã dựng.
- Ink gộp tag vào dòng thoại kế tiếp: opening mất nhịp, hai chuỗi gõ chồng nhau, ký giấy/chuyển thời gian bị dồn vào gate. Sửa cơ chế cue có thời lượng, không khôi phục lời dẫn dài.
- Chapter 1 vẫn có nhiều lời thoại tài chính và tự quyết. Mục tiêu 8–10 phút chiếm phần lớn tổng 15–25 phút của bốn chương; nên đo thời gian đọc của người chơi trước khi cắt thêm. Không rút lời thoại máy móc.
- Gate khám phá hiện là 2/3 manh mối; giữ nguyên và vẫn cho xem món thứ ba. Đổi thành bắt buộc 3/3 sẽ thay đổi gameplay gate nên chưa thực hiện.
- Không thêm Cinemachine: manifest không có package này; camera hiện có đáp ứng nhiệm vụ và tránh thêm hệ điều khiển camera.

## Phase 3 — Implement

- Thêm dòng điều khiển `@cue` và tag `wait:` có runtime xử lý; ẩn Dialogue UI trong cue, chặn Continue/choice bỏ qua thời lượng. Không phát textbox rỗng hay nội dung `@cue`.
- Tách tiếng mưa, tiếng khóc, fade, gõ 1 + 2 nhịp, mở cửa, ký giấy, chuyển thời gian và nhìn lại lúc kết. Giữ knot/biến/choice/gate.
- Audio gõ dùng anchor cửa chính và chống coroutine trùng. Âm mở cửa vẫn do tương tác cửa phát, Ink không phát lại.
- Mã bắt đầu giữa lối vào ngoài ngưỡng cửa, cập nhật Humanoid pose trước dựng camera; giữ di chuyển vào hall và root motion tắt.
- Reuse DialogueTagRouter và ThirdPersonCameraController: Two-Shot, cận Kiều, cận Mã; explicit camera trước speaker fallback; giữ góc tối thiểu và blend về gameplay. Góc cửa được chọn theo đường nhìn/vật cản và viewport, cận được dựng từ pose đang chạy.
- Capsule Kiều đang ngồi chồng vào mép giường: bed x tối đa -4.17, player x -4.239, bán kính capsule .25. Khi đứng dậy, khóa input/position và tạm ngừng capsule depenetration; chuyển .5 m theo thời gian animation tới marker có khoảng trống; bật lại capsule và blend camera sau khi về Idle. Chặn spam E.
- Setup opening trước đây gán controller Kiều cho mẹ, ghi đè phân vai animation. Sửa để mẹ tiếp tục dùng shared controller/Talking2.
- Người đưa tin trước đây chỉ là anchor offscreen. Thêm nhân vật scene tại anchor, hiện khi báo tin và ẩn trước cảnh ra đi; tạm reuse model Mã hiện có, cần model riêng để hoàn thiện hình ảnh.

## Phase 4–5 — Verification và giới hạn

- `unity recompile --format json`: thành công, không lỗi/cảnh báo compiler.
- `Chapter01InkReworkCheck`: 36 tổ hợp 3 cặp manh mối × 4 thăm dò × 3 lựa chọn khế; kiểm tra stat, gate, inventory, ending và persistence; xem `Tools/Chapter01/ReworkInkReport.json`.
- `Chapter01ReworkPlayCheck` chạy input E/WASD thực tế từ giường tới kết và chuyển sang Chapter02 placeholder; bổ sung kiểm tra cue/UI, 3 nhịp gõ, camera ở cửa và hall, người đưa tin hiện hình. Lượt cuối đạt 89/89 kiểm tra; kết quả ở `Tools/Chapter01/ReworkPlayReport.json`. Thời gian ~60 giây là test bấm nhanh, không phải thời lượng đọc thực tế.
- Các lượt test đã phát hiện và sửa Two-Shot xuyên đường nhìn front wall và cận Mã lấy bind pose khiến mặt nằm dưới khung UI; không dùng báo cáo lượt lỗi để kết luận đạt.
- Git diff được kiểm tra theo file; các thay đổi có trước nhiệm vụ được giữ. Main.json là output compiler hiện hữu.
- Giới hạn mỹ thuật: người đưa tin dùng model tạm; chưa có animation ôm, tháo/trao trâm, viết chữ hoặc người hầu mang hòm chuyên biệt. Ký giấy dùng gesture có sẵn và SFX giấy; bạc chuyển bằng fade và tin báo. Không tuyên bố các động tác chuyên biệt này đã hoàn thiện.
