using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[AddComponentMenu("UIMaster/Button/ButTwoHead")]
[DisallowMultipleComponent]
public class UIM_ButTwoHead : MonoBehaviour
{

    public Button leftBut,rightBut;

    public TMP_Dropdown targetDrop;


    private void Awake()
    {
        ChangeValue(0);
        leftBut.onClick.AddListener(delegate { ChangeValue(-1); });
        rightBut.onClick.AddListener(delegate { ChangeValue(1); });
    }

    public void ChangeValue(int i,bool isLoop=false)
    {
       
        int num = targetDrop.value += i;
        //当模式开了循环，则从最大值调到1
        if (num >= targetDrop.options.Count && isLoop)
        {
            num = 0;
        }
        targetDrop.value = num;

        //刷新文字
        targetDrop.GetComponent<TMP_Text>().text = targetDrop.options[targetDrop.value].text;
        FreshButton();

    }

    public void FreshButton()
    {

        leftBut.interactable = true;
        rightBut.interactable = true;
        if (targetDrop.value == 0)
        {
            leftBut.interactable = false;

        }
        if (targetDrop.value == targetDrop.options.Count - 1)
        {
            rightBut.interactable = false;

        }

        UIM_UIManager.Instance.RefreshLayoutsRecursively();

    }

}
