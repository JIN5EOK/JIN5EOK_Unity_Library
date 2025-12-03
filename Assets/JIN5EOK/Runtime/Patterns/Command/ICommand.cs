namespace Jin5eok
{
    /// <summary>
    /// 커맨드 패턴을 위한 인터페이스입니다.
    /// 특정 타입의 타겟에 대해 실행할 수 있는 명령을 정의합니다.
    /// </summary>
    /// <typeparam name="T">명령을 실행할 타겟의 타입</typeparam>
    public interface ICommand<T>
    {
        /// <summary>
        /// 타겟에 대해 명령을 실행합니다.
        /// </summary>
        /// <param name="target">명령을 실행할 타겟</param>
        public void Execute(T target);
    }
}