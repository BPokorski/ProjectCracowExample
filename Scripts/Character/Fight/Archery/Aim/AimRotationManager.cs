using System.Collections;
using Character.Data;
using Events.EventArgs;
using Events.EventListeners;
using RayProviders;
using UnityEngine;
using Zenject;

namespace Character.Fight.Archery.Aim
{
    public class AimRotationManager : MonoBehaviour, IAimDirectionProvider
    {
        [Header("Aim Target")]
        [SerializeField] private GameObject _aimTarget;
        
        [Header("Aim Data")]
        [SerializeField] private AimData _aimData;

        [Header("Events")] 
        [SerializeField] private BoolEventListener _aimEventListener;
        
        public Vector3 AimDirection { get; private set; }
        
        private IRayProvider _screenRayProvider;
        private WaitForSeconds _aimingRotationInterval;
        
        private Transform _thisTransform;
        private Transform _aimingTargetTransform;
        
        private Coroutine _rotatingTowardTargetCoroutine;
        private bool _isAiming;
        
        [Inject]
        private void Construct([Inject(Id = RayProvider.Screen)] IRayProvider screenRayProvider)
        {
            _screenRayProvider = screenRayProvider;
        }

        private void Awake()
        {
            _aimingRotationInterval = new WaitForSeconds(_aimData.AimRotatingTick);
            
            _thisTransform = transform;
            _aimingTargetTransform = _aimTarget.transform;
        }
        
        private void OnEnable()
        {
            _aimEventListener.AddListener(OnAim);
        }
        
        private void OnDisable()
        {
            _aimEventListener.AddListener(OnAim);
            _thisTransform.forward = new Vector3(_thisTransform.forward.x, 0.0f, _thisTransform.forward.z);
        }

        private void OnAim(BoolEventArgs eventArgs)
        {
            _isAiming = eventArgs.Value;
            if (_isAiming)
            {
                _rotatingTowardTargetCoroutine = StartCoroutine(RotatingTowardTarget());
            }
            else
            {
                StopCoroutine(_rotatingTowardTargetCoroutine);
                _thisTransform.forward = new Vector3(_thisTransform.forward.x, 0.0f, _thisTransform.forward.z);
            }
            
        }
        
        private void CalculateAimDirection()
        {
            AimDirection = Vector3.zero;
            Ray ray = _screenRayProvider.CreateRay();
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, _aimData.AimColliderLayerMask))
            {
                AimDirection = raycastHit.point;
            }
        }
        
        private void RotateTowardTarget(float time)
        {
            Vector3 worldAimTarget = AimDirection;
            
            worldAimTarget.y = _thisTransform.position.y;
            Vector3 directionDifference = (worldAimTarget - _thisTransform.position).normalized;
            transform.forward = Vector3.Lerp(_thisTransform.forward, directionDifference, time);
            _aimingTargetTransform.position = AimDirection;
        }
        
        private IEnumerator RotatingTowardTarget()
        {
            while (_isAiming)
            {
                CalculateAimDirection();
                RotateTowardTarget(Time.deltaTime / _aimData.AimRotatingTick);
                yield return _aimingRotationInterval;
            }
            
        }
    }
}