using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class ScreenManager : SingletonMonoBehaviour<ScreenManager>
    {
        private FullScreenMode screenMode = FullScreenMode.ExclusiveFullScreen;
        private ResolutionSO resolutionSO;

        public void SetScreen(ResolutionSO resolutionSO, FullScreenMode screenMode)
        {
            this.resolutionSO = resolutionSO;
            this.screenMode = screenMode;

            Screen.SetResolution(resolutionSO.width, resolutionSO.height, screenMode);
        }
    }
}

