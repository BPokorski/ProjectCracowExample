using Items;
using Items.Managers;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Character.Fight.Archery.ArrowManagement
{
    public class ArrowSpawner : IArrowSpawner
    {
        private Transform _arrowParent;
        private Arrow _arrowItem;
        private ObjectPool<ArrowProjectile> _pool;
        
        public ArrowSpawner(Transform arrowParent, Arrow arrowItem)
        {
            _arrowParent = arrowParent;
            _arrowItem = arrowItem;
            
            CreatePool();
        }

        private void CreatePool()
        {
            _pool = new ObjectPool<ArrowProjectile>(OnCreateItem, OnTakeItemFromPool,
                OnReturnItemToPool, OnDestroyItem,
                true, 5, 10);
        }
        private ArrowProjectile OnCreateItem()
        {
            GameObject arrow = Object.Instantiate(_arrowItem.ArrowLivePrefab, _arrowParent);

            var arrowProjectile = arrow.GetComponent<ArrowProjectile>();
            arrowProjectile.ArrowSpawner = this;
            return arrowProjectile;
        }

        private void OnTakeItemFromPool(ArrowProjectile arrow)
        {
            arrow.ThisTransform.localPosition = _arrowItem.ArrowInHandPosition.position;
            arrow.ThisTransform.localRotation = _arrowItem.ArrowInHandPosition.rotation;
            arrow.gameObject.SetActive(true);
        }

        private void OnReturnItemToPool(ArrowProjectile arrow)
        {
            arrow.ThisTransform.SetParent(_arrowParent);
            arrow.gameObject.SetActive(false);
        }

        private void OnDestroyItem(ArrowProjectile arrow)
        {
            Object.Destroy(arrow);
        }

        public ArrowProjectile GetArrow()
        {
            return _pool.Get();
        }

        public void ReleaseArrow(ArrowProjectile arrow)
        {
            _pool.Release(arrow);
        }
    }
}