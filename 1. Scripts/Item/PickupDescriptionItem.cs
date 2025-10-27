using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [RequireComponent(typeof(Collider))]
    public class PickupDescriptionItem : MonoBehaviour
    {
        public bool canInteract = false;
        public ItemSO itemSO;
        public GameEnding gameEnding;
        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        private void SetCanInteract(bool enable)
        {
            canInteract = enable;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (canInteract && other.gameObject.CompareTag(TagAndLayer.Player))
            {
                // Description UI Open & Get Item Sound Play
                ControlDescriptionPanel.Instance.OpenPanel(itemSO);
                // if Description UI Close 
                ControlDescriptionPanel.Instance.OnPanelClose += PanelClose;
            }
        }
        public void PanelClose(ItemSO itemSO)
        {
            if (this.itemSO == itemSO)
            {
                gameEnding.Play();
            }
        }
    }
}