# InputHandlers 사용자 가이드

* Keycode와 OldInputSystem을 래핑하여 입력 기능 구현을 돕는 클래스입니다. 할당 키와 조합을 런타임 도중 편집이 가능합니다.
* 이벤트 기반으로 키 입력을 수행할 수 있습니다.
* 한가지 액션에 여러가지 키를 할당할 수 있습니다.
* 키 리바인딩을 지원합니다.
* 일반적인 키보드 마우스, 게임패드 입력 처리와 키 리바인딩 처리는 가능하지만 이 이상의 기능이 필요하다면 더 많은 입력기기와 기능을 지원하는 유니티의 공식 패키지 `Input System`을 사용해보세요.

## 기본 버튼 입력

스페이스바를 이용해 점프 이벤트를 등록하는 예시입니다.

```csharp
using Jin5eok;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private ButtonInputHandlerKeyCode _jumpInput;
    
    private void Start()
    {
        // Space 키로 점프 입력 생성
        _jumpInput = new ButtonInputHandlerKeyCode(KeyCode.Space);
        
        // 입력값 변경 이벤트 구독
        _jumpInput.InputValueChanged += OnJumpInput;
    }
    
    private void OnJumpInput(bool isPressed)
    {
        if (isPressed)
        {
            Debug.Log("점프!");
            // 점프 로직...
        }
    }
    
    private void OnDestroy()
    {
        _jumpInput?.Dispose();
    }
}
```

## 입력값 직접 확인

이벤트 대신 직접 값을 확인할 수도 있습니다:

```csharp
private void Update()
{
    if (_jumpInput.Value)
    {
        // 점프 로직...
    }
}
```

## KeyCode와 Old Input System 선택 가능

InputHandler는 KeyCode 기반과 Old Input System 기반 두 가지 방식을 모두 지원합니다.

### KeyCode 방식

```csharp
public class KeyCodeInputExample : MonoBehaviour
{
    private ButtonInputHandlerKeyCode _fireInput;
    
    private void Start()
    {
        // KeyCode로 직접 지정
        _fireInput = new ButtonInputHandlerKeyCode(KeyCode.Space);
        _fireInput.InputValueChanged += OnFire;
    }
    
    private void OnFire(bool isPressed)
    {
        if (isPressed) Debug.Log("발사!");
    }
    
    private void OnDestroy()
    {
        _fireInput?.Dispose();
    }
}
```

### Old Input System 방식

```csharp
public class OldInputSystemExample : MonoBehaviour
{
    private AxisInputHandlerOldInputSystem _horizontalInput;
    
    private void Start()
    {
        // Input Manager에 설정된 이름 사용
        _horizontalInput = new AxisInputHandlerOldInputSystem("Horizontal", isUsingAxisRaw: false);
        
        _horizontalInput.InputValueChanged += OnHorizontalInput;
    }
    
    private void OnHorizontalInput(float horizontal)
    {
        transform.position += Vector3.right * horizontal * Time.deltaTime * 5f;
    }
    
    private void OnDestroy()
    {
        _horizontalInput?.Dispose();
    }
}
```


## 이동 입력 (Vector2)

WASD 키로 이동하는 예제

```csharp
public class PlayerMovement : MonoBehaviour
{
    private Vector2InputHandlerKeyCode _moveInput;
    
    private void Start()
    {
        // WASD 키로 이동 입력 생성
        _moveInput = new Vector2InputHandlerKeyCode(
            KeyCode.W,  // Up
            KeyCode.S,  // Down
            KeyCode.A,  // Left
            KeyCode.D   // Right
        );
        
        _moveInput.InputValueChanged += OnMoveInput;
    }
    
    private void OnMoveInput(Vector2 direction)
    {
        // 이동 처리
        transform.position += (Vector3)direction * Time.deltaTime * 5f;
    }
    
    private void OnDestroy()
    {
        _moveInput?.Dispose();
    }
}
```

## 축 입력 (Axis)

좌우 이동을 축 입력으로 처리
```csharp
public class HorizontalMovement : MonoBehaviour
{
    private AxisInputHandlerKeyCode _horizontalInput;
    
    private void Start()
    {
        // A/D 키로 좌우 이동
        _horizontalInput = new AxisInputHandlerKeyCode(
            KeyCode.D,  // Positive (오른쪽)
            KeyCode.A   // Negative (왼쪽)
        );
    }
    
    private void Update()
    {
        float horizontal = _horizontalInput.Value; // -1 ~ 1
        transform.position += Vector3.right * horizontal * Time.deltaTime * 5f;
    }
    
    private void OnDestroy()
    {
        _horizontalInput?.Dispose();
    }
}
```

## 키 리바인딩

런타임에 키를 변경할 수 있습니다

```csharp
public class RebindableInput : MonoBehaviour
{
    private ButtonInputHandlerKeyCode _attackInput;
    
    private void Start()
    {
        _attackInput = new ButtonInputHandlerKeyCode(KeyCode.Space);
    }
    
    public void RebindAttackKey(KeyCode newKey)
    {
        _attackInput.KeyCode = newKey; // 키 변경
    }
    
    private void OnDestroy()
    {
        _attackInput?.Dispose();
    }
}
```

## CompositeInputHandler - 여러 입력 수단을 조합하여 사용

여러 InputHandler를 조합해 한 가지 액션에 여러 입력 방법을 할당할 수 있습니다.

```csharp
public class CompositeInputExample : MonoBehaviour
{
    private BoolCompositeInputHandler _attackInput;
    
    private void Start()
    {
        // Space 키 또는 Old Input System의 "Fire1" 버튼으로 공격
        var keyCodeInput = new ButtonInputHandlerKeyCode(KeyCode.Space);
        var oldInputSystem = new ButtonInputHandlerOldInputSystem("Fire1");
        
        _attackInput = new BoolCompositeInputHandler(keyCodeInput, oldInputSystem);
        
        _attackInput.InputValueChanged += OnAttack;
    }
    
    private void OnAttack(bool isPressed)
    {
        if (isPressed)
        {
            Debug.Log("공격!");
        }
    }
    
    private void OnDestroy()
    {
        _attackInput?.Dispose();
    }
}
```

### 마우스 위치 추적

마우스 위치를 추적하는 예제

```csharp
public class MouseTracker : MonoBehaviour
{
    private MousePositionInputHandler _mouseInput;
    
    private void Start()
    {
        _mouseInput = new MousePositionInputHandler();
        _mouseInput.InputValueChanged += OnMouseMove;
    }
    
    private void OnMouseMove(Vector3 mousePosition)
    {
        // 마우스 위치를 월드 좌표로 변환
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = worldPos;
    }
    
    private void OnDestroy()
    {
        _mouseInput?.Dispose();
    }
}
```

## 주의사항

⚠️ **반드시 Dispose 호출**
- InputHandler는 IDisposable을 구현합니다.
- 사용이 끝나면 반드시 Dispose()를 호출하세요.
- Dispose를 호출하지 않으면 메모리 누수가 발생할 수 있습니다.

⚠️ **CompositeInputHandler의 생명주기**
- CompositeInputHandler에 포함된 InputHandler는 CompositeInputHandler와 생명주기를 함께합니다.
- CompositeInputHandler를 Dispose하면 내부 InputHandler들도 자동으로 Dispose됩니다.
- 따라서 이 경우 개별 InputHandler를 독립적으로 Dispose하지 않는걸 권장합니다.