var manager=ThuyKieu.Dialogue.DialogueManager.Instance;
var director=UnityEngine.Object.FindAnyObjectByType<ThuyKieu.Core.Chapter01Director>();
var ui=UnityEngine.Object.FindAnyObjectByType<ThuyKieu.Dialogue.DialogueUI>();
if(director.CurrentStage==ThuyKieu.Core.Chapter01Director.Stage.Opening) {manager.Advance();manager.Advance();}
if(director.CurrentStage==ThuyKieu.Core.Chapter01Director.Stage.Mother) director.Interact(ThuyKieu.Core.Chapter01Director.Stage.Mother,director.transform);
for(int i=0;i<12 && manager.IsDialogueActive && ui.CurrentName!="Mã Giám Sinh";i++)manager.Advance();
var animator=UnityEngine.GameObject.Find("MaGiamSinh").GetComponentInChildren<UnityEngine.Animator>();
animator.Update(0.3f);animator.Update(0.3f);
UnityEngine.ScreenCapture.CaptureScreenshot("Tools/MaGiamSinh/TalkingPreview.png");
return new{speaker=ui.CurrentName,state=animator.GetCurrentAnimatorStateInfo(0).IsName("Talking"),scale=animator.transform.localScale.ToString(),feet=animator.GetBoneTransform(UnityEngine.HumanBodyBones.LeftFoot).position.ToString()};
