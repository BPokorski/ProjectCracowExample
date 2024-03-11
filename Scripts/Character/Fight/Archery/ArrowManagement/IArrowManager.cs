using UnityEngine;

namespace Character.Fight.Archery.ArrowManagement
{
    public interface IArrowManager
    {
        public void ShootArrow(int damage, Vector3 direction);
        
        public bool IsArrowDrawn { get; }
    }
}