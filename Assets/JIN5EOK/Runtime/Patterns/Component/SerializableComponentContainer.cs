using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Jin5eok.Patterns
{
    [Serializable]
    public class SerializableComponentContainer<T> : ComponentContainerBase<T> where T : SerializableComponent
    {
        [SerializeReference] private SerializableComponentWrapper serializableComponentWrapper = new();
        private Dictionary<Type, T> _componentMap;
        protected override Dictionary<Type, T> ComponentsMap
        {
            get
            {
                if (_componentMap == null)
                {
                    _componentMap = new Dictionary<Type, T>();
                    foreach (var data in serializableComponentWrapper.Components)
                    {
                        _componentMap.Add(data.GetType(), data as T);
                    }
                }
                return _componentMap;
            }
            set => _componentMap = value;
        }
    }
}