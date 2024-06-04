using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

[AddComponentMenu("UIMaster/Button/ButEffectTrigger")]
[DisallowMultipleComponent]
public class UIM_ButEffectTrigger : MonoBehaviour, ISelectHandler
{

    //包装一下按钮，不只是发出效果
    public string presskey="id_click_01";
    public string selectkey = "id_click_02";

    public bool isShake;

    public void OnSelect(BaseEventData eventData)
    {
        // 打印出被选中按钮的名字
        //Debug.Log("Button selected: " + gameObject.name);
        if (selectkey!=string.Empty)
        {
            UIM_SoundManager.Play_SF(selectkey);
        }
      
        UIM_CanvasMovtion.instance.OnCanvasActive();
    }

    private void Awake()
    {
        if (GetComponent<Button>())
        {
            GetComponent<Button>().onClick.AddListener(delegate { UIM_SoundManager.Play_SF(presskey); });
        }
        if (GetComponent<Toggle>())
        {
            GetComponent<Toggle>().onValueChanged.AddListener(delegate { UIM_SoundManager.Play_SF(presskey); });
        }
       

        //SFPlayer = GetComponent<AudioSource>() ?
        //    GetComponent<AudioSource>() : gameObject.AddComponent<AudioSource>();
    }



}
