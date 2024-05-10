using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIM_SoundManager : MonoBehaviour
{
    public static UIM_SoundManager Instance;
    public SO_AudioData so_AudioData;
    public AudioSource SFPlayer,BGMPlyaer;


    private void Awake()
    {
        Instance = this;


    }




    public AudioClip GetAduio(string key)
    {
        AudioClip au=null;
        foreach (var item in so_AudioData.audioDataList)
        {
            if (item.key == key)
            {
                au = item.audioClip;
                break;
            }
        }
        return au;
    }


    public static void Play_SF(string key)
    {

        var sm = UIM_SoundManager.Instance ? UIM_SoundManager.Instance : null;
        var au = sm.GetAduio(key);

        if (!au) return;
        sm.SFPlayer.clip = au;
        sm.SFPlayer.Play();
    }


}
