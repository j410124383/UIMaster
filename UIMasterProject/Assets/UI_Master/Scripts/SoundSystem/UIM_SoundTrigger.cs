using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[AddComponentMenu("UIM/SoundTrigger")]
[DisallowMultipleComponent]
public class UIM_SoundTrigger : MonoBehaviour
{

    public string key="id_click_01";

    private void Awake()
    {
        if (GetComponent<Button>())
        {
            GetComponent<Button>().onClick.AddListener(delegate { UIM_SoundManager.Play_SF(key); });
        }
        if (GetComponent<Toggle>())
        {
            GetComponent<Toggle>().onValueChanged.AddListener(delegate { UIM_SoundManager.Play_SF(key); });
        }
       

        //SFPlayer = GetComponent<AudioSource>() ?
        //    GetComponent<AudioSource>() : gameObject.AddComponent<AudioSource>();
    }

 

}
