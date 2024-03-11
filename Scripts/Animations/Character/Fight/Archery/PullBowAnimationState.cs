using Character.Fight;
using UnityEngine;
using Zenject;

namespace Animations.Character.Fight.Archery
{
    public class PullBowAnimationState : StateMachineBehaviour
    {
        private IBowPuller _bowPuller;
        
        [Inject]
        private void Construct(IBowPuller bowPuller)
        {
            _bowPuller = bowPuller;
        }
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _bowPuller.PullBow();
        }
    }
}