using UnityEngine.InputSystem;

namespace Inputs
{
    public interface IAttackInputs
    {
        public InputAction AttackAction { get; }
        public InputAction BlockAction { get; }
        public InputAction AimAction { get; }
        public InputAction DodgeAction { get; }
    }
}