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
        // ƴ���ļ�·��
        string filePath = log.text;

        // ����ļ��Ƿ����
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
