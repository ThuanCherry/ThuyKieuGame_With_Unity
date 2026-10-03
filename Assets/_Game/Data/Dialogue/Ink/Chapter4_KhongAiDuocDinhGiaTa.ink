=== chapter_4 ===
# chapter:4
# scene:VuongGia_BinhMinh
# music:final_tension
# camera:Wide_Kieu_Returns

# speaker:Narrator
Kiều trở về khi trời vừa sáng. Vương Ông đã được về nhà, nhưng vụ án vẫn chờ kết luận cuối cùng.

# speaker:VuongOng
# emotion:overwhelmed
# camera:CU_VuongOng
Vương Ông: Con... còn sống.

# speaker:Kieu
# emotion:teary_smile
Kiều: Con còn nợ cha một lần trở về.

# speaker:Narrator
Kiều chưa kịp bước qua ngưỡng cửa thì tiếng vỗ tay vang lên phía sau.

# speaker:MaGiamSinh
# emotion:smug
Mã Giám Sinh: Cảm động lắm.

# speaker:TuBa
# emotion:cold
Tú Bà: Nhưng người của ta thì vẫn là người của ta.

# speaker:Kieu
# emotion:steady
# camera:Hero_Kieu_Turn
Kiều: Ta tưởng hai người bận cãi nhau chuyện chia tiền.

# speaker:TuBa
# emotion:angry
Tú Bà: Con ranh!

# speaker:MaGiamSinh
Mã Giám Sinh: Khế còn đây. Nàng đã ký. Theo luật, nàng phải theo chúng ta.

# speaker:VuongOng
# emotion:guilty
Vương Ông: Là tại cha... Nếu không vì cha...

# speaker:Kieu
# emotion:firm_soft
Kiều: Không.

# speaker:Kieu
Kiều: Cha gặp nạn không biến con thành tài sản của bất kỳ ai.

# speaker:Narrator
Kiều đặt túi bạc xuống bàn đá trước sân.

# speaker:Kieu
Kiều: Đây là số tiền gia đình ta cần. Món nợ tiền bạc, ta trả bằng tiền.

# speaker:Kieu
Kiều: Còn món "nợ" mà hai người tự viết lên đời ta... hôm nay ta trả bằng sự thật.

// Người chơi được chọn cách đối đầu dựa trên những gì đã làm trước đó.

* {bang_chung} [Đưa trang sổ mua bán ra trước mọi người.]
    -> c4_use_evidence

* {not bang_chung && tinh_tao >= 3} [Buộc Mã Giám Sinh và Tú Bà tự mâu thuẫn trước mặt nhân chứng.]
    -> c4_use_logic

* {not bang_chung && tinh_tao < 3} [Kêu gọi những người từng làm ăn với Kiều đứng ra làm chứng cho nàng.]
    -> c4_use_reputation


=== c4_use_evidence ===
# camera:Insert_Ledger
# speaker:Kieu
# emotion:confident
Kiều: Tờ khế của ông gọi là hôn ước. Nhưng sổ của Tú Bà ghi rõ: "tiền mua người" và "phí môi giới cho Mã Giám Sinh".

# speaker:MaGiamSinh
# emotion:panic
Mã Giám Sinh: Ngươi ăn cắp sổ!

# speaker:Kieu
Kiều: Vậy ông thừa nhận sổ này là thật?

# speaker:Narrator
Mã Giám Sinh cứng họng.

# speaker:TuBa
# emotion:rage
Tú Bà: Đồ ngu!

# speaker:Narrator
Người dân trước cổng bắt đầu xì xào. Một quan sai đang chờ xét lại vụ án Vương gia bước tới nhận trang sổ.

# speaker:QuanSai
Quan sai: Giấy này đủ để mở một vụ khác. Mời hai người theo chúng ta.

~ danh_tieng += 2
-> c4_resolution


=== c4_use_logic ===
# speaker:Kieu
# emotion:calm_trap
Kiều: Mã công tử nói ta là vợ ông. Tú Bà lại nói ta là người của bà. Một người không thể vừa là vợ của người này, vừa là tài sản của người kia.

# speaker:Kieu
Kiều: Vậy xin hai vị nói trước mặt quan sai: rốt cuộc bốn trăm lạng kia là sính lễ hay tiền mua bán?

# speaker:MaGiamSinh
# emotion:panicked
Mã Giám Sinh: Tất nhiên là sính lễ!

# speaker:TuBa
# emotion:angry_interrupt
Tú Bà: Sính lễ cái gì? Ta đã trả ngươi tiền môi giới—

# speaker:TuBa
# emotion:frozen
Tú Bà: ...

# speaker:Narrator
Sân im bặt.

# speaker:Kieu
# emotion:small_smile
Kiều: Cảm ơn bà. Phần còn lại chắc quan sai hỏi sẽ rõ hơn.

# speaker:QuanSai
Quan sai: Hai người, theo chúng ta.

~ danh_tieng += 1
-> c4_resolution


=== c4_use_reputation ===
# speaker:Narrator
Kiều không có sổ sách trong tay. Nàng chỉ có lời mình.

# speaker:TuBa
# emotion:smug
Tú Bà: Không bằng không chứng. Ai tin ngươi?

# speaker:Narrator
Từ ngoài cổng, chủ trà quán, khách buôn và vài người trong chợ lần lượt bước tới.

# speaker:ChuQuan
Chủ quán: Ta tin.

# speaker:KhachBuon
Khách buôn: Ta cũng vậy. Cô ấy làm việc bằng tên thật, nhận tiền bằng công sức thật.

# speaker:Narrator
Một người không đủ sức chống lại một tờ khế. Nhưng nhiều người cùng lên tiếng khiến quan sai không thể làm ngơ.

# speaker:QuanSai
Quan sai: Việc này còn nhiều nghi vấn. Tạm giữ hai người để đối chất.

# speaker:Kieu
# emotion:relieved
Kiều: Có những ngày danh tiếng không mua được cơm.

# speaker:Kieu
Kiều: Nhưng hôm nay, nó mua cho ta quyền được người khác lắng nghe.

-> c4_resolution


=== c4_resolution ===
# music:release
# scene:VuongGia_Sunrise

# speaker:Narrator
Mã Giám Sinh và Tú Bà bị áp giải đi để điều tra việc dùng khế giả và mua bán người.

# speaker:Narrator
Số bạc Kiều mang về giúp Vương gia thanh toán khoản nợ còn lại và thuê người theo vụ án đến cùng.

# speaker:VuongOng
# emotion:ashamed
Vương Ông: Cha từng nghĩ con hy sinh vì gia đình là điều đáng tự hào.

# speaker:VuongOng
Vương Ông: Bây giờ cha mới hiểu... nếu một gia đình chỉ sống được bằng cách đẩy một người xuống vực, thì đó không phải là cứu.

# speaker:Kieu
# emotion:gentle
Kiều: Con vẫn chọn cứu cha nếu phải làm lại.

# speaker:Kieu
Kiều: Nhưng con sẽ không còn nghĩ chữ Hiếu bắt mình phải biến mất.

# speaker:Narrator
Một giọng quen thuộc vang lên ngoài cổng.

# speaker:KimTrong
# emotion:breathless
# camera:Reveal_KimTrong
Kim Trọng: Kiều!

# speaker:Narrator
Kim Trọng vừa trở về sau chuyến đi xa. Chàng nhìn túi bạc, quan sai phía xa, chiếc dép không cùng đôi của Kiều... rồi im lặng khá lâu.

# speaker:KimTrong
# emotion:confused_worried
Kim Trọng: Ta đi có mấy tuần.

# speaker:KimTrong
Kim Trọng: Nàng... đã làm gì vậy?

# speaker:Kieu
# emotion:deadpan_then_smile
# camera:CU_Kieu_Final
Kiều: Không có gì.

# speaker:Kieu
Kiều: Cứu cha. Trốn khỏi một cuộc mua bán. Chạy qua nửa cái chợ. Đàn thuê. Viết thư thuê. Buôn vải. Kiếm bốn trăm lạng. Rồi về nhà.

# speaker:KimTrong
# emotion:stunned
Kim Trọng: ...Không có gì?

# speaker:Kieu
# emotion:warm_smile
Kiều: Ừ.

# speaker:Kieu
Kiều: Kiếm tiền có gì khó nhỉ?

# speaker:Narrator
# camera:Wide_Family_Sunrise
Nắng sớm rơi xuống sân Vương gia.

# speaker:Narrator
Kiều đã không thắng số phận bằng cách trở nên mạnh hơn tất cả mọi người.

# speaker:Narrator
Nàng thắng bằng cách thôi tin rằng số phận của mình phải do người khác định giá.

{tu_trong >= 4:
    # ending:bright
    # speaker:Narrator
    Từ đó, Kiều mở một lớp nhỏ dạy đàn và chữ cho những cô gái muốn tự kiếm sống. Người ta nhớ tiếng đàn của nàng, nhưng nàng muốn họ nhớ hơn cả một điều: tài năng có thể nuôi mình mà không cần bán mình.
}

{tu_trong < 4:
    # ending:reflective
    # speaker:Narrator
    Kiều không tự nhận mình đã luôn đúng. Có những đồng bạc nàng kiếm được bằng cách khiến người khác chịu phần thiệt. Nàng giữ chúng như lời nhắc rằng tự do không chỉ là thoát khỏi kẻ xấu — mà còn là không trở thành họ.
}

# speaker:Narrator
# music:main_theme
# camera:TitleCard
HAPPY END — THÚY KIỀU: KIẾM TIỀN CÓ GÌ KHÓ NHỈ?

-> END
