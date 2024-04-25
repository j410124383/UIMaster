using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Text;

public class UIT_LogsLoad : MonoBehaviour
{
    public TextAsset log;
    public TMP_Text textLog;



    void Start()
    {

        // 检查文件是否存在
        if (textLog)
        {
            textLog.text = ReadTextFile(log);
            string text = ReadTextFile(log);
            //Debug.Log(text);
        }
         

        string ReadTextFile(TextAsset asset)
        {
            using (MemoryStream stream = new MemoryStream(asset.bytes))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
