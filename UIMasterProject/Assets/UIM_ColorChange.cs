using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIM_ColorChange : MonoBehaviour
{

    public int palatteNum; //在当前主题中选择的色彩序号

    void Start()
    {

        // 订阅事件
        UIM_EventManager.OnChangeColor += ChangeColor;
    }

    void OnDestroy()
    {
        // 取消订阅事件
        UIM_EventManager.OnChangeColor -= ChangeColor;


    }


    // 响应事件的方法
    private void ChangeColor(SO_UIPalette _UIPalette)
    {
        if (palatteNum >= 0 && palatteNum < _UIPalette.colors.Length)
        {
            Color targetcolor = _UIPalette.colors[palatteNum];

            var t = GetComponent<TMP_Text>() ? GetComponent<TMP_Text>() : null;
            if (t) { t.color = targetcolor; };

            var i = GetComponent<Image>() ? GetComponent<Image>() : null;
            if (i) { i.color = targetcolor; };

            Debug.Log(name+"改变颜色了");

        }
        else
        {
            Debug.Log("不在索引规范内");
        }
       

   


    }

}
