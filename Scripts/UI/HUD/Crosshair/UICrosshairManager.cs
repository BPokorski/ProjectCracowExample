using Character.Data;
using Events.EventArgs;
using Events.EventListeners;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.Crosshair
{
    public class UICrosshairManager :MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject _crosshair;
        [SerializeField] private Image _crosshairImage;
        
        [Header("Aim Data")]
        [SerializeField] private AimData _aimData;

        [Header("Events")]
        [SerializeField] private BoolEventListener _aimingEventListener;
        
        private void Awake()
        {
            _crosshairImage.sprite = _aimData.AimCrosshair;
            _crosshair.SetActive(false);
        }
        
        private void OnEnable()
        {
            _aimingEventListener.AddListener(OnAim);
        }
        
        private void OnDisable()
        {
            _aimingEventListener.RemoveListener(OnAim);
        }
        
        private void OnAim(BoolEventArgs aimingEventArgs)
        {
            _crosshair.SetActive(aimingEventArgs.Value);
        }
    }
}