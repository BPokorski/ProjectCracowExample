using System;
using Events.GameEvents;
using UnityEngine;

namespace Events.EventListeners
{
    public abstract class BaseGameEventListener<T> : ScriptableObject
    {
        [SerializeField] protected BaseGameEvent<T> _gameEvent;


        public void AddListener(Action<T> gameEventListener)
        {
            _gameEvent.AddListener(gameEventListener);
        }
        
        public void RemoveListener(Action<T> gameEventListener)
        {
            _gameEvent.RemoveListener(gameEventListener);
        }
    }
}