# 복합 InputHandler
* 같은 반환형을 가진 입력 핸들러 여러개가 합성된 복합 입력 핸들러를 구성합니다.
* 한가지 입력에 여러가지 입력수단을 할당하고 싶을 때 사용합니다.
    * 예) 공격 명령에 ```KeyCode``` 입력값 Space Bar 키, Old InputSystem의 Attack 입력을 할당합니다.

> ### public abstract class CompositeInputHandlerBase\<T> : InputHandler\<T> where T : notnull
* 베이스 클래스
### ```public IInputHandler<T>[] GetInputs()```
* CompositeInput 내부에 포함된 InputHandler들을 반환합니다.

### ```public void AddInput(IInputHandle<T> inputHandler)```
* InputHandler를 추가합니다.

### ```public void RemoveInput(IInputHandler<T> inputHandler)```
* InputHandler를 제거합니다.

### ```public void RemoveAllInputs()```
* InputHandler를 모두 제거합니다.

> ### public class IntCompositeInputHandler : CompositeInputHandlerBase\<int>

> ### public class FloatCompositeInputHandler : CompositeInputHandlerBase\<float>

> ### public class BoolCompositeInputHandler : CompositeInputHandlerBase\<bool>

> ### public class Vector2CompositeInputHandler : CompositeInputHandlerBase\<Vector2>

> ### public class Vector3CompositeInputHandler : CompositeInputHandlerBase\<Vector3>
* 각 타입 별 CompositeInputHandler

## 주의사항
* ```CompositeInputHandler```에 포함된 개별 ```InputHandler```를 포함시키면 해당 ```InputHandler```는 ```CompositeInputHandler```와 생명주기를 함께합니다. 따라서 요소로 넣은 핸들러를 독립적으로 다룰수는 있으나 권장하지는 않습니다. 
