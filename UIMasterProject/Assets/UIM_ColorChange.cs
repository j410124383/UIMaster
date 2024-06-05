using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIM_ColorChange : MonoBehaviour
{

    public int palatteNum; //在当前主题中选择的色彩序号

    private Image targetImage; // 需要改变颜色的图片
    private TMP_Text targetText;

    private Color startColor; // 起始颜色
    private Color endColor ; // 目标颜色
    private float duration = 0.3f; // 颜色变化的持续时间

    private float elapsedTime = 0f; // 记录已经经过的时间
    private bool isLerping = false; // 标记是否正在进行颜色变化




    void Start()
    {

        // 订阅事件
        UIM_EventManager.OnChangeColor += ChangeColor;
        new WaitForEndOfFrame();
        targetText = GetComponent<TMP_Text>() ? GetComponent<TMP_Text>() : null;
        targetImage = GetComponent<Image>() ? GetComponent<Image>() : null;


        FreshColor();
    }

    void OnDestroy()
    {
        // 取消订阅事件
        UIM_EventManager.OnChangeColor -= ChangeColor;
        //FreshColor();

    }

    private void OnEnable()
    {

        FreshColor();
    }

    public void FreshColor()
    {
        var i = UIM_SettingManager.instance;
        if (i != null)
        {
            ChangeColor(i.curPalette);
            t = 0.99f;
        }
    }


    // 响应事件的方法
    private void ChangeColor(SO_UIPalette _UIPalette)
    {
        if (targetImage)
        {
            startColor = targetImage.color;
        }
        else if (targetText)
        {
            startColor = targetText.color;
        }

        palatteNum = Mathf.Clamp(palatteNum,0, _UIPalette.colors.Length);

        endColor = _UIPalette.colors[palatteNum];

        elapsedTime = 0f;
        isLerping = true;

    }


    float t;
    private void Update()
    {
        if (isLerping)
        {
            // 逐帧更新颜色变化
            elapsedTime += Time.deltaTime;
            t = Mathf.Clamp01(elapsedTime / duration);
            if (targetText)
            {
                targetText.color = Color.Lerp(startColor, endColor, t);
            }
            if (targetImage)
            {
                targetImage.color = Color.Lerp(startColor, endColor, t);
            }
          
        
            // 检查是否完成颜色变化
            if (t >= 1.0f)
            {
                isLerping = false;
            }
        }
    }


}
