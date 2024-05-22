using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEditor;


[AddComponentMenu("UIMaster/SaveLoad")]
[DisallowMultipleComponent]
//此脚本执行游戏内有关存储和读取的问题
public class UIM_SaveLoad :ScriptableObject
{
    /// <summary>
    /// 查找本地存储路径，如果没有则创建
    /// </summary>
    /// <returns></returns>
    public static string CheckPath()
    {
        string folderPath = Application.persistentDataPath + "/SaveData";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            //Debug.Log("Folder path created: " + folderPath);
        }
        else
        {
            //Debug.Log("Folder path already exists: " + folderPath);
        }
        return folderPath;
    }

    public static string CheckName(string s) => string.Format("/{0}.json", s);

    /// <summary>
    /// 将myScriptableObject写入本地，转为jason文件
    /// </summary>
    /// <param name="myScriptableObject"></param>
    public static void SaveData(ScriptableObject myScriptableObject,string name)
    {
        var path = CheckPath()+CheckName(name);
        string json = JsonUtility.ToJson(myScriptableObject);
        File.WriteAllText(path, json);

        Debug.Log("<color=green>[SUCCESS]</color>存储数据成功！");

        //打开目前存储的文件
        //System.Diagnostics.Process.Start(path);

    }

    /// <summary>
    /// 读取本地文件，由Jason转为ScriptableObject
    /// </summary>
    /// <param name="myScriptableObject"></param>
    public static void LoadData(ScriptableObject myScriptableObject, string name)
    {
        var path = CheckPath() + CheckName(name);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            //判断SO脚本的类型，然后处理
            if (myScriptableObject is SO_SettingData)
            {
                SO_SettingData data = JsonConvert.DeserializeObject<SO_SettingData>(json);
                var x = (SO_SettingData)myScriptableObject;
                x.CopyNewData(data);
            }
            if (myScriptableObject is SO_AchievementInfo) 
            {
                SO_AchieveStates data = JsonConvert.DeserializeObject<SO_AchieveStates>(json);
                var x = (SO_AchievementInfo)myScriptableObject;
                for (int i = 0; i < x.so_AchieveStates.achieveStateList.Count; i++)
                {
                    x.so_AchieveStates.achieveStateList[i].CopyState(data.achieveStateList[i]);
                }

                x.PullData();
                //x.CopyNewData(data.AchievementList);
            }


            Debug.Log("<color=green>[SUCCESS]</color>加载数据成功！");
        }
        else
        {
            Debug.LogWarning("<color=red>[WARRING]</color>路径不存在！");
        }

    }


}
