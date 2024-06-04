using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIM_ColorChange : MonoBehaviour
{

    public int palatteNum; //�ڵ�ǰ������ѡ���ɫ�����

    void Start()
    {

        // �����¼�
        UIM_EventManager.OnChangeColor += ChangeColor;
    }

    void OnDestroy()
    {
        // ȡ�������¼�
        UIM_EventManager.OnChangeColor -= ChangeColor;


    }


    // ��Ӧ�¼��ķ���
    private void ChangeColor(SO_UIPalette _UIPalette)
    {
        if (palatteNum >= 0 && palatteNum < _UIPalette.colors.Length)
        {
            Color targetcolor = _UIPalette.colors[palatteNum];

            var t = GetComponent<TMP_Text>() ? GetComponent<TMP_Text>() : null;
            if (t) { t.color = targetcolor; };

            var i = GetComponent<Image>() ? GetComponent<Image>() : null;
            if (i) { i.color = targetcolor; };

            Debug.Log(name+"�ı���ɫ��");

        }
        else
        {
            Debug.Log("���������淶��");
        }
       

   


    }

}
