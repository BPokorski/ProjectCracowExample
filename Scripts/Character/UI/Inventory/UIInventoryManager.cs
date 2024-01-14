using Character.UI.Inventory.ItemSlots;
using Events.EventArgs;
using Events.EventArgs.UI;
using Events.EventListeners;
using Events.GameEvents;
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
        
        [Header("UI Elements")]
        [SerializeField] private TMP_InputField _itemDescription;
        
        [Header("Events")]
        [SerializeField] private BoolEvent _inventoryOpenEvent;
        [SerializeField] private CurrentTabUIEventListener _currentTabUIEventListener;
        [SerializeField] private BoolEventListener _menuOpenCloseEventListener;
        
        private void Awake()
        {
            _tabType = UIMenuTabType.Inventory;
        }

        private void OnEnable()
        {
            _currentTabUIEventListener.AddListener(OnTabActivate);
            _menuOpenCloseEventListener.AddListener(OnMenuOpenClose);
            UIItemSlot.PointerEnter += OnPointerEnter;
            UIItemSlot.PointerExit += OnPointerExit;
        }

        

        private void OnDisable()
        {
            _currentTabUIEventListener.RemoveListener(OnTabActivate);
            _menuOpenCloseEventListener.RemoveListener(OnMenuOpenClose);
            UIItemSlot.PointerEnter -= OnPointerEnter;
            UIItemSlot.PointerExit -= OnPointerExit;
        }
        
        private void OnPointerEnter(ItemSlotEventArgs itemSlotEventArgs)
        {
            _itemDescription.text = itemSlotEventArgs.ItemSlot?.CurrentItem.Description;
        }
        
        private void OnPointerExit()
        {
            _itemDescription.text = "";
        }

        private void OnTabActivate(CurrentTabUIEventArgs currentTabUIEventArgs)
        {
            if (_tabType == currentTabUIEventArgs.CurrentTabType)
            {
                SwitchActiveState(true);
            }
            else
            {
                SwitchActiveState(false);
            }
        }

        private void OnMenuOpenClose(BoolEventArgs menuOpenCloseEventArgs)
        {
            SwitchActiveState(menuOpenCloseEventArgs.Value);
        }

        private void SwitchActiveState(bool isActive)
        {
            _inventoryUI.SetActive(isActive);
            _inventoryOpenEvent.RaiseEvent(new BoolEventArgs
            {
                Value = isActive
            });
        }
    }
}