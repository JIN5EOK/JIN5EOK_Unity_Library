## InputHandler 인터페이스 / 추상클래스
> ### public interface IInputHandlerBase : IDisposable
> * InputHandler\<T>들의 일괄적인 Update, Dispose 수행을 위해 상속받는 인터페이스 입니다.

> ### public interface IInputHandler\<T> : IInputHandlerBase where T : notnull
> * InputHandler\<T>들에 입력 Value관련 기능을 상속하기 위한 인터페이스 입니다.

> ### public abstract class InputHandler<T> : IInputHandler<T> where T : notnull
> * 모든 InputHandler들이 상속받는 베이스 클래스입니다.
> * IInputHandlerBase, InputHandlerBase를 상속받습니다.

### ```public event InputCallback<T> InputValueChanged;```
* 입력값이 변경될 때 실행될 콜백 이벤트입니다.
* IInputHandler\<T>로부터 상속됩니다.

### ```public T Value { get; }```
* 현재 입력된 키 입력값을 반환합니다.
* IInputHandler\<T>로부터 상속됩니다.

### ```public void UpdateState();```
* ```InputHandlerUpdater``` 에 의해 매 프레임 호출되며 호출시 입력 값을 반영하는 업데이트를 수행합니다.
* 일반적으로는 사용자가 직접 호출할 필요 없습니다.
* IInputHandlerBase부터 상속되는 함수입니다.

### ```public virtual void Dispose()```
* ```InputHandlerUpdater```에 할당된 콜백을 지워 자동으로 업데이트 되지 않도록 합니다.
* IInputHandlerBase부터 상속되는 함수입니다.