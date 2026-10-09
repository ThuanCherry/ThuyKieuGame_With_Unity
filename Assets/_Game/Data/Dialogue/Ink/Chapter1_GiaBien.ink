// ============================================================
// CHAPTER 1 — GIA BIẾN
// Chủ đề:
// Hiếu thảo không đồng nghĩa với việc từ bỏ quyền lựa chọn.
//
// Bối cảnh:
// Vương gia — đêm mưa.
//
// Thời lượng mục tiêu:
// 8–10 phút.
//
// LƯU Ý:
// Các biến toàn cục như tinh_tao, tu_trong...
// được giả định đã tồn tại trong Main.ink.
// ============================================================


// ------------------------------------------------------------
// BIẾN RIÊNG CHAPTER 1
// ------------------------------------------------------------

VAR c1_clues = 0

VAR c1_xem_to_trat = false
VAR c1_xem_tui_bac = false
VAR c1_xem_nu_trang = false

VAR c1_hoi_lai_lich = false
VAR c1_quan_sat_ma = false
VAR c1_hoi_400 = false
VAR c1_tien_truoc = false

VAR c1_doc_ky_khe = false
VAR c1_co_tram_gia_dinh = false


// ============================================================
// CHAPTER 1
// ============================================================

=== chapter_1 ===

# chapter:1
# scene:VuongGia_DemMua
# music:sad_strings


// ============================================================
// CẢNH 1 — TIẾNG KHÓC TRONG ĐÊM
// ============================================================

# camera:Fade_Black
# sfx:rain_muffled
# speaker:Narrator
Màn hình vẫn còn tối. Chỉ có tiếng mưa rơi đều trên mái ngói Vương gia, khi gần khi xa theo từng cơn gió.

# sfx:woman_crying_distant
# speaker:Narrator
Giữa tiếng mưa, từ gian nhà phía ngoài vọng vào một tiếng khóc rất khẽ. Không phải tiếng khóc thành lời, mà là những tiếng nấc bị cố nén xuống như thể người khóc không muốn đánh thức ai.

# camera:FadeIn_KieuRoom
# speaker:Narrator
Ánh sáng dần trở lại. Trong phòng riêng của Thúy Kiều, ngọn đèn dầu trên bàn đã cháy gần cạn, chỉ còn một quầng sáng nhỏ lay động trên vách.

# speaker:Narrator
Kiều ngồi lặng bên mép giường một lúc, cố nghe xem tiếng động ban nãy có phải mình nằm mơ hay không.

# speaker:Kieu
# emotion:worried
Kiều: ...Mẹ?

# speaker:Narrator
Không có tiếng trả lời. Chỉ một lúc sau, tiếng nấc lại vọng tới từ gian chính.

# speaker:Kieu
# emotion:worried
Kiều: Giờ này mẹ vẫn chưa nghỉ sao...?

# gameplay:enable_player
# objective:Tim_Me_Kieu
# gate:hallway

// Unity có thể trả quyền điều khiển tại đây.
// Player tự rời phòng Kiều và đi về gian chính.

-> c1_hanh_lang


// ============================================================
// CẢNH 1B — DẤU VẾT CÒN LẠI
// ============================================================

=== c1_hanh_lang ===

# camera:Gameplay_Kieu
# speaker:Narrator
Kiều bước ra khỏi phòng. Càng đi về phía gian chính, nàng càng nhận ra căn nhà đêm nay khác hẳn mọi ngày.

# speaker:Narrator
Cửa chính vẫn còn hé mở. Gió mang theo hơi mưa thổi vào nền nhà. Một chiếc ghế nằm nghiêng bên cạnh bàn, dưới đất là mảnh chén vỡ và những dấu giày dính bùn kéo dài từ ngoài sân vào tận bên trong.

# speaker:Kieu
# emotion:uneasy
Kiều: Những dấu chân này... là của người ngoài.

# speaker:Kieu
Kiều: Chiếc ghế của cha cũng bị xô đổ. Rốt cuộc tối nay đã xảy ra chuyện gì?

# speaker:Narrator
Kiều không dừng lại lâu. Tiếng khóc của mẹ vẫn còn ở phía trước.

-> c1_mainhall


// ============================================================
// CẢNH 2 — MẸ KIỀU
// ============================================================

=== c1_mainhall ===
# gate:mother

# camera:WS_MainHall
# speaker:Narrator
Ở gian chính, Mẹ Kiều đang ngồi một mình bên bàn. Trước mặt bà là một tờ giấy của quan phủ, một túi bạc nhỏ và chiếc hộp nữ trang đã mở nắp.

# speaker:Narrator
Bà cúi đầu rất thấp. Hai tay vẫn giữ chặt chiếc khăn đã thấm nước mắt đến nhàu nhĩ.

# speaker:Kieu
# emotion:soft_worried
Kiều: Mẹ.

# camera:MCU_MeKieu
# speaker:Narrator
Mẹ Kiều giật mình. Bà vội đưa tay lau nước mắt như thể vẫn còn muốn giấu con gái chuyện vừa xảy ra.

# speaker:MeKieu
# emotion:crying_controlled
Mẹ Kiều: Kiều nhi... sao con còn chưa ngủ? Trời đã khuya lắm rồi, lại lạnh thế này. Con về phòng nghỉ đi, chuyện ngoài này để mẹ lo.

# camera:MCU_Kieu
# speaker:Kieu
# emotion:sad_controlled
Kiều: Con nghe tiếng mẹ khóc nên mới ra đây. Cha không có trong phòng, Quan nhi cũng không thấy đâu. Ngoài cửa thì toàn dấu chân người lạ, bàn ghế lại bị xô đổ như vừa có một trận giằng co.

# speaker:Kieu
Kiều: Mẹ đừng giấu con nữa. Cha và em đã xảy ra chuyện gì?

# speaker:Narrator
Mẹ Kiều quay mặt đi. Bà định nói, nhưng phải mất một lúc mới có thể cất thành lời.

# speaker:MeKieu
# emotion:crying
Mẹ Kiều: Lúc tối... người của quan phủ tới.

# speaker:MeKieu
Mẹ Kiều: Họ nói có người tố cha con liên quan đến một vụ án. Cha con hết lời giải thích rằng mình không làm, Quan nhi cũng đứng ra phân trần, nhưng chẳng ai chịu nghe.

# speaker:MeKieu
Mẹ Kiều: Họ lục tung cả nhà. Đồ đạc bị xô đổ, giấy tờ bị giở tung. Sau đó họ trói cả cha con lẫn Quan nhi rồi áp giải đi ngay trong mưa.

# speaker:MeKieu
Mẹ Kiều: Mẹ chạy theo đến tận cửa, van xin họ cho thêm thời gian... nhưng ngoài sân toàn là quan sai. Mẹ chỉ có thể đứng nhìn cha con và em con bị đưa đi mà chẳng làm được gì.

# speaker:Narrator
Kiều không nói ngay. Nàng nhìn chiếc ghế bị đổ phía sau mẹ, rồi nhìn những vệt bùn vẫn còn kéo dài trên nền nhà.

# speaker:Kieu
# emotion:controlled_pain
Kiều: Họ đưa cha và Quan nhi đi đâu?

# speaker:MeKieu
Mẹ Kiều: Công đường.

# speaker:MeKieu
Mẹ Kiều: Có một người quen của cha con đã chạy đi dò hỏi. Vừa rồi người ấy mới trở về báo tin rằng họ vẫn đang giữ cả hai người.

# speaker:MeKieu
Mẹ Kiều: Nếu muốn vụ án được xem xét lại, nếu muốn cha con và Quan nhi có cơ hội trở về... họ đòi nhà ta phải lo một khoản bạc trước sáng mai.

# camera:Insert_DebtPaper
# speaker:Kieu
# emotion:tense
Kiều: Bao nhiêu?

# speaker:Narrator
Mẹ Kiều nhìn con gái. Bà gần như không muốn nói ra con số ấy.

# speaker:MeKieu
# emotion:broken
Mẹ Kiều: Bốn trăm lạng.

# speaker:Narrator
Căn phòng im hẳn. Ngoài kia, tiếng mưa dường như trở nên rõ hơn.

# speaker:Kieu
# emotion:thoughtful
Kiều: Bốn trăm lạng...


// ============================================================
// CẢNH 3 — GIA ĐÌNH KHÔNG CÒN ĐƯỜNG LÙI
// ============================================================

# speaker:MeKieu
# emotion:crying_controlled
Mẹ Kiều: Mẹ đã ngồi tính từ lúc họ rời đi đến giờ. Tiền cha con dành dụm bao năm, số bạc mẹ để dành phòng lúc đau ốm, tiền còn lại trong nhà... gom hết lại cũng chẳng được bao nhiêu.

# speaker:MeKieu
Mẹ Kiều: Mẹ còn lấy cả nữ trang của mình ra. Nếu cần, nữ trang của con mẹ cũng sẽ đem bán. Nhưng giữa đêm thế này, người ta biết nhà mình đang cần tiền, họ sẽ ép giá. Kể cả bán hết... vẫn còn thiếu quá nhiều.

# speaker:Kieu
# emotion:calm
Kiều: Còn ruộng đất của nhà ta?

# speaker:MeKieu
Mẹ Kiều: Ai sẽ mua ngay trong một đêm? Cho dù tìm được người mua, giấy tờ sang nhượng cũng đâu thể làm xong trước sáng.

# speaker:Kieu
Kiều: Còn căn nhà này?

# speaker:MeKieu
# emotion:pained
Mẹ Kiều: Nếu có người chịu mua ngay, mẹ cũng bán.

# speaker:Narrator
Mẹ Kiều nhìn quanh căn nhà đã gắn với gia đình bao năm.

# speaker:MeKieu
Mẹ Kiều: Nhưng mẹ sợ bán cả mái nhà cũng chưa chắc kịp. Mà nếu mất luôn nơi này... đến lúc cha con và Quan nhi trở về, chúng ta sẽ đưa họ về đâu?

# speaker:Narrator
Kiều ngồi xuống bên cạnh mẹ và nhẹ nhàng nắm lấy tay bà.

# speaker:Kieu
# emotion:soft_firm
Kiều: Mẹ nhìn con này. Chuyện chưa đến sáng, vậy vẫn chưa phải đường cùng.

# speaker:Kieu
Kiều: Con biết bốn trăm lạng là số tiền rất lớn. Nhưng chúng ta càng hoảng sợ thì càng không thể nghĩ được gì. Mẹ hãy kể cho con nghe thật rõ tất cả những gì người của quan phủ đã nói và những gì người đi dò tin đã báo về.

# speaker:MeKieu
# emotion:crying
Mẹ Kiều: Mẹ không tiếc tiền, Kiều nhi. Căn nhà này, ruộng đất, số của cải cha con tích góp bao năm... nếu đổi được cha con và Quan nhi trở về bình an, mẹ chẳng tiếc một thứ nào.

# speaker:MeKieu
Mẹ Kiều: Điều mẹ sợ là cho dù chúng ta có gom đủ tiền, họ vẫn tìm cách làm khó. Mẹ càng sợ hơn rằng cha con ở trong đó đang chịu khổ, còn mẹ chỉ biết ngồi ở đây đếm từng đồng bạc.

# speaker:Kieu
# emotion:sad_controlled
Kiều: Cha từng dạy con rằng lúc mọi việc rối nhất thì càng phải nhìn cho rõ. Nếu trước mắt họ cần tiền, chúng ta tìm cách lo tiền. Nếu có người cố tình hại cha, sau này chúng ta tìm chứng cứ để minh oan.

# speaker:Kieu
Kiều: Nhưng đêm nay, điều quan trọng nhất là phải đưa cha và Quan nhi ra khỏi nơi đó trước đã.

# speaker:Narrator
Mẹ Kiều nhìn con gái thật lâu. Trong đôi mắt đỏ hoe của bà, nỗi lo vẫn còn nguyên nhưng đã bớt phần hoảng loạn.

# speaker:MeKieu
# emotion:soft_sad
Mẹ Kiều: Con nói giống cha con quá.

# speaker:Kieu
# emotion:gently_smiling
Kiều: Vậy thì mẹ đừng khóc nữa. Nếu cha trở về nhìn thấy mắt mẹ sưng thế này, người sẽ lại tự trách mình.

# speaker:Narrator
Mẹ Kiều cố lau nước mắt, nhưng càng lau, nước mắt càng rơi.

# speaker:Narrator
Kiều không nói thêm. Nàng chỉ ôm lấy mẹ.

# sfx:rain_room
# speaker:Narrator
Trong vài khoảnh khắc, căn phòng chỉ còn lại tiếng mưa.

-> c1_explore


// ============================================================
// CẢNH 4 — NGƯỜI CHƠI TÌM HIỂU TÌNH HÌNH
//
// Sau này Codex có thể chuyển phần này thành tương tác vật thể
// trong Unity thay vì Choice UI.
// ============================================================

=== c1_explore ===

{ c1_clues < 2:
    # speaker:Narrator
    Kiều biết mình cần nhìn kỹ những gì còn lại trong nhà trước khi quyết định bước tiếp.
}

# gameplay:enable_player
# objective:Tim_hieu_tinh_hinh_Vuong_Gia
# gate:explore
-> DONE

=== c1_clue_debt ===
{ c1_xem_to_trat: -> c1_explore }
~ c1_xem_to_trat = true
~ c1_clues += 1

# camera:Insert_DebtPaper
# speaker:Narrator
Kiều cầm tờ trát lên và đọc từng dòng một.

# speaker:Kieu
# emotion:thoughtful
Kiều: Bốn trăm lạng, thời hạn trước sáng... nhưng cả tờ giấy lại không có một dòng nào nói rõ bằng chứng buộc tội cha.

# speaker:Kieu
Kiều: Họ viết rất rõ số tiền phải nộp, nhưng lại chẳng viết rõ cha đã phạm tội gì.

# speaker:Narrator
Kiều đặt tờ trát xuống, trong lòng càng thêm bất an.

-> c1_explore

=== c1_clue_money ===
{ c1_xem_tui_bac: -> c1_explore }
~ c1_xem_tui_bac = true
~ c1_clues += 1

# camera:Insert_MoneyBag
# speaker:Narrator
Kiều mở túi bạc. Những đồng bạc bên trong va vào nhau phát ra âm thanh nhỏ đến đáng buồn.

# speaker:Kieu
# emotion:sad
Kiều: Chừng này còn chưa bằng một phần tư số họ đòi. Cha mẹ đã dành dụm bao năm, vậy mà trước một con số trên tờ giấy... lại trở nên nhỏ bé đến thế.

-> c1_explore

=== c1_clue_jewelry ===
{ c1_xem_nu_trang: -> c1_explore }
~ c1_xem_nu_trang = true
~ c1_clues += 1

# camera:Insert_Jewelry
# speaker:Narrator
Kiều mở chiếc hộp. Bên trong là những món trang sức Mẹ Kiều đã giữ từ những năm đầu về làm dâu Vương gia.

# speaker:Kieu
# emotion:soft_sad
Kiều: Mẹ đã đem cả những thứ này ra rồi...

# speaker:Kieu
Kiều: Bán hết gia sản để cứu người nhà đã là đau. Nhưng nếu bán hết mà vẫn không đủ thì phải làm sao?

# speaker:Narrator
Kiều khép chiếc hộp lại thật nhẹ.

-> c1_explore

=== c1_before_knock ===

# camera:MCU_Kieu_Me
# speaker:Kieu
# emotion:thoughtful
Kiều: Mẹ, bốn trăm lạng trong một đêm gần như là điều không thể với nhà ta. Nhưng con vẫn nghĩ phải còn một con đường nào đó. Ngày mai con sẽ—

# sfx:door_knock
# speaker:Narrator
Cốc.

# speaker:Narrator
Cả hai cùng dừng lại.

# sfx:door_knock_double
# speaker:Narrator
Cốc. Cốc.

# camera:MCU_MeKieu
# speaker:MeKieu
# emotion:afraid
Mẹ Kiều: Giờ này còn ai tới nữa? Hay... lại là người của quan phủ?

# speaker:Narrator
Mẹ Kiều đứng bật dậy. Sắc mặt vừa mới bình tĩnh lại lập tức tái đi.

# speaker:Kieu
# emotion:calm
Kiều: Mẹ ở đây. Con ra xem.

# speaker:MeKieu
# emotion:protective
Mẹ Kiều: Không được. Nếu thật sự là họ thì con càng không nên ra ngoài.

# speaker:Kieu
# emotion:soft_firm
Kiều: Nếu họ thật sự muốn vào, cánh cửa này cũng không ngăn được. Con chỉ ra hỏi xem là ai. Mẹ đừng lo.

# gameplay:enable_player
# objective:Ra_mo_cua
# gate:door

// Player tự đi tới cửa và tương tác.

-> c1_ma_arrives


// ============================================================
// CẢNH 6 — MÃ GIÁM SINH XUẤT HIỆN
// ============================================================

=== c1_ma_arrives ===

# sfx:door_open
# camera:Door_Reveal
# speaker:Narrator
Cánh cửa mở ra. Ngoài hiên là một người đàn ông đứng dưới mái che, y phục chỉnh tề đến mức gần như không mang dấu vết của người vừa đi giữa một đêm mưa.

# camera:Reveal_MaGiamSinh
# speaker:MaGiamSinh
# emotion:polite_fake
Mã Giám Sinh: Đêm khuya còn đến quấy rầy, quả thật thất lễ. Kẻ hèn họ Mã. Nghe tin Vương gia gặp biến cố nên mới mạo muội tìm tới.

# speaker:Narrator
Kiều không lập tức nhường đường.

# speaker:Kieu
# emotion:calm_probe
Kiều: Tin trong nhà ta truyền nhanh đến vậy sao?

# speaker:MaGiamSinh
# emotion:polite_fake
Mã Giám Sinh: Ở nơi đông người, chuyện đã tới công môn thì khó giữ kín. Huống hồ Vương gia cũng là gia đình có danh tiếng. Người quen biết hỏi thăm nhau lúc hoạn nạn vốn là chuyện thường.

# speaker:MeKieu
# emotion:uncertain
Mẹ Kiều: Công tử quen biết lão gia nhà tôi sao?

# speaker:MaGiamSinh
Mã Giám Sinh: Chưa có duyên gặp mặt.

# speaker:Narrator
Mẹ Kiều hơi khựng lại.

# speaker:MaGiamSinh
# emotion:polite_fake
Mã Giám Sinh: Nhưng người với người đôi khi không nhất thiết phải quen biết nhiều năm mới có thể giúp nhau lúc khó khăn. Nếu phu nhân không ngại, xin cho phép ta vào trong nói chuyện. Chuyện ta sắp nói... có lẽ không nên đứng ngoài cửa.

# speaker:Narrator
Kiều nhìn người đàn ông trước mặt thêm một lúc rồi mới lùi sang một bên.

-> c1_offer


// ============================================================
// CẢNH 7 — “TA CÓ THỂ GIÚP”
// ============================================================

=== c1_offer ===
# gate:offer

# camera:WS_MainHall_Ma
# speaker:Narrator
Mã Giám Sinh ngồi đối diện hai mẹ con. Hắn không vội nói. Cuối cùng, chính Mẹ Kiều là người mất kiên nhẫn trước.

# speaker:MeKieu
# emotion:desperate_controlled
Mẹ Kiều: Công tử nói mình biết chuyện nhà tôi, lại bảo có thể giúp. Xin công tử cứ nói thẳng. Chồng và con trai tôi hiện vẫn còn trong tay quan phủ. Lúc này tôi thật sự không còn tâm trí để đoán ý người khác nữa.

# speaker:MaGiamSinh
# emotion:calm
Mã Giám Sinh: Phu nhân nói phải.

# camera:Insert_DebtPaper
# speaker:Narrator
Mã Giám Sinh liếc về phía tờ trát trên bàn.

# speaker:MaGiamSinh
# emotion:controlled
Mã Giám Sinh: Bốn trăm lạng.

# camera:CU_Kieu
# speaker:Narrator
Kiều lập tức nhìn hắn.

# speaker:MaGiamSinh
Mã Giám Sinh: Với một gia đình phải xoay xở trong một đêm, đó quả thật là con số gần như không thể. Nhưng với ta, bốn trăm lạng chưa phải khoản tiền không thể thu xếp.

# speaker:MeKieu
# emotion:hopeful_uncertain
Mẹ Kiều: Ý công tử là... có thể cho nhà tôi vay sao?

# speaker:MaGiamSinh
# emotion:smug_hidden
Mã Giám Sinh: Nếu chỉ là chuyện vay tiền thì đơn giản quá.

# speaker:Narrator
Ánh hy vọng vừa xuất hiện trên khuôn mặt Mẹ Kiều chậm rãi biến mất.

# speaker:MaGiamSinh
# emotion:polite_fake
Mã Giám Sinh: Ta không tới đây để làm chủ nợ của Vương gia. Ta vốn đang muốn tìm một mối lương duyên, lại nghe danh Vương cô nương đã lâu — tài sắc, gia giáo, hiếu thuận.

# speaker:MaGiamSinh
Mã Giám Sinh: Nếu hôm nay hai nhà có thể kết một mối nhân duyên, ta sẵn lòng đứng ra lo liệu chuyện trước mắt. Bốn trăm lạng sẽ được đưa tới công môn ngay trong đêm.

# speaker:MeKieu
# emotion:shocked
Mẹ Kiều: Không được!

# speaker:Narrator
Mẹ Kiều đứng bật dậy.

# speaker:MeKieu
# emotion:angry_crying
Mẹ Kiều: Con gái tôi không phải thứ đem ra đổi lấy bạc. Dù nhà tôi có rơi vào đường cùng, tôi cũng không thể coi đời con mình như một món hàng để mặc cả.

# speaker:MaGiamSinh
# emotion:calm_fake
Mã Giám Sinh: Phu nhân hiểu lầm rồi. Ta chưa từng nói đến chuyện mua bán.

# speaker:MaGiamSinh
Mã Giám Sinh: Ta nói đến hôn nhân. Vương gia đang gặp nạn, ta đứng ra giúp. Sau đó Vương cô nương trở thành người nhà của ta. Xét cho cùng, chẳng phải hai bên đều có được điều mình cần hay sao?

# speaker:Narrator
Mẹ Kiều còn muốn phản bác, nhưng Kiều nhẹ nhàng đặt tay lên tay mẹ.

# speaker:Kieu
# emotion:calm
Kiều: Mẹ.

# speaker:Kieu
Kiều: Để con nói chuyện với công tử.

-> c1_probe


// ============================================================
// CẢNH 8 — THÚY KIỀU THĂM DÒ MÃ GIÁM SINH
// ============================================================

=== c1_probe ===

# camera:CU_Kieu_Ma

* [“Nếu đã nói đến hôn nhân, ít nhất ta cũng phải biết người muốn cưới mình là ai.”]
    ~ tinh_tao += 1
    ~ c1_hoi_lai_lich = true

    # speaker:Kieu
    # emotion:calm_probe
    Kiều: Công tử nói đến chuyện hôn nhân như một việc có thể quyết ngay trong tối nay. Nhưng ta còn chưa biết quê quán công tử ở đâu, gia đình thế nào, hiện làm việc gì. Nếu công tử thật lòng muốn cầu thân, những câu ấy hẳn không phải chuyện quá khó trả lời.

    # speaker:MaGiamSinh
    # emotion:irritated_hidden
    Mã Giám Sinh: Vương cô nương quả thật cẩn trọng. Ta là người buôn bán, thường xuyên đi lại nhiều nơi nên chuyện quê quán, gia nghiệp nếu nói trong một hai câu cũng khó đầy đủ. Sau khi về nhà ta, nàng sẽ tự khắc biết.

    # speaker:Kieu
    # emotion:thoughtful
    Kiều: Công tử có thể mang bốn trăm lạng đến cứu một gia đình chưa từng quen biết, nhưng lại không thể nói một câu rõ ràng về chính mình. Đúng là chuyện không dễ hiểu.

    # speaker:Narrator
    Lần đầu tiên, nụ cười trên môi Mã Giám Sinh mất đi vẻ tự nhiên.

    -> c1_contract


* [Không vội đáp. Quan sát cách hắn nói về tiền và những thứ hắn đã chuẩn bị.]
    ~ tinh_tao += 1
    ~ c1_quan_sat_ma = true

    # camera:POV_Kieu_MaHands
    # speaker:Narrator
    Kiều không trả lời ngay. Nàng nhìn chiếc hòm bạc phía sau Mã Giám Sinh, rồi nhìn một góc giấy được chuẩn bị sẵn lộ ra từ tay áo hắn.

    # speaker:Kieu
    # emotion:thoughtful
    Kiều: Công tử chuẩn bị chu đáo thật. Người trong nhà ta đến giờ còn chưa biết sẽ làm gì, vậy mà công tử đã mang đủ bạc, lại còn mang theo giấy tờ.

    # speaker:Kieu
    Kiều: Dường như trước khi bước qua cánh cửa này, công tử đã biết mình sẽ nhận được câu trả lời thế nào.

    # speaker:MaGiamSinh
    # emotion:defensive
    Mã Giám Sinh: Ta chỉ là người không thích làm việc thiếu chuẩn bị.

    # speaker:Kieu
    Kiều: Vậy sao? Ta thì lại nghĩ người chuẩn bị quá kỹ thường là người đã biết trước rất nhiều chuyện.

    -> c1_contract


* [“Tại sao công tử biết chính xác nhà ta cần bốn trăm lạng?”]
    ~ tinh_tao += 2
    ~ c1_hoi_400 = true

    # speaker:Kieu
    # emotion:firm
    Kiều: Ta có một câu muốn hỏi. Tờ giấy này được đưa tới nhà ta chưa lâu. Người ngoài đáng lẽ chỉ biết cha và em ta gặp chuyện.

    # speaker:Kieu
    Kiều: Vậy tại sao công tử lại biết chính xác con số bốn trăm lạng?

    # speaker:Narrator
    Trong một nhịp rất ngắn, Mã Giám Sinh không trả lời.

    # speaker:MaGiamSinh
    # emotion:caught_off_guard
    Mã Giám Sinh: Ta có vài người quen ở công môn. Nghe chuyện Vương gia gặp nạn, ta tiện hỏi thêm đôi câu.

    # speaker:Kieu
    # emotion:calm_probe
    Kiều: Chỉ vài câu... mà đến cả số bạc trên tờ trát cũng biết rõ.

    # speaker:MaGiamSinh
    # emotion:defensive
    Mã Giám Sinh: Cô nương đang nghi ngờ thiện ý của ta sao?

    # speaker:Kieu
    # emotion:calm
    Kiều: Không. Ta chỉ không muốn vì đang gặp nạn mà quên mất cách đặt câu hỏi.

    -> c1_contract


* [“Tiền phải đến tay quan phủ trước khi ta bước khỏi căn nhà này.”]
    ~ tinh_tao += 2
    ~ c1_tien_truoc = true

    # speaker:Kieu
    # emotion:firm
    Kiều: Nếu công tử thật sự muốn giúp, ta có một điều kiện. Bốn trăm lạng phải được đưa tới quan phủ trước khi ta rời khỏi Vương gia. Ta phải biết cha và em mình được an toàn trước.

    # speaker:MaGiamSinh
    # emotion:caught_off_guard
    Mã Giám Sinh: Cô nương không tin ta?

    # speaker:Kieu
    # emotion:calm
    Kiều: Chúng ta vừa gặp nhau chưa đầy một khắc. Nếu công tử ở vị trí của ta, công tử có tin không?

    # speaker:Narrator
    Mã Giám Sinh không trả lời câu hỏi ấy.

    # speaker:MaGiamSinh
    Mã Giám Sinh: ...Được.

    # speaker:Kieu
    Kiều: Vậy xin ghi điều đó vào giấy.

    # speaker:Narrator
    Nụ cười trên môi Mã Giám Sinh khựng lại rất nhẹ.

    -> c1_contract


// ============================================================
// CẢNH 9 — TỜ KHẾ
// ============================================================

=== c1_contract ===
# gate:contract

# scene:VuongGia_BanKyKhe
# camera:Table_Contract

# speaker:Narrator
Mã Giám Sinh lấy từ trong tay áo một tờ giấy đã được chuẩn bị từ trước rồi đặt lên bàn.

# speaker:MaGiamSinh
# emotion:polite_fake
Mã Giám Sinh: Ta vốn nghĩ chuyện này sớm muộn cũng sẽ được nhắc tới, nên đã cho người chuẩn bị trước một bản hôn ước. Có giấy trắng mực đen, sau này hai bên cũng tránh được chuyện nói qua nói lại.

# speaker:Kieu
# emotion:suspicious
Kiều: Công tử đã chuẩn bị hôn ước trước cả khi biết ta có đồng ý hay không?

# speaker:MaGiamSinh
Mã Giám Sinh: Có chuẩn bị vẫn tốt hơn không.

{ c1_tien_truoc:
    # speaker:Kieu
    # emotion:firm
    Kiều: Vậy thêm vào đó điều chúng ta vừa thống nhất. Bốn trăm lạng phải được giao tới quan phủ trước khi ta rời khỏi căn nhà này.

    # speaker:MaGiamSinh
    # emotion:annoyed_hidden
    Mã Giám Sinh: Ta đã đồng ý thì tự nhiên sẽ giữ lời.

    # speaker:Kieu
    Kiều: Nếu đã định viết giấy, vậy càng nên viết cho rõ.

    # speaker:Narrator
    Mã Giám Sinh im lặng một lúc rồi mới đồng ý cho ghi thêm điều khoản.
}

# speaker:Narrator
Trên đầu tờ giấy là hai chữ “HÔN ƯỚC”. Nhưng càng xuống phía dưới, những dòng chữ càng nhỏ và dày hơn.

* [Đọc kỹ toàn bộ tờ khế trước khi đặt bút.]
    ~ tinh_tao += 1
    ~ c1_doc_ky_khe = true

    # speaker:Narrator
    Kiều đọc từng dòng, chậm hơn hẳn cách Mã Giám Sinh mong muốn. Cuối cùng, ngón tay nàng dừng lại ở một câu.

    # speaker:Narrator
    “Người nhận có toàn quyền định đoạt nơi ở và công việc của người giao.”

    # speaker:Kieu
    # emotion:cold
    Kiều: Công tử gọi đây là hôn ước sao?

    # speaker:MaGiamSinh
    Mã Giám Sinh: Có điều gì không ổn?

    # speaker:Kieu
    Kiều: Hôn ước là chuyện hai người kết duyên. Còn câu này lại cho một người quyền quyết định nơi ở, công việc và cả việc đi hay ở của người kia.

    # speaker:Kieu
    Kiều: Nếu thay hai chữ “hôn ước” trên đầu bằng “khế bán người”, ta e nội dung phía dưới còn hợp hơn.

    # speaker:MaGiamSinh
    # emotion:annoyed
    Mã Giám Sinh: Đó chỉ là lệ buôn—

    # speaker:Narrator
    Hắn đột ngột ngừng lại.

    # camera:CU_Kieu_Eyes
    # speaker:Narrator
    Kiều nhìn thẳng vào hắn. Mẹ Kiều cũng không rời mắt.

    # speaker:MaGiamSinh
    # emotion:recovering
    Mã Giám Sinh: ...Ý ta là lệ làm giấy ở nơi ta. Cô nương không cần nghĩ quá nhiều.

    # speaker:Kieu
    # emotion:cold
    Kiều: Ra vậy.

    -> c1_decision


* [Chỉ kiểm tra điều khoản về số bạc và thời điểm giao tiền.]
    ~ tinh_tao += 1

    # speaker:Narrator
    Kiều không đọc hết từng chữ. Nàng chỉ tìm đến phần ghi về bốn trăm lạng và thời điểm giao bạc.

    { c1_tien_truoc:
        # speaker:Kieu
        Kiều: Điều ta yêu cầu đã được ghi ở đây. Bạc phải tới quan phủ trước khi ta rời nhà.
    - else:
        # speaker:Kieu
        Kiều: Ở đây chỉ ghi số bạc, lại không ghi rõ tiền được giao trước hay sau khi ta rời nhà.

        # speaker:MaGiamSinh
        Mã Giám Sinh: Chuyện ấy có gì khác nhau? Ta đã mang bạc tới thì tự nhiên sẽ giao.

        # speaker:Kieu
        # emotion:firm
        Kiều: Với công tử có thể không khác. Với ta, khác rất nhiều.

        # speaker:Narrator
        Sau một hồi miễn cưỡng, Mã Giám Sinh đồng ý sửa lại phần ấy.
        ~ c1_tien_truoc = true
    }

    -> c1_decision


* [Ký ngay vì sợ không còn thời gian cứu cha và em.]
    ~ tu_trong -= 1

    # speaker:Narrator
    Kiều nhìn ra ngoài trời. Mưa vẫn chưa ngừng, nhưng thời gian thì không còn nhiều.

    # speaker:Narrator
    Nàng cầm lấy bút.

    -> c1_decision


// ============================================================
// CẢNH 10 — MẸ KIỀU PHẢN ĐỐI
// ============================================================

=== c1_decision ===

# camera:MCU_MeKieu
# speaker:MeKieu
# emotion:crying
Mẹ Kiều: Không. Kiều nhi, chúng ta không ký nữa.

# speaker:MeKieu
Mẹ Kiều: Mẹ có thể bán nhà. Mẹ có thể đi vay. Mẹ đi cầu xin từng người một cũng được. Nhưng mẹ không thể đứng đây nhìn con đặt tên mình lên một tờ giấy như thế này.

# speaker:MeKieu
Mẹ Kiều: Cha con mà biết chuyện, người cũng sẽ không bao giờ đồng ý. Cha con và Quan nhi có trở về mà trong nhà lại không còn con... thì đó còn gọi là cứu gia đình sao?

# camera:CU_Kieu
# speaker:Narrator
Kiều nhìn mẹ. Lần đầu tiên từ đầu đêm, vẻ bình tĩnh trên khuôn mặt nàng bắt đầu dao động.

# speaker:Kieu
# emotion:holding_tears
Kiều: Mẹ nghĩ con không sợ sao?

# speaker:Kieu
Kiều: Con cũng sợ. Con không biết sau cánh cửa nhà người này là nơi nào. Không biết những điều hắn nói có bao nhiêu phần thật. Cũng không biết nếu bước ra khỏi nhà đêm nay, đến bao giờ con mới được trở lại.

# speaker:Narrator
Mẹ Kiều siết chặt tay con gái.

# speaker:Kieu
# emotion:soft_firm
Kiều: Nhưng cha và Quan nhi đang ở công đường ngay lúc này. Con không thể ngồi ở đây đến sáng, biết rõ trước mặt mình có một cách để cứu họ mà lại không làm chỉ vì con sợ.

# speaker:MeKieu
# emotion:broken
Mẹ Kiều: Nhưng cả đời con thì sao? Mẹ sinh con ra không phải để đến một ngày nhìn con đem cả cuộc đời mình đổi lấy một món nợ.

# speaker:Narrator
Kiều im lặng một lúc rất lâu.

# speaker:Kieu
# emotion:determined
Kiều: Con chọn cứu gia đình.

# speaker:Kieu
Kiều: Đó là lựa chọn của con, không phải món nợ cha mẹ bắt con phải trả.

# camera:CU_Kieu_Ma
# speaker:Kieu
# emotion:firm
Kiều: Nhưng công tử cũng nên nhớ một điều. Việc ta tự nguyện bước ra khỏi căn nhà này hôm nay không có nghĩa từ giờ trở đi công tử có quyền quyết định thay ta mọi chuyện.

# speaker:Kieu
Kiều: Ta có thể ký một tờ giấy. Nhưng ta không giao cả con người mình cho bất kỳ ai.

# speaker:Narrator
Mã Giám Sinh nhìn Kiều vài giây rồi mới mỉm cười.

# speaker:MaGiamSinh
# emotion:smug_hidden
Mã Giám Sinh: Vương cô nương quả thật khác với những gì người ta kể.

# speaker:Narrator
Kiều không đáp.

# camera:Insert_Signing
# speaker:Narrator
Nàng đặt bút xuống.

# speaker:Narrator
Một nét mực rất nhẹ.

# speaker:Narrator
Nhưng đối với những người trong căn nhà ấy, nó nặng hơn bất kỳ thứ gì đã được đặt lên bàn trong đêm nay.

-> c1_wait_news


// ============================================================
// CẢNH 11 — CHỜ TIN
// ============================================================

=== c1_wait_news ===

# speaker:Narrator
Mã Giám Sinh khẽ ra hiệu cho người đi theo.

# speaker:Narrator
Chiếc hòm bạc được mang khỏi Vương gia và biến mất sau màn mưa.

# transition:short_time_pass
# music:sad_strings_low

# speaker:Narrator
Không ai nói thêm gì.

# speaker:Narrator
Mẹ Kiều vẫn nắm tay con gái. Mã Giám Sinh ngồi ở phía đối diện, bình thản đến mức khiến khoảng thời gian chờ đợi càng trở nên dài hơn.

# gameplay:enable_player
# objective:Cho_tin
# gate:waiting

// Có thể cho player đi lại trong nhà lần cuối,
// xem ghế của cha, căn phòng Kiều, cửa chính...

# sfx:footsteps_outside
# speaker:Narrator
Một lúc sau, từ ngoài sân vang lên tiếng bước chân vội vã.

# camera:Door_Messenger
# speaker:Messenger
# emotion:urgent
Người đưa tin: Công tử! Phu nhân!

# speaker:Messenger
Người đưa tin: Bạc đã được giao tới công môn. Quan phủ đã nhận đủ và cho truyền lời rằng Vương lão gia cùng Vương công tử sẽ được tạm thả để chờ xét lại vụ án.

# camera:MCU_MeKieu
# speaker:MeKieu
# emotion:crying_relief
Mẹ Kiều: Được thả rồi... Cha con và Quan nhi được trở về rồi...

# speaker:Narrator
Mẹ Kiều gần như khuỵu xuống vì nhẹ nhõm. Bà vừa khóc vừa cười, hai tay vẫn không buông khỏi tay Kiều.

# camera:CU_Kieu
# speaker:Narrator
Kiều nhắm mắt và thở ra thật chậm. Lần đầu tiên trong cả đêm, sức nặng trong lòng nàng nhẹ đi một chút.

# speaker:Narrator
Nhưng khi mở mắt, nàng nhìn thấy Mã Giám Sinh vẫn đang ngồi ở đó.

# speaker:Narrator
Niềm nhẹ nhõm chỉ kéo dài trong một khoảnh khắc.

# speaker:Narrator
Bởi bây giờ... đến lượt nàng phải rời đi.

-> c1_farewell


// ============================================================
// CẢNH 12 — LỜI TẠM BIỆT
// ============================================================

=== c1_farewell ===

# camera:WS_MainHall
# speaker:MaGiamSinh
# emotion:satisfied
Mã Giám Sinh: Việc của Vương gia đã tạm ổn. Trời cũng không còn sớm. Chúng ta nên lên đường thôi... nương tử.

# camera:CU_Kieu
# speaker:Kieu
# emotion:calm_firm
Kiều: Công tử nên gọi ta là Vương cô nương.

# speaker:MaGiamSinh
# emotion:slightly_annoyed
Mã Giám Sinh: Chúng ta đã có hôn ước.

# speaker:Kieu
Kiều: Một tờ giấy không khiến hai người vừa gặp nhau lập tức trở thành phu thê. Ít nhất cho đến khi ta tự cho phép công tử gọi mình bằng một cách khác, xin cứ gọi ta là Vương cô nương.

# speaker:Narrator
Mã Giám Sinh nhìn nàng, nhưng lần này không tranh luận.

# camera:MCU_MeKieu_Kieu
# speaker:Narrator
Mẹ Kiều bước tới. Bà đưa tay chỉnh lại cổ áo cho con gái — một việc rất bình thường mà từ nhỏ bà đã làm không biết bao nhiêu lần.

# speaker:Narrator
Nhưng lần này, tay bà run.

# speaker:MeKieu
# emotion:crying_soft
Mẹ Kiều: Con từ nhỏ chưa từng rời nhà lâu. Mỗi khi trời trở lạnh, con lại hay đau đầu. Đến nơi xa lạ phải nhớ giữ ấm, ăn uống đừng vì buồn mà bỏ bữa.

# speaker:Kieu
# emotion:trying_to_smile
Kiều: Mẹ đang dặn con như thể con chỉ sang nhà họ hàng vài hôm vậy.

# speaker:Narrator
Mẹ Kiều nhìn con gái.

# speaker:MeKieu
# emotion:broken_soft
Mẹ Kiều: Nếu mẹ không nói như vậy... mẹ sợ mình sẽ không để con đi được.

# speaker:Narrator
Kiều không còn cười được nữa.

# speaker:Narrator
Mẹ Kiều tháo chiếc trâm trên tóc rồi đặt vào lòng bàn tay con gái.

# camera:Insert_Hairpin
# speaker:Kieu
# emotion:surprised_sad
Kiều: Mẹ... đây là chiếc trâm cha tặng mẹ mà.

# speaker:MeKieu
# emotion:soft
Mẹ Kiều: Mẹ biết. Chính vì vậy con càng phải giữ nó.

# speaker:MeKieu
Mẹ Kiều: Nếu có ngày con đi quá xa, hoặc có lúc con cảm thấy mình chẳng còn gì thuộc về nơi này nữa... hãy nhìn nó.

# speaker:MeKieu
Mẹ Kiều: Để nhớ rằng con vẫn còn một mái nhà. Vẫn còn cha, còn mẹ, còn Quan nhi... và vẫn còn người chờ con trở về.

# speaker:Narrator
Mẹ Kiều khép bàn tay Kiều lại quanh chiếc trâm.

# speaker:Kieu
# emotion:holding_tears
Kiều: Con sẽ trở về.

# speaker:MeKieu
# emotion:crying_soft
Mẹ Kiều: Mẹ sẽ chờ.

~ c1_co_tram_gia_dinh = true
# inventory:add:tram_gia_dinh

-> c1_end


// ============================================================
// CẢNH 13 — KẾT CHAPTER 1
// ============================================================

=== c1_end ===
# gate:exit

# camera:Hero_Kieu_Rain
# sfx:rain_exterior
# music:sad_strings_end

# speaker:Narrator
Cánh cửa Vương gia mở ra.

# speaker:Narrator
Mã Giám Sinh đã đứng chờ ngoài hiên. Phía sau hắn là màn mưa kéo dài đến tận con đường tối phía trước.

# speaker:Narrator
Kiều bước tới cửa rồi dừng lại.

# camera:Kieu_LookBack
# speaker:Narrator
Nàng quay đầu nhìn căn nhà lần cuối.

# speaker:Narrator
Mẹ vẫn đứng giữa gian chính. Chiếc ghế của cha vẫn còn trống. Ngọn đèn dầu trên bàn lay động theo từng cơn gió lọt qua khe cửa.

# speaker:Narrator
Đêm ấy, Thúy Kiều bước khỏi Vương gia bằng chính lựa chọn của mình.

# speaker:Narrator
Sau lưng nàng là người mẹ đang chờ chồng và con trai trở về.

# speaker:Narrator
Là căn nhà vừa thoát khỏi một tai họa.

# speaker:Narrator
Là chữ Hiếu mà nàng không thể quay lưng.

# speaker:Narrator
Kiều bước xuống bậc thềm. Mưa chạm lên vai áo.

# camera:CU_Kieu_Rain
# speaker:Narrator
Nhưng phía trước nàng không phải một cuộc hôn nhân.

# speaker:Narrator
Đó là một chiếc lồng đang mở cửa.

# speaker:Narrator
Và trước khi cánh cửa ấy khép lại...

# speaker:Narrator
...nàng phải tìm được chìa khóa.

# chapter_end:1
// Unity resumes chapter_2 when the next scene is ready.
-> DONE
