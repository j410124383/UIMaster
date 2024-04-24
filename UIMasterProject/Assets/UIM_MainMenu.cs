using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIM_MainMenu : MonoBehaviour
{
    //将版本号预显示文本拖入
    public TMP_Text textVersion;


    private void Awake()
    {
        if (textVersion)
        {
            textVersion.text = Application.version;
        }
        
    }




}
