using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New SettingData", menuName = "SettingData")]
public class SO_SettingData : ScriptableObject
{
    //public List<int> antiAliasingList;

    //public List<Vector2> resolutionList;

    //public int sliderStep=11;
    [Header("初始值")]
    public int initial_Language;
    public int initial_Quality;
    public int initial_MasterVol;
    public int initial_BGMVol;
    public int initial_SEVol;
    public int initial_FullScreen;
    public int initial_AntiAliasing;
    public int initial_vSync;
    public int initial_Resoulution;


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



    public SO_SettingData(int i01,int i02,int i03,int i04,int i05,int i06,int i07,int i08,int i09)
    {
        num_Language = i01;
        num_Quality = i02;
        num_MasterVol = i03;
        num_BGMVol = i04;
        num_SEVol = i05;
        num_FullScreen = i06;
        num_AntiAliasing = i07;
        num_vSync = i08;
        num_Resoulution = i09;
        ResetToInitialValues();
    }

    /// <summary>
    /// 充值化初始值
    /// </summary>
    public void ResetToInitialValues()
    {
        num_Language = initial_Language;
        num_Quality = initial_Quality;
        num_MasterVol = initial_MasterVol;
        num_BGMVol = initial_BGMVol;
        num_SEVol = initial_SEVol;
        num_FullScreen = initial_FullScreen;
        num_AntiAliasing = initial_AntiAliasing;
        num_vSync = initial_vSync;
        num_Resoulution = initial_Resoulution;



    }



}
