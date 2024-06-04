using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIM_EventManager : MonoBehaviour
{
    //public static UIM_EventManager instance;

    // 定义委托类型
    public delegate void ChangeColorEventHandler(SO_UIPalette _UIPalette);

    // 定义事件
    public static event ChangeColorEventHandler OnChangeColor;

    //private void Awake()
    //{
    //    instance = this;
    //}


    // 触发事件的方法
    public static void TriggerChangeColor(SO_UIPalette _UIPalette)
    {
        if (OnChangeColor != null)
        {
            OnChangeColor(_UIPalette);
        }
    }



}
