using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    /// <summary>
    /// 3인칭 카메라
    /// 1. 더블 뷰잉 체크
    /// 2. 리코일 (화살)
    /// 3. FOV 값 
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class ThirdPersonOrbitCamera : MonoBehaviour
    {
        #region Variables
        public Transform player;
        public Vector3 pivotOffset = new Vector3(0.0f, 2.0f, 0.0f);    // 바라볼 오브젝트의 피봇(기준) 벡터
        public Vector3 camOffset = new Vector3(0.0f, 0.5f, -2.0f);   // 위치 오프셋

        public float smooth = 10f;            // 카메라 반응 속도
        public float recoilAngleBounce = 5.0f;           // 사격 반동

        public float horizontalAimingSpeed = 6.0f;     // 수평 회전 속도
        public float verticalAimingSpeed = 6.0f;     // 수직 회전 속도
        public float maxVerticalAngle = 30.0f;    // 최대 카메라 엥글 제한
        public float minVerticalAngle = -60.0f;   // 최소 카메라 앵글 제한

        public float angleH = 0.0f; // 마우스 이동에 따른 카메라 수평이동
        public float angleV = 0.0f; // 마우스 이동에 따른 카메라 수직이동

        private Transform cameraTransform; // 카메라 Transform [캐싱]
        private Camera myCamera;

        private Vector3 relativeCamPos; // 플레이어로부터 카메라까지의 벡터
        private float relativeCamPosMag; // 플레이어로부터 카메라까지의 거리
        private Vector3 smoothPivotOffset;  // 카메라 피봇 보간용 벡터 스무스 보간 
        private Vector3 smoothCamOffset;    // 카메라 위치 보간용 벡터
        private Vector3 targetPivotOffset;  // 타겟 피봇 보간
        private Vector3 targetCamOffset;    // 타겟 위치 보간

        private float defaultFOV;
        private float targetFOV;
        private float targetMaxVerticleAngle;   // 카메라 수직 최대 각도
        private float recoilAngle = 0.0f;       // 반동값

        #endregion

        #region Properties
        public float GetH
        {
            get => angleH;
        }
        #endregion

        private void Awake()
        {
            // 캐싱
            cameraTransform = transform;
            myCamera = cameraTransform.GetComponent<Camera>();

            InitCamera();
        }
        private void Update()
        {
            // 입력을 받아 카메라 이동 
            angleH += Mathf.Clamp(Input.GetAxis("Mouse X"), -1f, 1f) * horizontalAimingSpeed;
            angleV += Mathf.Clamp(Input.GetAxis("Mouse Y"), -1f, 1f) * verticalAimingSpeed;

            // 카메라 수직 이동 제한.
            angleV = Mathf.Clamp(angleV, minVerticalAngle, targetMaxVerticleAngle);
            // 카메라 반동
            angleV = Mathf.LerpAngle(angleV, angleV + recoilAngle, 10f * Time.deltaTime);

            // 카메라 회전
            Quaternion camYRotation = Quaternion.Euler(0.0f, angleH, 0.0f);
            Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0.0f);
            cameraTransform.rotation = aimRotation;

            // Set FOV
            myCamera.fieldOfView = Mathf.Lerp(myCamera.fieldOfView, targetFOV, Time.deltaTime);

            Vector3 baseTempPosition = player.position + camYRotation * targetPivotOffset; // 기본 위치 값
            Vector3 noCollisionOffset = targetCamOffset; // 카메라를 만약 이동했을때 무언가 충돌할 경우의 위치값

            for (float zOffset = targetCamOffset.z; zOffset <= 0f; zOffset += 0.5f)
            {
                noCollisionOffset.z = zOffset;
                if (DoubleViewingPosCheck(baseTempPosition + aimRotation * noCollisionOffset, Mathf.Abs(zOffset)) || zOffset == 0f)
                {
                    break;
                }
            }

            //Reposition Camera
            smoothPivotOffset = Vector3.Lerp(smoothPivotOffset, targetPivotOffset, smooth * Time.deltaTime);
            smoothCamOffset = Vector3.Lerp(smoothCamOffset, noCollisionOffset, smooth * Time.deltaTime);

            cameraTransform.position = player.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset;

            // 리코일
            if (recoilAngle > 0.0f)
            {
                recoilAngle -= recoilAngleBounce * Time.deltaTime;
            }
            else if (recoilAngle < 0.0f)
            {
                recoilAngle += recoilAngleBounce * Time.deltaTime;
            }

        }

        private void InitCamera()
        {
            // 카메라 기본 위치 세팅
            cameraTransform.position =
                player.position + Quaternion.identity * pivotOffset + Quaternion.identity * camOffset;
            cameraTransform.rotation = Quaternion.identity; // 현재 카메라 회전 X

            // 카메라와 플레이어간의 상대 벡터 충돌체크용
            relativeCamPos = cameraTransform.position - player.position;
            relativeCamPosMag = relativeCamPos.magnitude - 0.5f; // 카메라와 플레이어간의 충돌체크 중 마지막에 플레이어가 걸리기 때문에 0.5f 크기를 뺀다.

            smoothPivotOffset = pivotOffset;
            smoothCamOffset = camOffset;
            defaultFOV = myCamera.fieldOfView;
            angleH = player.eulerAngles.y;

            ResetTargetOffsets();
            ResetFOV();
            ResetMaxVerticalAngle();
        }

        public void ResetTargetOffsets()
        {
            targetPivotOffset = pivotOffset;
            targetCamOffset = camOffset;
        }

        public void ResetFOV()
        {
            targetFOV = defaultFOV;
        }

        public void ResetMaxVerticalAngle()
        {
            targetMaxVerticleAngle = maxVerticalAngle;
        }

        public void BounceVertical(float degree)
        {
            recoilAngle = degree;
        }

        public void SetTargetOffset(Vector3 newPivotOffset, Vector3 newCamOffset)
        {
            targetPivotOffset = newPivotOffset;
            targetCamOffset = newCamOffset;
        }

        public void SetFOV(float customFOV)
        {
            targetFOV = customFOV;
        }

        bool ViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight)
        {
            Vector3 target = player.position + (Vector3.up * deltaPlayerHeight); // 플레이어의 피봇은 대부분 0.0.0 발바닥이여서 굳이 바닥과 체크할 이유가 없다
            if (Physics.SphereCast(checkPos, 0.2f, target - checkPos, out RaycastHit hit, relativeCamPosMag))
            {
                if (hit.transform != player && !hit.transform.GetComponent<Collider>().isTrigger)
                { // 충돌체가 플레이어가 아니여야 한다. 그리고 충돌체가 트리거가 아니여야 한다. 
                    return false;
                }
            }
            return true;
        }

        bool ReverseViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight, float maxDistance)
        {
            Vector3 origin = player.position + (Vector3.up * deltaPlayerHeight);
            if (Physics.SphereCast(origin, 0.2f, checkPos - origin, out RaycastHit hit, maxDistance))
            {
                if (hit.transform != player && hit.transform != transform && !hit.transform.GetComponent<Collider>().isTrigger)
                {
                    return false;
                }
            }
            return true;
        }

        bool DoubleViewingPosCheck(Vector3 checkPos, float offset)
        {
            float playerFocusHeight = player.GetComponent<CapsuleCollider>().height * 0.75f;
            return ViewingPosCheck(checkPos, playerFocusHeight) && ReverseViewingPosCheck(checkPos, playerFocusHeight, offset);
        }

        public float GetCurrentPivotMagnitude(Vector3 finalPivotOffset)
        {
            return Mathf.Abs((finalPivotOffset - smoothPivotOffset).magnitude);
        }
    }
}
