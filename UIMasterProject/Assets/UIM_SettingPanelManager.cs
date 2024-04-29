using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class UIM_SettingPanelManager : MonoBehaviour
{

    public static UIM_SettingPanelManager Instance;

    [Header("��ק��Ӧ���")]
    public TMP_Dropdown bdLanguage;
    public TMP_Dropdown bdQuality;
    public TMP_Dropdown bdMasterVol;
    public TMP_Dropdown bdBGM;
    public TMP_Dropdown bdSE;
    public TMP_Dropdown bdFullScreen;
    public TMP_Dropdown bdAntiAliasing;
    public TMP_Dropdown bdvSync;
    public TMP_Dropdown bdResolution;

    [Header("���ܰ�ť")]
    public Button butRestore;
    public Button butSave;

    private List<TMP_Dropdown.OptionData> dataListSlider;

    private UIM_UniversalSetting us;
    private SO_SettingData sdata;

    //�ȸ���Ҫ��˫ͷ��ť�������󶨵����񣬺ͳ�ʼ����
    private void Awake()
    {
        Instance = this;


    }

    private void Start()
    {
        us = UIM_UniversalSetting.Instance ? us = UIM_UniversalSetting.Instance : null;
        sdata = us.curSetData;
        dataListSlider = new List<TMP_Dropdown.OptionData>();
        //��ʼ�����ֳ���options
        for (int i = 0; i < us.sliderStep; i++)
        {
            dataListSlider.Add(new TMP_Dropdown.OptionData(i.ToString()));
        }

        //��ʼ��setting�ļ�����Ҫdropdown

        bdLanguage.onValueChanged.AddListener(delegate { us.OnChangeLanguage(bdLanguage.value); });
        bdQuality.onValueChanged.AddListener(delegate { us.OnChangeQuality(bdQuality.value); });
        bdMasterVol.onValueChanged.AddListener(delegate { us.OnChangeVolume("MasterVol", bdMasterVol.value); });
        bdBGM.onValueChanged.AddListener(delegate { us.OnChangeVolume("BGMVol", bdBGM.value); });
        bdSE.onValueChanged.AddListener(delegate { us.OnChangeVolume("SFVol", bdSE.value); });
        bdFullScreen.onValueChanged.AddListener(delegate { us.OnChangeFullScreen(bdFullScreen.value); });
        bdvSync.onValueChanged.AddListener(delegate { us.OnChangevSync(bdvSync.value); });
        bdAntiAliasing.onValueChanged.AddListener(delegate { us.OnChangeAntiAliasing(bdAntiAliasing.value); });
        bdResolution.onValueChanged.AddListener(delegate { us.OnChangeResolustion(bdResolution.value); });

        butRestore.onClick.AddListener(us.RestoreSetting);
        butSave.onClick.AddListener(WriteData);

        //�������ѡ��
        InitiabdLanguage();
        InitiabdQuality();
        initiabdResolution();
        initiabdAntiAliasing();
        InitiaDropDown(bdFullScreen, DPmod.����);
        InitiaDropDown(bdvSync, DPmod.����);

        InitiaDropDown(bdMasterVol, DPmod.����);
        InitiaDropDown(bdBGM, DPmod.����);
        InitiaDropDown(bdSE, DPmod.����);


        //��ȡ����
        DropdownReadData();
        FreshTwoHead();
        //bdLanguage.RefreshShownValue();
        //ˢ�¿��
        UIM_UIManager.Instance.RefreshLayoutsRecursively();
    }



    /// <summary>
    /// ˢ������������е�����˫ͷѡ��
    /// </summary>
    public void FreshTwoHead()
    {
        UIM_TwoHead[] twohead = GetComponentsInChildren<UIM_TwoHead>();
        foreach (var item in twohead)
        {
            item.FreshButton();
        }
    }



        /// <summary>
    /// ��ʼ������ѡ��
    /// </summary>
    public void InitiabdLanguage()
    {
        
        bdLanguage.ClearOptions();
        List<TMP_Dropdown.OptionData> list = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < us.locales.Count; i++)
        {
            list.Add(new TMP_Dropdown.OptionData(us.locales[i].ToString()));
        }

        bdLanguage.AddOptions(list);
    }



    /// <summary>
    /// ��ʼ��������ѡ��
    /// </summary>
    public void initiabdResolution()
    {
        bdResolution.ClearOptions();
        var reList = us.resolutionList;
        List<TMP_Dropdown.OptionData> list = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < reList.Count; i++)
        {
            list.Add(new TMP_Dropdown.OptionData(reList[i].x+"x" + reList[i].y));
        }
        bdResolution.AddOptions(list);
    }

    /// <summary>
    /// ��ʼ�������ѡ��
    /// </summary>
    public void initiabdAntiAliasing()
    {
        bdAntiAliasing.ClearOptions();
        List<TMP_Dropdown.OptionData> list = new List<TMP_Dropdown.OptionData>();
        var antiList = us.antiAliasingList;
        for (int i = 0; i < antiList.Count; i++)
        {
            list.Add(new TMP_Dropdown.OptionData((float)antiList[i]+ "x MSAA"));
        }
        bdAntiAliasing.AddOptions(list);

    }



    /// <summary>
    /// ��ʼ��ͼ������ѡ��
    /// </summary>
    public void InitiabdQuality()
    {
        bdQuality.ClearOptions();
        List<TMP_Dropdown.OptionData> lsit = new List<TMP_Dropdown.OptionData>();
        // ��ȡ���л���������������ƺ�����
        string[] qualityNames = QualitySettings.names;
        int qualityCount = QualitySettings.names.Length;

        // ��ӡ������л���������������ƺ�����
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
    /// Ϊbropdown��value��ֵ��ui��ȡ����
    /// </summary>
    public void DropdownReadData()
    {
        //LoadData();
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
        print("��������д��ɹ�����Ϊbropdown��ֵ");

    }

    /// <summary>
    /// д������
    /// </summary>
    public void WriteData()
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
        print("�������ݴ洢�ɹ�");
    }

    public enum DPmod
    {
        ����, ����
    }

    public void InitiaDropDown(TMP_Dropdown dropdown, DPmod dPmod)
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
