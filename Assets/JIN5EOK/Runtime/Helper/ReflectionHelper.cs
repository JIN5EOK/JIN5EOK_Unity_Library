using System;
using System.Collections.Generic;
using System.Reflection;

namespace Jin5eok
{
    /// <summary>
    /// 리플렉션을 사용한 타입 검색을 위한 헬퍼 클래스입니다.
    /// </summary>
    public class ReflectionHelper
    {
        /// <summary>
        /// 지정된 타입의 모든 비추상 서브클래스를 반환합니다.
        /// </summary>
        /// <typeparam name="T">기준이 되는 베이스 타입</typeparam>
        /// <returns>베이스 타입을 상속받는 모든 비추상 클래스의 리스트</returns>
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