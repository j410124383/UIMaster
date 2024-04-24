using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIM_ScrollBar : MonoBehaviour
{
    private ScrollRect scrollView;
    public bool isAuto;
    private RectTransform content;
    public float scrollSpeed = 30f;
    public float idleTimeToScroll = 5f; // 5秒内没有操作则开始自动滚动

    private float lastInputTime;

    public Animator gradientStrip;
    void Awake()
    {
        lastInputTime = Time.time;
        scrollView = GetComponent<ScrollRect>();
        content = scrollView.content;
        
    }

    private void OnEnable()
    {
        scrollView.verticalScrollbar.value = 1f;
    }

    void Update()
    {
        if (isAuto)
        {
            AutoPlay();
        }



        // 如果 Content 滚动到底部，则重置滚动位置
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

        // 检测鼠标和键盘输入
        if (Input.anyKeyDown || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            lastInputTime = Time.time; // 更新最后一次输入时间
        }

        // 如果在 idleTimeToScroll 内没有输入操作，则自动滚动 Content
        if (Time.time - lastInputTime > idleTimeToScroll)
        {
            // 向下滚动 Content
            content.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        }
    }

     bool IsDown()
    {
        return scrollView.verticalScrollbar.value <= 0 ? true : false;
    }


}
