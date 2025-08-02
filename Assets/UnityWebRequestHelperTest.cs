using System.Collections.Generic;
using Jin5eok;
using UnityEngine;
using Cysharp.Threading.Tasks;
public class UnityWebRequestHelperTest : MonoBehaviour
{
    private async void Start()
    {
        // var a = await UnityWebRequestHelper.GetUniTaskAsync("https://microsoftedge.github.io/Demos/json-dummy-data/64KBjson");
        // Debug.Log(a.ToString());

        UnityWebRequestHelper.GetTexture("https://thumbs.dreamstime.com/b/innovative-medical-device-featuring-eye-image-illustrating-advanced-tracking-technology-generated-ai-358374352.jpge", OnSuccess, OnError);
        UnityWebRequestHelper.Get("https://microsoftedge.github.io/Demos/json-dummy-data/64KBjson", OnSuccess, OnError);
    }

    private void OnSuccess(Texture2D obj)
    {
        Debug.Log(obj.name);
    }

    private void OnSuccess(string obj)
    {
        Debug.Log(obj);
    }
    
    void OnError(UnityWebRequestException obj)
    {
        Debug.Log(obj);
    }
    
    public async UniTask Test()
    {
        
    }
}
