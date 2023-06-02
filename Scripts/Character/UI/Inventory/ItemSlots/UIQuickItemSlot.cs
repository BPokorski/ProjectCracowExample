using System;
using Character.UI.Inventory.ItemInventoryManagers;
using Equipment;
using Settings;
using UnityEngine.EventSystems;

namespace Character.UI.Inventory.ItemSlots
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