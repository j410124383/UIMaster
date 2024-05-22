using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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

[AddComponentMenu("UIMaster/Button/ButtonManager")]
[DisallowMultipleComponent]
public class UIM_ButtonManager : MonoBehaviour
{
    public List<ButContent> butContentsList;
    public Button butToStart;
   
    public GameObject butFirst; //�ó����򿪺󣬵�һ����ť

    //�������İ�ť��ί��
    //�趨ʱ������ǿ���
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
        // ȷ��EventSystem����
        if (EventSystem.current == null)
        {
            Debug.LogError("No EventSystem found in the scene. Please add an EventSystem.");
            return;
        }

        // ���ð�ťΪ��һ��ѡ��
        EventSystem.current.firstSelectedGameObject = butFirst.gameObject;

        // �ֶ�ѡ��ť
        EventSystem.current.SetSelectedGameObject(butFirst.gameObject);
    }



    /// <summary>
    /// ���ع�������
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



            //����ģʽ
            if (butContent.isOpen)
            {
                item.SetActive(true);
                anim.SetBool("ISSWITCH",false);
            }
            else
            {
                anim.SetBool("ISSWITCH", true);

            }

            UIM_UIManager.Instance.RefreshLayoutsRecursively();
        }

    }


    public void DeactivatePanel()
    {

        gameObject.SetActive(false);
    }





}
