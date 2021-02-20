using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Isao
{
// List<T>
    [Serializable]
    public class Serialization<T>
    {
        [SerializeField] private List<T> target;

        public List<T> ToList()
        {
            return this.target;
        }

        public Serialization(List<T> target)
        {
            this.target = target;
        }
    }

// Dictionary<TKey, TValue>
    [Serializable]
    public class Serialization<TKey, TValue> : ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> keys;
        [SerializeField] private List<TValue> values;

        private Dictionary<TKey, TValue> target;

        public Dictionary<TKey, TValue> ToDictionary()
        {
            return this.target;
        }

        public Serialization(Dictionary<TKey, TValue> target)
        {
            this.target = target;
        }

        public void OnBeforeSerialize()
        {
            this.keys = new List<TKey>(this.target.Keys);
            this.values = new List<TValue>(this.target.Values);
        }

        public void OnAfterDeserialize()
        {
            var count = Math.Min(this.keys.Count, this.values.Count);
            this.target = new Dictionary<TKey, TValue>(count);
            for (var i = 0; i < count; ++i)
            {
                this.target.Add(this.keys[i], this.values[i]);
            }
        }
    }
}