using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIM_TwoHead : MonoBehaviour
{

    public Button leftBut,rightBut;

    public TMP_Dropdown targetDrop;


    private void Awake()
    {
        ChangeValue(0);
        leftBut.onClick.AddListener(delegate { ChangeValue(-1); });
        rightBut.onClick.AddListener(delegate { ChangeValue(1); });
    }

    public void ChangeValue(int i)
    {
       
        targetDrop.value += i;
        targetDrop.GetComponent<TMP_Text>().text = targetDrop.options[targetDrop.value].text;
        leftBut.interactable = true;
        rightBut.interactable = true;
        if (targetDrop.value == 0)
        {
            leftBut.interactable = false;
            
        }
        if (targetDrop.value== targetDrop.options.Count-1)
        {
            rightBut.interactable = false;
         
        }
        UIM_UIManager.Instance.RefreshLayoutsRecursively();

        //print(targetDrop.value);
        

    }

}
