using System.Collections.Generic;
// using Character.UI.Inventory.ItemSlot;
using Character.UI.Inventory.ItemSlots;
using Equipment;
using Items;
using TMPro;
using UnityEngine;
using Zenject;

namespace Character.UI.Inventory
{
    public class UIInventoryManager : MonoBehaviour
    {
        
        [Header("Inventory")]
        [SerializeField] private GameObject _inventoryUI;
        [SerializeField] private UIMenuTabType _tabType;
        
        [SerializeField] private TMP_InputField _itemDescription;

        private IUITabActivate _tabActivate;
        [Inject]
        private void Construct(IUITabActivate tabActivate)
        {
            _tabActivate = tabActivate;
        }
        
        private void Awake()
        {
            
            _tabType = UIMenuTabType.Inventory;
            
        }

        private void OnEnable()
        {
            _tabActivate.TabActivate += OnTabActivate;
            UIItemSlot.PointerEnter += ShowDescription;
            UIItemSlot.PointerExit += HideDescription;
        }

        

        private void OnDisable()
        {
            _tabActivate.TabActivate -= OnTabActivate;
            UIItemSlot.PointerEnter -= ShowDescription;
            UIItemSlot.PointerExit -= HideDescription;
        }
        
        private void ShowDescription(ItemSlot itemSlot)
        {
            _itemDescription.text = itemSlot?.CurrentItem.Description;
        }
        
        private void HideDescription()
        {
            _itemDescription.text = "";
        }

        private void OnTabActivate(UIMenuTabType tabType)
        {
            if (_tabType == tabType)
            {
                _inventoryUI.SetActive(true);
            }
            else
            {
                _inventoryUI.SetActive(false);
            }
        }
    }
}