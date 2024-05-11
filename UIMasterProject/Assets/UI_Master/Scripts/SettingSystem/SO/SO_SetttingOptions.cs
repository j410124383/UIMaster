using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

[CreateAssetMenu(fileName = "New SettingOptions", menuName = "SettingOptions")]
public class SO_SetttingOptions : ScriptableObject
{
    //设置类相关脚本，主要实现目前的基本功能
    public List<int> antiAliasingList;
    public List<Locale> locales;
    public List<Vector2> resolutionList;
    public int sliderStep = 11;



}
