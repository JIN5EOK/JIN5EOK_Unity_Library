# ServiceLocator

**전역(static) / 싱글턴을 제공하지 않는** 인스턴스 기반 서비스 로케이터 템플릿입니다.

## 기본 사용 예시 (Composition Root)

```csharp
using Jin5eok;

public sealed class GameInstaller
{
    private readonly ServiceLocator _root = new();

    public IServiceLocator Services => _root;

    public void Install()
    {
        _root.Register<ILogger>(new UnityLogger());
        _root.Register<IPlayerRepository>(new PlayerRepository());
    }
}
```

## 스코프(부모-자식) 사용 예시

```csharp
using Jin5eok;

public sealed class BattleContext
{
    private readonly IServiceLocator _scope;

    public BattleContext(IServiceLocator root)
    {
        _scope = root.CreateChild();
        _scope.Register(new BattleState());
    }
}
```

## 설계 의도

- **소유(수명) 결정은 호출자가 한다**: 필요한 곳에서 `new ServiceLocator()`로 만들고, 보관/파기 타이밍은 사용자 코드(Composition Root)가 결정합니다.
- **스코프 지원**: 자식 로케이터는 자기 스코프에서 우선 조회하고, 없으면 부모 체인에서 조회합니다.
- **단순함 우선**: 인스턴스 등록/조회만 제공하고, 팩토리/캐싱/자동 Dispose 같은 정책은 포함하지 않습니다. (필요 시 사용자 측에서 확장/구현)

