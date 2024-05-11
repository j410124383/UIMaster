using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIM_PersistentSceneManager : MonoBehaviour
{
    private string scenceName = "========PersistentScene========";

    // ����Ϸ��ʼʱ�������ó���������
    void Awake()
    {
        // ������ó����Ƿ��Ѿ�����
        Scene persistentScene = SceneManager.GetSceneByName(scenceName);
        if (!persistentScene.isLoaded)
        {
            // �������ó���������
            persistentScene = SceneManager.CreateScene(scenceName);
            SceneManager.LoadScene(scenceName, LoadSceneMode.Additive);
        }

        // ����ǰ�����Ƶ����ó�����
        SceneManager.MoveGameObjectToScene(gameObject, persistentScene);
    }



}
