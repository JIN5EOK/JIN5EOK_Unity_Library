using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Jin5eok.Patterns.Component
{
    [Serializable]
    public class CompositeComponent<T> : IComponent where T : class, IComponent
    {
        private Dictionary<Type, T> _componentsMap = new ();

        public virtual int Count => _componentsMap.Count;
        
        [CanBeNull]
        public virtual TElement Get<TElement>() where TElement : IComponent
        {
            _componentsMap.TryGetValue(typeof(T), out var status);
            return status is TElement value ? value : default;
        }
        
        public virtual bool Get<TElement>([CanBeNull] out TElement result) where TElement : IComponent
        {
            result = Get<TElement>();
            return result != null;
        }

        public virtual T[] GetAll()
        {
            return _componentsMap.Values.ToArray();
        }
        
        public virtual bool Add(T status)
        {
            return _componentsMap.TryAdd(status.GetType(), status);
        }
        
        public virtual bool Remove(Type type)
        {
            return _componentsMap.Remove(type);
        }

        public virtual void Clear()
        {
            _componentsMap.Clear();
        }

        public virtual bool Contains(T item)
        {
            return item != null && _componentsMap.ContainsKey(item.GetType());
        }
        
        public virtual void CopyFrom(IComponent source)
        {
            if (source is CompositeComponent<T> composite)
            {
                var fromComponents = composite.GetAll();
                foreach (var fromComponent in fromComponents)
                {
                    if (_componentsMap.TryGetValue(fromComponent.GetType(), out var thisComponent))
                    {
                        thisComponent.CopyFrom(fromComponent);
                    }
                }    
            }
            else if (_componentsMap.TryGetValue(source.GetType(), out var thisComponent))
            {
                thisComponent.CopyFrom(source);
            }
        }
        
        public virtual IComponent GetDefault() => null;
    }
}