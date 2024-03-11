using Events.EventArgs;
using UnityEngine;

namespace Events.GameEvents
{
    [CreateAssetMenu(fileName = "New Bool Event", menuName = "Events/GameEvent/GameObjectEvent")]
    public class GameObjectEvent : BaseGameEvent<GameObjectEventArgs>
    {
        
    }
}