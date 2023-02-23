using System;
using System.Collections.Generic;
using Character.Data;
using Equipment;
using Items;
using Character;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Character
{
    public class UIManager : MonoBehaviour
    {
        #region Inputs
        public static Action<InputAction.CallbackContext> InventoryOpenInput;
        public static Action<InputAction.CallbackContext> InventoryCloseInput;
        public static Action<InputAction.CallbackContext> InventoryNextTabInput;
        public static Action<InputAction.CallbackContext> InventoryPreviousTabInput;
        #endregion
        
        [SerializeField] private GameObject crosshair;
        [Header("Inventory")]
        #region InventoryFields
        [SerializeField] private GameObject _inventoryUI;
        
        [SerializeField] private GameObject _tabsPlacement;
        
        [SerializeField] private TextMeshProUGUI _currentTabName;
        [SerializeField] private Inventory inventory;
        
        [SerializeField] private GameObject _itemsPlacement;
        [SerializeField] private InventoryUIData _inventoryUIData;
        #endregion
        [Header("Interaction")]
        #region InteractionFields
        [SerializeField] private GameObject _interactionWindow;
        [SerializeField] private TextMeshProUGUI _interactedObjectName;
        [SerializeField] private TextMeshProUGUI _interactedAction;
        #endregion
        
        private int _currentTabId = (int) ItemType.Weapon;

        private List<Button> _tabsButtons = new List<Button>();
        private List<GameObject> _tabsHighlightFrames = new List<GameObject>();
        private ObjectPool<GameObject> _itemsPool;
        
        private int _currentItemNumber;

        private void Awake()
        {
            _currentItemNumber = 0;
            crosshair.SetActive(false);
            _inventoryUI.SetActive(false);
            _interactionWindow.SetActive(false);
            
            CreateInventoryTabs();

            _itemsPool = new ObjectPool<GameObject>(CreateItemSlot, OnGetItemSlotFromPool, OnReleaseItemSlotToPool,
                OnDestroyItemSlotPool, false, 25, 50);
        }

        
        private void OnEnable()
        {
            InventoryOpenInput += ShowInventory;
            InventoryCloseInput += HideInventory;

            InventoryNextTabInput += SwitchNextTab;
            InventoryPreviousTabInput += SwitchPreviousTab;
            
            PlayerEvents.OnAim += ShowHideCrosshair;
            PlayerEvents.OnInteractiveCollisionDetected += DisplayHideInteractiveObject;
        }
        
        private void OnDisable()
        {
            InventoryOpenInput -= ShowInventory;
            InventoryCloseInput -= HideInventory;
            
            InventoryNextTabInput -= SwitchNextTab;
            InventoryPreviousTabInput -= SwitchPreviousTab;
            
            PlayerEvents.OnAim -= ShowHideCrosshair;
            PlayerEvents.OnInteractiveCollisionDetected -= DisplayHideInteractiveObject;
        }

        #region ItemSlotPool
        private GameObject CreateItemSlot()
        {
            var inventorySlotGameObject = Instantiate(_inventoryUIData.ItemSlot,
                Vector3.zero, 
                Quaternion.identity, _itemsPlacement.transform);
            
            inventorySlotGameObject.SetActive(false);
            inventorySlotGameObject.transform.localPosition = GetItemSlotPosition(_currentItemNumber);
            _currentItemNumber += 1;
            return inventorySlotGameObject;
        }

        private void OnDestroyItemSlotPool(GameObject itemSlot)
        {
            Destroy(itemSlot);
        }

        private void OnGetItemSlotFromPool(GameObject itemSlot)
        {
            itemSlot.SetActive(true);
        }

        private void OnReleaseItemSlotToPool(GameObject itemslot)
        {
            itemslot.SetActive(false);
        }
        

        #endregion

        #region Crosshair
        private void ShowHideCrosshair(bool isAiming)
        {
            if (isAiming)
                crosshair.SetActive(true);
            else
                crosshair.SetActive(false);
        }
        #endregion

        #region Interaction
        private void DisplayHideInteractiveObject(bool isColliding, string collidingObjectName, string interactiveTaskName)
        {
            if (isColliding)
            {
                _interactionWindow.SetActive(true);
            }
            else
            {
                _interactionWindow.SetActive(false);
            }
            _interactedObjectName.text = collidingObjectName;
            _interactedAction.text = interactiveTaskName;
        }
        #endregion

        #region Inventory
        private void CreateInventoryTabs()
        {
            float initXTabPosition = _inventoryUIData.InitXTabPosition;
            float xSpaceBetweenTabs = _inventoryUIData.XSpaceBetweenTabs;
            float currentXTabPosition = initXTabPosition;
            foreach (var tab in _inventoryUIData.TabsData)
            {
                var instantiatedTab = Instantiate(_inventoryUIData.TabSlot, _tabsPlacement.transform);
                instantiatedTab.transform.localPosition = new Vector3(currentXTabPosition, 0f, 0f);
                
                var buttonTab = instantiatedTab.transform.GetChild(0).gameObject.GetComponent<Button>();
                var highlightTabFrame = instantiatedTab.transform.GetChild(1).gameObject;
                
                buttonTab.image.sprite = tab.TabIcon;
                
                _tabsButtons.Add(buttonTab);
                _tabsHighlightFrames.Add(highlightTabFrame);
                
                currentXTabPosition += xSpaceBetweenTabs;
                
            }
            for (int i = 0; i < _tabsButtons.Count; i++)
            {
                var index = i;
                _tabsButtons[i].onClick.AddListener(() =>
                {
                    SwitchActiveTab(index);
                });
            }
        }
        private void SwitchActiveTab(int newTabPosition)
        {
            var currentTab = _tabsHighlightFrames[_currentTabId];
            currentTab.SetActive(false);
            
            _currentTabId = newTabPosition;
            var newTab = _tabsHighlightFrames[_currentTabId];
            newTab.SetActive(true);
            _currentTabName.text = _inventoryUIData.TabsData[_currentTabId].TabName;
            ShowItems(itemType:(ItemType) newTabPosition);
        }

        private void ShowItems(ItemType itemType)
        {
            foreach (Transform slot in _itemsPlacement.transform)
            {
                _itemsPool.Release(slot.gameObject);
            }
            var items = inventory.GetItemsByType(itemType);

            foreach (var itemID in items.Keys)
            {
                var item = items[itemID];
                var inventorySlotGameObject = _itemsPool.Get();

                if (inventorySlotGameObject.TryGetComponent<ItemSlot>(out var itemSlot))
                {
                    itemSlot.SetItemSlot(item.CurrentItem.ItemIcon, item.Amount.ToString());
                }
            }
        }

        private Vector3 GetItemSlotPosition(int itemNumber)
        {
            return new Vector3(_inventoryUIData.InitXInventoryPosition + (_inventoryUIData.XSpaceBetweenItems * (itemNumber % _inventoryUIData.ItemsColumnNumber)),
                _inventoryUIData.InitYInventoryPosition + (-_inventoryUIData.YSpaceBetweenItems * (itemNumber / _inventoryUIData.ItemsColumnNumber)),
                0f);
        }

        #endregion

        #region InputsMethods
        private void ShowInventory(InputAction.CallbackContext context)
        {
            _inventoryUI.SetActive(true);
            var defaultTab = _tabsHighlightFrames[_currentTabId];
            defaultTab.SetActive(true);
            _currentTabName.text = _inventoryUIData.TabsData[_currentTabId].TabName;
            ShowItems((ItemType) _currentTabId);
        }
        
        private void HideInventory(InputAction.CallbackContext context)
        {
            _inventoryUI.SetActive(false);
        }
        
        private void SwitchPreviousTab(InputAction.CallbackContext context)
        {
            var previousMarkId = _currentTabId - 1;
            if (previousMarkId >= 0)
            {
                SwitchActiveTab(previousMarkId);
            }
            else
            {
                SwitchActiveTab(_tabsButtons.Count - 1);
            }
        }

        private void SwitchNextTab(InputAction.CallbackContext context)
        {
            var nextMarkId = _currentTabId + 1;
            if (nextMarkId <= _tabsButtons.Count - 1)
            {
                SwitchActiveTab(nextMarkId);
            }
            else
            {
                SwitchActiveTab(0);
            }
            
        }
        #endregion
        
        
    }
}