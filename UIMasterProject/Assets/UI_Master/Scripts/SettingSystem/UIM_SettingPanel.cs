using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using System.Reflection;

[AddComponentMenu("UIMaster/Setting/SettingPanel")]
[DisallowMultipleComponent]
public class UIM_SettingPanel : MonoBehaviour
{

    public static UIM_SettingPanel Instance;

    [Header("拖拽对应组件")]
    public TMP_Dropdown ddLanguage;
    public TMP_Dropdown ddTheme;

    public TMP_Dropdown ddQuality;
    public TMP_Dropdown ddMasterVol;
    public TMP_Dropdown ddBGMVol;
    public TMP_Dropdown ddSEVol;
    public TMP_Dropdown ddFullScreen;
    public TMP_Dropdown ddAntiAliasing;
    public TMP_Dropdown ddvSync;
    public TMP_Dropdown ddResolution;
    public TMP_Dropdown ddFrameRate;
    public TMP_Dropdown ddRefreshRate;
    //urp
    public TMP_Dropdown ddChromaticAberration;
    public TMP_Dropdown ddFlimGrain;
    public TMP_Dropdown ddVignette;

    public TMP_Dropdown ddAllowedGF;

    [Header("功能按钮")]
    public Button butRestore;
    public Button butSave;

    private List<TMP_Dropdown.OptionData> dataListSlider;

    private UIM_SettingManager SM;
    private SO_SettingData sData;
    private SO_SetttingOptions sOptions;

    //先给需要的双头按钮，发布绑定的任务，和初始化。
    private void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        SM = UIM_SettingManager.instance;
        sData = SM.curSetData;
        sOptions = SM.settingOtions;


        dataListSlider = new List<TMP_Dropdown.OptionData>();
        //初始化两种常用options
        for (int i = 0; i < sOptions.sliderStep; i++)
        {
            dataListSlider.Add(new TMP_Dropdown.OptionData(i.ToString()));
        }

        //添加监听
        DropDownsAddListener();

        butRestore.onClick.AddListener(SM.RestoreSetting);
        butSave.onClick.AddListener(delegate { DropdownWRData(WRmod.写入); });

        //下拉选项选项实例化生成
        InitializeDDOption();

        //读取数据
        DropdownWRData(WRmod.读取);
        FreshTwoHead();
        //bdLanguage.RefreshShownValue();
        //刷新框架
        UIM_UIManager.Instance.RefreshLayoutsRecursively();
    }

    /// <summary>
    /// 为已有的dd添加监听事件。
    /// </summary>
    public void DropDownsAddListener()
    {
        ddLanguage.onValueChanged.AddListener(delegate { SM.OnChangeLanguage(ddLanguage.value); });
        ddQuality.onValueChanged.AddListener(delegate { SM.OnChangeQuality(ddQuality.value); });
        ddMasterVol.onValueChanged.AddListener(delegate { SM.OnChangeVolume("MasterVol", ddMasterVol.value); });
        ddBGMVol.onValueChanged.AddListener(delegate { SM.OnChangeVolume("BGMVol", ddBGMVol.value); });
        ddSEVol.onValueChanged.AddListener(delegate { SM.OnChangeVolume("SFVol", ddSEVol.value); });
        //bdFullScreen.onValueChanged.AddListener(delegate { us.OnChangeFullScreen(bdFullScreen.value); });
        ddvSync.onValueChanged.AddListener(delegate { SM.OnChangevSync(ddvSync.value); });
        ddAntiAliasing.onValueChanged.AddListener(delegate { SM.OnChangeAntiAliasing(ddAntiAliasing.value); });

        ddFullScreen.onValueChanged.AddListener(delegate { SM.OnChangeResolustion(ddResolution.value, ddFullScreen.value, ddRefreshRate.value); });
        ddResolution.onValueChanged.AddListener(delegate { SM.OnChangeResolustion(ddResolution.value, ddFullScreen.value, ddRefreshRate.value); });
        ddRefreshRate.onValueChanged.AddListener(delegate { SM.OnChangeResolustion(ddResolution.value, ddFullScreen.value, ddRefreshRate.value); });

        ddFrameRate.onValueChanged.AddListener(delegate { SM.OnChangeFrameRate(ddFrameRate.value); });

        ddChromaticAberration.onValueChanged.AddListener(delegate { SM.OnChangeChromaticAberration(ddChromaticAberration.value); });
        ddFlimGrain.onValueChanged.AddListener(delegate { SM.OnChangeFlimGrain(ddFlimGrain.value); });
        ddVignette.onValueChanged.AddListener(delegate { SM.OnChangeVignette(ddVignette.value); });

        ddTheme.onValueChanged.AddListener(delegate { SM.OnChangeTheme(ddTheme.value); });
        ddAllowedGF.onValueChanged.AddListener(delegate { SM.OnChangeAllowedGF(ddAllowedGF.value); });


    }


    /// <summary>
    /// 添加下拉选项
    /// </summary>
    public void InitializeDDOption()
    {
        //特殊选项生成
        InitiaOptionsLanguage();
        InitiaOptionsQuality();
        InitiaOptionsResolution();
        InitiaOptionsAntiAliasing();
        InitiaOptionsRefreshRate();
        InitiaOptionsFrameRate();
        InitiaOptionsTheme();

        //布尔类选项生成
        InitiaOptions(ddFullScreen, DPmod.布尔);
        InitiaOptions(ddvSync, DPmod.布尔);
        InitiaOptions(ddChromaticAberration, DPmod.布尔);
        InitiaOptions(ddFlimGrain, DPmod.布尔);
        InitiaOptions(ddVignette, DPmod.布尔);
        InitiaOptions(ddAllowedGF, DPmod.布尔);

        //线性slider选项生成
        InitiaOptions(ddMasterVol, DPmod.步进);
        InitiaOptions(ddBGMVol, DPmod.步进);
        InitiaOptions(ddSEVol, DPmod.步进);
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


    #region 生成选项
    /// <summary>
    /// 初始化语言选项
    /// </summary>
    public void InitiaOptionsLanguage()
    {

        ddLanguage.ClearOptions();
        var list = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < sOptions.locales.Count; i++)
        {
            list.Add(new TMP_Dropdown.OptionData(sOptions.locales[i].ToString()));
        }

        ddLanguage.AddOptions(list);
    }



    /// <summary>
    /// 初始化解析度选项
    /// </summary>
    public void InitiaOptionsResolution()
    {
        ddResolution.ClearOptions();
        var reList = sOptions.resolutionList;
        var list = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < reList.Count; i++)
        {
            list.Add(new TMP_Dropdown.OptionData(reList[i].x + "x" + reList[i].y));
        }
        ddResolution.AddOptions(list);
    }

    /// <summary>
    /// 初始化抗锯齿选项
    /// </summary>
    public void InitiaOptionsAntiAliasing()
    {
        ddAntiAliasing.ClearOptions();
        var list = new List<TMP_Dropdown.OptionData>();
        var antiList = sOptions.antiAliasingList;
        for (int i = 0; i < antiList.Count; i++)
        {
            list.Add(new TMP_Dropdown.OptionData((float)antiList[i] + "x MSAA"));
        }
        ddAntiAliasing.AddOptions(list);

    }



    /// <summary>
    /// 初始化图像质量选项
    /// </summary>
    public void InitiaOptionsQuality()
    {
        ddQuality.ClearOptions();
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

        ddQuality.AddOptions(list);

    }

    /// <summary>
    /// 刷新率选项生成
    /// </summary>
    public void InitiaOptionsRefreshRate()
    {
        ddRefreshRate.ClearOptions();
        var list = new List<TMP_Dropdown.OptionData>();
        var refList = sOptions.refreshRateList;
        for (int i = 0; i < refList.Count; i++)
        {
            list.Add(new TMP_Dropdown.OptionData((float)refList[i] + " Hz"));
        }
        ddRefreshRate.AddOptions(list);

    }


    /// <summary>
    /// 帧率选项生成
    /// </summary>
    public void InitiaOptionsFrameRate()
    {
        ddFrameRate.ClearOptions();
        var list = new List<TMP_Dropdown.OptionData>();
        var fList = sOptions.frameRateList;
        for (int i = 0; i < fList.Count; i++)
        {
            list.Add(new TMP_Dropdown.OptionData((float)fList[i] + " FPS"));
        }
        ddFrameRate.AddOptions(list);


    }

    /// <summary>
    /// 主题选项生成
    /// </summary>
    public void InitiaOptionsTheme()
    {

        ddTheme.ClearOptions();
        var list = new List<TMP_Dropdown.OptionData>();
        var fList = sOptions.paletteList;
        for (int i = 0; i < fList.Count; i++)
        {
            list.Add(new TMP_Dropdown.OptionData(fList[i].paletteName));
        }
        ddTheme.AddOptions(list);
    }

    #endregion

    public enum WRmod
    {
        读取,
        写入
    }

    public void DropdownWRData(WRmod mod)
    {
        #region 校对UI实际数据与SO数据
        void DropdownReadData()
        {
            foreach (FieldInfo field in sData.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                var tfield = "dd" + field.Name.Substring(4);
                //Debug.Log("dataName: "+tfield+"  fileName: "+ field.Name);
                TMP_Dropdown dropdown = (TMP_Dropdown)GetType().GetField(tfield).GetValue(this);
                Debug.Log(dropdown.name);
                dropdown.value = (int)field.GetValue(sData);
            }
        }

        void DropdownWriteData()
        {
            foreach (FieldInfo field in sData.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                var tfield = "dd" + field.Name.Substring(4);
                //Debug.Log("dataName: " + tfield + "  fileName: " + field.Name);
                TMP_Dropdown dropdown = (TMP_Dropdown)GetType().GetField(tfield).GetValue(this);
                field.SetValue(sData, dropdown.value);
            }
        }


       
        #endregion

        switch (mod)
        {
            case WRmod.读取:
                DropdownReadData();
                //DataCover(bdValues, sDataValues);
                print(UIM_SaveLoad.GreenT() + "界面UI已读取完数据并显示！");
                break;
            case WRmod.写入:
                //DataCover(sDataValues, bdValues);
                DropdownWriteData();
                UIM_SaveLoad.SaveData(sData, "SettingData"); //写入本地json数据中
                break;
            default:
                break;
        }


        void DataCover(List<int> target, List<int> origin)
        {
            for (int i = 0; i < target.Count; i++)
            {
                target[i] = origin[i];
            }
        }


        FreshTwoHead();

    }





    public enum DPmod
    {
        布尔, 步进
    }

    public void InitiaOptions(TMP_Dropdown dropdown, DPmod dPmod)
    {
        dropdown.ClearOptions();
        if (dPmod == DPmod.布尔)
        {

            var dataListBool = new List<TMP_Dropdown.OptionData>();
            dataListBool.Add(new TMP_Dropdown.OptionData("ON"));
            dataListBool.Add(new TMP_Dropdown.OptionData("OFF"));
            dropdown.AddOptions(dataListBool);
        }
        else if (dPmod == DPmod.步进)
        {
            dropdown.AddOptions(dataListSlider);
        }

    }


}
