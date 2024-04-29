using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoadTest : MonoBehaviour
{

    public SO_SettingData test;
    public TMP_Text loadt, savet;

    private void Update()
    {
        loadt.text = test.ToString();
    }


    public void ButLoad()
    {
        UIT_SaveLoad.LoadData(test,"/test.json");
        
    }


    public void ButSave()
    {
        UIT_SaveLoad.SaveData(test, "/test.json");
    }


}
