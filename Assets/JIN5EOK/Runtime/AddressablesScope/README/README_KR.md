# AddressablesScope
* 어드레서블로 로드한 에셋 / 프리펩 인스턴스의 생명주기 관리를 용이하게 하기 위한 스코프 클래스입니다.
* 스코프를 통해 에셋을 로드할 경우 핸들들이 스코프 내부에 캐싱되며 스코프가 Dispose될 때 포함된 에셋들을 모두 Release처리합니다.
  * 에셋의 경우 : 같은 주소 혹은 에셋 레퍼런스당 하나의 핸들만 캐싱하며 캐싱된 상태에서 요청시 이전에 반환된 핸들을 반환합니다
  * 프리펩 인스턴스의 경우 : 인스턴스와 핸들은 1:1 관계이므로 인스턴스 생성시마다 새로운 핸들이 생성되어 반환됩니다. 
* Dispose 호출을 누락한채 스코프에 대한 참조를 잃어버렸을 경우 이후 소멸자에서 Dispose를 호출합니다.
  * 하지만 GC의 소멸자 호출 시점은 명확하지 않으며 그에 따른 갑작스런 성능저하가 발생할 수 있으므로 권장하지 않습니다.

### ```public AsyncOperationHandle<T> LoadAssetAsync<T>(AssetReference assetReference)  where T : Object```
* 에셋 레퍼런스를 기반으로 에셋을 로드하여 반환합니다.

### ```public AsyncOperationHandle<T> LoadAssetAsync<T>(string address) where T : Object```
* 주소를 기반으로 에셋을 로드하여 반환합니다.

### ```public AsyncOperationHandle<GameObject> InstantiateAsync(AssetReference assetReference)```
* 에셋 레퍼런스를 기반으로 프리펩을 인스턴스화하여 반환합니다.

### ```public AsyncOperationHandle<GameObject> InstantiateAsync(string address)```
* 주소를 기반으로 프리펩을 인스턴스화하여 반환합니다.

## ```public void Dispose()```
* 내부에 캐싱된 핸들들을 모두 Release처리합니다
* Dispose이후 스코프의 함수를 호출하면 예외가 발생합니다.