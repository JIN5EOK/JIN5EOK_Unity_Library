using System;
using System.Collections.Generic;
using System.Reflection;

namespace Jin5eok.Helper
{
    public class ReflectionHelper
    {
        public static List<Type> GetSubclasses<T>()
        {
            Type baseType = typeof(T);
            List<Type> subclasses = new List<Type>();
            
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsClass && !type.IsAbstract && baseType.IsAssignableFrom(type))
                    {
                        subclasses.Add(type);
                    }
                }
            }
            return subclasses;
        }
    }
}