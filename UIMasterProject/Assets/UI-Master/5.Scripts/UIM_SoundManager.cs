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



}
