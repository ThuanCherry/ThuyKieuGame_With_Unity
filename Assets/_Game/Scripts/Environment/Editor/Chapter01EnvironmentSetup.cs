using System;
using System.IO;
using System.Linq;
using ThuyKieu.Core;
using ThuyKieu.Core.Editor;
using ThuyKieu.Dialogue;
using ThuyKieu.Interaction;
using ThuyKieu.Player;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace ThuyKieu.Environment.Editor
{
    public static class Chapter01EnvironmentSetup
    {
        private const string Materials = "Assets/_Game/Materials/Chapter01Environment";
        private const string Modules = "Assets/_Game/Prefabs/Architecture/Chapter01Modules";
        private static Material _wood, _trim, _stone, _tile, _paper, _glow;
        private static Transform _root;

        [MenuItem("ThuyKieu/Chapter 1/Complete environment")]
        public static void Apply()
        {
            if (Application.isPlaying || SceneManager.GetActiveScene().path != Chapter01Setup.ScenePath)
                throw new InvalidOperationException("Open Chapter01 in Edit Mode before applying environment.");
            Directory.CreateDirectory(Materials); Directory.CreateDirectory(Modules); AssetDatabase.Refresh();
            _wood = Material("DarkWood", new Color(.19f,.085f,.035f), .25f);
            _trim = Material("WoodTrim", new Color(.36f,.19f,.078f), .3f);
            _stone = Material("WetStone", new Color(.21f,.26f,.29f), .7f);
            _tile = Material("RoofTile", new Color(.12f,.17f,.19f), .4f);
            _paper = Material("WarmPaper", new Color(.78f,.63f,.38f), .15f);
            _glow = Material("LanternGlow", new Color(1,.62f,.22f), .1f);
            _glow.EnableKeyword("_EMISSION"); _glow.SetColor("_EmissionColor", new Color(1,.45f,.1f)*1.5f);
            _root = Node(null,"VuongGia");
            var house = Node(_root,"MainHouse"); var hall = Node(house,"MainHall");
            var structure = Node(hall,"Structure"); var props = Node(hall,"InteriorProps");
            var court = Node(_root,"Courtyard");
            Reparent("CourtyardFloor",court); Reparent("MainHallFloor",structure);
            foreach (string name in new[]{"BackWall","WestWall","EastWall","FrontWallLeft","FrontWallRight","DoorLintel","RoofBeam"}) Reparent(name,structure);
            foreach (string name in new[]{"CourtyardWest","CourtyardEast","CourtyardGate"}) Reparent(name,court);
            var wallColor = Material("LimePlaster",new Color(.48f,.43f,.32f),.12f);
            foreach (var wall in structure.GetComponentsInChildren<Renderer>())
            {
                if (wall.name.Contains("Wall")) wall.sharedMaterial = wallColor;
                else wall.sharedMaterial = _wood;
            }
            // Retain the proven floor/wall colliders while adding visible timber construction.
            for (int i=0;i<24;i++)
                Block(structure,"FloorPlank"+i,new Vector3(-5.75f+i*.5f,.008f,2),new Vector3(.48f,.018f,8.85f),i%3==0?_trim:_wood,false);
            for (int x=-1;x<=1;x+=2)
            {
                foreach(float z in new[]{-2.35f,2,6.25f})
                {
                    Block(structure,"Column"+x+"_"+z,new Vector3(x*5.65f,1.8f,z),new Vector3(.26f,3.6f,.26f),_wood,true);
                    Block(structure,"ColumnBase"+x+"_"+z,new Vector3(x*5.65f,.18f,z),new Vector3(.42f,.36f,.42f),_stone,true);
                }
                Block(structure,"WallWainscot"+x,new Vector3(x*5.79f,.65f,2),new Vector3(.12f,1.3f,8.8f),_wood,false);
                for(int i=0;i<5;i++) Screen(structure,"Window"+x+"_"+i,new Vector3(x*5.77f,1.5f,-1.6f+i*1.7f),new Vector3(1.35f,1.45f,1),90);
                Block(structure,"FrontPanel"+x,new Vector3(x*3.8f,.6f,-2.69f),new Vector3(4.4f,1.2f,.1f),_wood,false);
                for(int i=0;i<3;i++) Screen(structure,"FrontWindow"+x+"_"+i,new Vector3(x*(2.25f+i*1.4f),1.5f,-2.69f),new Vector3(1.1f,1.35f,1),0);
            }
            Block(structure,"BackWainscot",new Vector3(0,.65f,6.28f),new Vector3(11.7f,1.3f,.1f),_wood,false);
            Block(structure,"UpperBeam",new Vector3(0,3.15f,-2.55f),new Vector3(12,.3f,.32f),_trim,false);
            Block(structure,"Ceiling",new Vector3(0,3.57f,2),new Vector3(12.2f,.16f,9.3f),_wood,true);
            var roof = RoofModule(house,"TiledRoof",new Vector3(0,3.65f,2));
            roof.localScale = Vector3.one;

            var table = Find("ContractTable"); table.SetParent(hall,true);
            table.GetComponent<Renderer>().enabled = false;
            var tableCollider=table.GetComponent<BoxCollider>();
            tableCollider.center=new Vector3(0,-2,0); tableCollider.size=new Vector3(1.3f,6,1.9f);
            foreach(var leg in SceneManager.GetActiveScene().GetRootGameObjects().Where(g=>g.name=="TableLeg"))
            { leg.transform.SetParent(table,true); leg.SetActive(false); }
            var furniture=Reuse(hall,"ChairsAndContractTable","Props/TableSet",new Vector3(0,0,3.1f),0,1.4f);
            foreach(var c in furniture.GetComponentsInChildren<Collider>()) c.enabled=false;
            var paper=Find("Tờ khế"); paper.SetParent(hall,true); paper.position=new Vector3(-.22f,.781f,2.4f);
            paper.localScale=new Vector3(.65f,.015f,.45f); paper.GetComponent<Collider>().enabled=false;
            var ink=Material("Ink",new Color(.11f,.075f,.05f),.1f);
            for(int i=0;i<6;i++) Block(hall,"ContractInkLine"+i,new Vector3(-.23f,.79f,2.29f+i*.045f),new Vector3(i==0?.38f:.45f,.002f,.006f),ink,false);
            Block(hall,"ContractSeal",new Vector3(-.01f,.792f,2.25f),new Vector3(.07f,.004f,.07f),Material("SealRed",new Color(.48f,.055f,.025f),.2f),false);
            var tableLabel=table.Find("Label") ?? hall.Find("ContractLabel");
            if(tableLabel!=null)
            {
                tableLabel.SetParent(hall,true);tableLabel.name="ContractLabel";tableLabel.localScale=Vector3.one;
                tableLabel.position=new Vector3(-.22f,1.35f,2.4f); tableLabel.GetComponent<TMPro.TMP_Text>().fontSize=.9f;
            }
            Reuse(props,"FamilyAltar","Props/Altar",new Vector3(-2.6f,0,5.95f),0,1.5f);
            Reuse(props,"ScholarCabinet","Props/Cabinet",new Vector3(4.8f,0,5.7f),180,1);
            var side=Node(house,"SideArea");
            Reuse(side,"Bed","Props/Bed",new Vector3(-4.6f,0,4.8f),90,1);
            Reuse(props,"Ceramics","Props/CeramicJar",new Vector3(4.7f,0,-1.5f),20,.75f);
            Screen(side,"DecorativeScreen",new Vector3(-3.8f,0,4.4f),new Vector3(2.8f,2.1f,1),90);
            var screenCollider=GetOrAdd<BoxCollider>(side.Find("DecorativeScreen").gameObject);screenCollider.center=new Vector3(0,.5f,0);screenCollider.size=new Vector3(1,1,.05f);
            var lanterns=Node(hall,"Lanterns");
            foreach (int x in new[]{-1,1}) Lantern(lanterns,"HallLantern"+x,new Vector3(x*3.4f,2.65f,2.3f));
            Reuse(lanterns,"ContractOilLamp","Props/OilLamp",new Vector3(.65f,.83f,3.2f),0,1);
            Reuse(lanterns,"AltarOilLamp","Props/OilLamp",new Vector3(-2.15f,1.35f,5.9f),0,1);

            var path=Node(court,"StonePath");
            Find("CourtyardFloor").GetComponent<Renderer>().sharedMaterial=_stone;
            for(int i=0;i<9;i++) for(int j=-1;j<=1;j++)
                Block(path,"Paving"+i+"_"+j,new Vector3(j*1.03f,.012f,-3.6f-i*1.06f),new Vector3(.98f,.03f,1.01f),_stone,false);
            var wet=Material("Puddles",new Color(.10f,.15f,.18f),.96f);
            for(int i=0;i<6;i++) Block(court,"Puddle"+i,new Vector3((i%2==0?-1:1)*(2.5f+i*.22f),.012f,-4.1f-i*1.3f),new Vector3(1.3f,.014f,.6f),wet,false,PrimitiveType.Cylinder);
            var plants=Node(court,"Plants");
            Reuse(plants,"Bamboo","Vegetation/Bamboo",new Vector3(-7.9f,0,-4.7f),25,.8f);
            Reuse(plants,"ArecaPalm","Vegetation/ArecaPalm",new Vector3(7.4f,0,-9.8f),30,.9f);
            Reuse(plants,"Banana","Vegetation/BananaTree",new Vector3(-7.4f,0,-11),-25,.7f);
            Reuse(court,"Well","Props/Well",new Vector3(6.5f,0,-5.8f),-30,1);
            // The imported house is a closed exterior mesh; retain it as an adjacent family wing.
            Reuse(_root,"FamilySideWing","Architecture/KieuHouse_Exterior",new Vector3(-13.6f,0,.7f),90,1);
            var gate=Node(_root,"MainGate");
            var oldGate=Find("CourtyardGate"); oldGate.GetComponent<Renderer>().enabled=false;
            for(int sideSign=-1;sideSign<=1;sideSign+=2)
            {
                Block(gate,"GatePost"+sideSign,new Vector3(sideSign*1.85f,1.6f,-12.9f),new Vector3(.4f,3.2f,.45f),_wood,true);
                Reuse(gate,"GateDoor"+sideSign,"Architecture/DoorLeaf",new Vector3(sideSign*.88f,0,-13.2f),0,1.65f);
                Reuse(gate,"Fence"+sideSign,"Architecture/Fence",new Vector3(sideSign*5.65f,0,-13.1f),0,2.3f);
                Lantern(gate,"GateLantern"+sideSign,new Vector3(sideSign*2.2f,2.2f,-12.7f));
            }
            var gateRoof=RoofModule(gate,"GateRoof",new Vector3(0,3.2f,-13)); gateRoof.localScale=new Vector3(.34f,.45f,.23f);
            Block(court,"BackBoundary",new Vector3(0,1.5f,9.8f),new Vector3(20,3,.3f),_stone,true);
            // Close the narrow side strips along the house so the finite demo floor cannot be left.
            foreach(int sign in new[]{-1,1}) Block(court,"SideBoundary"+sign,new Vector3(sign*9.5f,1.5f,4),new Vector3(.3f,3,12),_stone,true);

            var audio=ConfigureAudio();
            var door=Node(house,"MainDoor"); door.position=new Vector3(0,0,-2.5f);
            for(int sign=-1;sign<=1;sign+=2)
            {
                var pivot=Node(door,sign<0?"LeftPivot":"RightPivot"); pivot.localPosition=new Vector3(sign*1.5f,0,0); pivot.localRotation=Quaternion.identity;
                var leaf=Reuse(pivot,"DoorLeaf","Architecture/DoorLeaf",Vector3.zero,0,1.3f);
                leaf.localPosition=new Vector3(-sign*.68f,0,0);
                foreach(var c in leaf.GetComponentsInChildren<Collider>()) c.enabled=false;
            }
            var proximity=GetOrAdd<ProximityDoor>(door.gameObject);
            Set(proximity,"_left",door.Find("LeftPivot")); Set(proximity,"_right",door.Find("RightPivot"));
            Set(proximity,"_player",UnityEngine.Object.FindAnyObjectByType<PlayerMovement>().transform); Set(proximity,"_audio",audio);
            Reparent("MainDoorInteraction",house);
            ConfigureLighting(audio);
            ConfigureRain(court);
            var wide=Find("WS_VuongGia"); wide.position=new Vector3(.6f,2.4f,-1.45f); wide.LookAt(new Vector3(0,1.2f,3));
            var camera=Camera.main; camera.backgroundColor=new Color(.025f,.04f,.07f); camera.farClipPlane=75;
            RenderSettings.skybox=null; RenderSettings.ambientMode=AmbientMode.Trilight;
            RenderSettings.ambientSkyColor=new Color(.22f,.28f,.38f); RenderSettings.ambientEquatorColor=new Color(.11f,.14f,.20f); RenderSettings.ambientGroundColor=new Color(.065f,.07f,.09f);
            RenderSettings.fog=true; RenderSettings.fogMode=FogMode.Linear; RenderSettings.fogColor=new Color(.035f,.055f,.08f); RenderSettings.fogStartDistance=20; RenderSettings.fogEndDistance=65;
            Physics.SyncTransforms();
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene()); EditorSceneManager.SaveScene(SceneManager.GetActiveScene()); AssetDatabase.SaveAssets();
        }

        private static Chapter01Audio ConfigureAudio()
        {
            var parent=Node(_root,"EnvironmentAudio");
            var audio=GetOrAdd<Chapter01Audio>(parent.gameObject);
            Set(audio,"_director",UnityEngine.Object.FindAnyObjectByType<Chapter01Director>());
            Set(audio,"_player",UnityEngine.Object.FindAnyObjectByType<PlayerMovement>()); Set(audio,"_dialogueUI",UnityEngine.Object.FindAnyObjectByType<DialogueUI>());
            string[] names={"MusicSource","RainSource","WindSource","SFXSource","UISource","FootstepSource"};
            string[] fields={"_musicSource","_rainSource","_windSource","_sfxSource","_uiSource","_footstepSource"};
            float[] volumes={.18f,.13f,.025f,.4f,.2f,.25f};
            for(int i=0;i<names.Length;i++)
            {
                var go=Node(parent,names[i]); var source=GetOrAdd<AudioSource>(go.gameObject);
                source.playOnAwake=false; source.loop=i<3; source.volume=volumes[i]; source.spatialBlend=i==3||i==5?1:0;
                source.minDistance=1; source.maxDistance=12; source.rolloffMode=AudioRolloffMode.Linear; Set(audio,fields[i],source);
            }
            string[] loops={"SadStrings","Rain","Wind"};
            for(int i=0;i<loops.Length;i++)
            {
                var clip=AssetDatabase.LoadAssetAtPath<AudioClip>(Chapter01AudioSetup.AudioFolder+loops[i]+".wav");
                if(clip!=null)parent.Find(names[i]).GetComponent<AudioSource>().clip=clip;
            }
            string[] clipFields={"_paper","_doorOpen","_doorClose","_uiConfirm","_woodStep","_stoneStep","_dialogueLetter"};
            string[] clips={"Paper","DoorOpen","DoorClose","UIConfirm","WoodStep","StoneStep","DialogueLetter"};
            for(int i=0;i<clips.Length;i++)
            {
                var clip=AssetDatabase.LoadAssetAtPath<AudioClip>(Chapter01AudioSetup.AudioFolder+clips[i]+".wav");
                if(clip!=null)Set(audio,clipFields[i],clip);
            }
            return audio;
        }
        private static void ConfigureLighting(Chapter01Audio audio)
        {
            var lights=Node(_root,"EnvironmentLighting");
            foreach(string name in new[]{"Moonlight","OilLamp","ContractLamp"}) Reparent(name,lights);
            var moon=Find("Moonlight").GetComponent<Light>(); moon.color=new Color(.48f,.63f,.86f); moon.intensity=.5f; moon.shadows=LightShadows.Soft; moon.shadowStrength=.75f;
            var hall=Find("OilLamp").GetComponent<Light>(); hall.transform.position=new Vector3(-1.8f,2.7f,1.7f); hall.intensity=4.2f; hall.range=8; hall.color=new Color(1,.67f,.34f); hall.shadows=LightShadows.None;
            var contract=Find("ContractLamp").GetComponent<Light>(); contract.transform.position=new Vector3(.3f,2.3f,3); contract.intensity=2.4f; contract.range=5.5f; contract.color=new Color(1,.72f,.4f); contract.shadows=LightShadows.None;
            var entry=Point(lights,"EntranceGlow",new Vector3(2.2f,2.6f,-.2f),new Color(1,.66f,.35f),.8f,5);
            Point(lights,"PorchGlow",new Vector3(0,2.7f,-3),new Color(1,.65f,.3f),1.6f,5);
            Point(lights,"GateGlow",new Vector3(0,2.7f,-12.5f),new Color(.9f,.63f,.36f),1.8f,5);
            Point(lights,"CourtyardMoonBounce",new Vector3(1,4,-7),new Color(.43f,.58f,.85f),1.4f,13);
            var atmosphere=GetOrAdd<Chapter01Atmosphere>(lights.gameObject);
            Set(atmosphere,"_director",UnityEngine.Object.FindAnyObjectByType<Chapter01Director>());
            Set(atmosphere,"_hall",hall); Set(atmosphere,"_entrance",entry); Set(atmosphere,"_contract",contract);
        }
        private static void ConfigureRain(Transform court)
        {
            var go=Find("CourtyardRain"); go.SetParent(Node(court,"Rain"),true); go.position=new Vector3(0,7,-8.7f); go.rotation=Quaternion.Euler(90,0,-4);
            var rain=go.GetComponent<ParticleSystem>(); rain.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
            var main=rain.main; main.startLifetime=.72f; main.startSpeed=10; main.startSize=.018f; main.maxParticles=650; main.prewarm=true; main.loop=true;
            main.startColor=new Color(.55f,.70f,.86f,.45f); main.simulationSpace=ParticleSystemSimulationSpace.World;
            var shape=rain.shape; shape.shapeType=ParticleSystemShapeType.Box; shape.scale=new Vector3(17,7.6f,.05f);
            var emission=rain.emission; emission.rateOverTime=550;
            var collision=rain.collision; collision.enabled=true; collision.type=ParticleSystemCollisionType.World; collision.mode=ParticleSystemCollisionMode.Collision3D;
            collision.collidesWith=1<<8; collision.quality=ParticleSystemCollisionQuality.Low; collision.lifetimeLoss=1; collision.enableDynamicColliders=false;
            var renderer=rain.GetComponent<ParticleSystemRenderer>(); renderer.renderMode=ParticleSystemRenderMode.Stretch; renderer.lengthScale=3; renderer.velocityScale=.012f;
            renderer.shadowCastingMode=ShadowCastingMode.Off; renderer.receiveShadows=false;
            var rainMaterial=renderer.sharedMaterial; rainMaterial.SetFloat("_Surface",1); rainMaterial.SetFloat("_Blend",0); rainMaterial.SetFloat("_ZWrite",0);
            rainMaterial.SetFloat("_SrcBlend",(int)BlendMode.SrcAlpha); rainMaterial.SetFloat("_DstBlend",(int)BlendMode.OneMinusSrcAlpha);
            rainMaterial.EnableKeyword("_SURFACE_TYPE_TRANSPARENT"); rainMaterial.renderQueue=3000; EditorUtility.SetDirty(rainMaterial);
        }
        private static void Lantern(Transform parent,string name,Vector3 position)
        {
            var lantern=Node(parent,name); lantern.position=position;
            Block(lantern,"Shade",new Vector3(0,0,0),new Vector3(.35f,.42f,.35f),_glow,false);
            foreach(int sign in new[]{-1,1}) Block(lantern,"Cap"+sign,new Vector3(0,sign*.24f,0),new Vector3(.44f,.06f,.44f),_wood,false);
            foreach(int x in new[]{-1,1}) foreach(int z in new[]{-1,1}) Block(lantern,"Frame"+x+z,new Vector3(x*.18f,0,z*.18f),new Vector3(.03f,.48f,.03f),_wood,false);
            Block(lantern,"Cord",new Vector3(0,.56f,0),new Vector3(.025f,.6f,.025f),_wood,false);
        }
        private static void Screen(Transform parent,string name,Vector3 position,Vector3 scale,float yaw)
        {
            var screen=Node(parent,name); screen.gameObject.layer=8; screen.localPosition=position; screen.localRotation=Quaternion.Euler(0,yaw,0); screen.localScale=scale;
            Block(screen,"Panel",new Vector3(0,.5f,0),new Vector3(1,1,.04f),_paper,false);
            foreach(int sign in new[]{-1,1}) Block(screen,"Post"+sign,new Vector3(sign*.5f,.5f,-.04f),new Vector3(.05f,1.08f,.06f),_wood,false);
            for(int i=0;i<6;i++) Block(screen,"Slat"+i,new Vector3(-.42f+i*.168f,.5f,-.04f),new Vector3(.02f,1,.035f),_wood,false);
            for(int i=0;i<4;i++) Block(screen,"Rail"+i,new Vector3(0,i*.333f,-.04f),new Vector3(1,.025f,.035f),_trim,false);
        }
        private static Transform RoofModule(Transform parent,string name,Vector3 position)
        {
            string path=Modules+"/VuongGiaRoof.prefab";
            var existing=parent.Find(name); if(existing!=null){existing.position=position;return existing;}
            var prefab=AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if(prefab==null)
            {
                var source=Node(null,"VuongGiaRoofModule");
                for(int sign=-1;sign<=1;sign+=2)
                {
                    var slope=Node(source,"Slope"+sign); slope.localPosition=new Vector3(0,.8f,sign*2.65f); slope.localRotation=Quaternion.Euler(sign*18,0,0);
                    Block(slope,"Slab",Vector3.zero,new Vector3(13.7f,.16f,5.6f),_tile,true);
                    for(int i=0;i<46;i++)
                    {
                        var tile=Block(slope,"TileChannel"+i,new Vector3(-6.72f+i*.298f,.09f,0),new Vector3(.14f,2.8f,.14f),_tile,false,PrimitiveType.Cylinder);
                        tile.localRotation=Quaternion.Euler(90,0,0);
                    }
                    Block(slope,"Eave",new Vector3(0,-.04f,sign*2.78f),new Vector3(13.9f,.25f,.22f),_trim,false);
                }
                Block(source,"Ridge",new Vector3(0,1.75f,0),new Vector3(14,.23f,.28f),_tile,false);
                // Combine repeated tile channels into one render mesh; retain slab colliders.
                var filters=source.GetComponentsInChildren<MeshFilter>().Where(f=>f.GetComponent<Renderer>().sharedMaterial==_tile).ToArray();
                var mesh=new Mesh{name="VuongGiaRoof"}; mesh.indexFormat=IndexFormat.UInt32;
                mesh.CombineMeshes(filters.Select(f=>new CombineInstance{mesh=f.sharedMesh,transform=source.worldToLocalMatrix*f.transform.localToWorldMatrix}).ToArray());
                AssetDatabase.CreateAsset(mesh,Modules+"/VuongGiaRoofMesh.asset");
                source.gameObject.AddComponent<MeshFilter>().sharedMesh=mesh; source.gameObject.AddComponent<MeshRenderer>().sharedMaterial=_tile;
                foreach(var f in filters) f.GetComponent<Renderer>().enabled=false;
                prefab=PrefabUtility.SaveAsPrefabAsset(source.gameObject,path);
                source.name="ModuleAuthoringReference"; source.gameObject.SetActive(false); source.SetParent(Node(_root,"Authoring"),true);
            }
            var instance=(GameObject)PrefabUtility.InstantiatePrefab(prefab,parent); instance.name=name; instance.transform.position=position; return instance.transform;
        }
        private static Transform Reuse(Transform parent,string name,string asset,Vector3 position,float yaw,float scale)
        {
            var result=parent.Find(name);
            if(result==null){var prefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Game/Prefabs/"+asset+".prefab");if(prefab==null)throw new InvalidOperationException(asset);result=((GameObject)PrefabUtility.InstantiatePrefab(prefab,parent)).transform;result.name=name;}
            result.position=position; result.rotation=Quaternion.Euler(0,yaw,0); result.localScale=Vector3.one*scale;
            foreach(var t in result.GetComponentsInChildren<Transform>())t.gameObject.layer=8;
            return result;
        }
        private static Transform Block(Transform parent,string name,Vector3 position,Vector3 scale,Material material,bool collide,PrimitiveType type=PrimitiveType.Cube)
        {
            var result=parent.Find(name); if(result==null){result=GameObject.CreatePrimitive(type).transform;result.name=name;result.SetParent(parent,false);}
            result.localPosition=position; result.localScale=scale; result.gameObject.layer=8;
            result.GetComponent<Renderer>().sharedMaterial=material; result.GetComponent<Collider>().enabled=collide; return result;
        }
        private static Light Point(Transform parent,string name,Vector3 position,Color color,float intensity,float range)
        {var t=Node(parent,name);t.position=position;var light=GetOrAdd<Light>(t.gameObject);light.type=LightType.Point;light.color=color;light.intensity=intensity;light.range=range;light.shadows=LightShadows.None;return light;}
        private static T GetOrAdd<T>(GameObject go) where T:Component
        {var component=go.GetComponent<T>();return component!=null?component:go.AddComponent<T>();}
        private static Material Material(string name,Color color,float smoothness)
        {string path=Materials+"/"+name+".mat";var mat=AssetDatabase.LoadAssetAtPath<Material>(path);if(mat==null){mat=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(mat,path);}mat.color=color;mat.SetFloat("_Smoothness",smoothness);mat.enableInstancing=true;EditorUtility.SetDirty(mat);return mat;}
        private static Transform Node(Transform parent,string name)
        {var t=parent==null?SceneManager.GetActiveScene().GetRootGameObjects().FirstOrDefault(g=>g.name==name)?.transform:parent.Find(name);if(t==null){t=new GameObject(name).transform;t.SetParent(parent,false);}return t;}
        private static Transform Find(string name) => SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Transform>(true)).First(t=>t.name==name);
        private static void Reparent(string name,Transform parent)
        {foreach(var go in SceneManager.GetActiveScene().GetRootGameObjects().Where(g=>g.name==name))go.transform.SetParent(parent,true);}
        private static void Set(UnityEngine.Object target,string field,UnityEngine.Object value)
        {var so=new SerializedObject(target);so.FindProperty(field).objectReferenceValue=value;so.ApplyModifiedPropertiesWithoutUndo();}
    }
}
