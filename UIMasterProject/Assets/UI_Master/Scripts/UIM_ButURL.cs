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

    // ����ť���ʱ���õķ���
    void OpenExternalURL(string url)
    {
        // ��Ĭ��������д��ⲿ��ҳ
        Application.OpenURL(url);
    }

}
