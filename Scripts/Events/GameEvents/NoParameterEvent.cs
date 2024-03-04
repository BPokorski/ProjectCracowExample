using System;
using UnityEngine;

namespace Events.GameEvents
{
    [CreateAssetMenu(fileName = "New No Parameter Event", menuName = "Events/GameEvent/NoParameterEvent")]
    public class NoParameterEvent : ScriptableObject
    {
        protected event Action _event;
        
        public void RaiseEvent()
        {
            _event?.Invoke();
        }
        public void AddListener(Action gameEventListener)
        {
            _event += gameEventListener;
        }

        public void RemoveListener(Action gameEventListener)
        {
            _event -= gameEventListener;
        }
    }
}