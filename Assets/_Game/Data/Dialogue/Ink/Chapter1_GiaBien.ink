=== chapter_1 ===
# chapter:1
# scene:VuongGia_DemMua
# music:sad_strings

# speaker:Narrator
# camera:WS_VuongGia
Mưa quất lên mái ngói Vương gia. Trong sân, dấu chân quan sai còn lẫn với bùn đất.

# speaker:Narrator
Vương Ông và Vương Quan vừa bị áp giải đi vì một vụ án mà cả nhà đều biết là oan.

# speaker:MeKieu
# emotion:crying
# camera:MCU_MeKieu
Mẹ Kiều: Bốn trăm lạng... Nhà ta có bán hết cũng không đủ. Trời muốn dồn người ta đến đường cùng sao?

# speaker:Kieu
# emotion:sad_controlled
# camera:CU_Kieu
Kiều: Mẹ đừng khóc. Cha và em còn chờ chúng ta. Con sẽ tìm cách.

# speaker:Narrator
Ngoài cửa, tiếng người xướng danh vang lên.

# speaker:MaGiamSinh
# emotion:smug
# camera:Reveal_MaGiamSinh
Mã Giám Sinh: Kẻ hèn họ Mã, nghe Vương gia gặp nạn nên mạo muội đến. Ta... có thể giúp.

# speaker:MeKieu
# emotion:hopeful_uncertain
Mẹ Kiều: Công tử thật lòng muốn giúp sao?

# speaker:MaGiamSinh
# emotion:polite_fake
Mã Giám Sinh: Bốn trăm lạng, đổi lấy một cuộc hôn nhân. Vương cô nương theo ta, tiền sẽ đến tay quan sai ngay trong đêm.

# speaker:Narrator
# camera:CU_Kieu_Eyes
Kiều nhìn người đàn ông trước mặt. Quần áo sang, lời nói nhã, nhưng ánh mắt lại giống người đang định giá một món hàng.

* [Hỏi thẳng lai lịch của Mã Giám Sinh.]
    ~ tinh_tao += 1
    # speaker:Kieu
    # emotion:calm_probe
    Kiều: Công tử quê quán nơi nào? Gia thất ra sao? Hôn nhân là chuyện cả đời, xin đừng trách ta hỏi kỹ.

    # speaker:MaGiamSinh
    # emotion:irritated_hidden
    Mã Giám Sinh: Chuyện ấy... sau này nàng tự khắc biết. Việc trước mắt là cứu cha nàng, chẳng phải sao?

    # speaker:Narrator
    Hắn trả lời rất nhanh ở chuyện tiền, nhưng lại chậm ở chuyện thân thế.
    -> c1_contract

* [Im lặng, quan sát cách hắn nói về tiền.]
    ~ tinh_tao += 1
    # speaker:Narrator
    # camera:POV_Kieu_MaHands
    Kiều không đáp. Nàng nhìn túi bạc, nhìn tờ khế, rồi nhìn ngón tay Mã Giám Sinh gõ liên tục lên bàn.

    # speaker:Kieu
    # emotion:thoughtful
    Kiều: Người thật lòng cầu thân thường hỏi về người mình cưới. Công tử từ lúc vào nhà chỉ hỏi... bao giờ ký giấy.

    # speaker:MaGiamSinh
    # emotion:defensive
    Mã Giám Sinh: Cô nương thông minh quá đôi khi cũng khổ thân đấy.
    -> c1_contract

* [Chỉ hỏi một điều: "Tiền đến tay cha ta trước hay sau khi ta đi?"]
    ~ tinh_tao += 2
    # speaker:Kieu
    # emotion:firm
    Kiều: Ta chỉ hỏi một điều. Tiền đến tay cha ta trước, hay sau khi ta bước khỏi cửa này?

    # speaker:MaGiamSinh
    # emotion:caught_off_guard
    Mã Giám Sinh: ...Trước.

    # speaker:Kieu
    Kiều: Vậy xin ghi điều đó vào khế.

    # speaker:Narrator
    Nụ cười của Mã Giám Sinh khựng lại trong một nhịp.
    -> c1_contract


=== c1_contract ===
# scene:VuongGia_BanKyKhe
# camera:Table_Contract

# speaker:Narrator
Mã Giám Sinh đặt lên bàn một tờ khế đã viết sẵn. Dòng chữ "hôn ước" nằm trên cùng, nhưng phía dưới có những câu chữ cố tình viết nhỏ.

* [Đọc kỹ tờ khế trước khi ký.]
    ~ tinh_tao += 1
    # speaker:Narrator
    Kiều nhận ra một câu lạ: "Người nhận có toàn quyền định đoạt nơi ở và công việc của người giao".

    # speaker:Kieu
    # emotion:cold
    Kiều: Hôn ước mà lại viết như giấy mua bán súc vật. Công tử dùng văn từ thật mới lạ.

    # speaker:MaGiamSinh
    # emotion:annoyed
    Mã Giám Sinh: Chỉ là lệ buôn... à, lệ làm giấy ở chỗ ta.

    # speaker:Narrator
    Kiều ghi nhớ từng chữ. Nàng chưa đủ sức lật bàn, nhưng ít nhất đã biết mình đang bước vào đâu.
    -> c1_decision

* [Ký ngay để tiền được đưa đi cứu cha.]
    ~ tu_trong -= 1
    # speaker:Narrator
    Kiều cầm bút. Mực chạm giấy nhanh hơn cả một hơi thở.

    # speaker:Kieu
    # emotion:pained
    Kiều: Một chữ của ta đổi lấy mạng người nhà. Nếu đây là cái giá duy nhất... ta trả.

    # speaker:Narrator
    Nhưng khi đặt bút xuống, Kiều thấy Mã Giám Sinh mỉm cười. Nàng hiểu: nếu cứ coi mình là cái giá phải trả, sẽ luôn có kẻ khác định giá nàng.
    -> c1_decision


=== c1_decision ===
# speaker:MeKieu
# emotion:crying
Mẹ Kiều: Con ơi... hay là thôi. Mẹ không thể nhìn con tự bán đời mình.

# speaker:Kieu
# emotion:soft_firm
# camera:CU_Kieu
Kiều: Con đi không phải vì con rẻ hơn bốn trăm lạng.

# speaker:Kieu
Kiều: Con đi vì cha và em cần được cứu ngay hôm nay. Nhưng chuyện đời con... con chưa giao cho ai cả.

# speaker:Narrator
Mã Giám Sinh sai người mang bạc đi. Tin báo trở lại: Vương Ông và Vương Quan được tạm thả để chờ xét lại vụ án.

# speaker:MaGiamSinh
# emotion:satisfied
Mã Giám Sinh: Giờ thì lên đường thôi, nương tử.

# speaker:Kieu
# emotion:determined
Kiều: Đừng gọi ta như thế vội.

# speaker:Narrator
# camera:Hero_Kieu_Rain
Kiều bước ra khỏi nhà. Sau lưng là chữ Hiếu. Trước mặt là một chiếc lồng chưa đóng cửa.

# speaker:Narrator
Và nàng quyết định: trước khi cánh cửa ấy khép lại, nàng phải tìm được chìa khóa.

-> chapter_2


// ============================================================
// CHƯƠNG 2 — CÁI GIÁ CỦA MỘT CON NGƯỜI
// Chủ đề tương tác: Quan sát, bằng chứng và việc tự cứu mình.
// Thời lượng dự kiến: 4–6 phút.
