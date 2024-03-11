using Items;
using UnityEngine;
using Zenject;

namespace Character.Fight.Archery.ArrowManagement
{
    public class ArrowManager : MonoBehaviour, IArrowManager, IArrowDrawer
    {
        [SerializeField] private Arrow _arrow;
        [SerializeField] private Transform _arrowItemHolder;

        private GameObject _arrowModel;
        private IArrowSpawner _arrowSpawner;
        public bool IsArrowDrawn { get; private set; }

        [Inject]
        private void Construct(IArrowSpawner arrowSpawner)
        {
            _arrowSpawner = arrowSpawner;
        }
        private void Awake()
        {
            _arrowModel = Instantiate(_arrow.ItemPrefab, _arrowItemHolder);
            _arrowModel.transform.localPosition = _arrow.ArrowInHandPosition.position;
            _arrowModel.transform.localRotation = _arrow.ArrowInHandPosition.rotation;
            _arrowModel.SetActive(false);
        }


        public void ShootArrow(int damage, Vector3 direction)
        {
            if (_arrowModel)
            {
                _arrowModel.SetActive(false);
            }
            var arrowProjectile = _arrowSpawner.GetArrow();
                
            var targetDirection = (direction - arrowProjectile.transform.position).normalized;
            
            arrowProjectile.Shoot(damage, targetDirection);
            
            arrowProjectile.ThisTransform.parent = null;
            IsArrowDrawn = false;
        }
        
        public void DrawArrow()
        {
            _arrowModel.SetActive(true);
            IsArrowDrawn = true;
        }

        public void HideArrow()
        {
            _arrowModel.SetActive(false);
            IsArrowDrawn = false;
        }
    }
}