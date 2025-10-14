using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class HomingRockController : MonoBehaviour
    {
        [Range(1f, 10f)]
        public float speed = 1.0f;
        [Range(1f, 10f)]
        public float rotSpeed = 1.0f;
        public EffectList hitEffect;
        public SoundList hitSound;

        private Rigidbody myRigidbody;
        private Transform myTransform;
        private Collider myCollider;

        public Transform ownerTr;
        public Transform targetTr;

        // Start is called before the first frame update
        void Start()
        {
            myTransform = transform;
            myRigidbody = GetComponent<Rigidbody>();
            myCollider = GetComponent<Collider>();
        }

        public void Init(Transform ownerTr, Transform targetTr)
        {
            this.ownerTr = ownerTr;
            this.targetTr = targetTr;
        }
        private void FixedUpdate()
        {
            if (speed > Mathf.Epsilon && myRigidbody != null)
            {
                Vector3 direction = (targetTr.position - myTransform.position).normalized;
                myRigidbody.position += direction * (speed * Time.fixedDeltaTime);

                Quaternion lookRot = Quaternion.LookRotation(direction);
                Quaternion tilt = Quaternion.Euler(90f * Time.time, 0f, 0f);
                Quaternion finalRot = lookRot * tilt;

                myRigidbody.MoveRotation(Quaternion.RotateTowards(myRigidbody.rotation, finalRot, rotSpeed * Time.fixedDeltaTime * 100f));
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform == targetTr)
            {
                IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();

                damagable.OnDamage(this.gameObject, 20f);

                EffectClip hitEffectClip = DataManager.EffectData.effectClips[(int)hitEffect];
                hitEffectClip.PreLoad();
                EffectManager.Instance.PlayEffect(hitEffectClip, transform.position);

                SoundManager.Instance.PlayOneShotEffect(hitSound, transform.position, 1f);
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }

}
