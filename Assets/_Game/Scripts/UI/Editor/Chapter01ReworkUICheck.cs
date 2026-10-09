using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using ThuyKieu.Core;
using ThuyKieu.Dialogue;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ThuyKieu.UI.Editor
{
    public static class Chapter01ReworkUICheck
    {
        private static readonly BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        private static readonly Vector2Int[] Resolutions = {new Vector2Int(1920,1080),new Vector2Int(1600,900),new Vector2Int(1366,768),new Vector2Int(2560,1440)};
        private static readonly List<string> Checks = new List<string>();
        private static object _size, _sizes, _oldType;
        private static EditorWindow _view;
        private static int _oldWidth, _oldHeight, _resolution, _phase;
        private static float _start;
        private static bool _passed, _background;
        private static DialogueUI _ui;
        private static TMP_Text _body;
        private static string[] _lines;
        private static string[][] _choices;
        public static bool Running {get; private set;}

        public static void Start()
        {
            if (!Application.isPlaying || Running) throw new InvalidOperationException("Run in fresh Play Mode.");
            Checks.Clear();_passed=true;_resolution=0;_phase=0;Running=true;
            _background=Application.runInBackground;Application.runInBackground=true;
            _ui=UnityEngine.Object.FindAnyObjectByType<DialogueUI>();
            _body=_ui.GetComponentsInChildren<TMP_Text>(true).First(t=>t.name=="DialogueBodyText");
            var director=UnityEngine.Object.FindAnyObjectByType<Chapter01Director>();
            director.StopAllCoroutines();director.enabled=false;
            var fade=_ui.transform.Find("OpeningFade").GetComponent<CanvasGroup>();fade.alpha=0;
            _ui.enabled=false;
            var corpus=JObject.Parse(File.ReadAllText("Tools/Chapter01/ReworkDialogueCorpus.json"));
            _lines=corpus["lines"].ToObject<string[]>();
            _choices=corpus["choices"].ToObject<string[][]>();
            var asm=typeof(UnityEditor.Editor).Assembly;
            _view=EditorWindow.GetWindow(asm.GetType("UnityEditor.GameView"));_view.Show();_view.Focus();
            _size=_view.GetType().GetProperty("currentGameViewSize",Flags).GetValue(_view);
            _oldWidth=(int)_size.GetType().GetProperty("width").GetValue(_size);
            _oldHeight=(int)_size.GetType().GetProperty("height").GetValue(_size);
            _oldType=_size.GetType().GetProperty("sizeType").GetValue(_size);
            var sizesType=asm.GetType("UnityEditor.GameViewSizes");
            _sizes=sizesType.BaseType.GetProperty("instance",BindingFlags.Public|BindingFlags.Static).GetValue(null);
            SetResolution();
            EditorApplication.update+=Tick;
        }
        private static void SetResolution()
        {
            var resolution=Resolutions[_resolution];
            _size.GetType().GetProperty("width").SetValue(_size,resolution.x);
            _size.GetType().GetProperty("height").SetValue(_size,resolution.y);
            var property=_size.GetType().GetProperty("sizeType");
            property.SetValue(_size,Enum.Parse(property.PropertyType,"FixedResolution"));
            _sizes.GetType().GetMethod("Changed").Invoke(_sizes,null);
            _view.Repaint();_start=Time.realtimeSinceStartup;
        }
        private static void Check(bool ok,string label){_passed&=ok;Checks.Add((ok?"PASS: ":"FAIL: ")+label);}
        private static void Invoke(string name,params object[] args)=>typeof(DialogueUI).GetMethod(name,BindingFlags.Instance|BindingFlags.NonPublic).Invoke(_ui,args);
        private static void Tick()
        {
            try
            {
                if(!Application.isPlaying)throw new InvalidOperationException("Play stopped");
                var resolution=Resolutions[_resolution];string label=resolution.x+"x"+resolution.y;
                if(Time.realtimeSinceStartup-_start<.5f)return;
                if(_phase==0)
                {
                    if(Screen.width!=resolution.x||Screen.height!=resolution.y)
                    {if(Time.realtimeSinceStartup-_start>12)throw new InvalidOperationException("GameView resolution not applied: "+Screen.width+"x"+Screen.height);return;}
                    Check(true,label+" actual GameView resolution");
                    Invoke("HandleDialogueStarted",new object[]{null});
                    bool textFits=true,fontOk=true,panelFits=true;
                    foreach(string line in _lines)
                    {
                        Invoke("HandleLineChanged","Thúy Kiều",line);
                        Invoke("HandleChoicesChanged",new object[]{Array.Empty<DialogueChoice>()});
                        Invoke("CompleteReveal");Canvas.ForceUpdateCanvases();_body.ForceMeshUpdate();
                        textFits &= !_body.isTextOverflowing;
                        fontOk &= line.All(c=>char.IsWhiteSpace(c)||_body.font.HasCharacter(c,true,false));
                        panelFits &= PanelFits();
                    }
                    Check(textFits,label+" all "+_lines.Length+" authored lines fit");
                    Check(fontOk&&AssetDatabase.GetAssetPath(_body.font)=="Assets/Fonts/VietnameseTMP.asset",label+" Vietnamese glyphs use portable required font");
                    Check(panelFits,label+" all dialogue panels stay on screen");
                    bool choiceFits=true,overlap=false;
                    foreach(var choices in _choices)
                    {
                        Invoke("HandleLineChanged","Thúy Kiều",_lines.OrderByDescending(l=>l.Length).First());
                        Invoke("HandleChoicesChanged",new object[]{choices.Select(c=>new DialogueChoice(c)).ToArray()});
                        Invoke("CompleteReveal");Canvas.ForceUpdateCanvases();_body.ForceMeshUpdate();
                        foreach(var text in _ui.GetComponentsInChildren<TMP_Text>()) {text.ForceMeshUpdate();choiceFits &= !text.isTextOverflowing;}
                        var buttons=_ui.GetComponentsInChildren<Button>().Where(b=>b.name.Contains("Clone")).ToArray();
                        for(int i=1;i<buttons.Length;i++)
                        {
                            var a=new Vector3[4];var b=new Vector3[4];
                            buttons[i-1].GetComponent<RectTransform>().GetWorldCorners(a);buttons[i].GetComponent<RectTransform>().GetWorldCorners(b);
                            overlap |= b[1].y>a[0].y+.5f;
                        }
                        choiceFits &= PanelFits();
                    }
                    Check(choiceFits,label+" all choice groups and longest line fit");
                    Check(!overlap,label+" multiline choices do not overlap");
                    Invoke("HandleLineChanged","Thúy Kiều",_lines.OrderByDescending(l=>l.Length).First());
                    var four=_choices.First(c=>c.Length==4);
                    Invoke("HandleChoicesChanged",new object[]{four.Select(c=>new DialogueChoice(c)).ToArray()});
                    Invoke("CompleteReveal");Canvas.ForceUpdateCanvases();
                    _phase=1;_start=Time.realtimeSinceStartup;
                }
                else if (_phase == 1)
                {
                    ScreenCapture.CaptureScreenshot("Tools/Chapter01/ReworkUI_"+label+".png");
                    _phase=2;_start=Time.realtimeSinceStartup;
                }
                else
                {
                    _resolution++;
                    if(_resolution==Resolutions.Length){Finish();return;}
                    _phase=0;SetResolution();
                }
            }
            catch(Exception error){Check(false,error.ToString());Finish();}
        }
        private static bool PanelFits()
        {
            var corners=new Vector3[4];_body.transform.parent.GetComponent<RectTransform>().GetWorldCorners(corners);
            return corners[0].y>=-1&&corners[1].y<=Screen.height+1&&corners[0].x>=-1&&corners[2].x<=Screen.width+1;
        }
        private static void Finish()
        {
            EditorApplication.update-=Tick;Running=false;Application.runInBackground=_background;
            if(_size!=null)
            {
                _size.GetType().GetProperty("width").SetValue(_size,_oldWidth);
                _size.GetType().GetProperty("height").SetValue(_size,_oldHeight);
                _size.GetType().GetProperty("sizeType").SetValue(_size,_oldType);
                _sizes.GetType().GetMethod("Changed").Invoke(_sizes,null);_view.Repaint();
            }
            File.WriteAllText("Tools/Chapter01/ReworkUIReport.json",Newtonsoft.Json.JsonConvert.SerializeObject(new {passed=_passed,checks=Checks},Newtonsoft.Json.Formatting.Indented));
        }
    }
}
