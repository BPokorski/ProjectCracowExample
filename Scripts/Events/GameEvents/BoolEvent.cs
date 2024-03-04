using Events.EventArgs;
using Events.EventArgs.Player;
using UnityEngine;

namespace Events.GameEvents
{
    [CreateAssetMenu(fileName = "New Bool Event", menuName = "Events/GameEvent/BoolEvent")]
    public class BoolEvent : BaseGameEvent<BoolEventArgs>
    {
        
    }
}