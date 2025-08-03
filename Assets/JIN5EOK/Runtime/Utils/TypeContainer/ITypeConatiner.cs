using System.Collections.Generic;

namespace Jin5eok
{
    public interface ITypeContainer<T>
    {
        /// <returns> Count of TypeElements </returns>
        public int Count { get; }
        /// <summary> Get TypeElement by TKey </summary>
        public TKey Get<TKey>() where TKey : class, T;
        /// <summary> Get inherited TypeElement from TKey </summary>
        public TKey GetInherited<TKey>() where TKey : T;
        /// <summary> Get all TypeElements inherited from TKey </summary>
        public List<TKey> GetInheritedAll<TKey>() where TKey : T;
        /// <summary> Try to get TypeElement </summary>
        /// <returns> Is Succeed </returns>
        public bool TryGet<TKey>(out TKey result) where TKey : class, T;
        /// <summary> Get all TypeElements </summary>
        public T[] GetAll();
        /// <summary> Add an instance of T </summary>
        /// <returns> Is Succeed </returns>
        public bool Add(T item);
        /// <summary> Add TypeElement by TKey </summary>
        public TKey Add<TKey>() where TKey : class, T, new();
        /// <summary> If the Tkey exists, return it; otherwise, add a new key and return it. </summary>
        public TKey AddOrGet<TKey>() where TKey : class, T, new();
        /// <summary> Remove by TKey </summary>
        public bool Remove<TKey>() where TKey : class, T;
        /// <summary> Remove TypeElement inherited from TKey </summary>
        /// <returns> Is Succeed </returns>
        public bool RemoveInherited<TKey>() where TKey : T; 
        /// <summary> Remove all TypeElements inherited from TKey and return remove count </summary>
        /// <returns>Succed Count</returns>
        public int RemoveInheritedAll<TKey>() where TKey : T;
        /// <summary> Remove all collection </summary>
        public void Clear();
        /// <returns> TKey is contained </returns>
        public bool Contains<TKey>() where TKey : T;
    }
}