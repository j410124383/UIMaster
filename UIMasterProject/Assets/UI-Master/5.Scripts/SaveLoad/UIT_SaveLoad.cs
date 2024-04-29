using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEditor;

public static class UIT_SaveLoad 
{

    /// <summary>
    /// ��myScriptableObjectд�뱾�أ�תΪjason�ļ�
    /// </summary>
    /// <param name="myScriptableObject"></param>
    public static void SaveData(SO_SettingData myScriptableObject,string name)
    {
        var path = Application.persistentDataPath + name;
        string json = JsonUtility.ToJson(myScriptableObject);
        File.WriteAllText(path, json);
        //��
        System.Diagnostics.Process.Start(path);

        Debug.Log("<color=green>[SUCCESS]</color>�洢���ݳɹ���");
    }

    /// <summary>
    /// ��ȡ�����ļ�����jasonתΪscriptableObject
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
            Debug.Log("<color=green>[SUCCESS]</color>�������ݳɹ���");
        }
        else
        {
            Debug.LogWarning("<color=red>[WARRING]</color>·�������ڣ�");
        }


    }


}
