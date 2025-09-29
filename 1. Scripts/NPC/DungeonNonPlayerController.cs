using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class DungeonNonPlayerController : SaveNonPlayerControlelr
    {
        public SceneList sceneList = SceneList.DungeonScene;

        public override void Interact()
        {
            base.Interact();
            LoadScene.Instance.LoadAsync(sceneList);
        }
    }
}

