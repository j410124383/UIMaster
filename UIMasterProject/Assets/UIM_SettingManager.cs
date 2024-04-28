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
    //��������ؽű�����Ҫʵ��Ŀǰ�Ļ�������
    public List<int> antiAliasingList;

    public List<Vector2> resolutionList;
    public int sliderStep = 11;

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

    public AudioMixer mainAudioMixer;
    AsyncOperationHandle m_InitialzeOperation;

    public SO_SettingData curSetData;
    public SO_SettingData orignSettingData;

    //�ȸ���Ҫ��˫ͷ��ť�������󶨵����񣬺ͳ�ʼ����
    private void Awake()
    {
        dataListSlider = new List<TMP_Dropdown.OptionData>();
        //��ʼ�����ֳ���options
        for (int i = 0; i < sliderStep; i++)
        {
            dataListSlider.Add(new TMP_Dropdown.OptionData(i.ToString()));
        }


        //��ʼ��setting�ļ�����Ҫdropdown
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

        //�������ѡ��
        InitiabdLanguage();
        InitiabdQuality();
        initiabdResolution();
        initiabdAntiAliasing();
        InitiaDropDown(bdFullScreen, DPmod.����);
        InitiaDropDown(bdvSync, DPmod.����);
        
        InitiaDropDown(bdMasterVol,DPmod.����);
        InitiaDropDown(bdBGM, DPmod.����);
        InitiaDropDown(bdSE, DPmod.����);


        //��ȡ����
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
    /// ��ʼ��������ѡ��
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
    /// ��ʼ�������ѡ��
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
    /// ��ʼ������ѡ��
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
    /// �ı�����ѡ��
    /// </summary>
    /// <param name="i"></param>
    public void OnChangeLanguage(int i)
    {

        var l = LocalizationSettings.AvailableLocales.Locales;
        LocalizationSettings.Instance.SetSelectedLocale(l[i]);
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
    public void OnChangeVolume(string s,int DP)
    {
        var f=Mathf.Lerp( -80,20,((float)DP / (sliderStep - 1) ));
        mainAudioMixer.SetFloat(s,f);
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
        QualitySettings.vSyncCount = i==0 ? 1 : 0;
        //Debug.Log("VSync Mode Toggled: " + (QualitySettings.vSyncCount == 0 ? "Off" : "On"));
    }

    /// <summary>
    /// �޸Ŀ����
    /// </summary>
    public void OnChangeAntiAliasing(int i)
    {
        // �޸Ŀ����ģʽ
        QualitySettings.antiAliasing = antiAliasingList[i];
        //Debug.Log("Anti-Aliasing Mode Set to: " + antiAliasingList[i] + "x MSAA");
    }

    /// <summary>
    /// �޸ķֱ���
    /// </summary>
    public void OnChangeResolustion(int i)
    {
        var v2 = resolutionList[i];
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

    public enum DPmod
    {
        ����,����
    }

    public void InitiaDropDown(TMP_Dropdown dropdown,DPmod dPmod)
    {
        dropdown.ClearOptions();
        if (dPmod == DPmod.����)
        {
            
            var dataListBool = new List<TMP_Dropdown.OptionData>();
            dataListBool.Add(new TMP_Dropdown.OptionData("ON"));
            dataListBool.Add(new TMP_Dropdown.OptionData("OFF"));
            dropdown.AddOptions(dataListBool);
        }
        else if (dPmod==DPmod.����)
        {
            dropdown.AddOptions(dataListSlider);
        }
       
    }




    /// <summary>
    /// ��ȡ����
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
        print("��������д��ɹ�����Ϊbd��ֵ");
    }

    /// <summary>
    /// д������
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
        print("�������ݴ洢�ɹ�");
    }

    public void SaveData()
    {
        // �����ݶ���ת��Ϊ JSON ��ʽ���ַ���
        string json = JsonConvert.SerializeObject(curSetData);

        // �洢 JSON �ַ���
        PlayerPrefs.SetString("SettingData", json);
        PlayerPrefs.Save();
    }


    public SO_SettingData LoadData()
    {
        // �� PlayerPrefs �м��� JSON �ַ���
        string json = PlayerPrefs.GetString("SettingData", "{}");

        // �� JSON �ַ���ת��Ϊ���ݶ���
        SO_SettingData data = JsonConvert.DeserializeObject<SO_SettingData>(json);

        return data;
    }

    public void RestoreSetting()
    {
        curSetData.ResetToInitialValues();
        ReadData();
    }


}
