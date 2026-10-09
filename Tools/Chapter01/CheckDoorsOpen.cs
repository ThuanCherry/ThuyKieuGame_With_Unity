var house=GameObject.Find("VuongGia/MainHouse").transform;
return new {roomOpen=house.Find("KieuRoom/KieuRoomDoor").GetComponent<ThuyKieu.Interaction.ProximityDoor>().IsOpen,mainOpen=house.Find("MainDoor").GetComponent<ThuyKieu.Interaction.ProximityDoor>().IsOpen};
