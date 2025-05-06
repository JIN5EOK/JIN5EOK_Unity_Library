# 개별 InputHandler


>## 1. ButtonInput
* bool 형식의 버튼 입력 값을 반환합니다.

> ### public abstract class ButtonInputHandlerBase : InputHandler\<bool>
* 베이스 클래스

> ### public class ButtonInputHandlerKeyCode : ButtonInputHandlerBase
* ```KeyCode```를 기반으로 Button 입력을 수행합니다.

### ```public KeyCode KeyCode { get; set; }```
* 키 입력에 사용할 ```KeyCode```

> ### public class ButtonInputHandlerOldInputSystem : ButtonInputHandlerBase
* ```OldInputSystem```을 기반으로 Button 입력을 수행합니다.

### ```public string ButtonName { get; set; }```
* 키 입력에 사용할 ```OldInputSystem``` 버튼 이름

----


>## 2. AxisInput
* float 형식의 입력 값을 반환합니다.

> ### public abstract class AxisInputHandlerBase : InputHandler\<float>
* 베이스 클래스

> ### public class AxisInputHandlerKeyCode : AxisInputHandlerBase
* ```KeyCode```를 기반으로 AxisRaw 키 입력을 수행합니다.

### ```public KeyCode PositiveKeyCode { get; set; }```
### ```public KeyCode NegativeKeyCode { get; set; }```
* 키 입력에 사용할 양수, 음수에 해당하는 ```KeyCode```

> ### public class AxisInputHandlerOldInputSystem : AxisInputHandlerBase
* ```OldInputSystem```의 Axis 입력을 기반으로 키 입력을 수행합니다.

### ```public string AxisName { get; set; }```
* 키 입력에 사용할 ```OldInputSystem```의 Axis 이름
### ```public bool IsUsingAxisRaw { get; set; }```
* Input을 AxisRaw 형태로 받을지 Axis형태로 받을지 여부

---

> ## 3.Vector2Input
* Vector2 형식의 입력 값을 반환합니다.

> ### public abstract class Vector2InputHandlerBase : InputHandler\<Vector2>
* 베이스 클래스

> ### public class Vector2InputHandlerKeyCode : Vector2InputHandlerBase
* ```KeyCode```를 기반으로 Vector2 입력을 수행합니다.

### ```public KeyCode UpKeyCode { get; set; }```
### ```public KeyCode DownKeyCode { get; set; }```
### ```public KeyCode LeftKeyCode { get; set; }```
### ```public KeyCode RightKeyCode { get; set; }```
* 상,하,좌,우 입력에 사용할 ```KeyCode```

> ### public class Vector2InputHandlerOldInputSystem : Vector2InputHandlerBase
* ```OldInputSystem```을 기반으로 Vector2 입력을 수행합니다.

### ```public string AxisNameX```
### ```public string AxisNameY```
* X,Y 좌표 입력에 사용할 ```OldInputSystem``` Axis 이름

### ```public bool IsUsingAxisRaw```
* AxisRaw를 사용할지 Axis를 사용할지 여부

---

> ## 4.MouseInput
* 마우스 입력 InputHandler

> ### public class MousePositionInputHandler : InputHandler\<Vector3>
* Vector3 형식의 마우스 위치 값을 입력 값으로 반환합니다.


