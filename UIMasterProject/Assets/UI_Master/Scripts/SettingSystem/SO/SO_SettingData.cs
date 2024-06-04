using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New SettingData", menuName = "Setting/SettingData")]
public class SO_SettingData : ScriptableObject
{


    [Header("当前值")]
    public int num_Language;
    public int num_Quality;
    public int num_MasterVol;
    public int num_BGMVol;
    public int num_SEVol;
    public int num_FullScreen;
    public int num_AntiAliasing;
    public int num_vSync;
    public int num_Resoulution;
    public int num_RefreshRate;
    public int num_FrameRate;
    //主题
    public int num_Theme;

    //urp后处理相关
    public int num_ChromaticAberration;
    public int num_FlimGrain;
    public int num_Vignette;


    //允许金手指开关
    public int num_AllowedGF;


    public void CopyNewData(SO_SettingData s)
    {
        num_Language = s.num_Language;
        num_Quality = s.num_Quality;
        num_MasterVol = s.num_MasterVol;
        num_BGMVol = s.num_BGMVol;
        num_SEVol = s.num_SEVol;
        num_FullScreen = s.num_FullScreen;
        num_AntiAliasing = s.num_AntiAliasing;
        num_vSync = s.num_vSync;
        num_Resoulution = s.num_Resoulution;
        num_RefreshRate = s.num_RefreshRate;
        num_FrameRate = s.num_FrameRate;

        num_Theme = s.num_Theme;
        num_ChromaticAberration = s.num_ChromaticAberration;
        num_FlimGrain = s.num_FlimGrain;
        num_Vignette = s.num_Vignette;

        num_AllowedGF = s.num_AllowedGF;

    }


}
