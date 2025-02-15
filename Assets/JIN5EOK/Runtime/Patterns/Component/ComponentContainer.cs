using System;
using System.Collections.Generic;
using System.Linq;

namespace Jin5eok.Patterns.Component
{
    [Serializable]
    public class ComponentContainer<T>
    {
        protected Dictionary<Type, T> _componentsMap = new ();

        public virtual int Count => _componentsMap.Count;
        
        public virtual TKey Get<TKey>() where TKey : class, T
        {
            _componentsMap.TryGetValue(typeof(TKey), out var item);
            return item as TKey;
        }
        
        public virtual TKey GetInherited<TKey>() where TKey : T
        {
            foreach (var key in _componentsMap.Values)
            {
                if (key is TKey component)
                {
                    return component;
                }
            }
            return default;
        }

        public virtual List<TKey> GetInheritedAll<TKey>() where TKey : T
        {
            var list = new List<TKey>();
            foreach (var value in _componentsMap.Values)
            {
                if (value is TKey component)
                {
                    list.Add(component);
                }
            }
            return list;
        }

        public virtual bool Get<TKey>(out TKey result) where TKey : class, T
        {
            result = Get<TKey>();
            return result != null;
        }

        public virtual T[] GetAll()
        {
            return _componentsMap.Values.ToArray();
        }
        
        public virtual bool Add(T item)
        {
            return _componentsMap.TryAdd(item.GetType(), item);
        }

        public virtual bool Remove<TKey>() where TKey : class, T
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