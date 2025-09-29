using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace KJ
{
    public class WallTimeline : MonoBehaviour
    {
        private PlayableDirector playableDirector;

        // Start is called before the first frame update
        void Start()
        {
            playableDirector = GetComponent<PlayableDirector>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PlayTimeline()
        {
            playableDirector.Play();
            Debug.Log("Play");
        }
    }

}
