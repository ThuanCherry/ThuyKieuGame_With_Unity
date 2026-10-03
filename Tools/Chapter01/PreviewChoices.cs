var director=UnityEngine.Object.FindAnyObjectByType<ThuyKieu.Core.Chapter01Director>();
var manager=ThuyKieu.Dialogue.DialogueManager.Instance;
if(director.CurrentStage==ThuyKieu.Core.Chapter01Director.Stage.Opening) { manager.Advance(); manager.Advance(); }
if(director.CurrentStage==ThuyKieu.Core.Chapter01Director.Stage.Mother) director.Interact(ThuyKieu.Core.Chapter01Director.Stage.Mother,UnityEngine.Object.FindAnyObjectByType<ThuyKieu.Player.PlayerMovement>().transform);
for(int i=0;i<20 && manager.IsDialogueActive && !manager.IsAtChoicePoint;i++) manager.Advance();
UnityEngine.ScreenCapture.CaptureScreenshot("Tools/Chapter01/DialogueChoices.png");
return new{stage=director.CurrentStage.ToString(),choices=manager.InkStory.currentChoices.Select(c=>c.text).ToArray()};
