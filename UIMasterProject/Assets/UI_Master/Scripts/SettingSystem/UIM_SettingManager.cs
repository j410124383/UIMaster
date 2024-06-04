
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;


[AddComponentMenu("UIMaster/Setting/SettingManager")]
[DisallowMultipleComponent]
public class UIM_SettingManager : MonoBehaviour
{
    public static UIM_SettingManager Instance;

    public AudioMixer mainAudioMixer;
    AsyncOperationHandle m_InitialzeOperation;

    public SO_SetttingOptions settingOtions;
    public SO_SettingData curSetData;
    public SO_SettingData orignSettingData;

    private void Awake()
    {
        Instance = this;
        
    }

  


    private void Start()
    {
        UIM_SaveLoad.LoadData(curSetData, "SettingData"); //游戏开始时，先读取设置数据
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
        OnChangevSync(curSetData.num_vSync);
        OnChangeAntiAliasing(curSetData.num_AntiAliasing);
        OnChangeResolustion(curSetData.num_Resoulution, curSetData.num_FullScreen, curSetData.num_RefreshRate);
        OnChangeLanguage(curSetData.num_Language);
        //OnChangeFullScreen(curSetData.num_FullScreen);
        OnChangeFrameRate(curSetData.num_FrameRate);
        //print("设置初始化已调整完毕");

    }


    /// <summary>
    /// 改变语言选项
    /// </summary>
    /// <param name="i"></param>
    public void OnChangeLanguage(int i)
    {

        //var l = LocalizationSettings.AvailableLocales.Locales;
       
        LocalizationSettings.Instance.SetSelectedLocale(settingOtions.locales[i]);
        //print(settingOtions.locales[i]);
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
        var f = Mathf.Lerp(-80, 20, ((float)DP / (settingOtions.sliderStep - 1)));
        mainAudioMixer.SetFloat(s, f);
    }

    ///// <summary>
    ///// 调整全屏
    ///// </summary>
    //public void OnChangeFullScreen(int i)
    //{
    //    Screen.fullScreen = i == 0 ? true : false;
    //}

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
        QualitySettings.antiAliasing = settingOtions.antiAliasingList[i];
        //Debug.Log("Anti-Aliasing Mode Set to: " + antiAliasingList[i] + "x MSAA");
    }

    /// <summary>
    /// 修改分辨率
    /// </summary>
    public void OnChangeResolustion(int i,int fs,int y)
    {
        var v2 = settingOtions.resolutionList[i];
   
        SetResolution((int)v2.x, (int)v2.y);

       
        void SetResolution(int width, int height)
        {
            // 修改分辨率
            Screen.SetResolution(width, height, (fs == 0 ? true : false),settingOtions.refreshRateList[y]);
            //Debug.Log("Resolution Set to: " + width + "x" + height);
        }
    }



    /// <summary>
    /// 修改帧率
    /// </summary>
    public void OnChangeFrameRate(int i)
    {
        Application.targetFrameRate = settingOtions.frameRateList[i];
    }



    public void RefreshDropText(Dropdown dropdown)
    {
        dropdown.RefreshShownValue();
    }


    /// <summary>
    /// 将数据恢复初始化
    /// </summary>
    public void RestoreSetting()
    {
        curSetData.CopyNewData(orignSettingData);
        UIM_SettingPanel.Instance.DropdownReadData();
        print(UIM_SaveLoad.YellowT()+ "已恢复为初始默认值!");

    }




}
