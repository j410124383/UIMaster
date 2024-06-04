using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;


[AddComponentMenu("UIMaster/Setting/SettingPanel")]
[DisallowMultipleComponent]
public class UIM_SettingPanel : MonoBehaviour
{

    public static UIM_SettingPanel Instance;

    [Header("拖拽对应组件")]
    public TMP_Dropdown bdLanguage;
    public TMP_Dropdown bdTheme;

    public TMP_Dropdown bdQuality;
    public TMP_Dropdown bdMasterVol;
    public TMP_Dropdown bdBGM;
    public TMP_Dropdown bdSE;
    public TMP_Dropdown bdFullScreen;
    public TMP_Dropdown bdAntiAliasing;
    public TMP_Dropdown bdvSync;
    public TMP_Dropdown bdResolution;
    public TMP_Dropdown bdFrameRate;
    public TMP_Dropdown bdRefreshRate;
    //urp
    public TMP_Dropdown bdChromaticAberration;
    public TMP_Dropdown bdFlimGrain;
    public TMP_Dropdown bdVignette;

    public TMP_Dropdown bdAllowedGF;

    [Header("功能按钮")]
    public Button butRestore;
    public Button butSave;

    private List<TMP_Dropdown.OptionData> dataListSlider;

    private UIM_SettingManager us;
    private SO_SettingData sdata;
    private SO_SetttingOptions sOptions;

    //先给需要的双头按钮，发布绑定的任务，和初始化。
    private void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        us = UIM_SettingManager.Instance ? us = UIM_SettingManager.Instance : null;
        sdata = us.curSetData;
        sOptions =us.settingOtions;

        dataListSlider = new List<TMP_Dropdown.OptionData>();
        //初始化两种常用options
        for (int i = 0; i < sOptions.sliderStep; i++)
        {
            dataListSlider.Add(new TMP_Dropdown.OptionData(i.ToString()));
        }

        //初始化setting的几种需要dropdown

        bdLanguage.onValueChanged.AddListener(delegate { us.OnChangeLanguage(bdLanguage.value); });
        bdQuality.onValueChanged.AddListener(delegate { us.OnChangeQuality(bdQuality.value); });
        bdMasterVol.onValueChanged.AddListener(delegate { us.OnChangeVolume("MasterVol", bdMasterVol.value); });
        bdBGM.onValueChanged.AddListener(delegate { us.OnChangeVolume("BGMVol", bdBGM.value); });
        bdSE.onValueChanged.AddListener(delegate { us.OnChangeVolume("SFVol", bdSE.value); });
        //bdFullScreen.onValueChanged.AddListener(delegate { us.OnChangeFullScreen(bdFullScreen.value); });
        bdvSync.onValueChanged.AddListener(delegate { us.OnChangevSync(bdvSync.value); });
        bdAntiAliasing.onValueChanged.AddListener(delegate { us.OnChangeAntiAliasing(bdAntiAliasing.value); });

        bdFullScreen.onValueChanged.AddListener(delegate { us.OnChangeResolustion(bdResolution.value, bdFullScreen.value, bdRefreshRate.value); });
        bdResolution.onValueChanged.AddListener(delegate { us.OnChangeResolustion(bdResolution.value, bdFullScreen.value, bdRefreshRate.value); });
        bdRefreshRate.onValueChanged.AddListener(delegate { us.OnChangeResolustion(bdResolution.value, bdFullScreen.value, bdRefreshRate.value); });

        bdFrameRate.onValueChanged.AddListener(delegate { us.OnChangeFrameRate(bdFrameRate.value); });



        butRestore.onClick.AddListener(us.RestoreSetting);
        butSave.onClick.AddListener(DropdownWriteData);

        //添加下拉选项
        InitiabdLanguage();
        InitiabdQuality();
        InitiabdResolution();
        InitiabdAntiAliasing();
        InitiabdRefreshRate();
        InitiabdFrameRate();

        InitiaDropDown(bdFullScreen, DPmod.布尔);
        InitiaDropDown(bdvSync, DPmod.布尔);

        InitiaDropDown(bdMasterVol, DPmod.线性);
        InitiaDropDown(bdBGM, DPmod.线性);
        InitiaDropDown(bdSE, DPmod.线性);


        //读取数据
        DropdownReadData();
        FreshTwoHead();
        //bdLanguage.RefreshShownValue();
        //刷新框架
        UIM_UIManager.Instance.RefreshLayoutsRecursively();
    }



    /// <summary>
    /// 刷新子物体组件中的所有双头选项
    /// </summary>
    public void FreshTwoHead()
    {
        UIM_ButTwoHead[] twohead = GetComponentsInChildren<UIM_ButTwoHead>();
        foreach (var item in twohead)
        {
            item.FreshButton();
        }
    }



        /// <summary>
    /// 初始化语言选项
    /// </summary>
    public void InitiabdLanguage()
    {
        
        bdLanguage.ClearOptions();
        var list = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < sOptions.locales.Count; i++)
        {
            list.Add(new TMP_Dropdown.OptionData(sOptions.locales[i].ToString()));
        }

        bdLanguage.AddOptions(list);
    }



    /// <summary>
    /// 初始化解析度选项
    /// </summary>
    public void InitiabdResolution()
    {
        bdResolution.ClearOptions();
        var reList = sOptions.resolutionList;
        var list = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < reList.Count; i++)
        {
            list.Add(new TMP_Dropdown.OptionData(reList[i].x+"x" + reList[i].y));
        }
        bdResolution.AddOptions(list);
    }

    /// <summary>
    /// 初始化抗锯齿选项
    /// </summary>
    public void InitiabdAntiAliasing()
    {
        bdAntiAliasing.ClearOptions();
        var list = new List<TMP_Dropdown.OptionData>();
        var antiList = sOptions.antiAliasingList;
        for (int i = 0; i < antiList.Count; i++)
        {
            list.Add(new TMP_Dropdown.OptionData((float)antiList[i]+ "x MSAA"));
        }
        bdAntiAliasing.AddOptions(list);

    }



    /// <summary>
    /// 初始化图像质量选项
    /// </summary>
    public void InitiabdQuality()
    {
        bdQuality.ClearOptions();
        var list = new List<TMP_Dropdown.OptionData>();
        // 获取所有画面质量级别的名称和数量
        string[] qualityNames = QualitySettings.names;
        int qualityCount = QualitySettings.names.Length;

        // 打印输出所有画面质量级别的名称和数量
        //Debug.Log("Quality Levels:");
        for (int i = 0; i < qualityCount; i++)
        {
            //Debug.Log(qualityNames[i]);
            list.Add(new TMP_Dropdown.OptionData(qualityNames[i]));
        }
        //Debug.Log("Total Quality Levels: " + qualityCount);

        bdQuality.AddOptions(list);

    }

    /// <summary>
    /// 刷新率选项生成
    /// </summary>
    public void InitiabdRefreshRate()
    {
        bdRefreshRate.ClearOptions();
        var list= new List<TMP_Dropdown.OptionData>();
        var refList = sOptions.refreshRateList;
        for (int i = 0; i < refList.Count; i++)
        {
            list.Add(new TMP_Dropdown.OptionData((float)refList[i]+" Hz"));
        }
        bdRefreshRate.AddOptions(list);

    }


    /// <summary>
    /// 帧率选项生成
    /// </summary>
    public void InitiabdFrameRate()
    {
        bdFrameRate.ClearOptions();
        var list = new List<TMP_Dropdown.OptionData>();
        var fList = sOptions.frameRateList;
        for (int i = 0; i < fList.Count; i++)
        {
            list.Add(new TMP_Dropdown.OptionData((float)fList[i] + " FPS"));
        }
        bdFrameRate.AddOptions(list);


    }










    /// <summary>
    /// 为Dropdown的value赋值，ui读取数据
    /// </summary>
    public void DropdownReadData()
    {

        bdLanguage.value = sdata.num_Language;
        bdMasterVol.value = sdata.num_MasterVol;
        bdSE.value = sdata.num_SEVol;
        bdBGM.value = sdata.num_BGMVol;
        bdQuality.value = sdata.num_Quality;
        bdFullScreen.value = sdata.num_FullScreen;
        bdvSync.value = sdata.num_vSync;
        bdAntiAliasing.value = sdata.num_AntiAliasing;
        bdResolution.value = sdata.num_Resoulution;
        FreshTwoHead();
        print(UIM_SaveLoad.GreenT()+"界面UI已输入数据并完成显示！");

    }

    /// <summary>
    /// 写入数据
    /// </summary>
    public void DropdownWriteData()
    {
        sdata.num_Language = bdLanguage.value;
        sdata.num_MasterVol = bdMasterVol.value;
        sdata.num_SEVol = bdSE.value;
        sdata.num_BGMVol = bdBGM.value;
        sdata.num_Quality = bdQuality.value;
        sdata.num_FullScreen = bdFullScreen.value;
        sdata.num_vSync = bdvSync.value;
        sdata.num_AntiAliasing = bdAntiAliasing.value;
        sdata.num_Resoulution = bdResolution.value;
        FreshTwoHead();
        UIM_SaveLoad.SaveData(sdata, "SettingData"); //写入本地json数据中
        //print("设置数据存储成功");
    }

    public enum DPmod
    {
        布尔, 线性
    }

    public void InitiaDropDown(TMP_Dropdown dropdown, DPmod dPmod)
    {
        dropdown.ClearOptions();
        if (dPmod == DPmod.布尔)
        {

            var dataListBool = new List<TMP_Dropdown.OptionData>();
            dataListBool.Add(new TMP_Dropdown.OptionData("ON"));
            dataListBool.Add(new TMP_Dropdown.OptionData("OFF"));
            dropdown.AddOptions(dataListBool);
        }
        else if (dPmod == DPmod.线性)
        {
            dropdown.AddOptions(dataListSlider);
        }

    }


}
