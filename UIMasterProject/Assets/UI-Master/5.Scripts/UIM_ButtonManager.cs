using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct ButContent
{
    public Button but;
    public List<GameObject> targetObjs;
    public bool isOpen;

    public  ButContent(Button b,List<GameObject> to,bool o)
    {
        but = b;
        targetObjs = to;
        isOpen = o;

    }

}


public class UIM_ButtonManager : MonoBehaviour
{
    public List<ButContent> butContentsList;


    //给声明的按钮挂委托
    //设定时，如果是开启
    private void Awake()
    {
        foreach (var item in butContentsList)
        {
            item.but.onClick.AddListener(delegate { Switch(item); });
        }
    }

    public void Switch(ButContent butContent)
    {

        foreach (var item in butContent.targetObjs)
        {
            var anim = item.GetComponent<Animator>()?
                item.GetComponent<Animator>():item.AddComponent<Animator>();



            //开启模式
            if (butContent.isOpen)
            {
                item.SetActive(true);
                anim.SetBool("ISSWITCH",false);
            }
            else
            {
                anim.SetBool("ISSWITCH", true);

            }

            UIM_UIManager.Instance.RefreshLayoutsRecursively(item.transform);
        }

    }


    public void DeactivatePanel()
    {

        gameObject.SetActive(false);
    }


}
