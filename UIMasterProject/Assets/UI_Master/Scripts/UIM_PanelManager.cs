using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[System.Serializable]
public struct ButContent
{
    public Button but;
    public List<GameObject> targetObjs;
    public bool isOpen;
    public bool autoCloseSelf;

    public ButContent(Button b, List<GameObject> to, bool o=true,bool close=true)
    {
        but = b;
        targetObjs = to;
        isOpen = o;
        autoCloseSelf = close;
    }

}

[AddComponentMenu("UIMaster/Button/PanelManager")]
[DisallowMultipleComponent]
public class UIM_PanelManager : MonoBehaviour
{
    public List<ButContent> butContentsList;
    public Button butToStart;
   
    public GameObject butFirst; //该场景打开后，第一个按钮

    //给声明的按钮挂委托
    //设定时，如果是开启
    private void Awake()
    {
        if (butToStart)
        {
            butToStart.onClick.AddListener(OnStartGameButtonClicked);
        }

        if (butContentsList.Count == 0) return;
        foreach (var item in butContentsList)
        {
            item.but.onClick.AddListener(delegate { Switch(item); });
            
        }

    }

   

    void OnEnable()
    {
        // 确保EventSystem存在
        if (!EventSystem.current) {
            RebackFirstBut();
        }

    }

    private void Update()
    {
        if (Keyboard.current.anyKey.wasReleasedThisFrame)
        {
            RebackFirstBut();
        }
   
    }




    void RebackFirstBut()
    {

        // 设置按钮为第一个选择
        EventSystem.current.firstSelectedGameObject = butFirst.gameObject;
        //Debug.Log("默认已跳转到:" + butFirst.gameObject.name);

        // 手动选择按钮
        EventSystem.current.SetSelectedGameObject(butFirst.gameObject);
    }



    /// <summary>
    /// 加载过场场景
    /// </summary>
    public void OnStartGameButtonClicked()
    {
        SceneManager.LoadScene("LoadingScene");
    }

    public void Switch(ButContent butContent)
    {

        foreach (var item in butContent.targetObjs)
        {
            var anim = item.GetComponent<Animator>()?
                item.GetComponent<Animator>():item.AddComponent<Animator>();



            //开启模式
            item.SetActive(butContent.isOpen ? true : item.activeInHierarchy);
            anim.SetBool("ISSWITCH", !butContent.isOpen);

            //if (butContent.isOpen)
            //{
            //    item.SetActive(butContent.isOpen?true:item.activeInHierarchy);
            //    anim.SetBool("ISSWITCH",!butContent.isOpen);
            //}
            //else
            //{
            //    anim.SetBool("ISSWITCH", true);

            //}

            UIM_UIManager.Instance.RefreshLayoutsRecursively();
        }

        //判断是不是要关闭自身
        if (!butContent.autoCloseSelf) return;
        if (GetComponent<Animator>())
        {
            GetComponent<Animator>().SetBool("ISSWITCH", true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }


    public void DeactivatePanel()
    {

        gameObject.SetActive(false);
    }




}
