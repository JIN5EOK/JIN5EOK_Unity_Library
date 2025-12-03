using UnityEngine;

namespace Jin5eok
{
    /// <summary>
    /// ScriptableObject로 구현된 커맨드 패턴의 추상 클래스입니다.
    /// 에디터에서 설정 가능한 커맨드를 만들 때 사용합니다.
    /// </summary>
    /// <typeparam name="T">명령을 실행할 타겟의 타입</typeparam>
    public abstract class ScriptableCommand<T> : ScriptableObject, ICommand<T>
    {
        /// <summary>
        /// 타겟에 대해 명령을 실행합니다.
        /// </summary>
        /// <param name="target">명령을 실행할 타겟</param>
        public abstract void Execute(T target);
    }
}