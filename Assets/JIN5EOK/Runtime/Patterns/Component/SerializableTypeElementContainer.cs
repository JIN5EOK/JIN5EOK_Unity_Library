using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Jin5eok.Patterns
{
    [Serializable]
    public class SerializableTypeContainer<T> : TypeContainerBase<T> where T : SerializableTypeElement
    {
        [SerializeReference] private SerializableTypeElementWrapper serializableTypeElementWrapper = new();
        private Dictionary<Type, T> _typeMap;
        protected override Dictionary<Type, T> TypeMap
        {
            get
            {
                if (_typeMap == null)
                {
                    _typeMap = new Dictionary<Type, T>();
                    foreach (var data in serializableTypeElementWrapper.TypeElements)
                    {
                        _typeMap.Add(data.GetType(), data as T);
                    }
                }
                return _typeMap;
            }
            set => _typeMap = value;
        }
    }
}