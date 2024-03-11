using System.Collections;
using Character.Data;
using Events.EventArgs;
using Events.EventListeners;
using Events.GameEvents;
using UnityEngine;

namespace Character.Fight.Archery.Aim
{
    public class AimTickCounterManager : MonoBehaviour
    {
        [Header("Aim Data")]
        [SerializeField] private AimData _aimData;
        
        [Header("Aim Events")]
        [SerializeField] private IntEvent _aimingTickPassedEvent;
        [SerializeField] private NoParameterEvent _perfectAimingEvent;
        [SerializeField] private BoolEventListener _aimEventListener;
        
        
        private WaitForSeconds _aimingTickInterval;
        private Coroutine _aimTickPassedCoroutine;

        private bool _isAiming;
        
        private void Awake()
        {
            _aimingTickInterval = new WaitForSeconds(_aimData.AimingTick);
        }

        private void OnEnable()
        {
            _aimEventListener.AddListener(OnAim);
        }

        
        private void OnDisable()
        {
            _aimEventListener.RemoveListener(OnAim);
        }

        private void OnAim(BoolEventArgs eventArgs)
        {
            _isAiming = eventArgs.Value;
            if (_isAiming)
            {
                _aimTickPassedCoroutine = StartCoroutine(AimingTimePassed());
            }
            else
            {
                StopCoroutine(_aimTickPassedCoroutine);
            }
        }
        
        private IEnumerator AimingTimePassed()
        {
            var aimTicks = 0;
            while (aimTicks < _aimData.PerfectAimCrosshairPart.Count)
            {
                yield return _aimingTickInterval;
                if (!_isAiming)
                {
                    
                    yield break;
                }
                
                _aimingTickPassedEvent.RaiseEvent(new IntEventArgs
                {
                    Value = aimTicks
                });
                aimTicks += 1;
            }
            _perfectAimingEvent.RaiseEvent();
        }
    }
}