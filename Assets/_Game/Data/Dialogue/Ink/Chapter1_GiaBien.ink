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
// BẢN BIÊN TẬP: lời Narrator miêu tả động tác/âm thanh được thay bằng
// comment STAGING (không hiển thị trong game). Unity cần thực thi các
// cue camera/SFX/gate tương ứng, kể cả khi không có textbox Narrator.
// Giữ nguyên các knot, biến, nhánh lựa chọn và #gate của bản gốc.
// QUAN TRỌNG: Một số sự kiện nay chỉ còn camera/sfx tag, không có
// dòng thoại. Dialogue/Ink runner phải xử lý cue-only mà KHÔNG mở hộp thoại
// rỗng hoặc dồn toàn bộ cue vào dòng kế tiếp; cần test trực tiếp trong Unity.
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

// STAGING: Opening ngoài UI hội thoại: màn hình đen; nghe mưa rồi tiếng Mẹ Kiều khóc từ MainHall.
// STAGING: Camera fade dần vào phòng, Kiều đang ngồi ở mép giường; chỉ bật nút "Ngồi dậy" sau khi nghe tiếng khóc.
// STAGING: Không hiển thị hộp Narrator cho các chuyển động và âm thanh này.
# camera:Fade_Black
# sfx:rain_muffled
# wait:0.7
@cue

# sfx:woman_crying_distant
# wait:1.2
@cue

// STAGING: Ánh đèn dầu yếu, Kiều chú ý đến tiếng nấc ở gian chính.
# camera:FadeIn_KieuRoom
# wait:1.5
@cue


# speaker:Kieu
# emotion:worried
Kiều: ...Mẹ?


# speaker:Kieu
# emotion:worried
Kiều: Giờ này mẹ vẫn chưa nghỉ sao...?

# gameplay:enable_player
# objective:Tim_Me_Kieu
# gate:hallway
// GAMEPLAY: Khi người chơi nhấn "Ngồi dậy", dùng Sit To Stand (nếu đã thiết lập), rồi mới cho di chuyển.
// GAMEPLAY: Phòng Kiều đóng cửa lúc đầu; bấm E để mở, không dịch chuyển player tức thời.

// Unity có thể trả quyền điều khiển tại đây.
// Player tự rời phòng Kiều và đi về gian chính.

-> c1_hanh_lang


// ============================================================
// CẢNH 1B — DẤU VẾT CÒN LẠI
// ============================================================

=== c1_hanh_lang ===

// STAGING: Trong hành lang có dấu chân bùn, ghế xô đổ, chén vỡ; người chơi tận mắt nhìn thấy.
// STAGING: Tiếng khóc tăng dần khi Kiều tới gần gian chính.
# camera:Gameplay_Kieu


# speaker:Kieu
# emotion:uneasy
Kiều: Những dấu chân này... là của người ngoài.

# speaker:Kieu
Kiều: Chiếc ghế của cha cũng bị xô đổ. Rốt cuộc tối nay đã xảy ra chuyện gì?


-> c1_mainhall


// ============================================================
// CẢNH 2 — MẸ KIỀU
// ============================================================

=== c1_mainhall ===
# gate:mother

// STAGING: Mẹ Kiều ngồi KHÓC trên ghế cạnh bàn; trên bàn có tờ trát, túi bạc và hộp trang sức.
// STAGING: Vương Ông và Vương Quan KHÔNG xuất hiện trong nhà.
# camera:WS_MainHall


# speaker:Kieu
# emotion:soft_worried
Kiều: Mẹ.

# camera:MCU_MeKieu

# speaker:MeKieu
# emotion:crying_controlled
Mẹ Kiều: Kiều nhi... sao con còn chưa ngủ? Trời đã khuya lắm rồi, lại lạnh thế này. Con về phòng nghỉ đi, chuyện ngoài này để mẹ lo.

# camera:MCU_Kieu
# speaker:Kieu
# emotion:sad_controlled
Kiều: Con nghe tiếng mẹ khóc nên mới ra đây. Cha không có trong phòng, Quan nhi cũng không thấy đâu. Ngoài cửa thì toàn dấu chân người lạ, bàn ghế lại bị xô đổ như vừa có một trận giằng co.

# speaker:Kieu
Kiều: Mẹ đừng giấu con nữa. Cha và em đã xảy ra chuyện gì?


# speaker:MeKieu
# emotion:crying
Mẹ Kiều: Lúc tối... người của quan phủ tới.

# speaker:MeKieu
Mẹ Kiều: Họ nói có người tố cha con liên quan đến một vụ án. Cha con hết lời giải thích rằng mình không làm, Quan nhi cũng đứng ra phân trần, nhưng chẳng ai chịu nghe.

# speaker:MeKieu
Mẹ Kiều: Họ lục tung cả nhà. Đồ đạc bị xô đổ, giấy tờ bị giở tung. Sau đó họ trói cả cha con lẫn Quan nhi rồi áp giải đi ngay trong mưa.

# speaker:MeKieu
Mẹ Kiều: Mẹ chạy theo đến tận cửa, van xin họ cho thêm thời gian... nhưng ngoài sân toàn là quan sai. Mẹ chỉ có thể đứng nhìn cha con và em con bị đưa đi mà chẳng làm được gì.


# speaker:Kieu
# emotion:controlled_pain
Kiều: Họ đưa cha và Quan nhi đi đâu?

# speaker:MeKieu
Mẹ Kiều: Công đường.

# speaker:MeKieu
Mẹ Kiều: Có một người quen của cha con đã chạy đi dò hỏi. Vừa rồi người ấy mới trở về báo tin rằng họ vẫn đang giữ cả hai người.

# speaker:MeKieu
Mẹ Kiều: Nếu muốn vụ án được xem xét lại, nếu muốn cha con và Quan nhi có cơ hội trở về... họ đòi nhà ta phải lo một khoản bạc trước sáng mai.

// STAGING: Đưa camera về tờ trát hoặc UI giấy; hiển thị rõ con số 400 lạng.
# camera:Insert_DebtPaper
# speaker:Kieu
# emotion:tense
Kiều: Bao nhiêu?


# speaker:MeKieu
# emotion:broken
Mẹ Kiều: Bốn trăm lạng.


# speaker:Kieu
# emotion:thoughtful
Kiều: Bốn trăm lạng...


// ============================================================
// CẢNH 3 — GIA ĐÌNH KHÔNG CÒN ĐƯỜNG LÙI
// ============================================================

# speaker:MeKieu
# emotion:crying_controlled
Mẹ Kiều: Tiền dành dụm trong nhà, cả bạc mẹ để phòng đau ốm... mẹ đã gom hết rồi. Vẫn còn thiếu quá nhiều.

# speaker:MeKieu
Mẹ Kiều: Nữ trang của mẹ, của con cũng có thể bán. Nhưng giữa đêm, họ biết ta cần tiền sẽ ép giá.

# speaker:Kieu
# emotion:calm
Kiều: Còn ruộng đất, căn nhà này? Có thể tìm người mua không mẹ?

# speaker:MeKieu
Mẹ Kiều: Có người mua, mẹ cũng bán. Nhưng giấy tờ sao kịp trước sáng? Mất cả mái nhà rồi... cha con và Quan nhi trở về sẽ ở đâu?


# speaker:Kieu
# emotion:soft_firm
Kiều: Mẹ nghỉ một chút. Con sẽ đọc kỹ tờ trát, xem ta có bỏ sót điều gì không.

# speaker:Kieu
Kiều: Chuyện của cha và Quan nhi, con sẽ cùng mẹ gánh vác.

# speaker:MeKieu
# emotion:crying
Mẹ Kiều: Mẹ không tiếc của, Kiều nhi. Mẹ chỉ sợ gom đủ bạc rồi họ vẫn làm khó, còn cha con và em cứ phải chịu khổ trong đó.

# speaker:Kieu
# emotion:sad_controlled
Kiều: Cha từng dạy con, lúc rối nhất càng phải nhìn cho rõ. Đêm nay ta lo đưa cha và Quan nhi ra trước; chuyện minh oan, con không quên đâu.


# speaker:MeKieu
# emotion:soft_sad
Mẹ Kiều: Con nói giống cha con quá.

# speaker:Kieu
# emotion:gently_smiling
Kiều: Vậy thì mẹ đừng khóc nữa. Nếu cha trở về nhìn thấy mắt mẹ sưng thế này, người sẽ lại tự trách mình.


// STAGING: Kiều nắm tay rồi ôm mẹ; giữ camera ổn định và để tiếng mưa lấp khoảng lặng.
# sfx:rain_room

-> c1_explore


// ============================================================
// CẢNH 4 — NGƯỜI CHƠI TÌM HIỂU TÌNH HÌNH
//
// Các clue được gọi từ tương tác GameObject trong Unity qua các knot c1_clue_*.
// Giữ #gate:explore để pause và chờ input từ gameplay.
// ============================================================

=== c1_explore ===


# gameplay:enable_player
# objective:Tim_hieu_tinh_hinh_Vuong_Gia
# gate:explore
-> DONE

=== c1_clue_debt ===
{ c1_xem_to_trat: -> c1_explore }
~ c1_xem_to_trat = true
~ c1_clues += 1

# camera:Insert_DebtPaper

# speaker:Kieu
# emotion:thoughtful
Kiều: Bốn trăm lạng, thời hạn trước sáng... nhưng cả tờ giấy lại không có một dòng nào nói rõ bằng chứng buộc tội cha.

# speaker:Kieu
Kiều: Họ viết rất rõ số tiền phải nộp, nhưng lại chẳng viết rõ cha đã phạm tội gì.


-> c1_explore

=== c1_clue_money ===
{ c1_xem_tui_bac: -> c1_explore }
~ c1_xem_tui_bac = true
~ c1_clues += 1

# camera:Insert_MoneyBag

# speaker:Kieu
# emotion:sad
Kiều: Chừng này còn chưa bằng một phần tư số họ đòi. Cha mẹ đã dành dụm bao năm, vậy mà trước một con số trên tờ giấy... lại trở nên nhỏ bé đến thế.

-> c1_explore

=== c1_clue_jewelry ===
{ c1_xem_nu_trang: -> c1_explore }
~ c1_xem_nu_trang = true
~ c1_clues += 1

# camera:Insert_Jewelry

# speaker:Kieu
# emotion:soft_sad
Kiều: Mẹ đã đem cả những thứ này ra rồi...

# speaker:Kieu
Kiều: Bán hết gia sản để cứu người nhà đã là đau. Nhưng nếu bán hết mà vẫn không đủ thì phải làm sao?


-> c1_explore

=== c1_before_knock ===

// STAGING: Hai mẹ con ngồi/gần nhau; chuyển khung hình sang góc hai người, giữ nhịp lặng ngắn.
# camera:MCU_Kieu_Me
# speaker:Kieu
# emotion:thoughtful
Kiều: Mẹ, bốn trăm lạng trong một đêm gần như là điều không thể với nhà ta. Nhưng con vẫn nghĩ phải còn một con đường nào đó. Ngày mai con sẽ—

// STAGING: Hai mẹ con ngừng nói; phát SFX gõ cửa thật, KHÔNG hiển thị chữ "Cốc".
# sfx:door_knock
# staging:look_at_door
# speaker:Narrator
# wait:0.8
@cue


// STAGING: Phát nhịp gõ tiếp theo, không lặp lời dẫn chuyện.
# sfx:door_knock_double
# camera:MCU_MeKieu
# staging:look_at_mother
# wait:0.9
@cue

# camera:MCU_MeKieu
# speaker:MeKieu
# emotion:afraid
Mẹ Kiều: Giờ này còn ai tới nữa? Hay... lại là người của quan phủ?


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
// STAGING: Cửa mở nhờ interaction; Mã đứng ngoài hiên tránh mưa, trang phục chỉnh tề.
# camera:Door_Reveal
# wait:0.8
@cue

# camera:Reveal_MaGiamSinh
# speaker:MaGiamSinh
# staging:ma_greeting
# wait:3.7
@cue

# speaker:MaGiamSinh
# emotion:polite_fake
Mã Giám Sinh: Đêm khuya còn đến quấy rầy, quả thật thất lễ. Kẻ hèn họ Mã. Nghe tin Vương gia gặp biến cố nên mới mạo muội tìm tới.


# speaker:Kieu
# emotion:calm_probe
Kiều: Tin trong nhà ta truyền nhanh đến vậy sao?

# speaker:MaGiamSinh
# emotion:polite_fake
Mã Giám Sinh: Chuyện tới công môn thì khó giữ kín. Vương gia có danh tiếng, người quen nghe tin cũng hỏi thăm thôi.

# speaker:MeKieu
# emotion:uncertain
Mẹ Kiều: Công tử quen biết lão gia nhà tôi sao?

# speaker:MaGiamSinh
Mã Giám Sinh: Chưa có duyên gặp mặt.


# speaker:MaGiamSinh
# emotion:polite_fake
Mã Giám Sinh: Chưa quen cũng có thể giúp nhau. Nếu phu nhân cho phép, ta xin vào trong. Chuyện này... không tiện nói ngoài cửa.


-> c1_offer


// ============================================================
// CẢNH 7 — “TA CÓ THỂ GIÚP”
// ============================================================

=== c1_offer ===
# gate:offer

// STAGING: Mã ngồi đối diện hai mẹ con, ban đầu lịch sự và điềm tĩnh.
# camera:WS_MainHall_Ma

# speaker:MeKieu
# emotion:desperate_controlled
Mẹ Kiều: Công tử nói mình biết chuyện nhà tôi, lại bảo có thể giúp. Xin công tử cứ nói thẳng. Chồng và con trai tôi hiện vẫn còn trong tay quan phủ. Lúc này tôi thật sự không còn tâm trí để đoán ý người khác nữa.

# speaker:MaGiamSinh
# emotion:calm
Mã Giám Sinh: Phu nhân nói phải.

# camera:Insert_DebtPaper

# speaker:MaGiamSinh
# emotion:controlled
Mã Giám Sinh: Bốn trăm lạng.

# camera:CU_Kieu

# speaker:MaGiamSinh
Mã Giám Sinh: Nhà cô nương khó xoay kịp trong một đêm. Nhưng số bạc ấy, ta có thể lo.

# speaker:MeKieu
# emotion:hopeful_uncertain
Mẹ Kiều: Ý công tử là... có thể cho nhà tôi vay sao?

# speaker:MaGiamSinh
# emotion:smug_hidden
Mã Giám Sinh: Nếu chỉ là chuyện vay tiền thì đơn giản quá.


# speaker:MaGiamSinh
# emotion:polite_fake
Mã Giám Sinh: Ta không tới đây để làm chủ nợ của Vương gia. Ta vốn đang muốn tìm một mối lương duyên, lại nghe danh Vương cô nương đã lâu — tài sắc, gia giáo, hiếu thuận.

# speaker:MaGiamSinh
Mã Giám Sinh: Nếu hôm nay hai nhà có thể kết một mối nhân duyên, ta sẵn lòng đứng ra lo liệu chuyện trước mắt. Bốn trăm lạng sẽ được đưa tới công môn ngay trong đêm.

# speaker:MeKieu
# emotion:shocked
Mẹ Kiều: Không được!


# speaker:MeKieu
# emotion:angry_crying
Mẹ Kiều: Con gái tôi không phải thứ đem ra đổi lấy bạc. Dù nhà tôi có rơi vào đường cùng, tôi cũng không thể coi đời con mình như một món hàng để mặc cả.

# speaker:MaGiamSinh
# emotion:calm_fake
Mã Giám Sinh: Phu nhân hiểu lầm rồi. Ta chưa từng nói đến chuyện mua bán.

# speaker:MaGiamSinh
Mã Giám Sinh: Ta nói đến hôn nhân. Ta giúp Vương gia, cô nương trở thành người nhà của ta. Hai bên đều được điều mình cần.


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
    Mã Giám Sinh: Ta buôn bán, đi lại nhiều nơi. Chuyện gia nghiệp không nói hết trong vài câu được. Về nhà ta, nàng sẽ biết.

    # speaker:Kieu
    # emotion:thoughtful
    Kiều: Công tử có thể mang bốn trăm lạng đến cứu một gia đình chưa từng quen biết, nhưng lại không thể nói một câu rõ ràng về chính mình. Đúng là chuyện không dễ hiểu.


    -> c1_contract


* [Không vội đáp. Quan sát cách hắn nói về tiền và những thứ hắn đã chuẩn bị.]
    ~ tinh_tao += 1
    ~ c1_quan_sat_ma = true

    // STAGING: Cận hòm bạc rồi tới ánh mắt Kiều; tờ khế vẫn được giấu trong áo Mã.
    # camera:POV_Kieu_MaHands

    # speaker:Kieu
    # emotion:thoughtful
    Kiều: Công tử chuẩn bị chu đáo thật. Người trong nhà ta đến giờ còn chưa biết sẽ làm gì, vậy mà công tử đã mang đủ bạc ngay trong đêm.

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


    # speaker:MaGiamSinh
    Mã Giám Sinh: ...Được.

    # speaker:Kieu
    Kiều: Vậy xin ghi điều đó vào giấy.


    -> c1_contract


// ============================================================
// CẢNH 9 — TỜ KHẾ
// ============================================================

=== c1_contract ===
# camera:Table_Contract
# staging:present_contract
# wait:3.8
@cue

# gate:contract

# scene:VuongGia_BanKyKhe
// STAGING: Mã đặt tờ khế lên bàn; cận hai chữ "HÔN ƯỚC" và các điều khoản nhỏ.
// STAGING: Đọc khế qua UI/close-up, không để Narrator giải thích thay người chơi.
# camera:Table_Contract


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

    # speaker:MaGiamSinh
    # emotion:annoyed_hidden
    Mã Giám Sinh: ...Được. Ta sẽ ghi thêm điều đó.
}


* [Đọc kỹ toàn bộ tờ khế trước khi đặt bút.]
    ~ tinh_tao += 1
    ~ c1_doc_ky_khe = true


    # speaker:Kieu
    # emotion:cold
    Kiều: “Người nhận có toàn quyền định đoạt nơi ở và công việc của người giao”... Công tử gọi đây là hôn ước sao?

    # speaker:MaGiamSinh
    Mã Giám Sinh: Có điều gì không ổn?

    # speaker:Kieu
    Kiều: Hôn ước là chuyện hai người kết duyên. Còn câu này lại cho một người quyền quyết định nơi ở, công việc và cả việc đi hay ở của người kia.

    # speaker:Kieu
    Kiều: Nếu thay hai chữ “hôn ước” trên đầu bằng “khế bán người”, ta e nội dung phía dưới còn hợp hơn.

    # speaker:MaGiamSinh
    # emotion:annoyed
    Mã Giám Sinh: Đó chỉ là lệ buôn—


    // STAGING: Mã khựng khi lỡ nói "lệ buôn"; Kiều và Mẹ nhìn hắn, giữ im lặng ngắn.
    # camera:CU_Kieu_Eyes
    # speaker:Narrator
    # wait:1.3
    @cue

    # speaker:MaGiamSinh
    # emotion:recovering
    Mã Giám Sinh: ...Ý ta là lệ làm giấy ở nơi ta. Cô nương không cần nghĩ quá nhiều.

    # speaker:Kieu
    # emotion:cold
    Kiều: Ra vậy.

    -> c1_decision


* [Chỉ kiểm tra điều khoản về số bạc và thời điểm giao tiền.]
    ~ tinh_tao += 1


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

        # speaker:MaGiamSinh
        # emotion:annoyed_hidden
        Mã Giám Sinh: Được. Ta sẽ ghi rõ thời điểm giao bạc để cô nương yên lòng.

        ~ c1_tien_truoc = true
    }

    -> c1_decision


* [Ký ngay vì sợ không còn thời gian cứu cha và em.]
    ~ tu_trong -= 1


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

# speaker:Kieu
# emotion:holding_tears
Kiều: Mẹ nghĩ con không sợ sao?

# speaker:Kieu
Kiều: Con cũng sợ. Con không biết sau cánh cửa nhà người này là nơi nào. Không biết những điều hắn nói có bao nhiêu phần thật. Cũng không biết nếu bước ra khỏi nhà đêm nay, đến bao giờ con mới được trở lại.


# speaker:Kieu
# emotion:soft_firm
Kiều: Nhưng cha và Quan nhi đang ở công đường ngay lúc này. Con không thể ngồi ở đây đến sáng, biết rõ trước mặt mình có một cách để cứu họ mà lại không làm chỉ vì con sợ.

# speaker:MeKieu
# emotion:broken
Mẹ Kiều: Nhưng cả đời con thì sao? Mẹ sinh con ra không phải để đến một ngày nhìn con đem cả cuộc đời mình đổi lấy một món nợ.


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


# speaker:MaGiamSinh
# emotion:smug_hidden
Mã Giám Sinh: Vương cô nương quả thật khác với những gì người ta kể.


// STAGING: Kiều cầm bút rồi ký một lần; dùng animation/close-up/âm thanh giấy bút, không kể lại.
# camera:Insert_Signing
# speaker:Kieu
# wait:5.2
@cue


-> c1_wait_news


// ============================================================
// CẢNH 11 — CHỜ TIN
// ============================================================

=== c1_wait_news ===


// STAGING: Gia nhân của Mã mang bạc ra ngoài màn mưa; chuyển thời gian ngắn, không cần lời dẫn.
# transition:short_time_pass
# music:sad_strings_low
# wait:1.6
@cue


# gameplay:enable_player
# objective:Cho_tin
# gate:waiting
# wait:0.1
@cue
// GAMEPLAY: Có thể cho Kiều nhìn quanh nhà; sau khoảng chờ ngắn mới phát footsteps_outside và báo tin.

// Có thể cho player đi lại trong nhà lần cuối,
// xem ghế của cha, căn phòng Kiều, cửa chính...

# sfx:footsteps_outside

// STAGING: Người đưa tin đứng tại ngưỡng cửa; Vương Ông/Quan nhi chưa về đến nhà.
# camera:Door_Messenger
# speaker:Messenger
# emotion:urgent
Người đưa tin: Công tử! Phu nhân!

# speaker:Messenger
Người đưa tin: Bạc đã được giao tới công môn. Quan phủ đã nhận đủ và cho truyền lời rằng Vương lão gia cùng Vương công tử sẽ được tạm thả để chờ xét lại vụ án.

# camera:CU_Kieu
# speaker:Narrator
# staging:look_at_mother
# wait:1.1
@cue

// STAGING: Mẹ nhẹ nhõm, gần khuỵu; Kiều thở ra rồi nhìn về phía Mã đang chờ.
# camera:MCU_MeKieu
# speaker:MeKieu
# emotion:crying_relief
Mẹ Kiều: Được thả rồi... Cha con và Quan nhi được trở về rồi...


# camera:CU_Kieu
# speaker:Narrator
# staging:look_at_ma
# wait:1.3
@cue


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


// STAGING: Mẹ sửa lại cổ áo con; tay run nhẹ trước khi trao chiếc trâm.
# camera:MCU_MeKieu_Kieu


# speaker:MeKieu
# emotion:crying_soft
Mẹ Kiều: Con từ nhỏ chưa từng rời nhà lâu. Mỗi khi trời trở lạnh, con lại hay đau đầu. Đến nơi xa lạ phải nhớ giữ ấm, ăn uống đừng vì buồn mà bỏ bữa.

# speaker:Kieu
# emotion:trying_to_smile
Kiều: Mẹ đang dặn con như thể con chỉ sang nhà họ hàng vài hôm vậy.


# speaker:MeKieu
# emotion:broken_soft
Mẹ Kiều: Nếu mẹ không nói như vậy... mẹ sợ mình sẽ không để con đi được.


// STAGING: Mẹ lấy chiếc trâm trên tóc, trao vào tay Kiều; camera cận món đồ.
# camera:Insert_Hairpin
# speaker:Kieu
# emotion:surprised_sad
Kiều: Mẹ... đây là chiếc trâm cha tặng mẹ mà.

# camera:MCU_MeKieu_Kieu
# speaker:Narrator
# wait:1.8
@cue

# speaker:MeKieu
# emotion:soft
Mẹ Kiều: Mẹ biết. Chính vì vậy con càng phải giữ nó.

# speaker:MeKieu
Mẹ Kiều: Nếu có ngày con đi quá xa, hoặc có lúc con cảm thấy mình chẳng còn gì thuộc về nơi này nữa... hãy nhìn nó.

# speaker:MeKieu
Mẹ Kiều: Để nhớ rằng con vẫn còn một mái nhà. Vẫn còn cha, còn mẹ, còn Quan nhi... và vẫn còn người chờ con trở về.


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

// STAGING: Mã chờ ngoài hiên; Kiều bước đến cửa, quay lại nhìn Mẹ và chiếc ghế trống của cha.
// STAGING: Chỉ ở đoạn KẾT mới hiện lời Narrator, không kể mọi cử chỉ bằng textbox.
# camera:Hero_Kieu_Rain
# sfx:rain_exterior
# music:sad_strings_end
# wait:1.2
@cue


# camera:Kieu_LookBack
# wait:1.2
@cue


# camera:CU_Kieu_Rain
# speaker:Narrator
Sau lưng nàng là mái nhà và chữ Hiếu. Trước mặt là một chiếc lồng chưa khép cửa.

# speaker:Narrator
Nàng phải tìm được chìa khóa, trước khi cánh cửa ấy đóng lại.

# chapter_end:1
// Unity resumes chapter_2 when the next scene is ready.
-> DONE
