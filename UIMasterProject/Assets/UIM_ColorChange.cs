using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIM_ColorChange : MonoBehaviour
{
    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        // 订阅事件
        UIM_EventManager.OnChangeColor += ChangeColor;
    }

    void OnDestroy()
    {
        // 取消订阅事件
        UIM_EventManager.OnChangeColor -= ChangeColor;
    }

    // 响应事件的方法
    private void ChangeColor(Color newColor)
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = newColor;
        }
    }




}
