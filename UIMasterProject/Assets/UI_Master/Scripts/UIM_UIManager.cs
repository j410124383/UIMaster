using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[AddComponentMenu("UIMaster/UIManager")]
public class UIM_UIManager : MonoBehaviour
{
    public static UIM_UIManager Instance;



    private void Awake()
    {
        Instance = this;
      
    }

    public void Start()
    {
        // 刷新当前对象及其所有子对象中的布局组件
        RefreshLayoutsRecursively();
    }


    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    RefreshLayoutsRecursively();
        //}
    }

    /// <summary>
    /// 刷新对象上的layout组件
    /// </summary>
    public  void RefreshLayoutsRecursively(Transform parent=null)
    {
        StartCoroutine(refreshRecursively(parent));
        StartCoroutine(refreshRecursively(parent, true));
    }

    IEnumerator refreshRecursively(Transform parent = null,bool Again=false)
    {
        if (!Again)
        {
            yield return new WaitForEndOfFrame();
        }
        else
        {
            yield return new WaitForSeconds(0.3f);

        }
      

        if (parent == null) parent = transform;

        var list = parent.GetComponentsInChildren<HorizontalOrVerticalLayoutGroup>();
        var list02 = parent.GetComponentsInChildren<ContentSizeFitter>();

        foreach (var item in list)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(item.transform.GetComponent<RectTransform>());

        }

        foreach (var item in list02)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(item.transform.GetComponent<RectTransform>());
        }

    }


    public void StartQuit()
    {
        // 启动一个协程，延迟1秒后退出应用程序
        StartCoroutine(DelayQuit(1f));
        UIM_MainMenu.Instance.frameOptionsAC.SetTrigger("ISSWITCH");
    }


    /// <summary>
    /// 延迟1秒后退出应用程序的方法
    /// </summary>
    IEnumerator DelayQuit(float f)
    {
        yield return new WaitForSeconds(f);

        // 调用 Application.Quit() 方法退出应用程序
        Application.Quit();
    }


}
