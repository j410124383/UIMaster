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
        RefreshLayoutsRecursively();
    }

    public void Start()
    {
        // ˢ�µ�ǰ�����������Ӷ����еĲ������
        RefreshLayoutsRecursively();
    }

    /// <summary>
    /// ˢ�¶����ϵ�layout���
    /// </summary>
    public  void RefreshLayoutsRecursively(Transform parent=null)
    {

        StartCoroutine(refreshRecursively(parent));

    }


    IEnumerator refreshRecursively(Transform parent = null)
    {
        yield return new WaitForEndOfFrame();
        if (parent == null) parent = transform;

        var list = parent.GetComponentsInChildren<HorizontalOrVerticalLayoutGroup>();
        var list02 = parent.GetComponentsInChildren<ContentSizeFitter>();

        foreach (var item in list)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(item.transform.GetComponent<RectTransform>());

        }

        foreach (var item in list02)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(item.transform.GetComponent<RectTransform>());
        }

    }


}
