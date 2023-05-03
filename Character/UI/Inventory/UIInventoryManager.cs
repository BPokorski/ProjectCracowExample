using System.Collections.Generic;
using Equipment;
using Items;
using TMPro;
using UnityEngine;
namespace Character.UI.Inventory
{
    public class UIInventoryManager : MonoBehaviour
    {
        public static UIInventoryManager Instance { get; private set; }

        [Header("Inventory")]
        [SerializeField] private GameObject _inventoryUI;
        [SerializeField] private UIMenuTabType _tabType;
        
        [SerializeField] private TMP_InputField _itemDescription;

        private IItemSlotsManager _usableItemsManager;
        private IItemSlotsManager _equipmentItemsManager;

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

            _tabType = UIMenuTabType.Inventory;

            _usableItemsManager = GetComponent<UIUsableItemInventoryManager>();
            _equipmentItemsManager = GetComponent<UIEquipmentItemInventoryManager>();
        }

        private void OnEnable()
        {
            UIMenuManager.OnTabActivate += ShowHideInventory;
            UIItemSlot.OnPointerEntered += ShowDescription;
            UIItemSlot.OnPointerExited += HideDescription;
        }

        

        private void OnDisable()
        {
            UIMenuManager.OnTabActivate -= ShowHideInventory;
            UIItemSlot.OnPointerEntered -= ShowDescription;
            UIItemSlot.OnPointerExited -= HideDescription;
        }

        private void ShowDescription(ItemSlot itemSlot)
        {
            _itemDescription.text = itemSlot?.CurrentItem.Description;
        }
        
        private void HideDescription()
        {
            _itemDescription.text = "";
        }

        private void ShowHideInventory(UIMenuTabType tabType)
        {
            if (_tabType == tabType)
            {
                _inventoryUI.SetActive(true);
                
                _usableItemsManager.ShowItems();
                _equipmentItemsManager.ShowItems();
            }
            else
            {
                _inventoryUI.SetActive(false);
            }
        }
    }
}