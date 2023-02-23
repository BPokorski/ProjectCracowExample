using System;
using System.Collections.Generic;
using Character;
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
        [field: SerializeField] public float Weight { get; protected set; }
        [field: SerializeField] public ItemType ItemType { get; protected set; }
        // [field: SerializeField] public bool IsStockable { get; protected set; } = true;
        [field: SerializeField] public bool IsRemovable { get; protected set; } = true;

        private static int _currentMaxInstanceId = 1;


        protected Item()
        {
            ItemID = _currentMaxInstanceId;
            _currentMaxInstanceId += 1;
        }
        
        public class EqualityComparer : IEqualityComparer<Item>
        {
            public bool Equals(Item x, Item y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.ItemName == y.ItemName && x.ItemType == y.ItemType;
            }

            public int GetHashCode(Item obj)
            {
                return HashCode.Combine(obj.ItemName, (int) obj.ItemType);
            }
        }

    }
    
    
}