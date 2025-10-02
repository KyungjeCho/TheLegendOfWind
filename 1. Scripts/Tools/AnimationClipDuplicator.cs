using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KJ
{
    public class AnimationClipDuplicator 
    {
        [MenuItem("Tools/Duplicate AnimationClip")]
        static void DuplicateClip()
        {
            var clip = Selection.activeObject as AnimationClip;
            if (clip == null) return;

            AnimationClip newClip = new AnimationClip();
            EditorUtility.CopySerialized(clip, newClip);

            string path = "Assets/9. Resources/Resources/Data/" + clip.name + "_Copy.anim";
            AssetDatabase.CreateAsset(newClip, path);
            AssetDatabase.SaveAssets();
        }
    }
}
