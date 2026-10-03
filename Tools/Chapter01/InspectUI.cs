var ui = UnityEngine.Object.FindAnyObjectByType<ThuyKieu.Dialogue.DialogueUI>();
var canvas = UnityEngine.Object.FindAnyObjectByType<UnityEngine.Canvas>();
var keyboard = UnityEngine.InputSystem.Keyboard.current;
return new {uiEnabled=ui.enabled,panelVisible=ui.IsPanelVisible,canvasEnabled=canvas.enabled,canvasMode=canvas.renderMode.ToString(),keys=keyboard==null?null:new{keyboard.name,space=keyboard.spaceKey.isPressed,enter=keyboard.enterKey.isPressed,e=keyboard.eKey.isPressed},ui=canvas.GetComponentsInChildren<TMPro.TMP_Text>(true).Select(t=>new{t.name,t.text,active=t.gameObject.activeInHierarchy,rect=t.rectTransform.rect.ToString()}).ToArray(),frame=UnityEngine.Time.frameCount};
