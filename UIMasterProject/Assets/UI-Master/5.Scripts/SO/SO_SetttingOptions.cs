using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

[CreateAssetMenu(fileName = "New SettingOptions", menuName = "SettingOptions")]
public class SO_SetttingOptions : ScriptableObject
{
    //��������ؽű�����Ҫʵ��Ŀǰ�Ļ�������
    public List<int> antiAliasingList;
    public List<Locale> locales;
    public List<Vector2> resolutionList;
    public int sliderStep = 11;



}
