if (!UnityEngine.Application.isPlaying) throw new System.InvalidOperationException("Enter Play Mode first.");
var audio = UnityEngine.Object.FindAnyObjectByType<ThuyKieu.Environment.Chapter01Audio>();
var sources = audio.GetComponentsInChildren<UnityEngine.AudioSource>();
var checks = new System.Collections.Generic.List<string>();
System.Action<bool,string> check = (ok, name) => checks.Add((ok ? "PASS: " : "FAIL: ") + name);
check(UnityEngine.Object.FindObjectsByType<UnityEngine.AudioListener>().Count(l=>l.isActiveAndEnabled)==1,"One active listener");
check(!UnityEngine.AudioListener.pause && UnityEngine.AudioListener.volume>0,"Listener unmuted");
foreach (string name in new[]{"MusicSource","RainSource","WindSource"})
{
    var source=sources.Single(s=>s.name==name);
    check(source.clip!=null&&source.isPlaying&&!source.mute&&source.volume>0,name+" plays audible loop");
}
var sfx=sources.Single(s=>s.name=="SFXSource");
sfx.Stop();
audio.PlayDoor(true, UnityEngine.Camera.main.transform.position);
check(sfx.isPlaying,"Door opening plays");
sfx.Stop();
audio.PlayDoor(false, UnityEngine.Camera.main.transform.position);
check(sfx.isPlaying,"Door closing plays");
var flags=System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance;
typeof(ThuyKieu.Environment.Chapter01Audio).GetMethod("PlayUI",flags).Invoke(audio,null);
check(sources.Single(s=>s.name=="UISource").isPlaying,"UI confirmation plays");
sfx.Stop();
typeof(ThuyKieu.Environment.Chapter01Audio).GetMethod("OnCue",flags).Invoke(audio,new object[]{"camera","Table_Contract"});
check(sfx.isPlaying,"Contract paper cue plays");
var report=new {completed=true,passed=checks.All(c=>c.StartsWith("PASS:")),checks};
System.IO.File.WriteAllText("Tools/Chapter01/AudioReport.json",Newtonsoft.Json.JsonConvert.SerializeObject(report,Newtonsoft.Json.Formatting.Indented));
return report;
