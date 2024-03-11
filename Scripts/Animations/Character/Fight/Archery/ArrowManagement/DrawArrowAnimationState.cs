using Character.Fight.Archery.ArrowManagement;
using UnityEngine;
using Zenject;

namespace Animations.Character.Fight.Archery.ArrowManagement
{
    public class DrawArrowAnimationState : StateMachineBehaviour
    {
        private IArrowDrawer _arrowDrawer;


        [Inject]
        private void Construct(IArrowDrawer arrowDrawer)
        {
            _arrowDrawer = arrowDrawer;
        }
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _arrowDrawer.DrawArrow();
        }
    }
}