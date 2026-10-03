# TMP tiếng Việt dùng chung

Chọn `VietnameseTMP.asset` trong ô **Font Asset** của TextMeshPro/TextMeshProUGUI.
Material mặc định và texture atlas nằm bên trong asset, không cần gán thủ công.

Font dùng Liberation Sans có giấy phép SIL Open Font License đi kèm `OFL.txt`.
Atlas SDF 2048 × 2048 đã bake sẵn ở chế độ Static: không phụ thuộc font cài trên Windows,
không cần sinh thêm glyph khi chạy game. Bao gồm ASCII, Latin, toàn bộ dấu và chữ tiếng Việt
hoa/thường, dấu kết hợp Unicode, ký hiệu tiền đồng, dấu câu và mũi tên.

## Chia sẻ với máy khác

1. Gửi `Tools/Fonts/VietnameseTMP.unitypackage`, import vào project Unity có TextMeshPro/uGUI.
2. Chọn `Assets/Fonts/VietnameseTMP.asset` cho các thành phần TMP cần hiển thị tiếng Việt.
3. Nếu chép thư mục trực tiếp, chép **toàn bộ Assets/Fonts và các file .meta**, kể cả Fonts.meta.
   Package đã kèm các dependency shader để import thuận tiện hơn.
4. Material preset từ font khác không dùng được cho atlas này; chọn material mặc định của
   VietnameseTMP hoặc tạo preset mới từ nó.

Để mặc định cho TMP mới: Project Settings → TextMeshPro → Default Font Asset → VietnameseTMP.
Đổi mặc định không tự thay font của các Text/TMP có sẵn. Unity UI Text dùng LiberationSans.ttf.

Văn bản nên lưu UTF-8 và chuẩn hóa Form C (NFC) để đặt dấu kết hợp chính xác.
Font có dấu Unicode tách rời nhưng cách đặt dấu NFD phụ thuộc phiên bản TMP.
Font không chứa đầy đủ emoji, chữ Hán hoặc các hệ chữ ngoài Latin.

Menu trong project này: ThuyKieu → Fonts → Validate / Export portable Vietnamese TMP.
Mã tạo font: Assets/_Game/Scripts/UI/Editor/VietnameseFontSetup.cs.
