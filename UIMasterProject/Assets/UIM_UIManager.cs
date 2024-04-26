using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIM_UIManager : MonoBehaviour
{
    public static UIM_UIManager Instance;

    private void Awake()
    {
        Instance = this;
        // ˢ�µ�ǰ�����������Ӷ����еĲ������
        RefreshLayoutsRecursively(transform);
    }

    /// <summary>
    /// ˢ�¶����ϵ�layout���
    /// </summary>

    public  void RefreshLayoutsRecursively(Transform parent)
    {

        var list = parent.GetComponentsInChildren<HorizontalOrVerticalLayoutGroup>();

        foreach (var item in list)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(item.transform.GetComponent<RectTransform>());

        }


    }



}
