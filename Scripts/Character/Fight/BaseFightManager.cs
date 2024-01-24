using Equipment;
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
        
        protected IAnimationManager _animationManager;
        protected IInputsManager _inputs;
        
        [Inject]
        private void Construct(IAnimationManager animationManager, IInputsManager inputs)
        {
            _animationManager = animationManager;
            _inputs = inputs;
        }
        
        
    }
}