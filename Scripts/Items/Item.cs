using Settings;
using UnityEngine;

namespace Items
{
    public abstract class Item : ScriptableObject
    {
        [field: SerializeField][field: ReadOnly] public int ItemID { get; private set; } = 1;
        [field: SerializeField] public string ItemName { get; protected set; }
        [field: TextArea(15,15)]
        [field: SerializeField] public string Description { get; protected set; }
        [field: SerializeField] public Sprite ItemIcon { get; protected set; }
        [field: SerializeField] public GameObject ItemPrefab { get; protected set; }
        [field: SerializeField] public ItemType ItemType { get; protected set; }
        [field: SerializeField] public bool IsRemovable { get; protected set; } = true;

        private static int _currentMaxInstanceId = 1;


        protected Item()
        {
            ItemID = _currentMaxInstanceId;
            _currentMaxInstanceId += 1;
        }
    }
    
    
}