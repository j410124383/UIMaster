
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;


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
        UIT_SaveLoad.LoadData(curSetData, "SettingData"); //��Ϸ��ʼʱ���ȶ�ȡ��������
        RefreshSetting();
    }

    /// <summary>
    /// ˢ��Ŀǰ�����ã�Ҳ�൱�ڶ�ȡ����Ϸ��ʼʱ��Ĭ�ϵ���һ�Ρ�
    /// </summary>
    void RefreshSetting()
    {
        OnChangeFullScreen(curSetData.num_FullScreen);
        OnChangeQuality(curSetData.num_Quality);
        OnChangeVolume("MasterVol", curSetData.num_MasterVol);
        OnChangeVolume("BGMVol", curSetData.num_BGMVol);
        OnChangeVolume("SFVol", curSetData.num_SEVol);
        OnChangevSync(curSetData.num_vSync);
        OnChangeAntiAliasing(curSetData.num_AntiAliasing);
        OnChangeResolustion(curSetData.num_Resoulution);
        OnChangeLanguage(curSetData.num_Language);
        //print("���ó�ʼ���ѵ������");

    }


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

    /// <summary>
    /// ����ȫ��
    /// </summary>
    public void OnChangeFullScreen(int i)
    {
        Screen.fullScreen = i == 0 ? true : false;
    }

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
    public void OnChangeResolustion(int i)
    {
        var v2 = settingOtions.resolutionList[i];
        // �޸ķֱ���
        SetResolution((int)v2.x, (int)v2.y);


        void SetResolution(int width, int height)
        {
            // �޸ķֱ���
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
        curSetData.CopyNewData(orignSettingData);
        UIM_SettingPanel.Instance.DropdownReadData();
        print("�ѻָ�Ϊ��ʼĬ��ֵ");

    }




}
