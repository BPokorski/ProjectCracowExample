using System.Collections.Generic;
using Character.Data;
using DG.Tweening;
using Events.EventArgs;
using Events.EventListeners;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.Crosshair
{
    public class UIPerfectAimCrosshairManager : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Transform _perfectAimPartContainer;
        
        [Header("Aim Data")]
        [SerializeField] private AimData _aimData;

        [Header("Events")]
        [SerializeField] private BoolEventListener _aimingEventListener;
        [SerializeField] private IntEventListener _aimingTicksPassedEventListener;
        
        private List<Transform> _perfectAimPartTransforms = new List<Transform>();
        private List<GameObject> _perfectAimPartGameObjects = new List<GameObject>();

        private Vector3 _initAimPartSize;
        private Vector3 _endAimPartSize;

        private void Awake()
        {
            _initAimPartSize = new Vector3(
                _aimData.InitAimPartSize, _aimData.InitAimPartSize, _aimData.InitAimPartSize
            );
            
            _endAimPartSize = new Vector3(
                _aimData.EndAimPartSize, _aimData.EndAimPartSize, _aimData.EndAimPartSize
            );
            InitPerfectAimParts();
        }

        private void OnEnable()
        {
            RestartPerfectAimCrosshair();
            _aimingEventListener.AddListener(OnAim);
            _aimingTicksPassedEventListener.AddListener(OnAimingTicksPassed);
            
        }

        private void OnDisable()
        {
            RestartPerfectAimCrosshair();
            _aimingEventListener.RemoveListener(OnAim);
            _aimingTicksPassedEventListener.RemoveListener(OnAimingTicksPassed);
        }

        private void InitPerfectAimParts()
        {
            foreach (var perfectAimPartSprite in _aimData.PerfectAimCrosshairPart)
            {
                var instantiatedAimPartPrefab = Instantiate(_aimData.PerfectAimCrosshairPartPrefab,
                    _perfectAimPartContainer);
                
                var imageComponent = instantiatedAimPartPrefab.GetComponent<Image>();
                imageComponent.sprite = perfectAimPartSprite;
                
                instantiatedAimPartPrefab.SetActive(false);
                instantiatedAimPartPrefab.transform.localScale = _initAimPartSize;
                
                _perfectAimPartTransforms.Add(instantiatedAimPartPrefab.transform);
                _perfectAimPartGameObjects.Add(instantiatedAimPartPrefab);
            }
        }
        
        private void OnAimingTicksPassed(IntEventArgs aimingTicksPassedEventArgs)
        {
            var aimingTick = aimingTicksPassedEventArgs.Value;
            _perfectAimPartGameObjects[aimingTick].SetActive(true);
            _perfectAimPartTransforms[aimingTick].DOScale(_endAimPartSize, _aimData.AimingTick);
        }
        private void OnAim(BoolEventArgs aimingEventArgs)
        {
            if (!aimingEventArgs.Value)
            {
                RestartPerfectAimCrosshair();
            }
        }
        private void RestartPerfectAimCrosshair()
        {
            for (int i = 0; i < _perfectAimPartTransforms.Count; i++)
            {
                _perfectAimPartTransforms[i].DOKill();
                _perfectAimPartTransforms[i].localScale = _initAimPartSize;
                _perfectAimPartGameObjects[i].SetActive(false);
            }
        }
    }
}