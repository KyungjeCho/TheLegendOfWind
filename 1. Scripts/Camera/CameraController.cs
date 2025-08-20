using KJ.ThirdPersonCamStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ.CameraControl
{
    public interface IGetTargetPos
    {
        Vector3 GetPos();
    }
    public interface IGetTargetRot
    {
        Quaternion GetRot();
    }
    public enum CameraMoveType
    {
        None = -1,
        To3PFromNPC,
        ToNPCFrom3P
    }
    public class CameraController : MonoBehaviour
    {
        private Vector3 targetPos;
        private Quaternion targetRot;
        private CameraMoveType moveType = CameraMoveType.None;

        [SerializeField]
        private ThirdPersonOrbitCamera thirdPersonOrbitCam;
        [SerializeField]
        private NPCDialogCamera npcDialogCam;
        [SerializeField]
        private float smooth = 10f;

        private StateMachine<CameraController> stateMachine;

        public float Smooth => smooth;
        public Vector3 TargetPos => targetPos;
        public Quaternion TargetRot => targetRot;
        public CameraMoveType MoveType { get { return moveType; } set { moveType = value; } }
        public ThirdPersonOrbitCamera ThirdPersonOrbitCam => thirdPersonOrbitCam;
        // ī�޶� ����
        // ���� ��ġ ���� ��� ī�޶� ��� off, ���� �� 
        // ���� �Ϸ�� ī�޶� on

        // ���� ���� �� ī�޶� �̵�
        private void Start()
        {
            stateMachine = new StateMachine<CameraController>(this, new IdleState());

            stateMachine.AddState(new MoveWithLerpState());
            stateMachine.AddState(new MoveWithoutLerpState());
        }
        private void Update()
        {
            stateMachine.Update(Time.deltaTime);
        }
        public void MoveToNPCFrom3P(Transform npcTr)
        {
            Debug.Log("Move To NPC From 3P : " + npcTr.name);
            moveType = CameraMoveType.ToNPCFrom3P;
            npcDialogCam.SetNPCTransform(npcTr);
            targetPos = npcDialogCam.GetPos();
            targetRot = npcDialogCam.GetRot();
            thirdPersonOrbitCam.StopCamera();

            // ���� �̵� ����
            stateMachine.ChangeState<MoveWithLerpState>();
        }
        public void MoveTo3PFromNPC()
        {
            moveType = CameraMoveType.To3PFromNPC;
            targetPos = thirdPersonOrbitCam.GetPos();
            targetRot = thirdPersonOrbitCam.GetRot();

            // ���� �̵� ����
            stateMachine.ChangeState<MoveWithLerpState>();
        }
    }
}