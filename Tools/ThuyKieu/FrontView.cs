var player=UnityEngine.GameObject.Find("ThuyKieu");
var camera=UnityEngine.Camera.main;
camera.GetComponent<ThuyKieu.Player.ThirdPersonCameraController>().enabled=false;
camera.transform.position=player.transform.position+player.transform.forward*2.8f+UnityEngine.Vector3.up*1.45f;
camera.transform.LookAt(player.transform.position+UnityEngine.Vector3.up*0.95f);
return "Front appearance capture; temporary Play Mode camera only.";
