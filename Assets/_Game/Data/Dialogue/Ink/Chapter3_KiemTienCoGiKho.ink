=== chapter_3 ===
# chapter:3
# scene:ThiTran_BinhMinh
# music:hopeful

# speaker:Narrator
Kiều đến một thị trấn cách đó nửa ngày đường. Nàng thuê một góc phòng nhỏ bằng chiếc trâm cuối cùng còn giữ được.

# speaker:Narrator
Tin từ quê gửi đến: án của Vương Ông chưa khép lại. Muốn trả hết khoản nợ và thuê người làm chứng, gia đình vẫn cần khoảng bốn trăm lạng.

# speaker:Kieu
# emotion:thinking
# camera:CU_Kieu_Table
Kiều: Ta từng nghĩ bán mình là cách nhanh nhất để có tiền.

# speaker:Kieu
Kiều: Có lẽ vì ta chưa từng thử định giá tài năng của chính mình.

# speaker:Narrator
Trong những ngày tiếp theo, Kiều bắt đầu kiếm sống bằng điều nàng thật sự sở hữu: tiếng đàn, con chữ và đầu óc.

-> c3_hub


=== c3_hub ===
{so_viec >= 4:
    -> c3_after_work
}

* {not da_dan} [Nhận đàn tại một trà quán đông khách.]
    -> c3_music

* {not da_viet_thu} [Nhận viết thư thuê cho một khách buôn trẻ.]
    -> c3_letter

* {not da_buon_vai} [Giúp một cửa hàng vải thương lượng với khách lớn.]
    -> c3_trade


=== c3_music ===
~ da_dan = true
~ so_viec += 1
# scene:TraQuan
# music:dan_tranh
# camera:Stage_Kieu

# speaker:Narrator
Trà quán đông nghịt. Sau một khúc đàn, chủ quán đặt trước Kiều hai túi bạc.

# speaker:ChuQuan
# emotion:tempting
Chủ quán: Khách ở phòng riêng muốn cô nương sang đàn thêm. Họ trả gấp đôi. Chỉ cần... biết chiều ý một chút.

# speaker:Narrator
Câu nói ấy khiến Kiều nhớ đến ánh mắt của Tú Bà.

* [Chỉ nhận đàn ở gian công khai.]
    ~ tien += 220
    ~ danh_tieng += 2
    ~ tu_trong += 1
    # speaker:Kieu
    # emotion:firm_smile
    Kiều: Ta bán tiếng đàn, không bán quyền định đoạt ta. Nếu khách muốn nghe, mời họ ngồi ngoài này.

    # speaker:Narrator
    Vài người cười. Vài người khó chịu. Nhưng sáng hôm sau, trà quán lại đông hơn vì người ta truyền nhau về "cô nương đánh đàn không chịu cúi đầu".
    -> c3_hub

* [Nhận lời, nhưng yêu cầu cửa mở và chủ quán đứng làm chứng.]
    ~ tien += 250
    ~ tinh_tao += 1
    ~ danh_tieng += 1
    # speaker:Kieu
    # emotion:clever
    Kiều: Ta nhận. Nhưng cửa phải mở, chủ quán đứng ngoài, tiền trả trước.

    # speaker:ChuQuan
    Chủ quán: Cô nương đề phòng quá.

    # speaker:Kieu
    Kiều: Người từng suýt bị bán hai lần thường học nhanh.
    -> c3_hub


=== c3_letter ===
~ da_viet_thu = true
~ so_viec += 1
# scene:PhoSach
# camera:TwoShot_Kieu_Khach

# speaker:Narrator
Một khách buôn trẻ tìm đến nhờ Kiều viết thư cầu hôn con gái một nhà giàu.

# speaker:KhachBuon
# emotion:awkward
Khách buôn: Cô cứ viết rằng ta có ba cửa hàng, hai kho lụa và một căn nhà lớn.

# speaker:Kieu
# emotion:neutral
Kiều: Công tử có thật không?

# speaker:KhachBuon
Khách buôn: Chưa. Nhưng cưới xong biết đâu sẽ có.

# speaker:Kieu
Kiều: À. Công tử muốn thuê ta viết thư hay thuê ta nói dối?

* [Viết một lá thư chân thành, bỏ hết tài sản giả.]
    ~ tien += 210
    ~ danh_tieng += 2
    ~ tu_trong += 1
    # speaker:Kieu
    # emotion:warm
    Kiều: Nếu nàng ấy thích một người không tồn tại, thì đến ngày gặp công tử thật, cả hai đều khổ.

    # speaker:Narrator
    Khách buôn miễn cưỡng đồng ý. Vài ngày sau, hắn quay lại — không có hôn lễ, nhưng có một lời cảm ơn.

    # speaker:KhachBuon
    Khách buôn: Nàng ấy từ chối ta... nhưng giới thiệu ta làm ăn với anh trai nàng. Kỳ lạ thật.

    # speaker:Kieu
    Kiều: Thành thật đôi khi lãi chậm. Nhưng ít khi phá sản.
    -> c3_hub

* [Viết lá thư bóng bẩy, nhưng buộc khách ký tên vào lời mình nói.]
    ~ tien += 240
    ~ tinh_tao += 1
    # speaker:Kieu
    # emotion:dry_humor
    Kiều: Ta sẽ viết hay. Nhưng mọi điều về tài sản, công tử tự ký xác nhận phía dưới.

    # speaker:KhachBuon
    Khách buôn: Sao phải rắc rối vậy?

    # speaker:Kieu
    Kiều: Vì ta vừa học được rằng giấy trắng mực đen rất có ích khi người ta định quên lời mình.
    -> c3_hub


=== c3_trade ===
~ da_buon_vai = true
~ so_viec += 1
# scene:ChoVai
# camera:Market_Wide

# speaker:Narrator
Một chủ hàng vải thuê Kiều giúp thương lượng với đoàn thương nhân phương xa.

# speaker:ChuHang
# emotion:whispering
Chủ hàng: Lô này bị ẩm một ít. Cô nương cứ khen là lụa thượng hạng. Chốt được giá cao, ta chia thêm bạc.

# speaker:Kieu
# emotion:disappointed
Kiều: Nếu họ phát hiện?

# speaker:ChuHang
Chủ hàng: Khi ấy họ đã đi xa rồi.

* [Nói rõ khuyết điểm, đổi lại thương lượng giá hợp lý.]
    ~ tien += 230
    ~ danh_tieng += 2
    ~ tu_trong += 1
    # speaker:Kieu
    # emotion:confident
    Kiều: Lụa có vết ẩm, nhưng sợi tốt. Giảm một phần giá, các vị vẫn lời khi nhuộm màu đậm.

    # speaker:Narrator
    Đoàn thương nhân đồng ý. Chủ hàng kiếm ít hơn dự tính, nhưng bán sạch lô vải và còn có khách quay lại.

    # speaker:ChuHang
    Chủ hàng: Cô nương làm ta hụt một khoản hôm nay.

    # speaker:Kieu
    Kiều: Nhưng giữ cho ông một cái tên để ngày mai còn bán hàng.
    -> c3_hub

* [Dùng lời khéo che đi vết ẩm để lấy giá cao.]
    ~ tien += 270
    ~ tu_trong -= 1
    # speaker:Narrator
    Kiều nói rất hay. Đoàn thương nhân mua hàng với giá cao.

    # speaker:Narrator
    Nhưng khi cầm phần bạc được chia, nàng chợt nhớ Mã Giám Sinh cũng từng dùng lời đẹp để che một tờ khế xấu.

    # speaker:Kieu
    # emotion:uneasy
    Kiều: Tiền này nhẹ hơn ta tưởng... mà sao cầm nặng tay thế.
    -> c3_hub


=== c3_after_work ===
# scene:PhongTro_Dem
# music:reflective

# speaker:Narrator
Sau nhiều ngày, số bạc Kiều dành dụm cuối cùng cũng vượt qua khoản gia đình cần.

# speaker:Narrator
Nàng đặt từng túi bạc lên bàn.

# speaker:Kieu
# camera:CU_Kieu_Coins
Kiều: Bốn trăm lạng.

# speaker:Kieu
# emotion:small_laugh
Kiều: Hóa ra kiếm tiền... đúng là khó.

# speaker:Narrator
Nàng cười một mình. Rồi nụ cười chậm rãi biến mất.

# speaker:Kieu
# emotion:reflective
Kiều: Nhưng khó nhất không phải kiếm đủ.

# speaker:Kieu
Kiều: Khó nhất là lúc thiếu tiền, mình rất dễ tin rằng có quyền làm điều sai với người khác để sống sót.

{tu_trong >= 4:
    # speaker:Narrator
    Kiều nhìn số bạc. Không đồng nào khiến nàng phải tránh ánh mắt của chính mình.
}

{tu_trong < 4:
    # speaker:Narrator
    Trong số bạc ấy có vài đồng khiến Kiều không thấy thanh thản. Nàng hiểu mình chưa khác những kẻ kia chỉ nhờ một quyết định; phải khác họ trong từng quyết định nhỏ.
}

# speaker:Narrator
Sáng hôm sau, Kiều lên đường về quê.

# speaker:Narrator
Nàng nghĩ mình chỉ cần trả nợ là mọi chuyện kết thúc.

# speaker:Narrator
Nhưng trước cổng Vương gia, hai người đã chờ sẵn.

-> chapter_4


// ============================================================
// CHƯƠNG 4 — KHÔNG AI ĐƯỢC ĐỊNH GIÁ TA
// Chủ đề tương tác: Tiền giải quyết món nợ; sự thật và cộng đồng giải quyết xiềng xích.
// Happy End bắt buộc, nhưng sắc thái thay đổi theo lựa chọn trước đó.
// Thời lượng dự kiến: 4–6 phút.
