=== chapter_2 ===
# chapter:2
# scene:TuBa_TiepKhachDuong
# music:tense_plucked_strings

# speaker:Narrator
Hai ngày sau, Kiều không được đưa về một nhà chồng.

# speaker:Narrator
Nàng bị đưa đến một căn nhà treo đèn đỏ cả đêm. Người chờ ở đó không phải mẹ chồng, mà là Tú Bà.

# speaker:TuBa
# emotion:assessing
# camera:Reveal_TuBa
Tú Bà: Đây là người ngươi nói đáng bốn trăm lạng?

# speaker:MaGiamSinh
# emotion:businesslike
Mã Giám Sinh: Nhan sắc, đàn, thơ, chữ — thứ nào cũng đáng tiền.

# speaker:Kieu
# emotion:disgust_hidden
# camera:CU_Kieu
Kiều: Vậy ra hai người bàn chuyện về ta ngay trước mặt ta, vì nghĩ món hàng không biết nghe?

# speaker:TuBa
# emotion:amused
Tú Bà: Ta thích con bé này. Miệng sắc. Nếu biết dùng đúng chỗ, lại càng sinh tiền.

# speaker:Narrator
Tú Bà ném tờ khế lên bàn. Kiều thoáng thấy con dấu dưới khế khác hẳn con dấu Mã Giám Sinh đã dùng ở Vương gia.

# speaker:TuBa
Tú Bà: Từ hôm nay, nợ của ngươi thuộc về ta. Ngoan ngoãn thì dễ sống.

# speaker:Kieu
# emotion:controlled
Kiều: Ta có thể xin một đêm suy nghĩ không?

# speaker:TuBa
# emotion:suspicious
Tú Bà: Một đêm thôi. Cửa khóa. Sân có người canh. Đừng làm chuyện ngu ngốc.

# speaker:Narrator
Đêm xuống. Kiều có rất ít thời gian. Nàng chỉ kịp làm hai việc trước khi nhà đổi phiên canh.

-> c2_hub


=== c2_hub ===
{so_viec >= 2:
    -> c2_escape_setup
}

* {not bang_chung} [Lẻn đến phòng sổ sách của Tú Bà.]
    -> c2_evidence

* {not biet_cua_sau} [Quan sát lối đi và tìm cửa sau.]
    -> c2_backdoor

* {tinh_tao < 4} [Nghe lén cuộc cãi vã giữa Tú Bà và Mã Giám Sinh.]
    -> c2_eavesdrop


=== c2_evidence ===
# scene:TuBa_SoSach
# music:stealth
# camera:OTS_Kieu_Ledger

# speaker:Narrator
Kiều men theo hành lang, dừng trước phòng sổ sách. Cửa không khóa hẳn — Tú Bà quá tin vào người canh ngoài sân.

# speaker:Narrator
Trong ngăn bàn có hai tờ giấy đặt cạnh nhau: một "hôn ước" mang tên Kiều, và một sổ mua bán ghi rõ Mã Giám Sinh nhận tiền môi giới.

* [Lấy cả trang sổ có tên mình.]
    ~ bang_chung = true
    ~ so_viec += 1
    # speaker:Kieu
    # emotion:focused
    Kiều: Một tờ giấy biến người thành món hàng. Vậy thì một tờ giấy khác cũng có thể chứng minh kẻ nào đã bán người.

    # speaker:Narrator
    Nàng gấp trang sổ, giấu vào lớp áo trong.
    -> c2_hub

* [Không lấy, chỉ ghi nhớ nội dung để tránh bị phát hiện.]
    ~ tinh_tao += 1
    ~ so_viec += 1
    # speaker:Narrator
    Kiều đọc thật nhanh từng tên, từng khoản tiền. Nàng không mang được vật chứng, nhưng đã biết trò lừa vận hành thế nào.
    -> c2_hub


=== c2_backdoor ===
# scene:TuBa_HanhLangSau
# camera:Tracking_Kieu

# speaker:Narrator
Kiều giả vờ đi lấy nước rồi đếm từng nhịp chân của người canh.

# speaker:Narrator
Cuối hành lang có một cửa nhỏ dẫn ra ngõ chợ. Khóa cửa treo bên trong, nhưng then gỗ đã mục.

* [Thử tháo then ngay.]
    ~ biet_cua_sau = true
    ~ so_viec += 1
    # speaker:Narrator
    Kiều nới then ra vừa đủ, rồi đặt lại như cũ. Chỉ cần một cú đẩy đúng lúc.
    -> c2_hub

* [Đánh dấu đường đi rồi quay về phòng.]
    ~ biet_cua_sau = true
    ~ tinh_tao += 1
    ~ so_viec += 1
    # speaker:Kieu
    # emotion:thoughtful
    Kiều: Trốn không khó. Khó là trốn sao để họ không kịp bắt lại.
    -> c2_hub


=== c2_eavesdrop ===
# scene:TuBa_Corridor
# camera:CU_Kieu_Listens

# speaker:MaGiamSinh
# emotion:angry
Mã Giám Sinh: Ta mang người đến, bà còn đòi bớt tiền?

# speaker:TuBa
# emotion:mocking
Tú Bà: Ngươi dùng con dấu giả mà còn muốn chia đủ? Nếu quan phủ ngửi thấy mùi, người vào ngục trước là ngươi.

# speaker:Narrator
Kiều nín thở.

# speaker:MaGiamSinh
Mã Giám Sinh: Bà cũng sạch sẽ gì? Sổ nợ trong phòng kia đủ chôn cả hai ta.

~ tinh_tao += 2
~ so_viec += 1

# speaker:Kieu
# emotion:realization
Kiều: Thì ra không phải ta mắc nợ họ. Chính họ đang sợ món nợ của mình bị phơi ra ánh sáng.

-> c2_hub


=== c2_escape_setup ===
# scene:TuBa_PhongKieu
# music:tension_rise

# speaker:Narrator
Canh ba. Tú Bà bước vào phòng Kiều.

# speaker:TuBa
# emotion:stern
Tú Bà: Nghĩ xong chưa? Sáng mai ta cho người dạy ngươi quy củ.

# speaker:Kieu
# emotion:calm
Kiều: Ta chỉ có một thắc mắc.

# speaker:TuBa
Tú Bà: Nói.

# speaker:Kieu
Kiều: Mã Giám Sinh bảo bà trả thiếu tiền cho hắn.

# speaker:TuBa
# emotion:angry
Tú Bà: Hắn nói thế?

# speaker:Kieu
Kiều: Hắn còn nói... bà giữ sổ sách chẳng kín.

# speaker:Narrator
Ngoài sân, đúng lúc Mã Giám Sinh vừa quay lại đòi tiền.

# speaker:MaGiamSinh
# emotion:shouting_offscreen
Mã Giám Sinh: Tú Bà! Ra đây nói cho rõ!

# speaker:TuBa
# emotion:furious
Tú Bà: Tên khốn này!

# speaker:Narrator
Tú Bà bỏ đi. Kiều chỉ có vài nhịp thở.

* {biet_cua_sau} [Chạy thẳng đến cửa sau đã chuẩn bị.]
    # speaker:Narrator
    # camera:RunCam_Kieu
    Kiều lao qua hành lang, đẩy mạnh then gỗ đã nới.

    # speaker:Narrator
    Cửa bật ra. Gió đêm quất vào mặt nàng như một cái tát — đau, nhưng là cái đau của tự do.
    -> c2_chase

* {not biet_cua_sau} [Dùng lúc hỗn loạn tìm đường qua bếp.]
    ~ tinh_tao += 1
    # speaker:Narrator
    Kiều luồn qua bếp, hất một rổ củi chắn lối rồi trèo qua cửa sổ thấp.

    # speaker:Narrator
    Sau lưng nàng, tiếng Tú Bà gào lên.
    -> c2_chase


=== c2_chase ===
# scene:PhoDem_Chase
# music:chase

# speaker:TuBa
# emotion:rage
# camera:TuBa_Doorway
Tú Bà: BẮT NÓ LẠI!

# speaker:Narrator
Kiều chạy xuyên qua ngõ chợ tối. Dép rơi mất một chiếc. Tóc xổ khỏi trâm. Tiếng chân phía sau mỗi lúc một gần.

# speaker:Kieu
# emotion:exhausted_determined
Kiều: Lần đầu ta rời nhà vì muốn cứu người thân.

# speaker:Kieu
Kiều: Lần này ta chạy... để cứu chính mình.

# speaker:Narrator
Nàng lao qua cầu gỗ, đẩy chiếc xe hàng chắn ngang lối. Khi bọn người đuổi tới, Kiều đã hòa vào dòng người chuẩn bị cho chợ sớm.

# speaker:Narrator
Đến khi trời sáng, trong tay nàng không có nhà, không có người che chở — và gần như không có tiền.

# speaker:Kieu
# emotion:tired_small_smile
Kiều: Tự do thật đắt.

# speaker:Kieu
Kiều: Nhưng ít nhất... lần này người trả giá là ta.

-> chapter_3


// ============================================================
// CHƯƠNG 3 — KIẾM TIỀN CÓ GÌ KHÓ?
// Chủ đề tương tác: Kiếm tiền mà không trở thành người từng lợi dụng mình.
// Thời lượng dự kiến: 5–7 phút.
// Người chơi chọn 2 trong 3 công việc; đây là phần replayable của demo.
