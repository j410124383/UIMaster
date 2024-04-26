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
        // 刷新当前对象及其所有子对象中的布局组件
        RefreshLayoutsRecursively(transform);
    }

    /// <summary>
    /// 刷新对象上的layout组件
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
