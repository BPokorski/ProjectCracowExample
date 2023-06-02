using Character.Data;
using Character.Inventory;
using Character.UI.Inventory.ItemSlots;
using Equipment;
using UnityEngine;
using Zenject;

namespace Character.UI.Inventory.ItemInventoryManagers
{
    public class UIQuickItemInventoryManager : UIBaseItemInventoryManager
    {
        [Header("Slot data")]
        [SerializeField] private ItemSlotsData _itemSlotsData;

        private IQuickSlotInventoryManager _quickSlotInventoryManager;

        [Inject]
        private void Construct(IQuickSlotInventoryManager quickSlotInventoryManager,
            UIQuickItemSlot.Factory quickItemSlotFactory)
        {
            _quickSlotInventoryManager = quickSlotInventoryManager;
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
            return _quickSlotInventoryManager.GetQuickItems();
        }
        
        private void UpdateQuickItemSlot(int quickSlotID, ItemSlot currentItemSlot)
        {
            if (currentItemSlot != null)
            {
                var quickSlotPosition = _itemSlotIDPosition[quickSlotID];
                _quickSlotInventoryManager.SetQuickSlotItem(quickSlotPosition, currentItemSlot);

                for (int i = 0; i < _items.Length; i++)
                {
                    var quickItem = _items[i];

                    var quickItemItemSlot = quickItem.CurrentItemSlot;
                    if (quickItemItemSlot != null && 
                        quickItemItemSlot.CurrentItem.ItemID == currentItemSlot.CurrentItem.ItemID &&
                        i != quickSlotPosition)
                    {
                        quickItem.CurrentItemSlot = null;
                        _quickSlotInventoryManager.RemoveQuickSLotItem(i);
                    }
                }
            }
        }
    }
}