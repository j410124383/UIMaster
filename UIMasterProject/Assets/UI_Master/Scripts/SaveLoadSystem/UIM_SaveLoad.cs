using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEditor;


[AddComponentMenu("UIMaster/SaveLoad")]
[DisallowMultipleComponent]
//�˽ű�ִ����Ϸ���йش洢�Ͷ�ȡ������
public class UIM_SaveLoad :ScriptableObject
{
    /// <summary>
    /// ���ұ��ش洢·�������û���򴴽�
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
    /// ��myScriptableObjectд�뱾�أ�תΪjason�ļ�
    /// </summary>
    /// <param name="myScriptableObject"></param>
    public static void SaveData(ScriptableObject myScriptableObject,string name)
    {
        var path = CheckPath()+CheckName(name);
        string json = JsonUtility.ToJson(myScriptableObject);
        File.WriteAllText(path, json);

        Debug.Log("<color=green>[SUCCESS]</color>�洢���ݳɹ���");

        //��Ŀǰ�洢���ļ�
        //System.Diagnostics.Process.Start(path);

    }

    /// <summary>
    /// ��ȡ�����ļ�����JasonתΪScriptableObject
    /// </summary>
    /// <param name="myScriptableObject"></param>
    public static void LoadData(ScriptableObject myScriptableObject, string name)
    {
        var path = CheckPath() + CheckName(name);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            //�ж�SO�ű������ͣ�Ȼ����
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


            Debug.Log("<color=green>[SUCCESS]</color>�������ݳɹ���");
        }
        else
        {
            Debug.LogWarning("<color=red>[WARRING]</color>·�������ڣ�");
        }

    }


}
