using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New SettingData", menuName = "SettingData")]
public class SO_SettingData : ScriptableObject
{


    [Header("µ±Ç°Öµ")]
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
  
    }


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

    }


}
