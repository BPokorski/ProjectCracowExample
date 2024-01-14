using Character.Data;
using Character.Inventory;
using Character.UI.Inventory.ItemSlots;
using Equipment;
using Events.EventArgs.UI;
using UnityEngine;
using Zenject;

namespace Character.UI.Inventory.ItemInventoryManagers
{
    public class UIQuickItemInventoryManager : UIBaseItemInventoryManager
    {
        [Header("Slot data")]
        [SerializeField] private ItemSlotsData _itemSlotsData;

        [Inject]
        private void Construct(UIQuickItemSlot.Factory quickItemSlotFactory)
        {
            _factory = quickItemSlotFactory;
        }
        
        protected override void Awake()
        {
            base.Awake();
            InitItems(_itemSlotsData.QuickSlotNumber);
            InitItemSlots();
        }

        private void OnEnable()
        {
            foreach (var itemSlot in _items)
            {
                itemSlot.ItemSlotActionTrigger += UpdateQuickItemSlot;
            }
            
            var items = GetItems();
            ShowItems(items);
        }

        private void OnDisable()
        {
            foreach (var itemSlot in _items)
            {
                itemSlot.ItemSlotActionTrigger -= UpdateQuickItemSlot;
            }
            HideItems();
        }
        
        private ItemSlot[] GetItems()
        {
            return _inventory.QuickItems;
        }
        
        private void UpdateQuickItemSlot(ItemSlotEventArgs itemSlotEventArgs)
        {
            var currentItemSlot = itemSlotEventArgs.ItemSlot;
            var quickSlotID = itemSlotEventArgs.ItemSlotId;
            if (currentItemSlot != null)
            {
                var quickSlotPosition = _itemSlotIDPosition[quickSlotID];
                // Debug.Log($"{quickSlotID}: pos: {quickSlotPosition}");
                _inventory.SetQuickSlotItem(quickSlotPosition, currentItemSlot);

                for (int i = 0; i < _items.Length; i++)
                {
                    var quickItem = _items[i];

                    var quickItemItemSlot = quickItem.CurrentItemSlot;
                    if (quickItemItemSlot != null && 
                        quickItemItemSlot.CurrentItem.ItemID == currentItemSlot.CurrentItem.ItemID &&
                        i != quickSlotPosition)
                    {
                        quickItem.CurrentItemSlot = null;
                        _inventory.RemoveQuickSLotItem(i);
                    }
                }
            }
        }
    }
}