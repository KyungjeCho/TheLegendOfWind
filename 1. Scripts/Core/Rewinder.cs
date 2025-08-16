using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class Rewinder : MonoBehaviour
    {
        private List<TransformData> transformHistory = new List<TransformData> ();
        private bool isRewinding = false;
        [SerializeField]
        private float recordTime = 5f;

        private void Update()
        {
            if (!isRewinding)
            {
                Record();
            }
            
            if (isRewinding)
            {
                Rewind();
            }
        }

        private void Record()
        {
            if (transformHistory.Count > Mathf.Round(recordTime / Time.deltaTime))
            {
                transformHistory.RemoveAt(transformHistory.Count - 1);
            }
            transformHistory.Insert(0, new TransformData(transform));
        }

        private void Rewind()
        {
            if (transformHistory.Count > 0)
            {
                TransformData data = transformHistory[0];
                transform.position = data.Position;
                transform.rotation = data.Rotation;
                transformHistory.RemoveAt(0);
            }
            else
            {
                StopRewind();
            }
        }

        public void StartRewind()
        {
            isRewinding = true;
        }
        public void StopRewind()
        {
            isRewinding = false;
        }
    }

    public class TransformData
    {
        private Vector3 position;
        private Quaternion rotation;

        public Vector3 Position => position;
        public Quaternion Rotation => rotation;

        public TransformData(Transform transform)
        {
            position = transform.position;
            rotation = transform.rotation; 
        }
    }
}
