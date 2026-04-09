using System;

namespace Jin5eok
{
    /// <summary>
    /// 서비스 로케이터 인터페이스입니다.
    /// 전역(static) 또는 싱글턴을 강제하지 않으며, 필요하다면 사용자 측에서 별도로 소유/보관하면 됩니다.
    /// </summary>
    public interface IServiceLocator
    {
        /// <summary>
        /// 부모 로케이터입니다. 부모가 없으면 null입니다.
        /// </summary>
        public IServiceLocator Parent { get; }

        /// <summary>
        /// 자식 스코프(로케이터)를 생성합니다.
        /// </summary>
        public IServiceLocator CreateChild();

        /// <summary>
        /// 타입을 키로 인스턴스를 등록합니다.
        /// (옵션 A: 인스턴스 등록/조회만 지원)
        /// </summary>
        public void Register(Type serviceType, object instance, bool overwrite = false);

        /// <summary>
        /// 제네릭 타입을 키로 인스턴스를 등록합니다.
        /// </summary>
        public void Register<T>(T instance, bool overwrite = false);

        /// <summary>
        /// 등록 해제합니다. 현재 로케이터 스코프에서만 제거됩니다.
        /// </summary>
        public bool Unregister(Type serviceType);

        /// <summary>
        /// 등록 해제합니다. 현재 로케이터 스코프에서만 제거됩니다.
        /// </summary>
        bool Unregister<T>();

        /// <summary>
        /// 현재 로케이터 또는 부모 체인에서 조회합니다.
        /// </summary>
        public bool TryGet(Type serviceType, out object service);

        /// <summary>
        /// 현재 로케이터 또는 부모 체인에서 조회합니다.
        /// </summary>
        public bool TryGet<T>(out T service);

        /// <summary>
        /// 현재 로케이터 또는 부모 체인에서 조회합니다. 없으면 null을 반환합니다.
        /// </summary>
        public object Get(Type serviceType);

        /// <summary>
        /// 현재 로케이터 또는 부모 체인에서 조회합니다. 없으면 default(T)를 반환합니다.
        /// </summary>
        public T Get<T>();
    }
}
