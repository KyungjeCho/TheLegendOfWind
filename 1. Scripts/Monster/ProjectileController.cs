using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class ProjectileController : MonoBehaviour
    {
        public float speed;
        public EffectList hitEffect;
        public EffectList muzzleEffect;
        public SoundList shotSound;
        public SoundList hitSound;

        private Rigidbody myRigidbody;
        private Collider myCollider;
        private Transform myTransform;

        public Transform owner;
        public Transform target;

        private bool collided;

        // Start is called before the first frame update
        void Start()
        {
            myTransform = transform;
            myRigidbody = GetComponent<Rigidbody>();
            myCollider = GetComponent<Collider>();

            if (owner != null)
            {
                Collider[] ownerColliders = owner.GetComponentsInChildren<Collider>();
                foreach (Collider collider in ownerColliders)
                {
                    Physics.IgnoreCollision(myCollider, collider); // 현재 콜라이더가 발사 오너의 콜라이더는 무시한다
                }
            }

            if (muzzleEffect != EffectList.None)
            {
                GameObject muzzleVFX = EffectManager.Instance.PlayEffect(muzzleEffect, myTransform.position);
                muzzleVFX.transform.forward = myTransform.forward;
                ParticleSystem particleSystem = muzzleVFX.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    Destroy(muzzleVFX, particleSystem.main.duration);
                }
                else
                {
                    Destroy(muzzleVFX, 0f);
                }
            }

            if (shotSound != SoundList.None && SoundManager.Instance != null)
            {
                SoundManager.Instance.PlayOneShotEffect(shotSound, myTransform.position, 1f);
            }

            if (target != null)
            {
                Vector3 dest = target.transform.position;
                dest.y += 1.5f;
                transform.LookAt(dest);
            }

            collided = false;
        }

        protected virtual void FixedUpdate()
        {
            if (speed != 0 && myRigidbody != null)
            {
                myRigidbody.position += (transform.forward) * (speed * Time.deltaTime);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collided)
            {
                return;
            }

            Collider projectileCollider = GetComponent<Collider>();
            projectileCollider.enabled = false;

            collided = true;

            if (hitSound != SoundList.None)
            {
                SoundManager.Instance.PlayOneShotEffect(hitSound, transform.position, 1f);
            }

            speed = 0;
            myRigidbody.isKinematic = true;

            ContactPoint contact = collision.contacts[0];
            Vector3 contactPosition = contact.point;

            if (hitEffect != EffectList.None)
            {
                GameObject hitVFX = EffectManager.Instance.PlayEffect(hitEffect, contactPosition);
                ParticleSystem particleSystem = hitVFX.GetComponent<ParticleSystem>();

                if (particleSystem == null)
                {
                    Destroy(hitVFX, 0f);
                }
                else
                {
                    Destroy(hitVFX, particleSystem.main.duration);
                }
            }

            // Damage
            IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
            if (damagable != null)
            {
                Debug.Log("todo : 투사체 공격 데미지 설정 owner에서 stat 받기");
                damagable.OnDamage(1f);
            }
            StartCoroutine(DestroyParticle(0.0f));
        }

        public IEnumerator DestroyParticle(float waitTime)
        {

            if (transform.childCount > 0 && waitTime != 0)
            {
                List<Transform> childs = new List<Transform>();

                foreach (Transform t in transform.GetChild(0).transform)
                {
                    childs.Add(t);
                }

                while (transform.GetChild(0).localScale.x > 0)
                {
                    yield return new WaitForSeconds(0.01f);
                    transform.GetChild(0).localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                    for (int i = 0; i < childs.Count; i++)
                    {
                        childs[i].localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                    }
                }
            }

            yield return new WaitForSeconds(waitTime);
            Destroy(gameObject);
        }
    }

}

