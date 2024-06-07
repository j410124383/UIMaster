using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIM_ColorChange : MonoBehaviour
{

    public int palatteNum; //�ڵ�ǰ������ѡ���ɫ�����

    private Image targetImage; // ��Ҫ�ı���ɫ��ͼƬ
    private TMP_Text targetText;

    private Color startColor; // ��ʼ��ɫ
    private Color endColor ; // Ŀ����ɫ
    private float duration = 0.3f; // ��ɫ�仯�ĳ���ʱ��

    private float elapsedTime = 0f; // ��¼�Ѿ�������ʱ��
    private bool isLerping = false; // ����Ƿ����ڽ�����ɫ�仯




    void Start()
    {
     
        // �����¼�
        UIM_EventManager.OnChangeColor += ChangeColor;



        FreshColor();
    }

    void OnDestroy()
    {
        // ȡ�������¼�
        UIM_EventManager.OnChangeColor -= ChangeColor;
        //FreshColor();

    }

    private void OnEnable()
    {

        FreshColor();
    }

    public void FreshColor()
    {

        targetText = GetComponent<TMP_Text>() ? GetComponent<TMP_Text>() : null;
        targetImage = GetComponent<Image>() ? GetComponent<Image>() : null;
        var i = UIM_SettingManager.instance;
        if (i != null)
        {
            ChangeColor(i.curPalette);
            SwitchColor(1f);
        }
    }


    // ��Ӧ�¼��ķ���
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

        if (!_UIPalette) return;
        palatteNum = Mathf.Clamp(palatteNum,0, _UIPalette.colors.Length);

        endColor = _UIPalette.colors[palatteNum];

        elapsedTime = 0f;
        isLerping = true;

    }


   
    private void Update()
    {
        if (isLerping)
        {
            // ��֡������ɫ�仯
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            SwitchColor(t);

            // ����Ƿ������ɫ�仯
            if (t >= 1.0f)
            {
                isLerping = false;
            }
        }
    }

    void SwitchColor(float i)
    {
        if (targetText)
        {
            targetText.color = Color.Lerp(startColor, endColor, i);
        }
        if (targetImage)
        {
            targetImage.color = Color.Lerp(startColor, endColor, i);
        }
    }



}
