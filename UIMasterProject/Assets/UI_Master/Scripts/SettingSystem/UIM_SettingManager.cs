
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[AddComponentMenu("UIMaster/Setting/SettingManager")]
[DisallowMultipleComponent]
public class UIM_SettingManager : MonoBehaviour
{
    public static UIM_SettingManager instance;

    public AudioMixer mainAudioMixer;
    AsyncOperationHandle m_InitialzeOperation;

    public SO_SetttingOptions settingOtions;
    public SO_SettingData curSetData;
    public SO_SettingData orignSettingData;
    [Header("��ǰ����")]
    public SO_UIPalette curPalette;

    private Volume volume;

    private void Awake()
    {
        instance = this;
        
    }

  


    private void Start()
    {
        UIM_SaveLoad.LoadData(curSetData, "SettingData"); //��Ϸ��ʼʱ���ȶ�ȡ��������
        volume = GetComponent<Volume>();
        RefreshSetting();
    }

    /// <summary>
    /// ˢ��Ŀǰ�����ã�Ҳ�൱�ڶ�ȡ����Ϸ��ʼʱ��Ĭ�ϵ���һ�Ρ�
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
        OnChangeChromaticAberration(curSetData.num_ChromaticAberration);
        OnChangeFlimGrain(curSetData.num_FlimGrain);
        OnChangeVignette(curSetData.num_Vignette);
        OnChangeTheme(curSetData.num_Theme);
        OnChangeAllowedGF(curSetData.num_Theme);


        //print("���ó�ʼ���ѵ������");

    }



    #region �����������õķ���
    /// <summary>
    /// �ı�����ѡ��
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
    /// �ı�ͼ������
    /// </summary>
    /// <param name="i"></param>
    public void OnChangeQuality(int i)
    {
        QualitySettings.SetQualityLevel(i);
        UIM_UIManager.Instance.RefreshLayoutsRecursively();
    }

    /// <summary>
    /// ������С����
    /// </summary>
    /// <param name="s"></param>
    /// <param name="DP"></param>
    public void OnChangeVolume(string s, int DP)
    {
        var f = Mathf.Lerp(-80, 20, ((float)DP / (settingOtions.sliderStep - 1)));
        mainAudioMixer.SetFloat(s, f);
    }

    ///// <summary>
    ///// ����ȫ��
    ///// </summary>
    //public void OnChangeFullScreen(int i)
    //{
    //    Screen.fullScreen = i == 0 ? true : false;
    //}

    /// <summary>
    /// ������ֱͬ��
    /// </summary>
    public void OnChangevSync(int i)
    {
        // �л���ֱͬ��ģʽ
        QualitySettings.vSyncCount = i == 0 ? 1 : 0;
        //Debug.Log("VSync Mode Toggled: " + (QualitySettings.vSyncCount == 0 ? "Off" : "On"));
    }

    /// <summary>
    /// �޸Ŀ����
    /// </summary>
    public void OnChangeAntiAliasing(int i)
    {
        // �޸Ŀ����ģʽ
        QualitySettings.antiAliasing = settingOtions.antiAliasingList[i];
        //Debug.Log("Anti-Aliasing Mode Set to: " + antiAliasingList[i] + "x MSAA");
    }

    /// <summary>
    /// �޸ķֱ���
    /// </summary>
    public void OnChangeResolustion(int i,int fs,int y)
    {
        var v2 = settingOtions.resolutionList[i];
   
        SetResolution((int)v2.x, (int)v2.y);

       
        void SetResolution(int width, int height)
        {
            // �޸ķֱ���
            Screen.SetResolution(width, height, (fs == 0 ? true : false),settingOtions.refreshRateList[y]);
            //Debug.Log("Resolution Set to: " + width + "x" + height);
        }
    }



    /// <summary>
    /// �޸�֡��
    /// </summary>
    public void OnChangeFrameRate(int i)
    {
        Application.targetFrameRate = settingOtions.frameRateList[i];
    }


    public void OnChangeChromaticAberration(int i)
    {

        volume.profile.TryGet<ChromaticAberration>(out var x);
        x.active = i == 0 ? true : false;

    }

    public void OnChangeFlimGrain(int i)
    {
        volume.profile.TryGet<FilmGrain>(out var x);
        x.active = i == 0 ? true : false;
    }

    public void OnChangeVignette(int i)
    {
        volume.profile.TryGet<Vignette>(out var x);
        x.active = i == 0 ? true : false;
    }

    /// <summary>
    /// �޸ĵ�ǰ������ɫ
    /// </summary>
    /// <param name="i"></param>
    public void OnChangeTheme(int i)
    {
        Camera.main.backgroundColor = settingOtions.paletteList[i].colors[0];
        curPalette = settingOtions.paletteList[i];
        UIM_EventManager.TriggerChangeColor(curPalette);
    }

    public void OnChangeAllowedGF(int i)
    {

    }


    #endregion





    public void RefreshDropText(Dropdown dropdown)
    {
        dropdown.RefreshShownValue();
    }


    /// <summary>
    /// �����ݻָ���ʼ��
    /// </summary>
    public void RestoreSetting()
    {
        curSetData.CopyNewData(orignSettingData);
        UIM_SettingPanel.Instance.DropdownWRData(UIM_SettingPanel.WRmod.��ȡ);
        print(UIM_SaveLoad.YellowT()+ "�ѻָ�Ϊ��ʼĬ��ֵ!");

    }




}
