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
        // ˢ�µ�ǰ�����������Ӷ����еĲ������
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
    /// ˢ�¶����ϵ�layout���
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
        // ����һ��Э�̣��ӳ�1����˳�Ӧ�ó���
        StartCoroutine(DelayQuit(1f));
        UIM_MainMenu.Instance.frameOptionsAC.SetTrigger("ISSWITCH");
    }


    /// <summary>
    /// �ӳ�1����˳�Ӧ�ó���ķ���
    /// </summary>
    IEnumerator DelayQuit(float f)
    {
        yield return new WaitForSeconds(f);

        // ���� Application.Quit() �����˳�Ӧ�ó���
        Application.Quit();
    }


}
