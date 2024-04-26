using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Text;

public class UIM_LogsLoad : MonoBehaviour
{
    public TextAsset log;
    public TMP_Text textLog;

    private void Awake()
    {
        textLog.text = ReadTextFile(log);
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
