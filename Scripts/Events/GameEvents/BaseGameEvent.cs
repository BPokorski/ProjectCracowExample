using System;
using Events.EventListeners;
using UnityEngine;
using UnityEngine.Events;

namespace Events.GameEvents
{
    public abstract class BaseGameEvent<T> : ScriptableObject
    {
        protected event Action<T> _event;
        
        public void RaiseEvent(T eventArgs)
        {
            _event?.Invoke(eventArgs);
        }
        public void AddListener(Action<T> gameEventListener)
        {
            _event += gameEventListener;
        }

        public void RemoveListener(Action<T> gameEventListener)
        {
            _event -= gameEventListener;
        }
    }
}