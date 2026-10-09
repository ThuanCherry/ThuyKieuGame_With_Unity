var house=GameObject.Find("VuongGia/MainHouse").transform;
var room=house.Find("KieuRoom/KieuRoomDoor").GetComponent<ThuyKieu.Interaction.ProximityDoor>();
var panel=room.GetComponentInChildren<Renderer>().bounds;
var main=house.Find("MainDoor").GetComponent<ThuyKieu.Interaction.ProximityDoor>();
var left=main.transform.Find("LeftPivot").GetComponentInChildren<Renderer>().bounds;
var right=main.transform.Find("RightPivot").GetComponentInChildren<Renderer>().bounds;
var result=new {roomTopGap=2.6f-panel.max.y,roomSideGap=(-3.1f)-panel.max.x,mainCenterGap=right.min.x-left.max.x,closed=!room.IsOpen&&!main.IsOpen};
room.SetStoryOpen(true);main.SetStoryOpen(true);
return result;
