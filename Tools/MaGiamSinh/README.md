# Mã Giám Sinh — Mixamo

Upload `MaGiamSinh_MixamoUpload.zip` to https://www.mixamo.com after signing in
with your Adobe ID. The ZIP contains the existing OBJ, its MTL and all four textures,
with material filenames preserved. Original project assets were not changed.

1. Choose Upload Character and upload the ZIP.
2. Place the rig markers as instructed by Mixamo and finish auto-rigging.
3. Download the rigged character as FBX Binary, With Skin. Save as
   `Assets/_Game/Art/Characters/MaGiamSinh/Models/MaGiamSinh_Rigged.fbx`.
4. For new motions, search Idle and Talking, preview them on the rigged character,
   and download each as FBX Binary, Without Skin, 30 FPS.
   Save to `Assets/_Game/Animations/MaGiamSinh/MGS-Idle.fbx` and `MGS-Talking.fbx`.
   Alternatively, the existing Humanoid TK-Idle/TK-Talking clips can be retargeted
   once this character has a valid Humanoid avatar.

Unity integration still needs to be performed after the downloads exist:
configure the model as Humanoid / Create From This Model, verify the avatar,
configure animation imports to copy that avatar and loop, create NPC Idle/Talking
states without root motion, replace the static visual in Chapter 1 while retaining
its root/scene references, and route MaGiamSinh speaker tags to its animator.

Reference: https://helpx.adobe.com/creative-cloud/help/mixamo-rigging-animation.html
No Mixamo download or rigging was completed by the agent in this session.
