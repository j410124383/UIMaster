
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class UIM_UniversalSetting : MonoBehaviour
{
    public static UIM_UniversalSetting Instance;

    //设置类相关脚本，主要实现目前的基本功能
    public List<int> antiAliasingList;
    public List<Locale> locales;

    public List<Vector2> resolutionList;
    public int sliderStep = 11;
  

    public AudioMixer mainAudioMixer;
    AsyncOperationHandle m_InitialzeOperation;

    public SO_SettingData curSetData;
    public SO_SettingData orignSettingData;

    private void Awake()
    {
        Instance = this;
        
    }

  


    private void Start()
    {
        RefreshSetting();
    }

    /// <summary>
    /// 刷新目前的设置，也相当于读取，游戏开始时，默认调用一次。
    /// </summary>
    void RefreshSetting()
    {
 
        OnChangeQuality(curSetData.num_Quality);
        OnChangeVolume("MasterVol", curSetData.num_MasterVol);
        OnChangeVolume("BGMVol", curSetData.num_BGMVol);
        OnChangeVolume("SFVol", curSetData.num_SEVol);
        OnChangeFullScreen(curSetData.num_FullScreen);
        OnChangevSync(curSetData.num_vSync);
        OnChangeAntiAliasing(curSetData.num_AntiAliasing);
        OnChangeResolustion(curSetData.num_Resoulution);
        OnChangeLanguage(curSetData.num_Language);
        print("设置初始化已调整完毕");

    }


    /// <summary>
    /// 改变语言选项
    /// </summary>
    /// <param name="i"></param>
    public void OnChangeLanguage(int i)
    {

        //var l = LocalizationSettings.AvailableLocales.Locales;
       
        LocalizationSettings.Instance.SetSelectedLocale(locales[i]);
        UIM_UIManager.Instance.RefreshLayoutsRecursively();
    }

    /// <summary>
    /// 改变图像质量
    /// </summary>
    /// <param name="i"></param>
    public void OnChangeQuality(int i)
    {
        QualitySettings.SetQualityLevel(i);
        UIM_UIManager.Instance.RefreshLayoutsRecursively();
    }

    /// <summary>
    /// 音量大小调整
    /// </summary>
    /// <param name="s"></param>
    /// <param name="DP"></param>
    public void OnChangeVolume(string s, int DP)
    {
        var f = Mathf.Lerp(-80, 20, ((float)DP / (sliderStep - 1)));
        mainAudioMixer.SetFloat(s, f);
    }

    /// <summary>
    /// 调整全屏
    /// </summary>
    public void OnChangeFullScreen(int i)
    {
        Screen.fullScreen = i == 0 ? true : false;
    }

    /// <summary>
    /// 调整垂直同步
    /// </summary>
    public void OnChangevSync(int i)
    {
        // 切换垂直同步模式
        QualitySettings.vSyncCount = i == 0 ? 1 : 0;
        //Debug.Log("VSync Mode Toggled: " + (QualitySettings.vSyncCount == 0 ? "Off" : "On"));
    }

    /// <summary>
    /// 修改抗锯齿
    /// </summary>
    public void OnChangeAntiAliasing(int i)
    {
        // 修改抗锯齿模式
        QualitySettings.antiAliasing = antiAliasingList[i];
        //Debug.Log("Anti-Aliasing Mode Set to: " + antiAliasingList[i] + "x MSAA");
    }

    /// <summary>
    /// 修改分辨力
    /// </summary>
    public void OnChangeResolustion(int i)
    {
        var v2 = resolutionList[i];
        // 修改分辨率
        SetResolution((int)v2.x, (int)v2.y);


        void SetResolution(int width, int height)
        {
            // 修改分辨率
            Screen.SetResolution(width, height, Screen.fullScreen);
            //Debug.Log("Resolution Set to: " + width + "x" + height);
        }
    }

    public void RefreshDropText(Dropdown dropdown)
    {
        dropdown.RefreshShownValue();
    }


    public void RestoreSetting()
    {
        //curSetData = new SO_SettingData(
        //    orignSettingData.num_Language,
        //    orignSettingData.num_Quality,
        //    orignSettingData.num_MasterVol,
        //    orignSettingData.num_BGMVol,
        //    orignSettingData.num_SEVol,
        //    orignSettingData.num_FullScreen,
        //    orignSettingData.num_AntiAliasing,
        //    orignSettingData.num_vSync,
        //    orignSettingData.num_Resoulution
        //    );


        curSetData.num_Language= orignSettingData.num_Language;
        curSetData.num_Quality= orignSettingData.num_Quality;
        curSetData.num_MasterVol=orignSettingData.num_MasterVol;
        curSetData.num_BGMVol=orignSettingData.num_BGMVol;
        curSetData.num_SEVol=orignSettingData.num_SEVol;
        curSetData.num_FullScreen=orignSettingData.num_FullScreen;
        curSetData.num_AntiAliasing=orignSettingData.num_AntiAliasing;
        curSetData.num_vSync= orignSettingData.num_vSync;
        curSetData.num_Resoulution=orignSettingData.num_Resoulution;
            
        UIM_SettingPanelManager.Instance.DropdownReadData();
        print("已恢复为初始默认值");
        //ReadData();
    }


}
