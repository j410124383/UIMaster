using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEditor;

public static class UIT_SaveLoad 
{

    /// <summary>
    /// 将myScriptableObject写入本地，转为jason文件
    /// </summary>
    /// <param name="myScriptableObject"></param>
    public static void SaveData(SO_SettingData myScriptableObject,string name)
    {
        var path = Application.persistentDataPath + name;
        string json = JsonUtility.ToJson(myScriptableObject);
        File.WriteAllText(path, json);
        //打开
        System.Diagnostics.Process.Start(path);

        Debug.Log("<color=green>[SUCCESS]</color>存储数据成功！");
    }

    /// <summary>
    /// 读取本地文件，由jason转为scriptableObject
    /// </summary>
    /// <param name="myScriptableObject"></param>
    public static void LoadData(SO_SettingData myScriptableObject, string name)
    {
        var path = Application.persistentDataPath + name;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SO_SettingData data = JsonConvert.DeserializeObject<SO_SettingData>(json) ;
            myScriptableObject.CopyNewData(data);
            Debug.Log(myScriptableObject.num_Language);
            Debug.Log("<color=green>[SUCCESS]</color>加载数据成功！");
        }
        else
        {
            Debug.LogWarning("<color=red>[WARRING]</color>路径不存在！");
        }


    }


}
