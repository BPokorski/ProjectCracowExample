using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Character.UI
{
    public class MenuUIManager : MonoBehaviour
    {
        public static event Action<MenuTabType> OnTabActivate;
        public static event Action<bool> OnMenuOpenClose;
        
        [Header("Menu")]
        [SerializeField] private GameObject _menu;
        [SerializeField] private List<GameObject> _tabObjects;
        
        
        private MenuTabType _currentActiveTabType;

        private int _currentActiveTabId;
        
        private List<MenuTab> _menuTabs= new List<MenuTab>();
        private bool _isMenuOpened = false;
        
        private void Awake()
        {
            _menu.SetActive(false);
            
        }

        
        private void OnEnable()
        {
            InputsManager.Instance.PlayerActions.Inventory.started += ShowHideInventory;
            InputsManager.Instance.MenuActions.Inventory.started += ShowHideInventory;
            
            InputsManager.Instance.MenuActions.NextTab.started += SwitchNextTab;
            InputsManager.Instance.MenuActions.PreviousTab.started += SwitchPreviousTab;
        }

        private void Start()
        {
            for (int i = 0; i < _tabObjects.Count; i++)
            {
                var index = i;
                var tabButton = _tabObjects[i].GetComponent<MenuTab>();
                var button = _tabObjects[i].GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    SwitchActiveTab(index);
                });
                
                _menuTabs.Add(tabButton);
                
            }
        }

        private void OnDisable()
        {
            InputsManager.Instance.PlayerActions.Inventory.started -= ShowHideInventory;
            InputsManager.Instance.MenuActions.Inventory.started -= ShowHideInventory;
            InputsManager.Instance.MenuActions.NextTab.started -= SwitchNextTab;
            InputsManager.Instance.MenuActions.PreviousTab.started -= SwitchPreviousTab;
            
        }
        
        #region InputsMethods
        private void ShowHideInventory(InputAction.CallbackContext context)
        {
            // _menu.SetActive(true);
            int tabIndex = GetTabIndexByType(MenuTabType.Inventory);
            ShowHideMenu(tabIndex);
        }
        
        private void SwitchPreviousTab(InputAction.CallbackContext context)
        {
            var previousMarkId = _currentActiveTabId - 1;
            if (previousMarkId < 0)
            {
                previousMarkId = _menuTabs.Count - 1;
            }
            SwitchActiveTab(previousMarkId);
        }

        private void SwitchNextTab(InputAction.CallbackContext context)
        {
            var nextMarkId = _currentActiveTabId + 1;
            
            if (nextMarkId > _menuTabs.Count - 1)
            {
                nextMarkId = 0;
            }
            SwitchActiveTab(nextMarkId);

        }
        #endregion
        
        private int GetTabIndexByType(MenuTabType tabType)
        {
            for (int tabIndex = 0; tabIndex < _menuTabs.Count; tabIndex++)
            {
                if (_menuTabs[tabIndex].TabType == tabType)
                {
                    return tabIndex;
                }
            }
            return 0;
        }
        
        private void SwitchActiveTab(int newActiveTabIndex)
        {
            _menuTabs[_currentActiveTabId].SwitchButtonState(false);
            _currentActiveTabId = newActiveTabIndex;
            
            var newActiveTab = _menuTabs[_currentActiveTabId];
            _currentActiveTabType = newActiveTab.TabType;
            newActiveTab.SwitchButtonState(true);
            OnTabActivate?.Invoke(_currentActiveTabType);
        }
        
        private void ShowHideMenu(int newTabIndex)
        {
            if (newTabIndex == _currentActiveTabId && _isMenuOpened)
            {
                _menu.SetActive(false);
                OnMenuOpenClose?.Invoke(false);
                InputsManager.Instance.OpenCloseMenu(false);
                _isMenuOpened = false;
            }
            else if (newTabIndex != _currentActiveTabId && _isMenuOpened)
            {
                SwitchActiveTab(newTabIndex);
            }
            else
            {
                _menu.SetActive(true);
                SwitchActiveTab(newTabIndex);
                OnMenuOpenClose?.Invoke(true);
                InputsManager.Instance.OpenCloseMenu(true);
                _isMenuOpened = true;
                
            }
        }
        
    }
}