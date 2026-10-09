using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThuyKieu.Dialogue;
using ThuyKieu.Environment;
using ThuyKieu.Interaction;
using ThuyKieu.Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

namespace ThuyKieu.Core.Editor
{
    public static class Chapter01ReworkPlayCheck
    {
        private static readonly List<string> Checks = new List<string>();
        private static readonly HashSet<string> Seen = new HashSet<string>();
        private static Chapter01Director _director;
        private static DialogueManager _dialogue;
        private static DialogueUI _ui;
        private static PlayerMovement _player;
        private static PlayerInteraction _interaction;
        private static Keyboard _keyboard, _previous;
        private static Key[] _keys;
        private static float _start, _stageStart, _nextAdvance, _waitStart;
        private static Chapter01Director.Stage _lastStage;
        private static bool _passed, _background, _pulse, _ending, _waitingChecked, _openingButtonPressed;
        private static int _exitWaypoint, _choices;
        private static int _capturedChoice;
        private static float _choiceReady;
        private static bool _choiceImageSaved;
        private static InputSettings.BackgroundBehavior _focus;
        public static bool Running {get; private set;}

        public static void Start()
        {
            if (!Application.isPlaying || Running) throw new InvalidOperationException("Start once in Play Mode.");
            _director = UnityEngine.Object.FindAnyObjectByType<Chapter01Director>();
            _dialogue = DialogueManager.Instance; _ui = UnityEngine.Object.FindAnyObjectByType<DialogueUI>();
            _player = UnityEngine.Object.FindAnyObjectByType<PlayerMovement>();
            _interaction = _player.GetComponent<PlayerInteraction>();
            Checks.Clear(); Seen.Clear(); _passed = true; _ending=false; _waitingChecked=false; _openingButtonPressed=false; _exitWaypoint=0; _choices=0; _capturedChoice=-1;
            _start=_stageStart=Time.realtimeSinceStartup; _nextAdvance=0; _lastStage=_director.CurrentStage;
            _previous=Keyboard.current; _keyboard=InputSystem.AddDevice<Keyboard>(); _keys=Array.Empty<Key>();
            _background=Application.runInBackground; _focus=InputSystem.settings.backgroundBehavior;
            Application.runInBackground=true; InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
            Running=true; InputSystem.onAfterUpdate+=Supply; EditorApplication.update+=Tick; Application.logMessageReceived+=Log;
            Check(Vector3.Distance(_player.transform.position,Find("KieuSittingStart").position)<.3f,"Kiều starts seated at the bed edge");
            Check(_player.ControlLocked,"Opening locks movement");
            Check(UnityEngine.Object.FindObjectsByType<PlayerMovement>().Length==1 && UnityEngine.Object.FindObjectsByType<DialogueManager>().Length==1,"Single player and dialogue runtime");
            Check(!UnityEngine.Object.FindObjectsByType<Transform>().Any(t=>t.name=="VuongOng"||t.name=="VuongQuan"),"Arrested men absent");
            var audio=UnityEngine.Object.FindAnyObjectByType<Chapter01Audio>();
            Check(audio.GetComponentsInChildren<AudioSource>().Any(s=>s.name=="RainSource"&&s.isPlaying),"Rain audible in opening");
            var room=Find("KieuRoom");
            Check(new[]{"Bed","EastPartition","Chest","Stool"}.All(n=>room.Find(n).GetComponent<Collider>().enabled)
                && Find("KieuRoomDoor").GetComponentInChildren<Collider>().enabled,"Bedroom furniture and closed room door have colliders");
            Check(UnityEngine.Object.FindObjectsByType<Light>().Count(l=>l.shadows!=LightShadows.None)<=2,"At most two realtime shadow lights");
            var rain=Find("CourtyardRain").GetComponent<ParticleSystem>();
            Check(rain.isPlaying&&rain.main.maxParticles<=650&&rain.collision.enabled,"Rain running with bounded particles and roof/floor collision");
            var particles=new ParticleSystem.Particle[rain.main.maxParticles];int count=rain.GetParticles(particles);
            Check(count>0&&particles.Take(count).All(p=>p.position.z<-4.3f),"Sampled rain remains outside the house");
            Check(!Physics.CheckSphere(Camera.main.transform.position,.07f,1<<8,QueryTriggerInteraction.Ignore),"Opening camera clear of furniture and walls");
            foreach(var actor in new[]{Find("ThuyKieu"),Find("MeKieu"),Find("MaGiamSinh")})
            {
                var animator=actor.GetComponentInChildren<Animator>(true);
                Check(animator!=null&&animator.avatar!=null&&animator.avatar.isValid&&animator.avatar.isHuman,actor.name+" has a valid Humanoid avatar");
                Check(actor.GetComponentsInChildren<SkinnedMeshRenderer>(true).All(r=>r.sharedMesh!=null&&r.sharedMaterials.All(m=>m!=null&&m.shader!=null)),actor.name+" has mesh/material references");
            }
            Check(Find("MeKieu").GetComponentInChildren<Animator>().GetBool("IsSitting"),"Mẹ Kiều begins seated on her chair");
        }
        private static void Supply()
        {
            if(InputState.currentUpdateType!=InputUpdateType.Dynamic)return;
            _keyboard.MakeCurrent();InputState.Change(_keyboard,new KeyboardState(_keys));
            if(_pulse){_keys=Array.Empty<Key>();_pulse=false;}
        }
        private static void Log(string message,string stack,LogType type)
        {if(type==LogType.Error||type==LogType.Exception||type==LogType.Assert)Check(false,"Runtime: "+message);}
        private static void Check(bool ok,string label){_passed&=ok;Checks.Add((ok?"PASS: ":"FAIL: ")+label);}
        private static void Once(string label, bool ok)
        {if(Seen.Add(label))Check(ok,label);}
        private static void Tick()
        {
            try
            {
                if(!Application.isPlaying)throw new InvalidOperationException("Play interrupted");
                if(SceneManager.GetActiveScene().name=="Chapter02_Placeholder")
                {
                    Check(InkStatePersistence.Instance!=null&&!string.IsNullOrEmpty(InkStatePersistence.Instance.StateJson),"Ink session persists into Chapter 2 placeholder");
                    var story=new Ink.Runtime.Story(AssetDatabase.LoadAssetAtPath<TextAsset>(ThuyKieu.Dialogue.Editor.InkStoryCompiler.Output).text);
                    story.state.LoadJson(InkStatePersistence.Instance.StateJson);
                    Check((bool)story.variablesState["c1_co_tram_gia_dinh"]&&(bool)story.variablesState["c1_hoi_400"]&&(bool)story.variablesState["c1_tien_truoc"],"Hairpin, probe and payment condition persist after scene load");
                    Check(story.canContinue&&story.Continue().Length>0,"Saved runtime resumes Chapter 2");
                    Finish();return;
                }
                if(Time.realtimeSinceStartup-_start>200)throw new InvalidOperationException("Full path timed out");
                var stage=_director.CurrentStage;
                if(stage!=_lastStage){_lastStage=stage;_stageStart=Time.realtimeSinceStartup;_keys=Array.Empty<Key>();}
                if(stage!=Chapter01Director.Stage.Negotiation&&Time.realtimeSinceStartup-_stageStart>30)
                    throw new InvalidOperationException("Gate "+stage+" timed out at "+_player.transform.position+" target="+(_interaction.CurrentTarget as Component)?.name);
                if(_dialogue.IsDialogueActive)
                {
                    _keys=Array.Empty<Key>();
                    Once("Dialogue locks movement and interaction",_player.ControlLocked&&_interaction.ControlLocked);
                    if(_ui.CurrentName=="Người đưa tin") Once("Messenger reports from doorway",true);
                    if(_ui.CurrentText.Contains("Màn hình vẫn"))
                        Once("Black screen during opening narration",Find("OpeningFade").GetComponent<CanvasGroup>().alpha>.99f);
                    if(_ui.CurrentText.Contains("Ánh sáng dần"))
                        ScreenCapture.CaptureScreenshot("Tools/Chapter01/ReworkOpening.png");
                    if(Time.realtimeSinceStartup<_nextAdvance)return;
                    _nextAdvance=Time.realtimeSinceStartup+.11f;
                    if(_dialogue.IsAtChoicePoint)
                    {
                        if(_capturedChoice!=_choices)
                        {
                            _capturedChoice=_choices;_choiceReady=Time.realtimeSinceStartup+1.6f;_choiceImageSaved=false;
                            return;
                        }
                        if(!_choiceImageSaved&&Time.realtimeSinceStartup>_choiceReady-.5f)
                        {ScreenCapture.CaptureScreenshot("Tools/Chapter01/ReworkChoice"+_choices+".png");_choiceImageSaved=true;}
                        if(Time.realtimeSinceStartup<_choiceReady)return;
                        int count=_dialogue.InkStory.currentChoices.Count;
                        Check(count==(_choices==0?4:3),"Choice count "+count);
                        if(_choices==0)ScreenCapture.CaptureScreenshot("Tools/Chapter01/ReworkProbe.png");
                        _ui.OnChoiceClicked(_choices++==0?2:1);
                    }
                    else {if(_ui.IsRevealing)_ui.OnContinueClicked();_ui.OnContinueClicked();}
                    return;
                }
                switch(stage)
                {
                    case Chapter01Director.Stage.Opening: return;
                    case Chapter01Director.Stage.Hallway:
                        var standButton=Find("OpeningStandButton").GetComponent<UnityEngine.UI.Button>();
                        var animator=_player.GetComponentInChildren<Animator>();
                        if(!_openingButtonPressed)
                        {
                            Once("Stand button appears while Kiều remains locked and seated",standButton.gameObject.activeInHierarchy&&_player.ControlLocked&&animator.GetBool("IsSitting"));
                            _keys=new[]{Key.E};_pulse=true;_openingButtonPressed=true;return;
                        }
                        if(_player.ControlLocked)
                        {
                            Once("Movement stays locked during Sit To Stand",true);
                            return;
                        }
                        Once("Sit To Stand returns Kiều to idle before movement",!animator.GetBool("IsSitting"));
                        if(Walk(Find("KieuRoomDoor").position+Vector3.forward*.22f))PressExpected("KieuRoomDoor");break;
                    case Chapter01Director.Stage.Mother:
                        if(_player.transform.position.z>2.3f){Walk(new Vector3(-3.7f,0,1.85f));break;}
                        Once("Hallway reached by walking through the opened room door",_player.transform.position.z<2.3f);
                        if(Walk(new Vector3(-2.5f,0,.6f)))PressExpected("MeKieu");break;
                    case Chapter01Director.Stage.Explore:
                        if(_director.ClueCount==0)
                        {
                            Once("Exploration begins before Ma arrival",!Find("MaGiamSinh").gameObject.activeInHierarchy);
                            if(Walk(new Vector3(-.85f,0,1.15f)))PressExpected("DebtPaper");
                        }
                        else if(_director.ClueCount==1)
                        {
                            Once("Debt flag set once",(bool)_dialogue.InkStory.variablesState["c1_xem_to_trat"]);
                            var clue=Find("DebtPaper").GetComponent<ChapterClueInteractable>();clue.Interact();
                            Once("Completed clue cannot retrigger",_director.ClueCount==1&&!_dialogue.IsDialogueActive);
                            if(Walk(new Vector3(.85f,0,1.15f)))PressExpected("MoneyBag");
                        }
                        else
                        {
                            Once("Two clues sufficient; third optional",_director.ClueCount==2&&!(bool)_dialogue.InkStory.variablesState["c1_xem_nu_trang"]);
                            if(Walk(new Vector3(-2.5f,0,.6f)))PressExpected("MeKieu");
                        }break;
                    case Chapter01Director.Stage.Door:
                        Once("Main door waits for E",!Find("MainDoor").GetComponent<ProximityDoor>().IsOpen);
                        if(Walk(new Vector3(-.4f,0,-1.1f)))PressExpected("MainDoorInteraction");break;
                    case Chapter01Director.Stage.Offer:
                        Once("Ma revealed only after door interaction",Find("MaGiamSinh").gameObject.activeInHierarchy);
                        if(Walk(new Vector3(-2.5f,0,.6f))&&_interaction.CurrentTarget!=null)PressExpected("MeKieu");break;
                    case Chapter01Director.Stage.Contract:
                        Once("Probe branch flag preserved",(bool)_dialogue.InkStory.variablesState["c1_hoi_400"]);
                        if(Walk(new Vector3(0,0,1.15f)))PressExpected("ContractTable");break;
                    case Chapter01Director.Stage.Waiting:
                        _keys=Array.Empty<Key>();
                        if(!_waitingChecked){_waitStart=Time.time;_waitingChecked=true;Check((bool)_dialogue.InkStory.variablesState["c1_tien_truoc"],"Contract payment clause added");}
                        if(Time.time-_waitStart>5.3f)Once("Waiting gate lasts at least five seconds",!_dialogue.IsDialogueActive);
                        break;
                    case Chapter01Director.Stage.Exit:
                        Once("Hairpin granted before departure",(bool)_dialogue.InkStory.variablesState["c1_co_tram_gia_dinh"]);
                        Vector3[] route={new Vector3(-.3f,0,.2f),new Vector3(-.3f,0,-1.3f),new Vector3(0,0,-5.55f)};
                        if(_exitWaypoint<route.Length){if(Walk(route[_exitWaypoint]))_exitWaypoint++;}
                        else PressExpected("ChapterExit");break;
                    case Chapter01Director.Stage.Complete:
                        if(_ending)return;
                        Check(_choices==2,"All dialogue choices completed");
                        Check(_player.transform.position.z<-4.8f,"Kiều physically exits into rain");
                        Check(_player.ControlLocked,"Completion locks gameplay");
                        Check(_player.GetComponent<CharacterController>().isGrounded,"No floor fallthrough");
                        var saved=new Ink.Runtime.Story(AssetDatabase.LoadAssetAtPath<TextAsset>(ThuyKieu.Dialogue.Editor.InkStoryCompiler.Output).text);
                        saved.state.LoadJson(InkStatePersistence.Instance.StateJson);
                        Check((bool)saved.variablesState["c1_co_tram_gia_dinh"],"Hairpin serialized in Ink session");
                        _ending=true;_director.ContinueAfterChapter();break;
                }
            }
            catch(Exception error){Check(false,error.ToString());Finish();}
        }
        private static bool Walk(Vector3 target)
        {
            Vector3 delta=target-_player.transform.position;delta.y=0;
            if(delta.magnitude<.18f){_keys=Array.Empty<Key>();return true;}
            var camera=Camera.main.transform;var forward=camera.forward;forward.y=0;forward.Normalize();var right=camera.right;right.y=0;right.Normalize();
            float x=Vector3.Dot(delta.normalized,right),y=Vector3.Dot(delta.normalized,forward);
            var keys=new List<Key>();if(Mathf.Abs(x)>.35f)keys.Add(x>0?Key.D:Key.A);if(Mathf.Abs(y)>.35f)keys.Add(y>0?Key.W:Key.S);
            _keys=keys.ToArray();return false;
        }
        private static void PressExpected(string name)
        {
            var component=_interaction.CurrentTarget as Component;
            if(component==null)return;
            if(component.name!=name)throw new InvalidOperationException("Expected "+name+" but target is "+component.name);
            Once("E interaction "+name,true);
            _keys=new[]{Key.E};_pulse=true;
        }
        private static Transform Find(string name)=>SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Transform>(true)).First(t=>t.name==name);
        private static void Finish()
        {
            Running=false;InputSystem.onAfterUpdate-=Supply;EditorApplication.update-=Tick;Application.logMessageReceived-=Log;
            InputSystem.RemoveDevice(_keyboard);if(_previous!=null&&_previous.added)_previous.MakeCurrent();
            InputSystem.settings.backgroundBehavior=_focus;Application.runInBackground=_background;
            File.WriteAllText("Tools/Chapter01/ReworkPlayReport.json",Newtonsoft.Json.JsonConvert.SerializeObject(new{passed=_passed,completed=true,seconds=Time.realtimeSinceStartup-_start,checks=Checks},Newtonsoft.Json.Formatting.Indented));
        }
    }
}
