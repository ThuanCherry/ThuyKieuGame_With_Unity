Bạn là Senior Unity Gameplay & Cinematic Developer.
Sử dụng Unity MCP để kiểm tra và chỉnh sửa scene:

Assets/\_Game/Scenes/Chapter01/Chapter01_GiaBien.unity

Yêu cầu:

- Kiểm tra GameObject, Animator, collider và những phần liên quan.
- Kiểm tra âm thanh crying của mẹ Thúy Kiều hoạt động ổn chưa
- Kiểm tra và cải thiện Third-Person Camera của Thúy Kiều trong Unity.
- Tôi có thêm animation open door, và think cho Kiều
- Kiểm tra xem thử sau khi nói chuyện xong thì nghe tiếng Gõ cửa bên của Mã Giám Sinh, nếu chưa thì bạn thêm vào
- Mã giám sinh thì đứng ở giữa cánh cửa, fix cái đó luôn
  Vấn đề hiện tại:

Camera xoay quá nhanh khi di chuột.

Góc nhìn chưa mềm mại và tự nhiên.

Tôi muốn cảm giác camera giống game phiêu lưu góc nhìn thứ ba, ưu tiên khám phá và hội thoại.

Yêu cầu:

Audit hệ thống Camera và PlayerController hiện có.

Xác định đang sử dụng Cinemachine hay custom camera.

Giảm mouse sensitivity và điều chỉnh damping/smoothing hợp lý.

Kiểm tra cách xử lý mouse delta và Time.deltaTime để tránh nhân sai tốc độ.

Kiểm tra camera follow, rotation, LateUpdate và khả năng rung do update không đồng bộ.

Kiểm tra Character Rotation để Kiều xoay người mềm mại.

Giữ nguyên hệ thống movement, animation và interaction hiện tại.

Nếu phù hợp, thêm tùy chỉnh Mouse Sensitivity trong Inspector.

- NHIỆM VỤ: Triển khai thử nghiệm Cinematic Dialogue Camera cho cuộc hội thoại giữa Thúy Kiều và Mẹ Kiều trong Chapter 1.
  Scene:
  Assets/\_Game/Scenes/Chapter01/Chapter01_GiaBien.unity

1. Quyền và cách làm việc

Sử dụng Unity MCP để kiểm tra/chỉnh sửa trực tiếp Unity Editor khi công cụ hỗ trợ.

Audit Camera, DialogueManager, Ink, PlayerController và scene hiện có.

Ưu tiên tái sử dụng hệ thống và Cinemachine nếu đã cài.

Không tạo CameraManager hoặc DialogueManager trùng lặp.

Không làm lại environment, nhân vật hoặc gameplay đã hoàn thành.

Chỉ tập trung cuộc hội thoại Kiều – Mẹ Kiều.

Được toàn quyền chỉnh sửa, tạo script/prefab, compile và test.

Phải hỏi trước khi xóa file hoặc asset.

Tiết kiệm token: chỉ đọc những file liên quan, không giải thích dài.

2. Cinematic Camera cần triển khai

Tạo hoặc cấu hình 3 góc camera tái sử dụng.

Camera A — Dialogue_TwoShot

Thấy cả Kiều và Mẹ Kiều.

Góc quay tự nhiên kiểu phim cổ trang.

Dùng khi bắt đầu hội thoại và những khoảnh khắc tình cảm.

Camera không bị bàn ghế che khuất.

Camera B — Dialogue_MeKieu

Medium Close-Up hướng vào Mẹ Kiều đang ngồi trên ghế.

Camera gần tầm mắt nhân vật khi ngồi.

Thấy mặt, vai và một phần tay.

Làm nổi bật cảm xúc buồn, khóc.

Không zoom sát mặt quá mức.

Camera C — Dialogue_Kieu

Medium Close-Up hoặc Over-the-Shoulder.

Tập trung vào khuôn mặt Kiều.

Góc ngang tầm mắt, hơi lệch 30–45 độ.

Thể hiện sự dịu dàng và điềm tĩnh.

Không đặt camera phía sau khiến người chơi chỉ nhìn thấy lưng Kiều.

3. Logic chuyển Camera

Khi player đi tới gần Mẹ Kiều và hội thoại bắt đầu:

Khóa movement và mouse-look gameplay.

Chuyển mượt từ GameplayCamera sang TwoShot.

Mẹ Kiều nói → ưu tiên camera Mẹ Kiều.

Kiều nói → ưu tiên camera Kiều.

Đoạn xúc động hoặc Kiều an ủi mẹ → TwoShot.

Khi hội thoại kết thúc → chuyển mượt về GameplayCamera.

Khôi phục movement và mouse-look.

QUAN TRỌNG:

Không cắt camera sau mỗi dòng Ink.

Không liên tục đổi góc khi cùng một nhân vật đang nói.

Chỉ chuyển khi đổi người nói hoặc có sự kiện cảm xúc quan trọng.

Ưu tiên camera tự nhiên, không giật và không xuyên tường.

Blend tham khảo 0.4–0.8 giây.

FOV tham khảo 35–50 độ.

Không để người chơi điều khiển camera trong hội thoại.

4. Tích hợp Ink

Đọc hệ thống speaker/camera tags hiện tại.

Ví dụ:

# speaker:MeKieu → Dialogue_MeKieu

# speaker:Kieu → Dialogue_Kieu

# camera:MCU_MeKieu → Dialogue_MeKieu

# camera:MCU_Kieu → Dialogue_Kieu

# camera:WS_MainHall → Dialogue_TwoShot

Ưu tiên tag camera rõ ràng nếu có; speaker làm fallback.

Không hard-code lời thoại trong C#.

Không sửa nội dung cốt truyện Ink, trừ lỗi tích hợp kỹ thuật bắt buộc.

Nếu camera tag không tồn tại, sử dụng camera mặc định phù hợp. Không gây NullReference.

5. Dialogue Box và bố cục

Giữ box hội thoại phía dưới màn hình.

Camera phải giữ mắt, khuôn mặt và cử chỉ nhân vật ở phía trên box.

Không để dialogue box che miệng nhân vật.

Tiếp tục dùng VietnameseTMP.asset.

Hỗ trợ lời thoại dài, không tràn chữ.

Đảm bảo hiển thị ổn ở 1920×1080 và 1366×768.

6. Animation nhân vật

Mẹ Kiều tiếp tục ngồi trên ghế trong hội thoại.

Giữ Sitting Idle/Sitting Talking nếu đang hoạt động.

Kiều đứng đối diện mẹ với vị trí và hướng nhìn tự nhiên.

Khi đổi góc quay không làm nhân vật teleport, giật xoay hoặc thay đổi animation sai.

Không cần tạo lip-sync.

7. Kiểm tra bắt buộc

Sử dụng Unity MCP và Play Mode nếu có thể:

Player đi đến Mẹ Kiều.

Dialogue bắt đầu.

Camera chuyển sang TwoShot mượt.

Mẹ Kiều nói → camera hướng đúng Mẹ.

Kiều nói → camera hướng đúng Kiều.

Không đổi camera liên tục gây khó chịu.

Nhân vật không bị che bởi UI hoặc vật thể.

Kết thúc hội thoại → quay lại Third-Person Camera.

Mouse-look và movement hoạt động bình thường.

Không compile error, NullReference hoặc Camera conflict.

Chỉ có một AudioListener active.

Chỉ coi hoàn thành nếu đã kiểm tra được hành vi thực tế. Nếu không thể chạy Play Mode, ghi rõ phần chưa xác minh, không báo đã test thành công.

Sử dụng Unity MCP để kiểm tra và chỉnh sửa nếu công cụ hỗ trợ.

Compile, Play/Test, sửa lỗi nếu có.

- Ưu tiên chỉnh sửa trực tiếp qua Unity MCP nếu công cụ hỗ trợ.
- Reuse hệ thống hiện có, không tạo code trùng.
- Tự compile, kiểm tra Console và test nếu có thể.
- Không tự ý xóa file hoặc asset; phải hỏi tôi trước, giải thích ngắn gọn.
- Tiết kiệm token: chỉ đọc file liên quan, không giải thích dài.

Sau khi hoàn thành, báo cáo ngắn những gì đã sửa, đã test và phần chưa xác minh.
