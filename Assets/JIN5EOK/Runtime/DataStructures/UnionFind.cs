using System;
using System.Collections.Generic;

namespace Jin5eok
{
    /// <summary>
    /// Union-Find 자료구조 (Disjoint Set Union)
    /// </summary>
    /// <typeparam name="T">집합 요소의 타입</typeparam>
    public class UnionFind<T> where T : IEquatable<T>
    {
        private Dictionary<T, T> _parent = new Dictionary<T, T>();
        private Dictionary<T, int> _rank = new Dictionary<T, int>();
        private readonly IEqualityComparer<T> _comparer;

        /// <summary>
        /// 현재 관리 중인 집합의 개수를 반환합니다.
        /// </summary>
        public int GroupCount
        {
            get
            {
                int count = 0;
                foreach (var kvp in _parent)
                {
                    if (_comparer.Equals(kvp.Key, kvp.Value))
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        /// <summary>
        /// UnionFind 인스턴스를 생성합니다.
        /// </summary>
        /// <param name="comparer">요소 비교를 위한 EqualityComparer (null이면 기본 비교자 사용)</param>
        public UnionFind(IEqualityComparer<T> comparer = null)
        {
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }

        /// <summary>
        /// 요소를 초기화합니다.
        /// </summary>
        /// <param name="x">초기화할 요소</param>
        public void MakeSet(T x)
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (!_parent.ContainsKey(x))
            {
                _parent[x] = x;
                _rank[x] = 0;
            }
        }

        /// <summary>
        /// 요소의 루트를 찾습니다 (경로 압축 사용).
        /// </summary>
        /// <param name="x">루트를 찾을 요소</param>
        /// <returns>요소가 속한 집합의 루트</returns>
        public T Find(T x)
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (!_parent.ContainsKey(x))
            {
                MakeSet(x);
                return x;
            }

            if (!_comparer.Equals(_parent[x], x))
            {
                _parent[x] = Find(_parent[x]); // 경로 압축
            }

            return _parent[x];
        }

        /// <summary>
        /// 두 요소가 같은 집합에 속하는지 확인합니다.
        /// </summary>
        /// <param name="x">첫 번째 요소</param>
        /// <param name="y">두 번째 요소</param>
        /// <returns>같은 집합에 속하면 true, 아니면 false</returns>
        public bool Connected(T x, T y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return _comparer.Equals(Find(x), Find(y));
        }

        /// <summary>
        /// 두 요소를 합칩니다 (Union by Rank 사용).
        /// </summary>
        /// <param name="x">첫 번째 요소</param>
        /// <param name="y">두 번째 요소</param>
        public void Union(T x, T y)
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            T rootX = Find(x);
            T rootY = Find(y);

            if (_comparer.Equals(rootX, rootY))
            {
                return; // 이미 같은 집합
            }

            // 랭크가 높은 쪽으로 편입
            if (_rank[rootX] < _rank[rootY])
            {
                _parent[rootX] = rootY;
            }
            else if (_rank[rootX] > _rank[rootY])
            {
                _parent[rootY] = rootX;
            }
            else
            {
                _parent[rootY] = rootX;
                _rank[rootX]++;
            }
        }

        /// <summary>
        /// 모든 집합을 초기화합니다.
        /// </summary>
        public void Clear()
        {
            _parent.Clear();
            _rank.Clear();
        }
    }
}

