using KJ.CameraControl;
using KJ.ThirdPersonCamStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ.ThirdPersonCamStates
{
    /// <summary>
    /// 3��Ī ī�޶�
    /// 1. ���� ���� üũ
    /// 2. ������ (ȭ��)
    /// 3. FOV �� 
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class ThirdPersonOrbitCamera : MonoBehaviour, IGetTargetPos, IGetTargetRot
    {
        #region Variables
        public Transform player;
        public Vector3 pivotOffset = new Vector3(0.0f, 2.0f, 0.0f);    // �ٶ� ������Ʈ�� �Ǻ�(����) ����
        public Vector3 camOffset = new Vector3(0.0f, 0.5f, -2.0f);   // ��ġ ������

        public float smooth = 10f;            // ī�޶� ���� �ӵ�
        public float recoilAngleBounce = 5.0f;           // ��� �ݵ�

        public float horizontalAimingSpeed = 6.0f;     // ���� ȸ�� �ӵ�
        public float verticalAimingSpeed = 6.0f;     // ���� ȸ�� �ӵ�
        public float maxVerticalAngle = 30.0f;    // �ִ� ī�޶� ���� ����
        public float minVerticalAngle = -60.0f;   // �ּ� ī�޶� �ޱ� ����

        public float angleH = 0.0f; // ���콺 �̵��� ���� ī�޶� �����̵�
        public float angleV = 0.0f; // ���콺 �̵��� ���� ī�޶� �����̵�

        private Transform cameraTransform; // ī�޶� Transform [ĳ��]
        private Camera myCamera;

        private Vector3 relativeCamPos; // �÷��̾�κ��� ī�޶������ ����
        private float relativeCamPosMag; // �÷��̾�κ��� ī�޶������ �Ÿ�
        private Vector3 smoothPivotOffset;  // ī�޶� �Ǻ� ������ ���� ������ ���� 
        private Vector3 smoothCamOffset;    // ī�޶� ��ġ ������ ����
        private Vector3 targetPivotOffset;  // Ÿ�� �Ǻ� ����
        private Vector3 targetCamOffset;    // Ÿ�� ��ġ ����

        private float defaultFOV;
        private float targetFOV;
        private float targetMaxVerticleAngle;   // ī�޶� ���� �ִ� ����
        private float recoilAngle = 0.0f;       // �ݵ���

        private Vector3 previousCamPos;
        private Quaternion previousCamRot;

        private bool isPlaying = false;

        private StateMachine<ThirdPersonOrbitCamera> stateMachine;
        #endregion

        #region Properties
        public float GetH
        {
            get => angleH;
        }
        public float TargetMaxVerticleAngle => targetMaxVerticleAngle;
        public float RecoilAngle { get { return recoilAngle; } set { recoilAngle = value; } }
        public float TargetFOV => targetFOV;
        public Vector3 TargetPivotOffset => targetPivotOffset;
        public Vector3 TargetCamOffset => targetCamOffset;
        public Vector3 SmoothPivotOffset { get { return smoothPivotOffset; } set { smoothPivotOffset = value; } }
        public Vector3 SmoothCamOffset { get { return smoothCamOffset; } set { smoothCamOffset = value; } }
        public Vector3 PreviousCamPos { get { return previousCamPos; } set {  previousCamPos = value; } }
        public Quaternion PreviousCamRot { get { return previousCamRot; } set { previousCamRot = value; ; } } 
        public bool IsPlaying { get { return isPlaying; }  set { isPlaying = value; } }
        #endregion

        private void Awake()
        {
            // ĳ��
            cameraTransform = transform;
            myCamera = cameraTransform.GetComponent<Camera>();

            InitCamera();
        }
        private void Start()
        {
            stateMachine = new StateMachine<ThirdPersonOrbitCamera>(this, new PlayState());

            stateMachine.AddState(new StopState());

            EventBusSystem.Subscribe(EventBusType.START, StartCamera);
            EventBusSystem.Subscribe(EventBusType.STOP, StopCamera);
        }
        private void Update()
        {
            stateMachine.Update(Time.deltaTime);
        }

        private void InitCamera()
        {
            // ī�޶� �⺻ ��ġ ����
            cameraTransform.position =
                player.position + Quaternion.identity * pivotOffset + Quaternion.identity * camOffset;
            cameraTransform.rotation = Quaternion.identity; // ���� ī�޶� ȸ�� X

            // ī�޶�� �÷��̾�� ��� ���� �浹üũ��
            relativeCamPos = cameraTransform.position - player.position;
            relativeCamPosMag = relativeCamPos.magnitude - 0.5f; // ī�޶�� �÷��̾�� �浹üũ �� �������� �÷��̾ �ɸ��� ������ 0.5f ũ�⸦ ����.

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
            Vector3 target = player.position + (Vector3.up * deltaPlayerHeight); // �÷��̾��� �Ǻ��� ��κ� 0.0.0 �߹ٴ��̿��� ���� �ٴڰ� üũ�� ������ ����
            if (Physics.SphereCast(checkPos, 0.2f, target - checkPos, out RaycastHit hit, relativeCamPosMag))
            {
                if (hit.transform != player && !hit.transform.GetComponent<Collider>().isTrigger)
                { // �浹ü�� �÷��̾ �ƴϿ��� �Ѵ�. �׸��� �浹ü�� Ʈ���Ű� �ƴϿ��� �Ѵ�. 
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

        public bool DoubleViewingPosCheck(Vector3 checkPos, float offset)
        {
            float playerFocusHeight = player.GetComponent<CapsuleCollider>().height * 0.75f;
            return ViewingPosCheck(checkPos, playerFocusHeight) && ReverseViewingPosCheck(checkPos, playerFocusHeight, offset);
        }

        public float GetCurrentPivotMagnitude(Vector3 finalPivotOffset)
        {
            return Mathf.Abs((finalPivotOffset - smoothPivotOffset).magnitude);
        }

        public void StartCamera()
        {
            stateMachine.ChangeState<PlayState>();
        }
        public void StopCamera()
        {
            stateMachine.ChangeState<StopState>();
        }

        public Vector3 GetPos()
        {
            return previousCamPos;
        }

        public Quaternion GetRot()
        {
            return previousCamRot;
        }
    }
}
