var director=UnityEngine.Object.FindAnyObjectByType<ThuyKieu.Core.Chapter01Director>();
var mother=GameObject.Find("MeKieu").GetComponentInChildren<Animator>();
var dialogue=UnityEngine.Object.FindAnyObjectByType<ThuyKieu.Dialogue.DialogueManager>();
var sad=mother.GetCurrentAnimatorStateInfo(0).IsName("SittingSad");
if(dialogue.IsDialogueActive)dialogue.EndDialogue();
dialogue.StartInkAt("c1_mainhall");
director.Interact(ThuyKieu.Core.Chapter01Director.Stage.Mother,mother.transform);
return new {sad,dialogueActive=dialogue.IsDialogueActive, cameraTarget="current mother chair"};
