using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KJ
{
    public class PortalController : MonoBehaviour
    {
        public SceneList sceneList;
        public UnityEvent OnExit;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagAndLayer.Player))
            {
                OnExit?.Invoke();
                LoadScene.Instance.LoadAsync(sceneList);
            }
        }
    }
}