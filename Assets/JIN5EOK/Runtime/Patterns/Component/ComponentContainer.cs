using System;
using System.Collections.Generic;
using System.Linq;

namespace Jin5eok.Patterns.Component
{
    [Serializable]
    public class ComponentContainer<T> where T : class, IComponent
    {
        protected Dictionary<Type, T> _componentsMap = new ();

        public virtual int Count => _componentsMap.Count;
        
        public virtual TElement Get<TElement>() where TElement : IComponent
        {
            _componentsMap.TryGetValue(typeof(TElement), out var item);
            return item is TElement value ? value : default;
        }
        
        public virtual bool Get<TElement>(out TElement result) where TElement : IComponent
        {
            result = Get<TElement>();
            return result != null;
        }

        public virtual T[] GetAll()
        {
            return _componentsMap.Values.ToArray();
        }
        
        public virtual bool Add<TKey>(T item) where TKey : T
        {
            return _componentsMap.TryAdd(typeof(TKey), item);
        }
        
        public virtual bool Remove<TKey>() where TKey : T
        {
            return _componentsMap.Remove(typeof(TKey));    
        }

        public virtual bool RemoveInherited<TKey>() where TKey : T
        {
            foreach (var componentType in _componentsMap.Keys)
            {
                if (typeof(TKey).IsAssignableFrom(componentType))
                {
                    return _componentsMap.Remove(typeof(TKey));
                }
            }
            return false;
        }
        
        public virtual void Clear()
        {
            _componentsMap.Clear();
        }

        public virtual bool Contains<TKey>() where TKey : T
        {
            return _componentsMap.ContainsKey(typeof(TKey));
        }
    }
}