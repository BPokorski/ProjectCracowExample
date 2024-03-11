using Events.EventArgs;
using Events.GameEvents;
using UnityEngine;

namespace Events.EventListeners
{
    [CreateAssetMenu(fileName = "New Game Object Event Listener", menuName = "Events/Listeners/GameObjectEventListener")]

    public class GameObjectEventListener : BaseGameEventListener<GameObjectEventArgs>
    {
        
    }
}