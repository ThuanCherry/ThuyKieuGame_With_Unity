var player=UnityEngine.GameObject.Find("ThuyKieu");
var model=player.transform.Find("Model");
var position=model.localPosition; position.y=0f; model.localPosition=position;
UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(model);
UnityEditor.PrefabUtility.ApplyPrefabInstance(player,UnityEditor.InteractionMode.AutomatedAction);
UnityEditor.AssetDatabase.SaveAssets();
UnityEditor.SceneManagement.EditorSceneManager.SaveScene(player.scene);
return model.localPosition.ToString();
