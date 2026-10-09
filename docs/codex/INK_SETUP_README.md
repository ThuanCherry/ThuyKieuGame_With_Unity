# Thúy Kiều — Ink split cho Unity

## File
- `Main.ink`: biến toàn cục + INCLUDE 4 chương + entry point.
- `Chapter1_GiaBien.ink`
- `Chapter2_CaiGiaCuaMotConNguoi.ink`
- `Chapter3_KiemTienCoGiKho.ink`
- `Chapter4_KhongAiDuocDinhGiaTa.ink`
- `CHAPTER1_CODEX_PROMPT.md`: prompt giao Codex dựng Chương 1 playable.

## Cách dùng Ink
Đặt toàn bộ `.ink` cùng một thư mục trong Unity và dùng `Main.ink` làm entry script.
Các chapter vẫn giữ knot `chapter_1`, `chapter_2`, ... . Chapter 1 kết thúc tại
`chapter_end:1` rồi `DONE`; Unity lưu trạng thái và chọn `chapter_2` khi chuyển scene.

Nguồn thực tế: `Assets/_Game/Data/Dialogue/Ink/Main.ink`; đầu ra: `Main.json` cùng thư mục.
`gate` là điểm tạm dừng cho gameplay. Các knot `c1_clue_debt`, `c1_clue_money`,
`c1_clue_jewelry` được gọi bởi vật thể trong scene, giữ nguyên cùng một Ink Story.

## Dialogue
Thiết kế mặc định: chữ + choices trên UI, không cần voice audio.
