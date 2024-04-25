using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class UIT_LogsLoad : MonoBehaviour
{
    public TextAsset log;
    public TMP_Text textLog;

    void Start()
    {
        // 拼接文件路径
        string filePath = log.text;

        // 检查文件是否存在
        if (textLog )
        {
            textLog.text = log.text;
        }
        else
        {
            Debug.LogWarning("Log file not found: " + filePath);
        }
    }
}
