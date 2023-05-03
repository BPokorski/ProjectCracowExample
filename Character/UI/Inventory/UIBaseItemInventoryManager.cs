using System;
using System.Collections.Generic;
using Equipment;
using Items;
using UnityEngine;

namespace Character.UI.Inventory
{
    public abstract class UIBaseItemInventoryManager : MonoBehaviour
    {
        protected Dictionary<int, int> _itemSlotIDPosition; //= new Dictionary<int, int>();
        protected IInventoryManager _inventoryManager;


        protected virtual void Awake()
        {
            _itemSlotIDPosition = new Dictionary<int, int>();
            _inventoryManager = GetComponent<IInventoryManager>();
        }

        protected void InstantiateItemSlotByType(UIItemSlot[] itemSlots, GameObject itemSlotsHolder, GameObject itemSlotPrefab)
        {
            
            for (int i = 0; i < itemSlots.Length; i++)
            {
                var instantiatedItemSlot = Instantiate(itemSlotPrefab, itemSlotsHolder.transform);
                var itemSlot = instantiatedItemSlot.GetComponent<UIItemSlot>();
                var itemSlotID = itemSlot.SetItemSlotID();
                itemSlots[i] = itemSlot;
                _itemSlotIDPosition[itemSlotID] = i;
                
            }
        }
        
        protected void ShowItemsByType(ItemType itemType, ref UIItemSlot[] itemSlots)
        {
            foreach (var slot in itemSlots)
            {
                slot.CurrentItemSlot = null;
            }

            Dictionary<int, ItemSlot> items = _inventoryManager.GetItemsByType(itemType);
            int slotIndex = 0;

            foreach (var itemIndex in items.Keys)
            {
                
                var slot = itemSlots[slotIndex];
                var item = items[itemIndex];
                slot.CurrentItemSlot = item;
                slotIndex += 1;
            }
        }
    }
    
    
}