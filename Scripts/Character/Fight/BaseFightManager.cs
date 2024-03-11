using System;
using Equipment;
using Events.EventArgs;
using Events.EventListeners;
using Inputs;
using Stats.PlayerStats;
using UnityEngine;
using Zenject;

namespace Character.Fight
{
    public abstract class BaseFightManager : MonoBehaviour
    {
        [Header("Stamina Manager and Stamina Cost")]
        [SerializeField] protected StaminaManager _staminaManager;
        [SerializeField] protected StaminaActionCost _baseAttackStaminaCost;
        
        [Header("Equipped Weapon")]
        [SerializeField] protected EquippedWeapon _equippedWeapon;

        [Header("Draw Weapon Event Listener")] 
        [SerializeField] protected GameObjectEventListener _drawWeaponEventListener;
        
        protected IAnimationManager _animationManager;
        protected IAttackInputs _inputs;

        protected virtual void OnEnable()
        {
            _drawWeaponEventListener.AddListener(OnDrawWeapon);
        }

        protected virtual void OnDisable()
        {
            _drawWeaponEventListener.RemoveListener(OnDrawWeapon);
        }
        
        [Inject]
        private void Construct(IAnimationManager animationManager, IAttackInputs inputs)
        {
            _animationManager = animationManager;
            _inputs = inputs;
        }

        protected abstract void OnDrawWeapon(GameObjectEventArgs drawWeaponEventArgs);
    }
}