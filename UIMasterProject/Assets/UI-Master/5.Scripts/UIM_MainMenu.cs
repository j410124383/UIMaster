using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIM_MainMenu : MonoBehaviour
{
    //���汾��Ԥ��ʾ�ı�����
    public TMP_Text textVersion;


    private void Awake()
    {
        if (textVersion)
        {
            textVersion.text = Application.version;
        }
        
    }




}
