using System;
using System.Collections.Generic;
using System.Linq;

namespace Jin5eok.Patterns.Component
{
    [Serializable]
    public class CompositeComponent<T> : IComponent where T : class, IComponent
    {
        protected Dictionary<Type, T> _componentsMap = new ();

        public virtual int Count => _componentsMap.Count;
        
        public virtual TElement Get<TElement>() where TElement : IComponent
        {
            _componentsMap.TryGetValue(typeof(T), out var status);
            return status is TElement value ? value : default;
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
        
        public virtual bool Add<TKey>(T status) where TKey : T
        {
            return _componentsMap.TryAdd(typeof(TKey), status);
        }
        
        public virtual bool Remove(Type type)
        {
            return _componentsMap.Remove(type);
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