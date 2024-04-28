using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class UIM_SettingManager : MonoBehaviour
{
    //设置类相关脚本，主要实现目前的基本功能
    public List<int> antiAliasingList;

    public List<Vector2> resolutionList;
    public int sliderStep = 11;

    [Header("拖拽对应组件")]
    public TMP_Dropdown bdLanguage;
    public TMP_Dropdown bdQuality;
    public TMP_Dropdown bdMasterVol;
    public TMP_Dropdown bdBGM;
    public TMP_Dropdown bdSE;
    public TMP_Dropdown bdFullScreen;
    public TMP_Dropdown bdAntiAliasing;
    public TMP_Dropdown bdvSync;
    public TMP_Dropdown bdResolution;

    [Header("功能按钮")]
    public Button butRestore;
    public Button butSave;

    private List<TMP_Dropdown.OptionData> dataListSlider;

    public AudioMixer mainAudioMixer;
    AsyncOperationHandle m_InitialzeOperation;

    public SO_SettingData curSetData;
    public SO_SettingData orignSettingData;

    //先给需要的双头按钮，发布绑定的任务，和初始化。
    private void Awake()
    {
        dataListSlider = new List<TMP_Dropdown.OptionData>();
        //初始化两种常用options
        for (int i = 0; i < sliderStep; i++)
        {
            dataListSlider.Add(new TMP_Dropdown.OptionData(i.ToString()));
        }


        //初始化setting的几种需要dropdown
        m_InitialzeOperation = LocalizationSettings.SelectedLocaleAsync;
        bdLanguage.onValueChanged.AddListener(delegate { OnChangeLanguage(bdLanguage.value); }) ;
        bdQuality.onValueChanged.AddListener(delegate { OnChangeQuality(bdQuality.value); });
        bdMasterVol.onValueChanged.AddListener(delegate { OnChangeVolume("MasterVol", bdMasterVol.value); });
        bdBGM.onValueChanged.AddListener(delegate { OnChangeVolume("BGMVol", bdBGM.value); });
        bdSE.onValueChanged.AddListener(delegate { OnChangeVolume("SFVol",bdSE.value); });
        bdFullScreen.onValueChanged.AddListener(delegate { OnChangeFullScreen(bdFullScreen.value); });
        bdvSync.onValueChanged.AddListener(delegate { OnChangevSync(bdvSync.value); });
        bdAntiAliasing.onValueChanged.AddListener(delegate { OnChangeAntiAliasing(bdAntiAliasing.value); });
        bdResolution.onValueChanged.AddListener(delegate { OnChangeResolustion(bdResolution.value); });

        butRestore.onClick.AddListener(RestoreSetting);
        butSave.onClick.AddListener(WriteData);

        //添加下拉选项
        InitiabdLanguage();
        InitiabdQuality();
        initiabdResolution();
        initiabdAntiAliasing();
        InitiaDropDown(bdFullScreen, DPmod.布尔);
        InitiaDropDown(bdvSync, DPmod.布尔);
        
        InitiaDropDown(bdMasterVol,DPmod.线性);
        InitiaDropDown(bdBGM, DPmod.线性);
        InitiaDropDown(bdSE, DPmod.线性);


        //读取数据
        ReadData();
        RefreshSetting();
        bdLanguage.RefreshShownValue();
        UIM_UIManager.Instance.RefreshLayoutsRecursively();
    }

    void RefreshSetting()
    {
        OnChangeLanguage(curSetData.num_Language);
        OnChangeQuality(curSetData.num_Quality);
        OnChangeVolume("MasterVol", curSetData.num_MasterVol);
        OnChangeVolume("BGMVol", curSetData.num_BGMVol);
        OnChangeVolume("SFVol", curSetData.num_SEVol);
        OnChangeFullScreen(curSetData.num_FullScreen);
        OnChangevSync(curSetData.num_vSync);
        OnChangeAntiAliasing(curSetData.initial_AntiAliasing);
        OnChangeResolustion(curSetData.num_Resoulution);


    }






    /// <summary>
    /// 初始化解析度选项
    /// </summary>
    public void initiabdResolution()
    {
        bdResolution.ClearOptions();
        var reList = resolutionList;
        List<TMP_Dropdown.OptionData> list = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < reList.Count; i++)
        {
            list.Add(new TMP_Dropdown.OptionData(reList[i].x+"x" + reList[i].y));
        }
        bdResolution.AddOptions(list);
    }

    /// <summary>
    /// 初始化抗锯齿选项
    /// </summary>
    public void initiabdAntiAliasing()
    {
        bdAntiAliasing.ClearOptions();
        List<TMP_Dropdown.OptionData> list = new List<TMP_Dropdown.OptionData>();
        var antiList = antiAliasingList;
        for (int i = 0; i < antiList.Count; i++)
        {
            list.Add(new TMP_Dropdown.OptionData((float)antiList[i]+ "x MSAA"));
        }
        bdAntiAliasing.AddOptions(list);

    }

    /// <summary>
    /// 初始化语言选项
    /// </summary>
    public void InitiabdLanguage()
    {
        
        bdLanguage.ClearOptions();
        List<TMP_Dropdown.OptionData> list = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
        {
            var x = LocalizationSettings.AvailableLocales.Locales[i];
            list.Add(new TMP_Dropdown.OptionData(x.LocaleName.ToString()));
        }

        bdLanguage.AddOptions(list);
    }

    /// <summary>
    /// 初始化图像质量选项
    /// </summary>
    public void InitiabdQuality()
    {
        bdQuality.ClearOptions();
        List<TMP_Dropdown.OptionData> lsit = new List<TMP_Dropdown.OptionData>();
        // 获取所有画面质量级别的名称和数量
        string[] qualityNames = QualitySettings.names;
        int qualityCount = QualitySettings.names.Length;

        // 打印输出所有画面质量级别的名称和数量
        //Debug.Log("Quality Levels:");
        for (int i = 0; i < qualityCount; i++)
        {
            //Debug.Log(qualityNames[i]);
            lsit.Add(new TMP_Dropdown.OptionData(qualityNames[i]));
        }
        //Debug.Log("Total Quality Levels: " + qualityCount);

        bdQuality.AddOptions(lsit);

    }



    /// <summary>
    /// 改变语言选项
    /// </summary>
    /// <param name="i"></param>
    public void OnChangeLanguage(int i)
    {

        var l = LocalizationSettings.AvailableLocales.Locales;
        LocalizationSettings.Instance.SetSelectedLocale(l[i]);
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
    public void OnChangeVolume(string s,int DP)
    {
        var f=Mathf.Lerp( -80,20,((float)DP / (sliderStep - 1) ));
        mainAudioMixer.SetFloat(s,f);
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
        QualitySettings.vSyncCount = i==0 ? 1 : 0;
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

    public enum DPmod
    {
        布尔,线性
    }

    public void InitiaDropDown(TMP_Dropdown dropdown,DPmod dPmod)
    {
        dropdown.ClearOptions();
        if (dPmod == DPmod.布尔)
        {
            
            var dataListBool = new List<TMP_Dropdown.OptionData>();
            dataListBool.Add(new TMP_Dropdown.OptionData("ON"));
            dataListBool.Add(new TMP_Dropdown.OptionData("OFF"));
            dropdown.AddOptions(dataListBool);
        }
        else if (dPmod==DPmod.线性)
        {
            dropdown.AddOptions(dataListSlider);
        }
       
    }




    /// <summary>
    /// 读取数据
    /// </summary>
    public void ReadData()
    {
        LoadData();
        bdLanguage.value = curSetData.num_Language;
        bdMasterVol.value = curSetData.num_MasterVol;
        bdSE.value = curSetData.num_SEVol;
        bdBGM.value = curSetData.num_BGMVol;
        bdQuality.value = curSetData.num_Quality;
        bdFullScreen.value = curSetData.num_FullScreen;
        bdvSync.value = curSetData.num_vSync;
        bdAntiAliasing.value = curSetData.num_AntiAliasing;
        bdResolution.value = curSetData.num_Resoulution;
        print("设置数据写入成功，已为bd赋值");
    }

    /// <summary>
    /// 写入数据
    /// </summary>
    public void WriteData()
    {
        curSetData.num_Language= bdLanguage.value ;
        curSetData.num_MasterVol= bdMasterVol.value ;
        curSetData.num_SEVol= bdSE.value ;
        curSetData.num_BGMVol= bdBGM.value;
        curSetData.num_Quality= bdQuality.value ;
        curSetData.num_FullScreen= bdFullScreen.value;
        curSetData.num_vSync= bdvSync.value;
        curSetData.num_AntiAliasing= bdAntiAliasing.value ;
        curSetData.num_Resoulution= bdResolution.value ;
        SaveData();
        print("设置数据存储成功");
    }

    public void SaveData()
    {
        // 将数据对象转换为 JSON 格式的字符串
        string json = JsonConvert.SerializeObject(curSetData);

        // 存储 JSON 字符串
        PlayerPrefs.SetString("SettingData", json);
        PlayerPrefs.Save();
    }


    public SO_SettingData LoadData()
    {
        // 从 PlayerPrefs 中加载 JSON 字符串
        string json = PlayerPrefs.GetString("SettingData", "{}");

        // 将 JSON 字符串转换为数据对象
        SO_SettingData data = JsonConvert.DeserializeObject<SO_SettingData>(json);

        return data;
    }

    public void RestoreSetting()
    {
        curSetData.ResetToInitialValues();
        ReadData();
    }


}
