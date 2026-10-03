var table=GameObject.Find("VuongGia/MainHouse/MainHall/ChairsAndContractTable");
var heights=new System.Collections.Generic.List<string>();
foreach(float z in new[]{2.25f,2.4f,2.55f,2.75f})
{
    float highest=-100;
    foreach(var filter in table.GetComponentsInChildren<MeshFilter>())
    {
        var vs=filter.sharedMesh.vertices;var ts=filter.sharedMesh.triangles;
        for(int i=0;i<ts.Length;i+=3)
        {
            var a=filter.transform.TransformPoint(vs[ts[i]]);var b=filter.transform.TransformPoint(vs[ts[i+1]]);var c=filter.transform.TransformPoint(vs[ts[i+2]]);
            float den=(b.z-c.z)*(a.x-c.x)+(c.x-b.x)*(a.z-c.z);if(Mathf.Abs(den)<.000001f)continue;
            float u=((b.z-c.z)*(-.22f-c.x)+(c.x-b.x)*(z-c.z))/den;
            float v=((c.z-a.z)*(-.22f-c.x)+(a.x-c.x)*(z-c.z))/den;
            if(u>=0&&v>=0&&u+v<=1)highest=Mathf.Max(highest,u*a.y+v*b.y+(1-u-v)*c.y);
        }
    }
    heights.Add("z="+z+" surface="+highest);
}
return new {tableHeights=heights.ToArray(),audio=AssetDatabase.FindAssets("t:AudioClip",new[]{"Assets"}).Select(AssetDatabase.GUIDToAssetPath).ToArray(),textures=table.scene.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Renderer>()).SelectMany(r=>r.sharedMaterials).Where(m=>m!=null&&m.mainTexture!=null).Select(m=>m.mainTexture).Distinct().Select(t=>new{t.name,t.width,t.height,path=AssetDatabase.GetAssetPath(t)}).ToArray()};
