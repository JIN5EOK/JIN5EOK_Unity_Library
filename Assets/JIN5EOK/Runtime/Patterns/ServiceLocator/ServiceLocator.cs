using System;
using System.Collections.Generic;

namespace Jin5eok
{
    /// <summary>
    /// 서비스 로케이터 인스턴스 클래스입니다.
    /// 부모-자식 스코프를 지원합니다. (자식에서 못 찾으면 부모 체인에서 조회)
    /// </summary>
    public class ServiceLocator : IServiceLocator
    {
        private readonly Dictionary<Type, object> _services = new();

        public IServiceLocator Parent { get; }

        public ServiceLocator(IServiceLocator parent = null)
        {
            Parent = parent;
        }

        public IServiceLocator CreateChild()
        {
            return new ServiceLocator(this);
        }

        public void Register(Type serviceType, object instance, bool overwrite = false)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (!serviceType.IsInstanceOfType(instance))
            {
                throw new ArgumentException($"Instance type '{instance.GetType().FullName}' is not assignable to '{serviceType.FullName}'.", nameof(instance));
            }

            if (!overwrite && _services.ContainsKey(serviceType))
            {
                throw new InvalidOperationException($"Service already registered for type '{serviceType.FullName}'.");
            }

            _services[serviceType] = instance;
        }

        public void Register<T>(T instance, bool overwrite = false)
        {
            Register(typeof(T), instance, overwrite);
        }

        public bool Unregister(Type serviceType)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            return _services.Remove(serviceType);
        }

        public bool Unregister<T>()
        {
            return Unregister(typeof(T));
        }

        public bool TryGet(Type serviceType, out object service)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));

            if (_services.TryGetValue(serviceType, out service))
            {
                return true;
            }

            if (Parent != null && Parent.TryGet(serviceType, out service))
            {
                return true;
            }

            service = null;
            return false;
        }

        public bool TryGet<T>(out T service)
        {
            if (TryGet(typeof(T), out object obj) && obj is T typed)
            {
                service = typed;
                return true;
            }

            service = default;
            return false;
        }

        public object Get(Type serviceType)
        {
            TryGet(serviceType, out object service);
            return service;
        }

        public T Get<T>()
        {
            return TryGet<T>(out var service) ? service : default;
        }
    }
}
