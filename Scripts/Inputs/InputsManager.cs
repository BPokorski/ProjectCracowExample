using Events.EventArgs;
using Events.EventListeners;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class InputsManager : MonoBehaviour, IMovementInputs, IAttackInputs,
        ICameraInputs, IInteractionInputs, IManageWeaponInputs, IMenuInputs
    {
        [Header("Input Asset")]
        [SerializeField] private InputActionAsset _inputAsset;

        #region Inputs
        [Header("Inputs")]

        #region MovementInputs
        [SerializeField] private InputActionReference _moveActionReference;
        [SerializeField] private InputActionReference _runActionReference;
        [SerializeField] private InputActionReference _jumpActionReference;
        #endregion
        
        #region AttackInputs
        [SerializeField] private InputActionReference _attackActionReference;
        [SerializeField] private InputActionReference _blockActionReference;
        [SerializeField] private InputActionReference _aimActionReference;
        [SerializeField] private InputActionReference _dodgeActionReference;
        #endregion

        #region ManageWeaponInputs
        [SerializeField] private InputActionReference _drawWeaponActionReference;
        [SerializeField] private InputActionReference _switchWeaponActionReference;
        #endregion
        #region CameraInputs
        [SerializeField] private InputActionReference _lookActionReference;
        
        #endregion

        #region InteractionInputs
        [SerializeField] private InputActionReference _interactActionReference;
        #endregion
        #region MenuInputs
        [Header("Menu Inputs")]
        [SerializeField] private InputActionReference _inventoryOpenActionReference;
        [SerializeField] private InputActionReference _inventoryCloseActionReference;
        [SerializeField] private InputActionReference _clickActionReference;
        [SerializeField] private InputActionReference _pointerActionReference;
        [SerializeField] private InputActionReference _previousTabActionReference;
        [SerializeField] private InputActionReference _nextTabActionReference;
        #endregion
        
        #endregion

        [Header("Action Maps")] 
        [SerializeField] private string _playerActionMapName;
        [SerializeField] private string _menuActionMapName;
        #region Events
        [Header("Events")]
        [SerializeField] private NoParameterEventListener _deathEventListener;
        [SerializeField] private BoolEventListener _menuOpenCloseEventListener;
        #endregion
        
        private InputActionMap _playerActionMap;
        private InputActionMap _menuActionMap;
        
        private void Awake()
        {
            // _controls = new Controls();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _playerActionMap = _inputAsset.FindActionMap(_playerActionMapName, true);
            _menuActionMap = _inputAsset.FindActionMap(_menuActionMapName, true);
        }

        private void OnEnable()
        {
            _inputAsset.Enable();
            _playerActionMap.Enable();
            _menuActionMap.Disable();
            _deathEventListener.AddListener(OnPlayerDeath);
            _menuOpenCloseEventListener.AddListener(OnMenuOpenClose);
            
        }

        
        private void OnDisable()
        {
            _deathEventListener.RemoveListener(OnPlayerDeath);
            _menuOpenCloseEventListener.RemoveListener(OnMenuOpenClose);
            
            _inputAsset.Disable();
        }

        #region InputActions
        #region MovementInputActions
        public InputAction MoveAction => _moveActionReference.action;
        public InputAction JumpAction => _jumpActionReference.action;
        public InputAction RunAction => _runActionReference.action;
        #endregion

        #region AttackInputActions
        public InputAction AttackAction => _attackActionReference.action;
        public InputAction BlockAction => _blockActionReference.action;
        public InputAction AimAction => _aimActionReference.action;
        public InputAction DodgeAction => _dodgeActionReference.action;
        #endregion
        #region ManageWeaponInputActions
        public InputAction DrawWeaponAction => _drawWeaponActionReference.action;
        public InputAction SwitchWeaponAction => _switchWeaponActionReference.action;
        #endregion
        
        #region CameraInputActions
        public InputAction LookAction => _lookActionReference.action;
        #endregion
        #region InteractionInputActions
        public InputAction InteractionAction => _interactActionReference.action;
        #endregion
        #region MenuInputActions
        public InputAction InventoryOpenAction => _inventoryOpenActionReference.action;
        public InputAction InventoryCloseAction => _inventoryCloseActionReference.action;
        public InputAction ClickAction => _clickActionReference.action;
        public InputAction PointerAction => _pointerActionReference.action;
        public InputAction PreviousTabAction => _previousTabActionReference.action;
        public InputAction NextTabAction => _nextTabActionReference.action;
        #endregion
        #endregion
        private void OnMenuOpenClose(BoolEventArgs menuOpenCloseEventArgs)
        {
            if (menuOpenCloseEventArgs.Value)
            {
                _playerActionMap.Disable();
                _menuActionMap.Enable();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                
                
            }
            else
            {
                _menuActionMap.Disable();
                _playerActionMap.Enable();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private void OnPlayerDeath()
        {
            _playerActionMap.Disable();
            _menuActionMap.Enable();
        }
    }
}