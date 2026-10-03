using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThuyKieu.Core;
using ThuyKieu.Dialogue;
using ThuyKieu.Interaction;
using ThuyKieu.Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace ThuyKieu.Environment.Editor
{
    public static class Chapter01EnvironmentCheck
    {
        [Serializable] private class Report { public bool completed; public bool passed=true; public List<string> checks=new List<string>(); }
        private static Report _report;
        private static PlayerMovement _player;
        private static ThirdPersonCameraController _camera;
        private static Chapter01Director _director;
        private static Keyboard _keyboard, _previousKeyboard;
        private static InputSettings.BackgroundBehavior _background;
        private static bool _runInBackground, _moving;
        private static bool _heardWood, _heardStone;
        private static int _phase, _waypoint;
        private static float _start;
        private static Vector3 _origin;
        private static Vector3[] _route;
        private static readonly string[] RouteNames={"Mother approach","Ma Giam Sinh approach","Contract approach","West aisle","Side area","West of table","Behind table west","Behind table","Behind table east","East aisle","Main door inside","Door to courtyard","Courtyard path","Main gate","Return across courtyard","Return through door"};
        public static bool Running { get; private set; }

        public static void Start()
        {
            if (!Application.isPlaying || Running) throw new InvalidOperationException("Start once in Chapter 1 Play Mode.");
            _player=UnityEngine.Object.FindAnyObjectByType<PlayerMovement>();
            _camera=UnityEngine.Object.FindAnyObjectByType<ThirdPersonCameraController>();
            _director=UnityEngine.Object.FindAnyObjectByType<Chapter01Director>();
            if(_player==null||_director==null)throw new InvalidOperationException("Chapter 1 is not loaded.");
            _report=new Report(); _phase=0;_waypoint=0;_moving=false;_heardWood=false;_heardStone=false;_start=Time.time;_origin=_player.transform.position;
            _previousKeyboard=Keyboard.current;_keyboard=InputSystem.AddDevice<Keyboard>();
            _background=InputSystem.settings.backgroundBehavior;_runInBackground=Application.runInBackground;
            InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;Application.runInBackground=true;
            _route=new[]{P(-2.5f,.2f),P(2.5f,-.6f),P(0,1.2f),P(-4,.2f),P(-4,2.55f),P(-2.1f,2.55f),P(-2.1f,4.8f),P(0,4.8f),P(2.1f,4.8f),P(2.1f,1.2f),P(0,-1.2f),P(0,-4.4f),P(0,-8),P(0,-11.5f),P(0,-4.4f),P(0,-1.2f)};
            Running=true;InputSystem.onAfterUpdate+=Supply;EditorApplication.update+=Tick;Application.logMessageReceived+=CaptureLog;
        }
        private static Vector3 P(float x,float z)=>new Vector3(x,0,z);
        private static void Supply()
        {
            if(_keyboard!=null&&InputState.currentUpdateType==InputUpdateType.Dynamic)
            {_keyboard.MakeCurrent();InputState.Change(_keyboard,_moving?new KeyboardState(Key.W):new KeyboardState());}
        }
        private static void CaptureLog(string message,string trace,LogType type)
        {if(type==LogType.Error||type==LogType.Exception||type==LogType.Assert)Check(false,"Runtime: "+message);}
        private static void Check(bool condition,string name)
        {_report.passed&=condition;_report.checks.Add((condition?"PASS: ":"FAIL: ")+name);}
        private static void Tick()
        {
            if(!Application.isPlaying){Finish();return;}
            try
            {
                float elapsed=Time.time-_start;
                if(elapsed>15)throw new InvalidOperationException("Environment phase "+_phase+", route "+_waypoint+" timed out at "+_player.transform.position);
                switch(_phase)
                {
                    case 0:
                        if(elapsed<2)return;
                        Audit();
                        var manager=DialogueManager.Instance;
                        while(manager.IsDialogueActive&&_director.CurrentStage==Chapter01Director.Stage.Opening)manager.Advance();
                        Check(!_player.ControlLocked,"Environment traversal starts after existing opening unlock");
                        _camera.enabled=false;_phase=1;_start=Time.time;break;
                    case 1:
                        var footsteps=UnityEngine.Object.FindAnyObjectByType<Chapter01Audio>().GetComponentsInChildren<AudioSource>().First(s=>s.name=="FootstepSource");
                        if(footsteps.isPlaying)
                        {
                            bool inside=Mathf.Abs(_player.transform.position.x)<6.3f&&_player.transform.position.z>-3.2f;
                            _heardWood|=inside;_heardStone|=!inside;
                        }
                        Vector3 direction=_route[_waypoint]-_player.transform.position;direction.y=0;
                        if(_player.transform.position.y<-.15f||_player.transform.position.y>.3f)throw new InvalidOperationException("Floor grounding lost: "+_player.transform.position);
                        if(direction.magnitude<.18f)
                        {
                            Check(true,RouteNames[_waypoint]+" reachable using W and CharacterController");
                            if(_waypoint==11)Check(UnityEngine.Object.FindAnyObjectByType<ProximityDoor>().IsOpen,"Main door is open before passing the threshold");
                            _waypoint++;_start=Time.time;_moving=false;
                            if(_waypoint==_route.Length)
                            {
                                Check(_heardWood,"Wood footsteps play during indoor traversal");
                                Check(_heardStone,"Stone footsteps play during courtyard traversal");
                                _phase=2;Cue("Reveal_MaGiamSinh");_camera.enabled=true;
                            }
                            return;
                        }
                        Camera.main.transform.rotation=Quaternion.LookRotation(direction);_moving=true;break;
                    case 2:
                        if(elapsed<1.5f)return;
                        var atmosphere=UnityEngine.Object.FindAnyObjectByType<Chapter01Atmosphere>();
                        Check(atmosphere.Moment=="Entrance"&&Find("EntranceGlow").GetComponent<Light>().intensity>2.2f,"Ma entrance subtly raises local light");
                        CheckCamera("Entrance camera");Cue("Table_Contract");_phase=3;_start=Time.time;break;
                    case 3:
                        if(elapsed<1.5f)return;
                        Check(Find("ContractLamp").GetComponent<Light>().intensity>4.8f,"Contract light emphasizes parchment");
                        CheckCamera("Contract camera");Cue("Hero_Kieu_Rain");_phase=4;_start=Time.time;break;
                    case 4:
                        if(elapsed<1.5f)return;
                        Check(UnityEngine.Object.FindAnyObjectByType<Chapter01Atmosphere>().Moment=="Ending"&&Find("Moonlight").GetComponent<Light>().color.b>Find("Moonlight").GetComponent<Light>().color.r,"Ending retains cold exterior");
                        CheckCamera("Player-relative ending camera");
                        Check(_player.GetComponent<PlayerInteraction>().ControlLocked==false,"Presentation cues preserve interaction state");
                        Finish();break;
                }
            }
            catch(Exception exception){Check(false,exception.ToString());Finish();}
        }
        private static void Audit()
        {
            var audio=UnityEngine.Object.FindAnyObjectByType<Chapter01Audio>();
            Check(UnityEngine.Object.FindObjectsByType<PlayerMovement>().Length==1,"Exactly one player");
            Check(UnityEngine.Object.FindObjectsByType<Chapter01Audio>().Length==1,"One scene audio component, no duplicate AudioManager");
            Check(audio!=null&&audio.GetComponentsInChildren<AudioSource>().Length==6,"Music/rain/wind/local SFX/UI/footstep hooks configured");
            var sources=audio.GetComponentsInChildren<AudioSource>();
            Check(sources.Where(s=>s.name=="MusicSource"||s.name=="RainSource"||s.name=="WindSource").All(s=>s.loop&&s.spatialBlend==0),"Music and ambience are quiet 2D loops");
            Check(sources.Where(s=>s.name=="SFXSource"||s.name=="FootstepSource").All(s=>s.spatialBlend==1),"Localized door/paper/footsteps use 3D audio");
            Check(sources.First(s=>s.name=="UISource").spatialBlend==0,"UI audio is 2D");
            Check(sources.Where(s=>s.name=="MusicSource"||s.name=="RainSource"||s.name=="WindSource").All(s=>s.clip!=null&&s.isPlaying),"Music, rain and wind clips play at runtime");
            var serializedAudio=new SerializedObject(audio);
            Check(new[]{"_paper","_doorOpen","_doorClose","_uiConfirm","_woodStep","_stoneStep"}.All(field=>serializedAudio.FindProperty(field).objectReferenceValue!=null),"All six interaction and footstep clips assigned");
            bool sheltered=true;
            foreach(float x in new[]{-4f,0,4f}) foreach(float z in new[]{-1f,2f,5f})
                sheltered &= Physics.Raycast(new Vector3(x,2.7f,z),Vector3.up,out var roofHit,4,1<<8)
                    && (roofHit.collider.name=="Ceiling"||roofHit.collider.name=="RoofBeam"||roofHit.collider.name=="Slab");
            Check(sheltered,"Main hall has solid overhead shelter across nine sample points");
            Check(Find("ChairsAndContractTable")!=null&&Find("FamilyAltar")!=null&&Find("ScholarCabinet")!=null,"Existing furniture prefabs reused");
            Check(Find("FamilySideWing")!=null&&Find("Well")!=null&&Find("Plants")!=null,"Existing architecture, well and vegetation reused");
            Check(UnityEngine.Object.FindObjectsByType<MeshCollider>().Count(c=>c.enabled)==0,"Primitive colliders, no whole-house MeshCollider");
            var lights=UnityEngine.Object.FindObjectsByType<Light>();
            Check(lights.Count(l=>l.shadows!=LightShadows.None)<=2,"At most two realtime shadow lights");
            var warm=Find("OilLamp").GetComponent<Light>();var cold=Find("Moonlight").GetComponent<Light>();
            Check(warm.color.r>warm.color.b*2&&cold.color.b>cold.color.r*1.5f,"Warm interior / cold exterior light separation");
            var rain=Find("CourtyardRain").GetComponent<ParticleSystem>();var particles=new ParticleSystem.Particle[rain.main.maxParticles];int count=rain.GetParticles(particles);
            Check(rain.isPlaying&&count>30&&rain.main.maxParticles<=650,"Courtyard rain running within particle budget");
            Check(particles.Take(count).All(p=>p.position.z < -4.3f),"All sampled raindrops remain outside the covered hall and porch");
            Check(rain.collision.enabled&&rain.collision.lifetimeLoss.constant==1,"Rain dies on floor/roof collision");
            Check(_player.transform.position.y>-.1f&&Vector3.Distance(_player.transform.position,_origin)<.15f,"Player spawns grounded at existing spawn");
        }
        private static Transform Find(string name)=>UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Transform>(true)).First(t=>t.name==name);
        private static void Cue(string name)
        {
            // Exercise the same tag router used by Ink without changing narrative state.
            typeof(Chapter01Director).GetMethod("RouteTags",System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance).Invoke(_director,new object[]{new[]{"camera:"+name,"speaker:Narrator"}});
            _camera.ControlLocked=true;
        }
        private static void CheckCamera(string name)
        {Check(!Physics.CheckSphere(Camera.main.transform.position,.07f,1<<8,QueryTriggerInteraction.Ignore),name+" remains outside environment colliders");}
        private static void Finish()
        {
            if(!Running)return;Running=false;_moving=false;
            InputSystem.onAfterUpdate-=Supply;EditorApplication.update-=Tick;Application.logMessageReceived-=CaptureLog;
            InputSystem.RemoveDevice(_keyboard);_keyboard=null;if(_previousKeyboard!=null)_previousKeyboard.MakeCurrent();
            InputSystem.settings.backgroundBehavior=_background;Application.runInBackground=_runInBackground;
            if(_camera!=null){_camera.enabled=true;_camera.ControlLocked=false;_camera.SetDialogueCue(null);}
            _report.completed=true;File.WriteAllText("Tools/Chapter01/EnvironmentReport.json",JsonUtility.ToJson(_report,true));
        }
    }
}
