# 개요
* ```Keycode```와 ```OldInputSystem```을 랩핑하여 이벤트 기반으로 키 입력을 수행하거나 키 리바인딩을 도와주는 클래스들입니다.

# InputHandler
* ```KeyCode``` 혹은 ```OldInputSystem의``` 어떤 키에 대한 입력을 핸들링할지 설정합니다.
* 설정한 키의 입력 상태를 얻을 수 있습니다.
* 입력 상태 변화에 따른 콜백 이벤트를 등록할 수 있습니다.
* 설정한 키는 언제든지 리바인딩할 수 있습니다.


## [InputHandlerBase](InputHandlerBase_KR.md)
* ```InputHandler```들이 공통적으로 상속받는 기본적인 인터페이스와 클래스들입니다.

## [InputHandlers](InputHandlers_KR.md)
* Axis : float
* Button : bool
* Vector2 : Vector2
* Mouse : Vector3
* 위와 같은 입력에 대한 InputHandler들입니다.

## [CompositeInputHandler](CompositeInputHandler_KR.md)
* 값의 형식이 동일한 입력 핸들러 여러개를 합성한 복합 ```InputHandler```를 구성합니다.
* 한가지 입력에 여러가지 입력수단을 할당하고 싶을 때 사용합니다.
  * 예) 공격 명령에 KeyCode 입력값 Space Bar 키, Old InputSystem의 Attack 입력을 할당합니다.

## InputHandlerUpdater
  * ```InputHandler```를 Update 주기에 맞춰 자동으로 업데이트 해줍니다.
  * ```InputHandlerUpdater```는 DontDestroyOnLoad 싱글턴 게임오브젝트로 ```InputHandler```를 생성하면 씬에 자동 생성됩니다.
  * ```InputHandlerUpdater```는 internal class로 선언되었으며 사용자는 이에 접근할 필요 없습니다.
    * 각 ```InputHandler``` 생성시 ```InputHandlerUpdater``` 의 컬렉션에 등록되어 자동으로 업데이트가 수행됩니다.
    * ```InputHandler```의 Dispose 호출시 컬렉션에서 해제됩니다.