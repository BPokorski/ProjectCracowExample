using System.Collections.Generic;
using Equipment;
using Items;
using TMPro;
using UnityEngine;

namespace Character.UI
{
    public class InventoryUIManager : MonoBehaviour
    {
        public static InventoryUIManager Instance { get; private set; }

        [Header("Inventory")]
        #region InventoryFields
        [SerializeField] private GameObject _inventoryUI;
        [SerializeField] private Inventory _inventory;
        [SerializeField] private MenuTabType _tabType;
        [Header("Items")]
        [SerializeField] private List<GameObject> _consumableItemSlots;
        [SerializeField] private List<GameObject> _questItemSlots;
        
        [Header("Weapons")]
        [SerializeField] private List<GameObject> _meleeWeaponSlots;
        [SerializeField] private List<GameObject> _rangedWeaponSlots;
        [SerializeField] private TMP_InputField _itemDescription;
        #endregion

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            _tabType = MenuTabType.Inventory;
        }

        private void OnEnable()
        {
            MenuUIManager.OnTabActivate += ShowHideInventory;
        }

        private void ShowHideInventory(MenuTabType tabType)
        {
            if (_tabType == tabType)
            {
                _inventoryUI.SetActive(true);
                ShowItems(ItemType.Consumable, _consumableItemSlots);
                ShowItems(ItemType.Quest, _questItemSlots);
                ShowItems(ItemType.MeleeWeapon, _meleeWeaponSlots);
                ShowItems(ItemType.RangedWeapon, _rangedWeaponSlots);
                
            }
            else
            {
                _inventoryUI.SetActive(false);
            }
        }

        private void OnDisable()
        {
            MenuUIManager.OnTabActivate -= ShowHideInventory;
        }
        
        private void ShowItems(ItemType itemType, List<GameObject> itemSlots)
        {
            foreach (var slot in itemSlots)
            {
                if (slot.TryGetComponent<ItemSlot>(out var itemSlot))
                {
                    itemSlot.SetItemSlot(null, 0);
                }
            }
            var items = _inventory.GetItemsByType(itemType);
            
            int slotIndex = 0;
            foreach (var itemIndex in items.Keys)
            {
                var slot = itemSlots[slotIndex];
                var item = items[itemIndex];
                
                if (slot.TryGetComponent<ItemSlot>(out var itemSlot))
                {
                    itemSlot.SetItemSlot(item.CurrentItem, item.Amount);
                }

                slotIndex += 1;
            }
            
        }
    }
}