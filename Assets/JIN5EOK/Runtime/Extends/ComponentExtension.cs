using UnityEngine;

namespace Jin5eok
{
    /// <summary>
    /// Component 관련 확장 메서드를 제공하는 클래스입니다.
    /// </summary>
    public static class ComponentExtension
    {
        /// <summary>
        /// 컴포넌트가 있으면 가져오고, 없으면 추가합니다.
        /// </summary>
        /// <typeparam name="T">컴포넌트 타입</typeparam>
        /// <param name="gameObject">대상 GameObject</param>
        /// <returns>컴포넌트 인스턴스</returns>
        public static T AddOrGetComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }
    }    
}

