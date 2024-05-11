using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIM_ScrollView : MonoBehaviour
{
    private ScrollRect scrollView;
    private Scrollbar scrollbar;

    public bool isAuto;
    private RectTransform content;
    public float scrollSpeed = 30f;
    public float idleTimeToScroll = 5f; // 5����û�в�����ʼ�Զ�����

    private float lastInputTime;

    public Animator gradientStrip_Down, gradientStrip_Up;
    

    void Awake()
    {
        lastInputTime = Time.time;
        scrollView = GetComponent<ScrollRect>();
        content = scrollView.content;
        scrollbar = scrollView.verticalScrollbar;
        scrollbar.onValueChanged.AddListener(delegate { SwitchGrandientStrip(); });

    }

    private void OnEnable()
    {
        scrollView.verticalScrollbar.value = 1f;
        UIM_UIManager.Instance.RefreshLayoutsRecursively();

    }

    void Update()
    {
        if (isAuto)
        {
            AutoPlay();
        }

    }

    public void SwitchGrandientStrip()
    {

        // ��� Content �������ײ��������ù���λ��
        if (!gradientStrip_Down.gameObject.activeInHierarchy) return;
        gradientStrip_Down.SetBool("ISSWITCH", IsDown());

        if (!gradientStrip_Up.gameObject.activeInHierarchy) return;
        gradientStrip_Up.SetBool("ISSWITCH", IsTop());

    }

    void AutoPlay()
    {
        if (IsDown()) return;

        // ������ͼ�������
        //if (Input.anyKeyDown || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        //{
        //    lastInputTime = Time.time; // �������һ������ʱ��
        //}

        // ����� idleTimeToScroll ��û��������������Զ����� Content
        if (Time.time - lastInputTime > idleTimeToScroll)
        {
            // ���¹��� Content
            content.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        }
    }

     bool IsDown()
    {
        return scrollbar.value <= 0.1f ? true : false;
    }

    bool IsTop()
    {
        return scrollbar.value >= 0.9f ? true : false;
    }

}
