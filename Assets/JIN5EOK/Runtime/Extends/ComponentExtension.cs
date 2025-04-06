using System.Collections.Generic;
using UnityEngine;

namespace Jin5eok.Extension
{
    public static class ComponentExtension
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

