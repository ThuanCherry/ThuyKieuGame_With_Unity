var player=UnityEngine.GameObject.Find("ThuyKieu");
var animator=player.GetComponentInChildren<UnityEngine.Animator>();
var renderer=player.GetComponentInChildren<UnityEngine.SkinnedMeshRenderer>();
var mesh=new UnityEngine.Mesh(); renderer.BakeMesh(mesh);
return new{root=player.transform.position.ToString(),model=renderer.transform.position.ToString(),meshBottom=mesh.vertices.Min(v=>renderer.transform.TransformPoint(v).y),leftFoot=animator.GetBoneTransform(UnityEngine.HumanBodyBones.LeftFoot).position.ToString(),rightFoot=animator.GetBoneTransform(UnityEngine.HumanBodyBones.RightFoot).position.ToString()};
