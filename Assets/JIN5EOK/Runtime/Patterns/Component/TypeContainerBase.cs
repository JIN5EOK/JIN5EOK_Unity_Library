using System;
using System.Collections.Generic;
using System.Linq;

namespace Jin5eok.Patterns
{
    public abstract class TypeContainerBase<T> : ITypeContainer<T>
    {
        protected abstract Dictionary<Type, T> TypeMap { get; set; }
        public virtual int Count => TypeMap.Count;
        public virtual TKey Get<TKey>() where TKey : class, T
        {   
            TypeMap.TryGetValue(typeof(TKey), out var item);
            return item as TKey;
        }
        
        public virtual TKey GetInherited<TKey>() where TKey : T
        {
            foreach (var key in TypeMap.Values)
            {
                if (key is TKey typeKey)
                {
                    return typeKey;
                }
            }
            return default;
        }

        public virtual List<TKey> GetInheritedAll<TKey>() where TKey : T
        {
            var list = new List<TKey>();
            foreach (var value in TypeMap.Values)
            {
                if (value is TKey typeKey)
                {
                    list.Add(typeKey);
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
            return TypeMap.Values.ToArray();
        }
        
        public virtual bool Add(T item)
        {
            return TypeMap.TryAdd(item.GetType(), item);
        }

        public virtual TKey Add<TKey>() where TKey : class, T, new()
        {
            if (TypeMap.ContainsKey(typeof(TKey)) == false)
            {
                var addTarget = new TKey();
                TypeMap.Add(typeof(TKey), addTarget);
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
            return TypeMap.Remove(typeof(TKey));    
        }

        public virtual bool RemoveInherited<TKey>() where TKey : T
        {
            foreach (var value in TypeMap.Keys)
            {
                if (typeof(TKey).IsAssignableFrom(value))
                {
                    return TypeMap.Remove(value);
                }
            }
            return false;
        }
        
        public virtual int RemoveInheritedAll<TKey>() where TKey : T
        {
            var removeCount = 0;
            var keys = TypeMap.Keys.ToArray();

            for (int i = keys.Length - 1; i >= 0; --i)
            {
                if (typeof(TKey).IsAssignableFrom(keys[i]))
                {
                    removeCount += TypeMap.Remove(keys[i]) ? 1 : 0;
                }
            }
            return removeCount;
        }
        
        public virtual void Clear()
        {
            TypeMap.Clear();
        }

        public virtual bool Contains<TKey>() where TKey : T
        {
            return TypeMap.ContainsKey(typeof(TKey));
        }
    }
}