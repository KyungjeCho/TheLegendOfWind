using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class Marker : MonoBehaviour
    {
        public Transform targetTr;
        public Canvas canvas;
        public Camera mainCam;

        public float edgeBuffer = 50f;

        private RectTransform canvasRectTr;
        private RectTransform myTr;

        // Start is called before the first frame update
        void Start()
        {
            if (mainCam == null)
            {
                mainCam = Camera.main;
            }
            canvasRectTr = canvas.GetComponent<RectTransform>();
            myTr = GetComponent<RectTransform>();
        }
        private void LateUpdate()
        {
            if (targetTr == null || myTr == null)
            {
                return;
            }

            UpdateMarkerPosition();
        }

        private void UpdateMarkerPosition()
        {
            Vector3 direction = targetTr.position - mainCam.transform.position;
            bool isBehind = Vector3.Dot(direction, mainCam.transform.forward) < 0;

            Vector3 viewportPos = mainCam.WorldToViewportPoint(targetTr.position);

            if (isBehind)
            {
                viewportPos.x = 1f - viewportPos.x;
                viewportPos.y = 1f - viewportPos.y;
            }

            bool isOnScreen = viewportPos.x > 0 && viewportPos.x < 1 &&
                          viewportPos.y > 0 && viewportPos.y < 1 &&
                          !isBehind;

            if (!isOnScreen)
            {
                viewportPos = ClampToScreenEdge(viewportPos);
            }

            Vector2 canvasPos = ViewportToCanvasPosition(viewportPos);
            myTr.transform.position = canvasPos;


        }

        private Vector3 ClampToScreenEdge(Vector3 viewportPos)
        {
            Vector2 center = new Vector2(0.5f, 0.5f);
            Vector2 direction = new Vector2(viewportPos.x - center.x, viewportPos.y - center.y);

            float maxX = 0.5f;
            float maxY = 0.5f;

            float scaleX = Mathf.Abs(direction.x) > 0.001f ? maxX / Mathf.Abs(direction.x) : float.MaxValue;
            float scaleY = Mathf.Abs(direction.y) > 0.001f ? maxY / Mathf.Abs(direction.y) : float.MaxValue;
            float scale = Mathf.Min(scaleX, scaleY);

            direction *= scale * 0.9f;

            viewportPos.x = 0.5f + direction.x;
            viewportPos.y = 0.5f + direction.y;

            return viewportPos;
        }

        private Vector2 ViewportToCanvasPosition(Vector3 viewportPos)
        {
            Vector2 canvasSize = canvasRectTr.sizeDelta;

            float x = viewportPos.x * canvasSize.x;
            float y = viewportPos.y * canvasSize.y;

            return new Vector2(x, y);
        }
    }

}
