// ============================================================
// THÚY KIỀU: KIẾM TIỀN CÓ GÌ KHÓ NHỈ?
// Demo 4 chương | Mục tiêu thời lượng: 15–25 phút
// Tone: Cổ đại, hài nhẹ, châm biếm, cảm xúc
//
// Gợi ý tích hợp Unreal/MetaHuman:
// - Các tag # speaker / # emotion / # camera có thể được đọc từ Ink runtime
//   để đổi nhân vật nói, facial expression và góc máy.
// - Có thể bỏ tag nếu hệ thống hiện tại chưa dùng tới.
// ============================================================

VAR tinh_tao = 0
VAR danh_tieng = 0
VAR tu_trong = 2
VAR tien = 0
VAR bang_chung = false
VAR biet_cua_sau = false
VAR da_dan = false
VAR da_viet_thu = false
VAR da_buon_vai = false
VAR so_viec = 0

CONST MUC_TIEU_TIEN = 400

INCLUDE Chapter1_GiaBien.ink
INCLUDE Chapter2_CaiGiaCuaMotConNguoi.ink
INCLUDE Chapter3_KiemTienCoGiKho.ink
INCLUDE Chapter4_KhongAiDuocDinhGiaTa.ink

-> chapter_1
