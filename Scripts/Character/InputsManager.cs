using Character.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class InputsManager : MonoBehaviour
    {
        public static InputsManager Instance { get; private set; }
        
        public bool analogMovement;
        
        private Controls _controls;
        public Controls.PlayerActions PlayerActions { get; private set; }
        public Controls.MenuActions MenuActions { get; private set; }
        
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
            _controls = new Controls();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            PlayerActions = _controls.Player;
            MenuActions = _controls.Menu;
        }

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
            }

            if (!PlayerActions.enabled)
            {
                PlayerActions.Enable();
            }
            
            #region Inventory
            // MenuUIManager.OnMenuOpenClose += OpenCloseMenu;
            #endregion
        }

        
        private void OnDisable()
        {
            PlayerActions.Disable();
            #region Inventory
            // MenuUIManager.OnMenuOpenClose -= OpenCloseMenu;
            #endregion
        }

        public void OpenCloseMenu(bool isMenuShown)
        {
            if (isMenuShown)
            {
                _controls.Player.Disable();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                _controls.Menu.Enable();
                
            }
            else
            {
                _controls.Menu.Disable();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                _controls.Player.Enable();
                
            }
        }
    }
}