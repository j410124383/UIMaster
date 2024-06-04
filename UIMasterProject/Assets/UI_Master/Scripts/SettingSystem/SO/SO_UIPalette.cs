using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New UIPalette", menuName = "Setting/UIPalette")]

public class SO_UIPalette : ScriptableObject
{

    //����
    public string paletteName;

    //����UI��Ҫ�õ���������ɫ
    //public Color c_light, c_Gray, c_Deep, c_Dark;


    [SerializeField]
    public Color[] colors = new Color[4];






}
