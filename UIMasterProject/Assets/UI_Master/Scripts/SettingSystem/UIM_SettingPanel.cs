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

    [Header("��ק��Ӧ���")]
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

    [Header("���ܰ�ť")]
    public Button butRestore;
    public Button butSave;

    private List<TMP_Dropdown.OptionData> dataListSlider;

    private UIM_SettingManager SM;
    private SO_SettingData sData;
    private SO_SetttingOptions sOptions;

    //�ȸ���Ҫ��˫ͷ��ť�������󶨵����񣬺ͳ�ʼ����
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
        //��ʼ�����ֳ���options
        for (int i = 0; i < sOptions.sliderStep; i++)
        {
            dataListSlider.Add(new TMP_Dropdown.OptionData(i.ToString()));
        }

        //��Ӽ���
        DropDownsAddListener();

        butRestore.onClick.AddListener(SM.RestoreSetting);
        butSave.onClick.AddListener(delegate { DropdownWRData(WRmod.д��); });

        //����ѡ��ѡ��ʵ��������
        InitializeDDOption();

        //��ȡ����
        DropdownWRData(WRmod.��ȡ);
        FreshTwoHead();
        //bdLanguage.RefreshShownValue();
        //ˢ�¿��
        UIM_UIManager.Instance.RefreshLayoutsRecursively();
    }

    /// <summary>
    /// Ϊ���е�dd��Ӽ����¼���
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
    /// �������ѡ��
    /// </summary>
    public void InitializeDDOption()
    {
        //����ѡ������
        InitiaOptionsLanguage();
        InitiaOptionsQuality();
        InitiaOptionsResolution();
        InitiaOptionsAntiAliasing();
        InitiaOptionsRefreshRate();
        InitiaOptionsFrameRate();
        InitiaOptionsTheme();

        //������ѡ������
        InitiaOptions(ddFullScreen, DPmod.����);
        InitiaOptions(ddvSync, DPmod.����);
        InitiaOptions(ddChromaticAberration, DPmod.����);
        InitiaOptions(ddFlimGrain, DPmod.����);
        InitiaOptions(ddVignette, DPmod.����);
        InitiaOptions(ddAllowedGF, DPmod.����);

        //����sliderѡ������
        InitiaOptions(ddMasterVol, DPmod.����);
        InitiaOptions(ddBGMVol, DPmod.����);
        InitiaOptions(ddSEVol, DPmod.����);
    }


    /// <summary>
    /// ˢ������������е�����˫ͷѡ��
    /// </summary>
    public void FreshTwoHead()
    {
        UIM_ButTwoHead[] twohead = GetComponentsInChildren<UIM_ButTwoHead>();
        foreach (var item in twohead)
        {
            item.FreshButton();
        }
    }


    #region ����ѡ��
    /// <summary>
    /// ��ʼ������ѡ��
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
    /// ��ʼ��������ѡ��
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
    /// ��ʼ�������ѡ��
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
    /// ��ʼ��ͼ������ѡ��
    /// </summary>
    public void InitiaOptionsQuality()
    {
        ddQuality.ClearOptions();
        var list = new List<TMP_Dropdown.OptionData>();
        // ��ȡ���л���������������ƺ�����
        string[] qualityNames = QualitySettings.names;
        int qualityCount = QualitySettings.names.Length;

        // ��ӡ������л���������������ƺ�����
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
    /// ˢ����ѡ������
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
    /// ֡��ѡ������
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
    /// ����ѡ������
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
        ��ȡ,
        д��
    }

    public void DropdownWRData(WRmod mod)
    {
        #region У��UIʵ��������SO����
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
            case WRmod.��ȡ:
                DropdownReadData();
                //DataCover(bdValues, sDataValues);
                print(UIM_SaveLoad.GreenT() + "����UI�Ѷ�ȡ�����ݲ���ʾ��");
                break;
            case WRmod.д��:
                //DataCover(sDataValues, bdValues);
                DropdownWriteData();
                UIM_SaveLoad.SaveData(sData, "SettingData"); //д�뱾��json������
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
        ����, ����
    }

    public void InitiaOptions(TMP_Dropdown dropdown, DPmod dPmod)
    {
        dropdown.ClearOptions();
        if (dPmod == DPmod.����)
        {

            var dataListBool = new List<TMP_Dropdown.OptionData>();
            dataListBool.Add(new TMP_Dropdown.OptionData("ON"));
            dataListBool.Add(new TMP_Dropdown.OptionData("OFF"));
            dropdown.AddOptions(dataListBool);
        }
        else if (dPmod == DPmod.����)
        {
            dropdown.AddOptions(dataListSlider);
        }

    }


}
