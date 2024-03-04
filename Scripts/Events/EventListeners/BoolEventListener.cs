using Events.EventArgs;
using Events.EventArgs.Player;
using UnityEngine;

namespace Events.EventListeners
{
    [CreateAssetMenu(fileName = "New Bool Event Listener", menuName = "Events/Listeners/BoolEventListener")]
    public class BoolEventListener : BaseGameEventListener<BoolEventArgs>
    {
        
    }
}