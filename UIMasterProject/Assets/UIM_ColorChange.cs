using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIM_ColorChange : MonoBehaviour
{
    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        // �����¼�
        UIM_EventManager.OnChangeColor += ChangeColor;
    }

    void OnDestroy()
    {
        // ȡ�������¼�
        UIM_EventManager.OnChangeColor -= ChangeColor;
    }

    // ��Ӧ�¼��ķ���
    private void ChangeColor(Color newColor)
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = newColor;
        }
    }




}
