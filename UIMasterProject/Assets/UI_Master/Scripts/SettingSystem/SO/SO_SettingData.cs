using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[CreateAssetMenu(fileName = "New SettingData", menuName = "Setting/SettingData")]
public class SO_SettingData : ScriptableObject
{


    [Header("通用")]    
    //主题
    public int num_Language=0;
    public int num_Theme=0;
 
    [Header("图像")]
    public int num_FullScreen=0;
    public int num_Resolution=10;    
    public int num_Quality=2;
    public int num_AntiAliasing=2;
    public int num_vSync=0;
    public int num_RefreshRate=3;
    public int num_FrameRate=2;

    [Header("音量")]
    public int num_MasterVol=8;
    public int num_BGMVol=8;
    public int num_SEVol=8;


    [Header("后处理")]
    public int num_ChromaticAberration=0;
    public int num_FlimGrain=0;
    public int num_Vignette=0;

    [Header("游戏")]
    public int num_AllowedGF=1;


    /// <summary>
    /// 反射，方便为class覆盖数据
    /// </summary>
    /// <param name="s"></param>
    public void CopyNewData(SO_SettingData s)
    {
        foreach (FieldInfo field in typeof(SO_SettingData).GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            field.SetValue(this, field.GetValue(s));
        }
    }


}
