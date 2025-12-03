# AddressablesScope 사용자 가이드

* 어드레서블로 로드한 에셋 / 프리펩 인스턴스의 생명주기 관리를 용이하게 하기 위한 스코프 클래스입니다.
* 스코프를 통해 에셋을 로드할 경우 핸들들이 스코프 내부에 캐싱되며 스코프가 Dispose될 때 포함된 에셋들을 모두 Release처리합니다.
    * 에셋의 경우 : 같은 주소 혹은 에셋 레퍼런스당 하나의 핸들만 캐싱하며 캐싱된 상태에서 요청시 이전에 반환된 핸들을 반환합니다
    * 프리펩 인스턴스의 경우 : 인스턴스와 핸들은 1:1 관계이므로 인스턴스 생성시마다 새로운 핸들이 생성되어 반환됩니다.
* Dispose 호출을 누락한채 스코프에 대한 참조를 잃어버렸을 경우 이후 소멸자에서 Dispose를 호출합니다.
    * 하지만 GC의 소멸자 호출 시점은 명확하지 않으며 그에 따른 갑작스런 성능저하가 발생할 수 있으므로 권장하지 않습니다.

## 에셋 불러오기 혹은 인스턴스 생성하기
### 주소 기반 사용
```csharp
using Jin5eok;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Example : MonoBehaviour
{
    private async void Start()
    {
        // 스코프 생성
        using var scope = new AddressablesScope();
        
        // 에셋 로드
        var handle = await scope.LoadAssetAsync<Sprite>("MySprite");
        var sprite = handle.Result;
        
        // 프리펩 인스턴스화
        var prefabHandle = await scope.InstantiateAsync("MyPrefab");
        var instance = prefabHandle.Result;
        
        // 스코프가 Dispose되면 자동으로 모든 핸들이 Release됩니다
    }
}
```
### AssetReference 기반 사용

```csharp
[SerializeField] private AssetReference _characterPrefab;

private async void SpawnCharacter()
{
    using var scope = new AddressablesScope();
    
    // AssetReference로 프리펩 인스턴스화
    var handle = await scope.InstantiateAsync(_characterPrefab);
    var character = handle.Result;
    
    // 스코프가 끝나면 자동으로 Release
}
```

## 사용 예) 씬 진입 시 핸들을 생성하고 씬 이동시 모두 릴리즈

씬이 로드될 때 필요한 에셋들을 스코프로 관리하는 예제:

```csharp
public class SceneManager : MonoBehaviour
{
    private AddressablesScope _sceneScope;
    
    private async void OnEnable()
    {
        _sceneScope = new AddressablesScope();
        
        // 씬에 필요한 에셋들 로드
        var uiHandle = await _sceneScope.LoadAssetAsync<GameObject>("UI_Panel");
        var bgmHandle = await _sceneScope.LoadAssetAsync<AudioClip>("BGM_Scene1");
        
        // 사용...
    }
    
    private void OnDisable()
    {
        // 씬이 비활성화될 때 모든 에셋 Release
        _sceneScope?.Dispose();
    }
}
```

## 주의사항

**⚠️ 스코프 내부 에셋들은 레퍼런스 카운트 기반 관리가 아님**

- 에셋을 한번 로드하면 이후 동일 에셋 로드시 처음 불러왔을때의 핸들을 재활용합니다.

```csharp
using var scope = new AddressablesScope();

// 첫 번째 요청 - 새 핸들 생성
var handle1 = await scope.LoadAssetAsync<Texture2D>("CommonTexture");

// 두 번째 요청 - 같은 핸들 반환 (캐싱됨)
var handle2 = await scope.LoadAssetAsync<Texture2D>("CommonTexture");

// handle1과 handle2는 같은 핸들입니다
Debug.Log(handle1 == handle2); // true
```
- 그러므로 핸들을 수동 릴리즈하는 것은 권장하지 않습니다.
- 스코프 단위로 일괄 해제하고 핸들의 개별관리가 필요한 에셋은 다른 방법으로 로드하는걸 권장합니다.

**⚠️ 사용이 끝날시 Dispose 호출**
- 스코프에 대한 참조를 잃어버리면 소멸자에서 Dispose가 호출됩니다.
- 하지만 GC의 소멸자 호출 시점은 불명확하며 성능 저하가 발생할 수 있습니다.
- 가급적 수동으로 Dispose를 호출하는걸 권장합니다.