using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jin5eok.Patterns
{
    public abstract class ComponentContainerBase<T> : IComponentContainer<T>
    {
        protected abstract Dictionary<Type, T> ComponentsMap { get; set; }
        public virtual int Count => ComponentsMap.Count;
        public virtual TKey Get<TKey>() where TKey : class, T
        {
            ComponentsMap.TryGetValue(typeof(TKey), out var item);
            return item as TKey;
        }
        
        public virtual TKey GetInherited<TKey>() where TKey : T
        {
            foreach (var key in ComponentsMap.Values)
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
            foreach (var value in ComponentsMap.Values)
            {
                if (value is TKey component)
                {
                    list.Add(component);
                }
            }
            return list;
        }

        public virtual bool TryGet<TKey>(out TKey result) where TKey : class, T
        {
            result = Get<TKey>();
            return result != null;
        }

        public virtual T[] GetAll()
        {
            return ComponentsMap.Values.ToArray();
        }
        
        public virtual bool Add(T item)
        {
            return ComponentsMap.TryAdd(item.GetType(), item);
        }

        public virtual TKey Add<TKey>() where TKey : class, T, new()
        {
            if (ComponentsMap.ContainsKey(typeof(TKey)) == false)
            {
                var addTarget = new TKey();
                ComponentsMap.Add(typeof(TKey), addTarget);
                return addTarget;
            }
            return default;
        }

        public virtual TKey AddOrGet<TKey>() where TKey : class, T, new()
        {
            if (TryGet<TKey>(out var result))
            {
                return result;
            }
            return Add<TKey>();
        }
        
        public virtual bool Remove<TKey>() where TKey : class, T
        {
            return ComponentsMap.Remove(typeof(TKey));    
        }

        public virtual bool RemoveInherited<TKey>() where TKey : T
        {
            foreach (var componentType in ComponentsMap.Keys)
            {
                if (typeof(TKey).IsAssignableFrom(componentType))
                {
                    return ComponentsMap.Remove(componentType);
                }
            }
            return false;
        }
        
        public virtual int RemoveInheritedAll<TKey>() where TKey : T
        {
            var removeCount = 0;
            var keys = ComponentsMap.Keys.ToArray();

            for (int i = keys.Length - 1; i >= 0; --i)
            {
                if (typeof(TKey).IsAssignableFrom(keys[i]))
                {
                    removeCount += ComponentsMap.Remove(keys[i]) ? 1 : 0;
                }
            }
            return removeCount;
        }
        
        public virtual void Clear()
        {
            ComponentsMap.Clear();
        }

        public virtual bool Contains<TKey>() where TKey : T
        {
            return ComponentsMap.ContainsKey(typeof(TKey));
        }
    }
}