using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIM_EventManager : MonoBehaviour
{
    //public static UIM_EventManager instance;

    // ����ί������
    public delegate void ChangeColorEventHandler(SO_UIPalette _UIPalette);

    // �����¼�
    public static event ChangeColorEventHandler OnChangeColor;

    //private void Awake()
    //{
    //    instance = this;
    //}


    // �����¼��ķ���
    public static void TriggerChangeColor(SO_UIPalette _UIPalette)
    {
        if (OnChangeColor != null)
        {
            OnChangeColor(_UIPalette);
        }
    }



}
