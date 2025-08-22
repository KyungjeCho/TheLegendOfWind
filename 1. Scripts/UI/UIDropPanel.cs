using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class DropSlotWrapper
    {
        public float destroyTime;
        public GameObject dropSlotObject;
    }

    public class UIDropPanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject dropSlotPrefab;

        private float elapsedTime = 0;

        private Queue<DropSlotWrapper> dropSlots = new Queue<DropSlotWrapper>();

        // Start is called before the first frame update
        void Start()
        {
            if (dropSlotPrefab == null)
            {
                //dropSlotPrefab = Resources.Load()
            }
        }

        // Update is called once per frame
        void Update()
        {
            elapsedTime += Time.deltaTime;

            if (dropSlots.TryPeek(out DropSlotWrapper dropSlotWrapper))
            {
                if (dropSlotWrapper.destroyTime < elapsedTime)
                {
                    DropSlotWrapper wrapper = dropSlots.Dequeue();
                    Destroy(wrapper.dropSlotObject);
                }
            }
        }

        public void AddSlot(string slotContent)
        {
            DropSlotWrapper wrapper = new DropSlotWrapper();
            wrapper.destroyTime = elapsedTime + 5f;
            wrapper.dropSlotObject = Instantiate(dropSlotPrefab, transform);
            UIDropSlot dropSlot = wrapper.dropSlotObject.GetComponent<UIDropSlot>();
            if (dropSlot != null)
            {
                dropSlot.SetText(slotContent);
            }
            dropSlots.Enqueue(wrapper);
        }
    }
}