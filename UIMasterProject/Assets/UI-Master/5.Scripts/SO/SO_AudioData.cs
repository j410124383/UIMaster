using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_AudioData", menuName = "AudioData")]
public class SO_AudioData : ScriptableObject
{
    [SerializeField]
    public List<AudioClipEntry> audioDataList;
    [Range(0F,1F)]
    public float BGMVolum=1F;
    [Range(0F, 1F)]
    public float SFVolume=1F;


}

[System.Serializable]
public struct AudioClipEntry
{
    public string key;
    public AudioClip audioClip;

    public AudioClipEntry(string key, AudioClip audioClip)
    {
        this.key = key;
        this.audioClip = audioClip;
    }
}