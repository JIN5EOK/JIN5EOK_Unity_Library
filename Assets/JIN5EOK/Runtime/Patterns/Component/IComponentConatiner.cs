using System.Collections.Generic;

namespace Jin5eok.Patterns
{
    public interface IComponentContainer<T>
    {
        /// <returns> Count of Components </returns>
        public int Count { get; }
        /// <summary> Get Component by TKey </summary>
        public TKey Get<TKey>() where TKey : class, T;
        /// <summary> Get inherited Component from TKey </summary>
        public TKey GetInherited<TKey>() where TKey : T;
        /// <summary> Get all Components inherited from TKey </summary>
        public List<TKey> GetInheritedAll<TKey>() where TKey : T;
        /// <summary> Try to get Component </summary>
        /// <returns> Is Succeed </returns>
        public bool TryGet<TKey>(out TKey result) where TKey : class, T;
        /// <summary> Get all Components </summary>
        public T[] GetAll();
        /// <summary> Add an instance of T </summary>
        /// <returns> Is Succeed </returns>
        public bool Add(T item);
        /// <summary> Add component by TKey </summary>
        public TKey Add<TKey>() where TKey : class, T, new();
        /// <summary> If TKey is contained, GetComponent otherwise AddComponent </summary>
        public TKey AddOrGet<TKey>() where TKey : class, T, new();
        /// <summary> Remove Component by TKey </summary>
        public bool Remove<TKey>() where TKey : class, T;
        /// <summary> Remove Component inherited from TKey </summary>
        /// <returns> Is Succeed </returns>
        public bool RemoveInherited<TKey>() where TKey : T; 
        /// <summary> Remove all Components inherited from TKey and return remove count </summary>
        /// <returns>Succed Count</returns>
        public int RemoveInheritedAll<TKey>() where TKey : T;
        /// <summary> Remove all collection </summary>
        public void Clear();
        /// <returns> TKey is contained </returns>
        public bool Contains<TKey>() where TKey : T;
    }
}