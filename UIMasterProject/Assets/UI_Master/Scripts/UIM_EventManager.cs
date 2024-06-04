using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIM_EventManager : MonoBehaviour
{
    // ����ί������
    public delegate void ChangeColorEventHandler(Color newColor);

    // �����¼�
    public static event ChangeColorEventHandler OnChangeColor;

    // �����¼��ķ���
    public static void TriggerChangeColor(Color newColor)
    {
        if (OnChangeColor != null)
        {
            OnChangeColor(newColor);
        }
    }
}
