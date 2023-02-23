using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class InputsHandler : MonoBehaviour
    {
        public static InputsHandler Instance;
        
        public bool analogMovement;
        
        private Controls _controls;
        private Controls.PlayerActions _playerActions;
        private Controls.InventoryActions _inventoryActions;
        
        private bool _isPlayerActionEnabled;
        private bool _isInventoryOpen;
        private void Awake()
        {
            Instance = this;
            _controls = new Controls();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _playerActions = _controls.Player;
            _inventoryActions = _controls.Inventory;
        }

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
            }

            if (!_playerActions.enabled)
            {
                _playerActions.Enable();
            }

            #region Movement
            _playerActions.Move.started += PlayerManager.MoveInput;
            _playerActions.Move.performed += PlayerManager.MoveInput;
            _playerActions.Move.canceled += PlayerManager.MoveInput;

            _playerActions.Sprint.started += PlayerManager.SprintInput;
            _playerActions.Sprint.performed += PlayerManager.SprintInput;
            _playerActions.Sprint.canceled += PlayerManager.SprintInput;

            _playerActions.Jump.started += PlayerManager.JumpInput;
            #endregion

            #region Camera
            _playerActions.Look.started += CameraManager.LookInput;
            _playerActions.Look.performed += CameraManager.LookInput;
            _playerActions.Look.canceled += CameraManager.LookInput;
            #endregion


            #region Fight
            _playerActions.DrawWeapon.started += PlayerFightManager.DrawWeaponInput;
            _playerActions.Attack.started += PlayerFightManager.AttackInput;
            
            _playerActions.AimBlock.started += PlayerFightManager.AimBlockInput;
            _playerActions.AimBlock.performed += PlayerFightManager.AimBlockInput;
            _playerActions.AimBlock.canceled += PlayerFightManager.AimBlockInput;
            #endregion
            
            
            _playerActions.Interact.started += PlayerCollisionManager.InteractionInput;

            #region Inventory
            _playerActions.InventoryOpen.started += OpenCloseInventory;
            _playerActions.InventoryOpen.started += UIManager.InventoryOpenInput;
            
            _inventoryActions.InventoryClose.started += OpenCloseInventory;
            _inventoryActions.InventoryClose.started += UIManager.InventoryCloseInput;
            
            _inventoryActions.InventoryNextTab.started += UIManager.InventoryNextTabInput;
            _inventoryActions.InventoryPreviousTab.started += UIManager.InventoryPreviousTabInput;
            #endregion
        }

        
        private void OnDisable()
        {
            _playerActions.Disable();

            #region Movement
            _playerActions.Move.started -= PlayerManager.MoveInput;
            _playerActions.Move.performed -= PlayerManager.MoveInput;
            _playerActions.Move.canceled -= PlayerManager.MoveInput;

            _playerActions.Sprint.started -= PlayerManager.SprintInput;
            _playerActions.Sprint.performed -= PlayerManager.SprintInput;
            _playerActions.Sprint.canceled -= PlayerManager.SprintInput;

            _playerActions.Jump.started -= PlayerManager.JumpInput;
            #endregion

            #region Camera
            _playerActions.Look.started -= CameraManager.LookInput;
            _playerActions.Look.performed -= CameraManager.LookInput;
            _playerActions.Look.canceled -= CameraManager.LookInput;
            #endregion

            #region Fight
            _playerActions.DrawWeapon.started -= PlayerFightManager.DrawWeaponInput;
            _playerActions.Attack.started -= PlayerFightManager.AttackInput;
            
            _playerActions.AimBlock.started -= PlayerFightManager.AimBlockInput;
            _playerActions.AimBlock.performed -= PlayerFightManager.AimBlockInput;
            _playerActions.AimBlock.canceled -= PlayerFightManager.AimBlockInput;
            #endregion
            
            
            _playerActions.Interact.started -= PlayerCollisionManager.InteractionInput;

            #region Inventory
            _playerActions.InventoryOpen.started -= OpenCloseInventory;
            _playerActions.InventoryOpen.started -= UIManager.InventoryOpenInput;
            
            _inventoryActions.InventoryClose.started -= OpenCloseInventory;
            _inventoryActions.InventoryClose.started -= UIManager.InventoryCloseInput;
            
            _inventoryActions.InventoryNextTab.started -= UIManager.InventoryNextTabInput;
            _inventoryActions.InventoryPreviousTab.started -= UIManager.InventoryPreviousTabInput;
            #endregion
            
        }

        private void OpenCloseInventory(InputAction.CallbackContext context)
        {
            if (_isInventoryOpen)
            {
                _controls.Inventory.Disable();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                _controls.Player.Enable();

                _isInventoryOpen = false;
            }
            else
            {
                _controls.Player.Disable();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                _controls.Inventory.Enable();

                _isInventoryOpen = true;
            }
            
        }
    }
}