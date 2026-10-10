var director=UnityEngine.Object.FindAnyObjectByType<ThuyKieu.Core.Chapter01Director>();
var ui=UnityEngine.Object.FindAnyObjectByType<ThuyKieu.Dialogue.DialogueUI>();
var dialogue=UnityEngine.Object.FindAnyObjectByType<ThuyKieu.Dialogue.DialogueManager>();
for(int i=0;i<20 && dialogue.IsDialogueActive;i++){if(ui.IsRevealing)ui.OnContinueClicked();ui.OnContinueClicked();}
var player=UnityEngine.Object.FindAnyObjectByType<ThuyKieu.Player.PlayerMovement>();
var mother=GameObject.Find("MeKieu").GetComponentInChildren<Animator>();
var serialized=new UnityEditor.SerializedObject(director);
var button=(UnityEngine.UI.Button)serialized.FindProperty("_standUpButton").objectReferenceValue;
var seated=player.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SittingIdle");
var motherSeated=mother.GetBool("IsSitting");
bool ready=button.gameObject.activeInHierarchy;
if(ready)button.onClick.Invoke();
return new {seated,motherSeated,ready,removedChair=GameObject.Find("MotherChair")==null};
