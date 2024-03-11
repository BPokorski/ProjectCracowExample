using UnityEngine.EventSystems;

namespace UI.Inventory.ItemSlots
{
    public class UIQuickItemSlot : UIItemSlot, IDropHandler
    {
        
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null) return;
            CurrentItemSlot = eventData.pointerDrag.GetComponent<UIItemSlot>().CurrentItemSlot;
            OnItemSlotActionTriggered(ItemSlotID, CurrentItemSlot);
        }
        
        
        public class Factory: UIItemSlotFactory
        {
            
        }
    }
}