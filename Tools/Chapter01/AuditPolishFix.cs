var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
var ts=scene.GetRootGameObjects().SelectMany(r=>r.GetComponentsInChildren<Transform>(true));
return ts.Where(t=>t.name.Contains("Door") || t.name=="Mother" || t.name=="HallTwoShot" || t.name=="MeKieu").Select(t=>new {t.name,p=t.position.ToString(),s=t.localScale.ToString(),children=t.GetComponentsInChildren<Renderer>().Select(r=>new {r.name,b=r.bounds.ToString()}).ToArray(),controller=t.GetComponentInChildren<Animator>()?.runtimeAnimatorController?.name}).ToArray();
