using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct URLGroup{

    public string urlPath;
    public Button urlBut;


}

[System.Serializable]
[AddComponentMenu("UIMaster/Button/ButURL")]
[DisallowMultipleComponent]
public class UIM_ButURL : MonoBehaviour
{
    
    public List<URLGroup> uilGroupList;


    private void Awake()
    {

        foreach (var item in uilGroupList)
        {
            item.urlBut.onClick.AddListener(delegate { OpenExternalURL(item.urlPath); });
        }

    }

    // 当按钮点击时调用的方法
    void OpenExternalURL(string url)
    {
        // 在默认浏览器中打开外部网页
        Application.OpenURL(url);
    }

}
