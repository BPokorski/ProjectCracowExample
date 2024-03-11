using Character.Data;
using Character.Fight.Archery.Aim;
using Character.Fight.Archery.ArrowManagement;
using Events.EventArgs;
using Events.EventArgs.Stats;
using Events.EventListeners;
using Events.GameEvents;
using Items.Managers.Bow;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Random = UnityEngine.Random;

namespace Character.Fight.Archery
{
    public class BowFightManager : BaseFightManager, IBowPuller
    {
        [Header("Aim Parameters")]
        [SerializeField] protected AimData _aimData;

        [Header("Events")] 
        [SerializeField] protected BoolEvent _aimEvent;
        [SerializeField] protected NoParameterEventListener _perfectAimEventListener;
        
        private IAimDirectionProvider _aimDirectionProvider;
        private IArrowManager _arrowManager;
        private IBowManager _bowManager;
        
        private bool _isAiming;
        private bool _isPerfectAiming;
        
        [Inject]
        private void Construct(IAimDirectionProvider aimDirectionProvider, IArrowManager arrowManager)
        {
            _aimDirectionProvider = aimDirectionProvider;
            _arrowManager = arrowManager;
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _inputs.AttackAction.started += OnShootInput;

            _inputs.AimAction.started += OnAimInput;
            _inputs.AimAction.canceled += OnAimInput;
            
            _staminaManager.Stamina.StatChanged += OnZeroStaminaValue;
            _perfectAimEventListener.AddListener(OnPerfectAim);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            _inputs.AttackAction.started -= OnShootInput;
            
            _inputs.AimAction.started -= OnAimInput;
            _inputs.AimAction.canceled -= OnAimInput;
            
            _staminaManager.Stamina.StatChanged -= OnZeroStaminaValue;
            _perfectAimEventListener.RemoveListener(OnPerfectAim);
        }
        
        protected override void OnDrawWeapon(GameObjectEventArgs drawWeaponEventArgs)
        {
            var weaponObject = drawWeaponEventArgs.GameObject;

            if (weaponObject != null)
            {
                _bowManager = drawWeaponEventArgs.GameObject.GetComponentInChildren<IBowManager>();
            }
        }
        
        private void OnAimInput(InputAction.CallbackContext context)
        {
            if (_equippedWeapon.IsWeaponDrawn)
            {
                if (_staminaManager.Stamina.CurrentValue < _baseAttackStaminaCost.Cost)
                {
                    StopAiming();
                    return;
                }
                if (context.started)
                {
                    if (!_equippedWeapon.IsWeaponDrawn) return;
                    if (!_arrowManager.IsArrowDrawn)
                    {
                        _animationManager.SetTrigger(_animationManager.Animations.AnimIDDrawArrow);
                    }
                    _isAiming = true;
                
                    
                    _staminaManager.ChangeStaminaValue(_baseAttackStaminaCost);
                    
                    
                    _aimEvent.RaiseEvent(new BoolEventArgs
                    {
                        Value = true
                    });
                    
                    _animationManager.SetBool(_animationManager.Animations.AnimIDAiming, true);
                    
                }
                else if (context.canceled)
                {
                    StopAiming();
                }
            }
            
        }
        
        private void OnShootInput(InputAction.CallbackContext context)
        {
            if (!_equippedWeapon.IsWeaponDrawn || !_isAiming || !_arrowManager.IsArrowDrawn) return;
            _animationManager.SetTrigger(_animationManager.Animations.AnimIDShoot);
                
            _bowManager.Shoot();
            
            var damage = Random.Range(_equippedWeapon.CurrentWeapon.MinDamage, _equippedWeapon.CurrentWeapon.MaxDamage);
                
            if (_isPerfectAiming)
            {
                damage *= _aimData.PerfectAimDamageMultiplier;
            }
            _arrowManager.ShootArrow(damage, _aimDirectionProvider.AimDirection);
            _isAiming = false;
                
            _staminaManager.ChangeStaminaValue(null);
        }
        public void PullBow()
        {
            _bowManager.PullString();
        }
        
        private void ReleaseAfterShoot()
        {
            _animationManager.SetBool(_animationManager.Animations.AnimIDAiming, false);
        }
        
        private void OnZeroStaminaValue(StatValueEventArgs statValueEventArgs)
        {
            if (_equippedWeapon.IsWeaponDrawn && statValueEventArgs.CurrentValue <= 0)
            {
                StopAiming();
            }
        }
        
        private void StopAiming()
        {
            _isAiming = false;
            _animationManager.SetBool(_animationManager.Animations.AnimIDAiming, false);
            _bowManager.ReleaseString();
            
            _aimEvent.RaiseEvent(new BoolEventArgs
            {
                Value = false
            });
            _animationManager.SetFloat(_animationManager.Animations.AnimIDMovementX, 0.0f);
            _animationManager.SetFloat(_animationManager.Animations.AnimIDMovementY, 0.0f);
            _staminaManager.ChangeStaminaValue(null);
            
            _isPerfectAiming = false;
            
        }

        private void OnPerfectAim()
        {
            _isPerfectAiming = true;
        }
    }
}