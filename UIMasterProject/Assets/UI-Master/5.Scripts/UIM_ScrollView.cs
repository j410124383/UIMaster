using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIM_ScrollView : MonoBehaviour
{
    private ScrollRect scrollView;
    public bool isAuto;
    private RectTransform content;
    public float scrollSpeed = 30f;
    public float idleTimeToScroll = 5f; // 5����û�в�����ʼ�Զ�����

    private float lastInputTime;

    public Animator gradientStrip;
    void Awake()
    {
        lastInputTime = Time.time;
        scrollView = GetComponent<ScrollRect>();
        content = scrollView.content;

        // ˢ�µ�ǰ�����������Ӷ����еĲ������
        RefreshLayoutsRecursively(transform);

    }

    private void Start()
    {
        RefreshLayoutsRecursively(transform);
    }

    private void OnEnable()
    {
        scrollView.verticalScrollbar.value = 1f;

        // ˢ�µ�ǰ�����������Ӷ����еĲ������
        RefreshLayoutsRecursively(transform);

    }

    void Update()
    {
        if (isAuto)
        {
            AutoPlay();
        }


        // ��� Content �������ײ��������ù���λ��
        if (!gradientStrip.gameObject.activeInHierarchy) return;
        if (IsDown())
        {
            gradientStrip.SetBool("ISSWITCH", true);
        }
        else
        {
            gradientStrip.SetBool("ISSWITCH", false);
        }

    }

    void AutoPlay()
    {
        if (IsDown()) return;

        // ������ͼ�������
        if (Input.anyKeyDown || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            lastInputTime = Time.time; // �������һ������ʱ��
        }

        // ����� idleTimeToScroll ��û��������������Զ����� Content
        if (Time.time - lastInputTime > idleTimeToScroll)
        {
            // ���¹��� Content
            content.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        }
    }

     bool IsDown()
    {
        return scrollView.verticalScrollbar.value <= 0 ? true : false;
    }


    /// <summary>
    /// ˢ�¶����ϵ�layout���
    /// </summary>
    /// <param ������="parent"></param>
    public static void RefreshLayoutsRecursively(Transform parent)
    {

        var list = parent.GetComponentsInChildren<HorizontalOrVerticalLayoutGroup>();

        foreach (var item in list)
        {
            //item.CalculateLayoutInputVertical();
            //item.SetLayoutVertical();
            //item.CalculateLayoutInputHorizontal();
            //item.SetLayoutHorizontal();
            LayoutRebuilder.ForceRebuildLayoutImmediate(item.transform.GetComponent<RectTransform>());
            //print("verticalˢ����:" + item.gameObject.name);
        }


    }
}