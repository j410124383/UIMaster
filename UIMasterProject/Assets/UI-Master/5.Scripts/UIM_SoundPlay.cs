using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIM_SoundPlay : MonoBehaviour
{
    private AudioSource SFPlayer;
    public string key="id_click_01";

    private void Awake()
    {
        if (GetComponent<Button>())
        {
            GetComponent<Button>().onClick.AddListener(delegate { PlaySound(key); });
        }
        if (GetComponent<Toggle>())
        {
            GetComponent<Toggle>().onValueChanged.AddListener(delegate { PlaySound(key); });
        }
       

        //SFPlayer = GetComponent<AudioSource>() ?
        //    GetComponent<AudioSource>() : gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound(string key)
    {
        var sm = UIM_SoundManager.Instance;
        var au = sm. GetAduio(key);
        SFPlayer = sm.SFPlayer;
        if (!au) return;

        //SFPlayer.volume = sm.so_AudioData.SFVolume;
        SFPlayer.clip = au;
        SFPlayer.Play();
    }

}
