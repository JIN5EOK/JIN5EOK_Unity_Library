using UnityEngine;

namespace Jin5eok.Extends
{
    public static class ComponentExtends
    {
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

