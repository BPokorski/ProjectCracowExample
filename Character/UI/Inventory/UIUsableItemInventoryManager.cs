using System;
using System.Collections.Generic;
using Character.Data;
using Equipment;
using Items;
using UnityEngine;

namespace Character.UI.Inventory
{
    public class UIUsableItemInventoryManager : UIBaseItemInventoryManager, IItemSlotsManager
    {
        [Header("Slots holders")]
        [SerializeField] private GameObject _quickItemSlotsHolder;
        [SerializeField] private GameObject _consumableItemSlotsHolder;
        [SerializeField] private GameObject _questItemSlotsHolder;
        
        [Header("Slot data")]
        [SerializeField] private ItemSlotsData _itemSlotsData;
        
        private UIItemSlot[] _quickItems;
        private UIItemSlot[] _consumableItems;
        private UIItemSlot[] _questItems;

        protected override void Awake()
        {
            base.Awake();
            // _itemSlotIDPosition = new Dictionary<int, int>();
            // _inventoryManager = GetComponent<IInventoryManager>();
            //
            _quickItems = new UIItemSlot[_itemSlotsData.QuickSlotNumber];
            _consumableItems = new UIItemSlot[_itemSlotsData.ConsumableSlotNumber];
            _questItems = new UIItemSlot[_itemSlotsData.QuestSlotNumber];
            
            
            InstantiateItemSlotByType(_quickItems, _quickItemSlotsHolder, _itemSlotsData.QuickSlotPrefab);
            InstantiateItemSlotByType(_consumableItems, _consumableItemSlotsHolder, _itemSlotsData.ConsumableSlotPrefab);
            InstantiateItemSlotByType(_questItems, _questItemSlotsHolder, _itemSlotsData.QuestSlotPrefab);
        }

        private void Start()
        {
            _inventoryManager = GetComponent<IInventoryManager>();
        }

        private void OnEnable()
        {
            UIQuickItemSlot.OnItemDropped += UpdateQuickItemSlot;
        }

        private void OnDisable()
        {
            UIQuickItemSlot.OnItemDropped -= UpdateQuickItemSlot;
        }

        public void ShowItems()
        {
            ShowItemsByType(ItemType.Consumable, ref _consumableItems);
            ShowItemsByType(ItemType.Quest, ref _questItems);
            ShowQuickItems();
        }

        
        private void UpdateQuickItemSlot(int quickSlotID, ItemSlot currentItemSlot)
        {
            if (currentItemSlot != null)
            {
                var quickSlotPosition = _itemSlotIDPosition[quickSlotID];
                Debug.Log($"{quickSlotID}: pos: {quickSlotPosition}");
                _inventoryManager.SetQuickSlotItem(quickSlotPosition, currentItemSlot);

                for (int i = 0; i < _quickItems.Length; i++)
                {
                    var quickItem = _quickItems[i];

                    var quickItemItemSlot = quickItem.CurrentItemSlot;
                    if (quickItemItemSlot != null && 
                        quickItemItemSlot.CurrentItem.ItemID == currentItemSlot.CurrentItem.ItemID &&
                        i != quickSlotPosition)
                    {
                        quickItem.CurrentItemSlot = null;
                        _inventoryManager.RemoveQuickSLotItem(i);
                    }
                }
            }
        }

        private void ShowQuickItems()
        {
            var quickItems = _inventoryManager.GetQuickItems();

            for (int i = 0; i < quickItems.Length; i++)
            {
                var quickItem = quickItems[i];

                _quickItems[i].CurrentItemSlot = quickItem;
            }
        }
    }
}