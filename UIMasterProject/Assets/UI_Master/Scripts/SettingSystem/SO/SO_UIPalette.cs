using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New UIPalette", menuName = "Setting/UIPalette")]

public class SO_UIPalette : ScriptableObject
{

    //名称
    public string paletteName;

    //定义UI需要用到的四种颜色
    //public Color c_light, c_Gray, c_Deep, c_Dark;


    [SerializeField]
    public Color[] colors = new Color[4];






}
