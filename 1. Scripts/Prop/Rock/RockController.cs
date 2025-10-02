using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class RockController : MonoBehaviour
    {
        public EffectList crushingEffect;
        private EffectClip crushingEffectClip;

        public SoundList crushingSound;
        public SoundList collisionSound;

        private bool isGround = false;

        // Start is called before the first frame update
        void Start()
        {
            crushingEffectClip = DataManager.EffectData.GetCopy((int)crushingEffect);
            crushingEffectClip.PreLoad();
            isGround = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!isGround && collision.gameObject.CompareTag(TagAndLayer.Player))
            {
                CrushRock();
                IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
                if (damagable != null) 
                {
                    damagable.OnDamage(gameObject, 40f);
                }
            }
            else if (collision.gameObject.CompareTag(TagAndLayer.Ground))
            {
                SoundManager.Instance.PlayOneShotEffect(collisionSound, transform.position, 1f);
                isGround = true;
            }
        }

        public void CrushRock()
        {
            SoundManager.Instance.PlayOneShotEffect(crushingSound, transform.position, 1f);
            EffectManager.Instance.PlayEffect(crushingEffectClip, transform.position);
            Destroy(this.gameObject);
        }
    }

}
