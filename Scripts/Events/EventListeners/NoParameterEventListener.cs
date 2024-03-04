using System;
using Events.GameEvents;
using UnityEngine;

namespace Events.EventListeners
{
    [CreateAssetMenu(fileName = "New No Parameter Event Listener", menuName = "Events/Listeners/NoParameterEventListener")]
    public class NoParameterEventListener : ScriptableObject
    {
        [SerializeField] protected NoParameterEvent _gameEvent;


        public void AddListener(Action gameEventListener)
        {
            _gameEvent.AddListener(gameEventListener);
        }
        
        public void RemoveListener(Action gameEventListener)
        {
            _gameEvent.RemoveListener(gameEventListener);
        }
    }
}