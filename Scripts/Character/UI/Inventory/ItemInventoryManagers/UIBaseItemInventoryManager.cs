using System;
using System.Collections.Generic;
using Character.Inventory;
using Character.UI.Inventory.ItemSlots;
using Equipment;
using Items;
using Settings;
using UnityEngine;

namespace Character.UI.Inventory.ItemInventoryManagers
{
    public abstract class UIBaseItemInventoryManager : MonoBehaviour
    {
        [SerializeField] protected GameObject _itemSlotsHolder;
        
        protected Dictionary<int, int> _itemSlotIDPosition; //= new Dictionary<int, int>();
        protected IInventoryManager _inventoryManager;
        protected UIItemSlot[] _items;
        protected UIItemSlotFactory _factory;
        
        protected virtual void Awake()
        {
            _itemSlotIDPosition = new Dictionary<int, int>();
        }
        

        protected virtual void InitItems(int length)
        {
            _items = new UIItemSlot[length];
        }
        protected void InitItemSlots()
        {

            for (int i = 0; i < _items.Length; i++)
            {
                var instantiatedItemSlot = _factory.Create();
                instantiatedItemSlot.transform.SetParent(_itemSlotsHolder.transform);
                // var itemSlot = instantiatedItemSlot.GetComponent<UIItemSlot>();

                var itemSlotID = instantiatedItemSlot.ItemSlotID;
                _items[i] = instantiatedItemSlot;
                _itemSlotIDPosition[itemSlotID] = i;
            }
        }
        protected void InstantiateItemSlotByType(UIItemSlot[] itemSlots, GameObject itemSlotsHolder, GameObject itemSlotPrefab)
        {
            
            for (int i = 0; i < itemSlots.Length; i++)
            {
                var instantiatedItemSlot = Instantiate(itemSlotPrefab, itemSlotsHolder.transform);
                var itemSlot = instantiatedItemSlot.GetComponent<UIItemSlot>();
                var itemSlotID = itemSlot.ItemSlotID;
                itemSlots[i] = itemSlot;
                _itemSlotIDPosition[itemSlotID] = i;
                
            }
        }
        
        protected void ShowItems(ItemSlot[] itemsToShow)
        {
            HideItems();
            
            int slotIndex = 0;

            foreach (var item in itemsToShow)
            {
                var slot = _items[slotIndex];
                slot.CurrentItemSlot = item;
                slotIndex += 1;
            }
        }

        protected void HideItems()
        {
            foreach (var slot in _items)
            {
                slot.CurrentItemSlot = null;
            }
        }
    }
    
    
}