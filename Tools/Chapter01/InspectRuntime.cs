var director = UnityEngine.Object.FindAnyObjectByType<ThuyKieu.Core.Chapter01Director>();
var manager = ThuyKieu.Dialogue.DialogueManager.Instance;
var ui = UnityEngine.Object.FindAnyObjectByType<ThuyKieu.Dialogue.DialogueUI>();
return new { playing=UnityEngine.Application.isPlaying, scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene().path, stage=director==null?null:director.CurrentStage.ToString(), active=manager==null?false:manager.IsDialogueActive, choiceCount=ui==null?0:ui.VisibleChoiceCount, text=ui==null?null:ui.CurrentText, checkRunning=ThuyKieu.Core.Editor.Chapter01PlayModeCheck.Running,
 players=UnityEngine.Object.FindObjectsByType<ThuyKieu.Player.PlayerMovement>(UnityEngine.FindObjectsSortMode.None).Select(p=>new{p.name,position=p.transform.position.ToString(),p.ControlLocked,avatar=p.GetComponentInChildren<UnityEngine.Animator>().avatar.isHuman}).ToArray(),
 animators=UnityEngine.Object.FindObjectsByType<UnityEngine.Animator>(UnityEngine.FindObjectsSortMode.None).Select(a=>new{a.name,avatar=a.avatar==null?null:a.avatar.isHuman.ToString(),controller=a.runtimeAnimatorController==null?null:a.runtimeAnimatorController.name}).ToArray()
};
