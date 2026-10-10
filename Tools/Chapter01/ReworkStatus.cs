var director = UnityEngine.Object.FindAnyObjectByType<ThuyKieu.Core.Chapter01Director>();
var player = UnityEngine.Object.FindAnyObjectByType<ThuyKieu.Player.PlayerMovement>();
var ui = UnityEngine.Object.FindAnyObjectByType<ThuyKieu.Dialogue.DialogueUI>();
return new {running = ThuyKieu.Core.Editor.Chapter01ReworkPlayCheck.Running, stage = director != null ? director.CurrentStage.ToString() : "none", player = player != null ? player.transform.position.ToString() : "none", text = ui != null ? ui.CurrentText : "none", report = System.IO.File.Exists("Tools/Chapter01/ReworkPlayReport.json") ? System.IO.File.ReadAllText("Tools/Chapter01/ReworkPlayReport.json") : "pending"};
