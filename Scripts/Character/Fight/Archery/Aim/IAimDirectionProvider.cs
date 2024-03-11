using UnityEngine;

namespace Character.Fight.Archery.Aim
{
    public interface IAimDirectionProvider
    {
        public Vector3 AimDirection { get; }
    }
}