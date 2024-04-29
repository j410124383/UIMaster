using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIM_MainMenu : MonoBehaviour
{
    public static UIM_MainMenu Instance;
    //将版本号预显示文本拖入
    public TMP_Text textVersion;
    public Button butQuit;
    public Animator frameOptionsAC;

    private void Awake()
    {
        Instance = this;


    }

    private void Start()
    {
        if (butQuit) butQuit.onClick.AddListener(UIM_UIManager.Instance.StartQuit);
        if (textVersion) textVersion.text = Application.version;
  
    }


}
