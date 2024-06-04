using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIM_EventManager : MonoBehaviour
{
    // 定义委托类型
    public delegate void ChangeColorEventHandler(Color newColor);

    // 定义事件
    public static event ChangeColorEventHandler OnChangeColor;

    // 触发事件的方法
    public static void TriggerChangeColor(Color newColor)
    {
        if (OnChangeColor != null)
        {
            OnChangeColor(newColor);
        }
    }
}
